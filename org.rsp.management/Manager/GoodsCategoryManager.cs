using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npoi.Mapper;
using org.rsp.database.Expressions;
using org.rsp.database.Extensions;
using org.rsp.database.Table;
using org.rsp.entity.Request;
using org.rsp.entity.Response;
using org.rsp.entity.service;
using org.rsp.management.Wrapper;
using Mapper = Npoi.Mapper.Mapper;

namespace org.rsp.management.Manager;

public class GoodsCategoryManager : IGoodsCategoryManager, ITransient
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;
    private readonly ILogger<GoodsCategoryManager> _logger;

    public GoodsCategoryManager(IRepositoryWrapper wrapper, IMapper mapper, ILogger<GoodsCategoryManager> logger)
    {
        _wrapper = wrapper;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// query all category
    /// </summary>
    /// <returns></returns>
    public async Task<QueryGoodsCategoryResponse> QueryGoodsCategoryAsync(QueryGoodsCategoryRequest request)
    {
        var response = new QueryGoodsCategoryResponse();

        try
        {
            Expression<Func<GoodsCategory, bool>> expression = ExpressionExtension.True<GoodsCategory>();

            if (!string.IsNullOrEmpty(request.GoodsCategoryName))
            {
                expression = expression.And(p => string.Equals(p.GoodsCategoryName, request.GoodsCategoryName));
            }

            expression = expression.And(p => p.IsDeleted == false);


            var goodsCategories = await _wrapper.GoodsCategoryRepository.FindByCondition(expression)
                .OrderByDescending(_ => _.UpdateTime)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();


            if (goodsCategories.Any())
            {
                response.GoodsCategories = goodsCategories;
            }

            response.TotalCount = await _wrapper.GoodsCategoryRepository.FindByCondition(expression).CountAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"QueryGoodsCategoryAsync error: {e.Message}");
            throw;
        }

        return response;
    }

    /// <summary>
    /// Del the category
    /// </summary>
    /// <param name="ids"></param>
    public async Task<List<string>> BatchDelGoodsCategoryAsync(List<int> ids)
    {
        var response = new List<string>();

        try
        {
            //logic del
            if (!ids.Any())
            {
                return response;
            }

            //先判断还有没关联的category，否则不能删除
            var delList = await _wrapper.GoodsCategoryRepository.FindByCondition(_ => ids.Contains(_.GoodsCategoryId))
                .ToListAsync();
            foreach (var category in delList)
            {
                var goodsList = await _wrapper.GoodsRepository
                    .FindByCondition(_ => _.GoodsCategoryId == category.GoodsCategoryId).ToListAsync();
                if (goodsList.Any())
                {
                    response.Add(($"{category.GoodsCategoryName}  has related goods, it can't be remove."));
                }
                else
                {
                    category.IsDeleted = true;
                    _wrapper.GoodsCategoryRepository.Update(category);
                }
            }

            await _wrapper.SaveChangeAsync();

            return response;
        }
        catch (Exception e)
        {
            _logger.LogError($"BatchDelGoodsCategoryAsync error: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// update the category
    /// </summary>
    /// <param name="request"></param>
    public async Task UpdateGoodsCategoryAsync(UpdateGoodsCategoryRequest request)
    {
        try
        {
            var goodsCategory = await _wrapper.GoodsCategoryRepository
                .FindByCondition(_ => _.GoodsCategoryId == request.GoodCategoryId)
                .FirstOrDefaultAsync();

            if (goodsCategory is null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(request.GoodsCategoryName) &&
                !string.Equals(request.GoodsCategoryName, goodsCategory.GoodsCategoryName))
            {
                var count = await _wrapper.GoodsCategoryRepository
                    .FindByCondition(_ => _.GoodsCategoryName == request.GoodsCategoryName).CountAsync();
                if (count > 0)
                {
                    //make sure not duplicate name.
                    return;
                }
                goodsCategory.GoodsCategoryName = request.GoodsCategoryName;
            }

            if (!string.Equals(request.Description, goodsCategory.Description))
            {
                goodsCategory.Description = request.Description;
            }

            if (!string.Equals(request.Remark, goodsCategory.Remark))
            {
                goodsCategory.Remark = request.Remark;
            }

            goodsCategory.UpdateBy = request.UpdateBy;
            goodsCategory.UpdateTime = DateTime.UtcNow;
            _wrapper.GoodsCategoryRepository.Update(goodsCategory);

            await _wrapper.SaveChangeAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"UpdateGoodsCategoryAsync error: {e.Message}");
            throw;
        }
    }


    /// <summary>
    /// Add GoodsCategory.
    /// </summary>
    /// <param name="request"></param>
    public async Task AddGoodsCategoryAsync(AddGoodsCategoryRequest request)
    {
        var goods = await _wrapper.GoodsCategoryRepository
            .FindByCondition(_ => _.GoodsCategoryName == request.GoodsCategoryName).FirstOrDefaultAsync();

        if (goods is not null)
            return;

        request.UpdateBy = request.CreateBy;

        var goodsCategory = _mapper.Map<GoodsCategory>(request);

        //TODO:Upload image，set image Id.
        _wrapper.GoodsCategoryRepository.Create(goodsCategory);
        await _wrapper.SaveChangeAsync();
    }


    /// <summary>
    /// query good category by page
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<QueryGoodsCategoryByPageResponse> QueryGoodsCategoryByPageAsync(
        QueryCategoryByConditionRequest request)
    {
        var response = new QueryGoodsCategoryByPageResponse();

        try
        {
            if (request.Pager.PageNumber < 0 || request.Pager.PageSize < 0)
            {
                return response;
            }

            Expression<Func<GoodsCategory, bool>> expression = ExpressionExtension.True<GoodsCategory>();
            if (!string.IsNullOrEmpty(request.Name))
            {
                expression = expression.And(p => p.GoodsCategoryName.Contains(request.Name));
            }

            if (!string.IsNullOrEmpty(request.Desctiption))
            {
                expression = expression.And(p => p.Description.Contains(request.Desctiption));
            }

            var goodsCategories = await _wrapper.GoodsCategoryRepository.FindByCondition(expression)
                .OrderByDescending(_ => _.UpdateTime)
                .Skip((request.Pager.PageNumber - 1) * request.Pager.PageSize)
                .Take(request.Pager.PageSize)
                .ToListAsync();

            if (goodsCategories.Any())
            {
                response.GoodsCategories = goodsCategories;
            }

            return response;
        }
        catch (Exception e)
        {
            _logger.LogError("QueryGoodsCategoryByPageAsync Exception: " + e.Message);
            throw;
        }
    }

    /// <summary>
    /// export the category
    /// </summary>
    public async Task ExportGoodsCategory()
    {
        var allGoodsCategory = await QueryGoodsCategoryAsync(new QueryGoodsCategoryRequest());
        await Task.Run(() =>
        {
            string date = DateTime.Now.ToShortDateString();

            var mapper = new Mapper();
            mapper.Map<GoodsCategory>("更新日期", s => s.UpdateTime).Format<GoodsCategory>("yyyy-MM-dd", s => s.UpdateTime);

            mapper.Save($"C:\\download\\category_{date}.xlsx", allGoodsCategory.GoodsCategories, "Goods_Category", true,
                xlsx: true);
        });
    }
}