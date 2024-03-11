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
}