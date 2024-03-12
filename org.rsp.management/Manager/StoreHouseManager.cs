using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto;
using org.rsp.database.Table;
using org.rsp.entity.Request;
using org.rsp.entity.service;
using org.rsp.management.Wrapper;

namespace org.rsp.management.Manager;

public class StoreHouseManager : IStoreHouseManager, ITransient
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly ILogger<StoreHouseManager> _logger;

    private readonly IMapper _mapper;

    public StoreHouseManager(ILogger<StoreHouseManager> logger, IMapper mapper, IRepositoryWrapper wrapper)
    {
        _logger = logger;
        _mapper = mapper;
        _wrapper = wrapper;
    }

    public async Task AddStoreHouseAsync(AddStoreHouseRequest request)
    {
        try
        {
            var storeHouse = await _wrapper.StoreHouseRepository
                .FindByCondition(_ => string.Equals(_.StoreHouseName, request.StoreHouseName)).FirstOrDefaultAsync();

            if (storeHouse is not null)
                return;

            var addEntity = _mapper.Map<StoreHouse>(request);

            //TODO:Upload image，set image Id.
            _wrapper.StoreHouseRepository.Create(addEntity);
            await _wrapper.SaveChangeAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"AddStoreHouseAsync error: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// 更新仓库
    /// </summary>
    /// <param name="request"></param>
    public async Task UpdateStoreHouseAsync(UpdateStoreHouseRequest request)
    {
        try
        {
            var entity = await _wrapper.StoreHouseRepository
                .FindByCondition(_ => _.StoreHouseId == request.StoreHouseId).FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(request.StoreHouseName))
            {
                //为空则不更新
                entity!.StoreHouseName = request.StoreHouseName;
            }

            if (!string.IsNullOrEmpty(request.Location))
            {
                entity!.Location = request.Location;
            }

            entity.UpdateBy = request.UpdateBy;

            _wrapper.StoreHouseRepository.Update(entity);
            await _wrapper.SaveChangeAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"UpdateStoreHouseAsync error: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// 批量删除仓库
    /// </summary>
    /// <param name="ids"></param>
    public async Task BatchDeleteStoreHouseAsync(List<int> ids)
    {
        try
        {
            if (!ids.Any())
                return;

            var delList = await _wrapper.StoreHouseRepository.FindByCondition(_ => ids.Contains(_.StoreHouseId))
                .ToListAsync();
            foreach (var storeHouse in delList)
            {
                storeHouse.IsDeleted = true;
                _wrapper.StoreHouseRepository.Update(storeHouse);
            }

            await _wrapper.SaveChangeAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"BatchDeleteStoreHouseAsync error: {e.Message}");
            throw;
        }
    }
}