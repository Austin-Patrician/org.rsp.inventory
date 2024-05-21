using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using org.rsp.database.Expressions;
using org.rsp.database.Table;
using org.rsp.entity.Model;
using org.rsp.entity.Request;
using org.rsp.entity.Response;
using org.rsp.entity.service;
using org.rsp.management.Wrapper;

namespace org.rsp.management.Manager;

public class GoodsManager : IGoodsManager, ITransient
{
    private readonly ILogger<GoodsManager> _logger;

    private readonly IMapper _mapper;

    private readonly IRepositoryWrapper _wrapper;

    public GoodsManager(ILogger<GoodsManager> logger, IMapper mapper, IRepositoryWrapper wrapper)
    {
        _logger = logger;
        _mapper = mapper;
        _wrapper = wrapper;
    }


    /// <summary>
    /// query all goods
    /// </summary>
    /// <returns></returns>
    public async Task<QueryGoodsResponse> QueryGoodsAsync(QueryGoodsRequest request)
    {
        var response = new QueryGoodsResponse();
        try
        {
            Expression<Func<Goods, bool>> expression = ExpressionExtension.True<Goods>();

            if (!string.IsNullOrEmpty(request.GoodsName))
            {
                expression = expression.And(p => p.GoodsName == request.GoodsName);
            }

            expression = expression.And(p => p.IsDeleted == false);
            
            var list = await _wrapper.GoodsRepository.FindByCondition(expression).OrderByDescending(_=>_.UpdateTime)
                .Include(o => o.GoodsCategory)
                .Include(p => p.StoreHouse)
                .Skip((request.PageNumber-1)* request.PageSize).Take(request.PageSize).ToListAsync();
            if (list.Any())
            {
                response.GoodsResponses = _mapper.Map<List<GoodsResponse>>(list);
            }

            response.TotalCount = await _wrapper.GoodsRepository.FindByCondition(expression).CountAsync();

            return response;
        }
        catch (Exception e)
        {
            _logger.LogError($"QueryGoodsAsync error: " +e.Message);
            throw;
        }
    }
    
    public async Task<List<GoodsResponse>> QueryGoodsByStoreHouseIdAsync(QueryGoodsByStoreHouseIdRequest request)
    {
        var response = new List<GoodsResponse>();
        try
        {
            var list = await _wrapper.GoodsRepository.FindByCondition(_ =>  _.StoreHouseId==request.storeHouseId && _.IsDeleted == false)
                .OrderByDescending(_=>_.UpdateTime)
                .Include(o => o.GoodsCategory)
                .Include(p => p.StoreHouse)
                .Skip((request.PageNumber-1)* request.PageSize).Take(request.PageSize).ToListAsync();
            if (list.Any())
            {
                response = _mapper.Map<List<GoodsResponse>>(list);
            }

            return response;
        }
        catch (Exception e)
        {
            _logger.LogError($"QueryGoodsByStoreHouseIdAsync error: " +e.Message);
            throw;
        }
    }

    /// <summary>
    /// batch delete goods
    /// </summary>
    /// <param name="ids"></param>
    public async Task<bool> BatchDelGoodsAsync(List<int> ids)
    {
        try
        {
            //logic del
            if (!ids.Any())
            {
                return true;
            }

            var delList = await _wrapper.GoodsRepository.FindByCondition(_ => ids.Contains(_.GoodsId)).ToListAsync();
            foreach (var goods in delList)
            {
                goods.IsDeleted = true;
                _wrapper.GoodsRepository.Update(goods);
            }

            await _wrapper.SaveChangeAsync();

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError($"BatchDelGoodsCategoryAsync Error: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// update the Goods
    /// </summary>
    /// <param name="request"></param>
    public async Task UpdateGoodsAsync(UpdateGoodsRequest request)
    {
        try
        {
            var goods =await _wrapper.GoodsRepository.FindByCondition(_ => _.GoodsId == request.GoodsId).FirstOrDefaultAsync();
            if (goods is null)
                return;
            //remark可以为空
            if (!string.Equals(request.Remark, goods.Remark))
            {
                goods.Remark = request.Remark;
            }

            if (request.Price is not null && request.Price != goods.Price)
            {
                goods.Price = request.Price ?? 0;
            }

            if (!string.IsNullOrEmpty(request.Description) && !string.Equals(request.Description, goods.Description))
            {
                goods.Description = request.Description;
            }
            
            goods.UpdateBy = request.UpdateBy;
            goods.UpdateTime=DateTime.Now;
            _wrapper.GoodsRepository.Update(goods);
            await _wrapper.SaveChangeAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"UpdateGoodsAsync Error: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// export the detail
    /// </summary>
    /// <returns></returns>
    public Task ExportGoodsAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Add a Goods
    /// </summary>
    /// <param name="request"></param>
    public async Task AddGoodsAsync(AddGoodsRequest request)
    {
        try
        {
            var goods = await _wrapper.GoodsRepository.FindByCondition(_ =>
                    _.GoodsName == request.GoodsName && _.StoreHouseId == request.StoreHouseId)
                .FirstOrDefaultAsync();

            if (goods is not null)
                return;

            var addGoods = _mapper.Map<Goods>(request);
            
            _wrapper.GoodsRepository.Create(addGoods);

            await _wrapper.SaveChangeAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"AddGoodsAsync error: {e.Message}");
            throw;
        }
    }
}