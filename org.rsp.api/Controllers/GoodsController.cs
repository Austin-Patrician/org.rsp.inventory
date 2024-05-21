using Microsoft.AspNetCore.Mvc;
using org.rsp.api.Auth;
using org.rsp.database.Table;
using org.rsp.entity.Common;
using org.rsp.entity.Model;
using org.rsp.entity.Request;
using org.rsp.entity.Response;
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
    [Yes(Roles = "Administrator,Regular")]
    public async Task AddGoods(AddGoodsRequest request)
    {
        await _goodsManager.AddGoodsAsync(request);
    }

    [HttpPost]
    public async Task<ResponseResult<QueryGoodsResponse>> QueryGoods(QueryGoodsRequest request)
    {
        return await _goodsManager.QueryGoodsAsync(request);
    }

    [HttpPost]
    [Yes(Roles = "Administrator,Regular")]
    public async Task UpdateGoods(UpdateGoodsRequest request)
    {
        await _goodsManager.UpdateGoodsAsync(request);
    }
    
    [HttpPost]
    [Yes(Roles = "Administrator,Regular")]
    public async Task<bool> DelGoods(List<int> ids)
    {
        return await _goodsManager.BatchDelGoodsAsync(ids);
    }
    
    [HttpPost]
    public async Task<ResponseResult<List<GoodsResponse>>> QueryGoodsByStoreHouseId(QueryGoodsByStoreHouseIdRequest request)
    {
        return await _goodsManager.QueryGoodsByStoreHouseIdAsync(request);
    }
}