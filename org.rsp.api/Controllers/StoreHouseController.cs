using Microsoft.AspNetCore.Mvc;
using org.rsp.database.Table;
using org.rsp.entity.Common;
using org.rsp.entity.Request;
using org.rsp.entity.service;

namespace org.rsp.api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class StoreHouseController
{
    private readonly IStoreHouseManager _manager;
    private readonly ILogger<StoreHouseController> _logger;

    public StoreHouseController(IStoreHouseManager manager, ILogger<StoreHouseController> logger)
    {
        _manager = manager;
        _logger = logger;
    }

    /// <summary>
    /// 新增仓库地址
    /// </summary>
    /// <param name="request"></param>
    [HttpPost]
    public async Task AddStoreHouse(AddStoreHouseRequest request)
    {
        await _manager.AddStoreHouseAsync(request);
    }

    [HttpPost]
    public async Task UpdateStoreHouse(UpdateStoreHouseRequest request)
    {
        await _manager.UpdateStoreHouseAsync(request);
    }

    [HttpPost]
    public async Task BatchDeleteStoreHouse(List<int> ids)
    {
        await _manager.BatchDeleteStoreHouseAsync(ids);
    }


    [HttpPost]
    public async Task<ResponseResult<List<StoreHouse>>> QueryStoreHouse(QueryStoreHouseByConditionRequest request)
    {
        return await _manager.QueryStoreHouseAsync(request);
    }
}