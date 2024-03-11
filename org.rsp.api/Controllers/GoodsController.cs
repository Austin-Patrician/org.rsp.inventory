using Microsoft.AspNetCore.Mvc;
using org.rsp.database.Table;
using org.rsp.entity.Common;
using org.rsp.entity.Request;
using org.rsp.entity.service;

namespace org.rsp.api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class GoodsController : ApiBaseController
{
    private readonly IGoodsManager _goodsManager;

    private readonly ILogger<GoodsController> _logger;

    public GoodsController(IGoodsManager goodsManager, ILogger<GoodsController> logger)
    {
        _goodsManager = goodsManager;
        _logger = logger;
    }

    [HttpPost]
    public async Task AddGoods(AddGoodsRequest request)
    {
        await _goodsManager.AddGoodsAsync(request);
    }

    [HttpGet]
    public async Task<ResponseResult<List<Goods>>> QueryGoodsCategory()
    {
        return await _goodsManager.QueryGoodsAsync();
    }

    [HttpPost]
    public async Task<ResponseResult<bool>> UpdateGoods(UpdateGoodsRequest request)
    {
        return await _goodsManager.UpdateGoodsAsync(request);
    }
    
    [HttpPost]
    public async Task<bool> DelGoods(List<int> ids)
    {
        return await _goodsManager.BatchDelGoodsAsync(ids);
    }
}