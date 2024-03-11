using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using org.rsp.database.Table;
using org.rsp.entity.Request;
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
    public async Task<List<Goods>> QueryGoodsAsync()
    {
        return await _wrapper.GoodsRepository.FindAll()
            .Include(o => o.GoodsCategory)
            .Include(p => p.StoreHouse)
            .Where(_ => _.IsDeleted == false).ToListAsync();
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
    public async Task<bool> UpdateGoodsAsync(UpdateGoodsRequest request)
    {
        try
        {
            //need to check if image change.
            var goodsCategory = _mapper.Map<Goods>(request);
            _wrapper.GoodsRepository.Update(goodsCategory);

            await _wrapper.SaveChangeAsync();

            return true;
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