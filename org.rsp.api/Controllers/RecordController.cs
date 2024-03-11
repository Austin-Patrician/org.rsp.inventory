using Microsoft.AspNetCore.Mvc;
using org.rsp.database.Table;
using org.rsp.entity.Common;
using org.rsp.entity.Request;
using org.rsp.entity.service;

namespace org.rsp.api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class RecordController
{
    private readonly IRecordManager _recordManager;
    private readonly ILogger<RecordController> _logger;

    public RecordController(IRecordManager recordManager, ILogger<RecordController> logger)
    {
        _recordManager = recordManager;
        _logger = logger;
    }

    /// <summary>
    /// 添加出库/入库记录
    /// </summary>
    /// <param name="request"></param>
    [HttpPost]
    public async Task AddWareHouseRecord(AddWareHouseRecordRequest request)
    {
        await _recordManager.AddWareHouseRecordAsync(request);
    }

    /// <summary>
    /// 查询流水
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ResponseResult<List<Record>>> QueryRecord(QueryRecordConditionRequest request)
    {
        return await _recordManager.QueryRecordAsync(request);
    }

    /// <summary>
    /// 删除流水
    /// </summary>
    /// <param name="ids"></param>
    [HttpPost]
    public async Task DelGoodsCategory(List<int> ids)
    {
        await _recordManager.DelRecord(ids);
    }
}