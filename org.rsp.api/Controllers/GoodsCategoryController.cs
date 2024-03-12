using System.Net;
using Microsoft.AspNetCore.Mvc;
using org.rsp.database.Table;
using org.rsp.entity.Common;
using org.rsp.entity.Request;
using org.rsp.entity.Response;
using org.rsp.entity.service;

namespace org.rsp.api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class GoodsCategoryController
{
    private readonly IGoodsCategoryManager _manager;

    public GoodsCategoryController(IGoodsCategoryManager manager)
    {
        _manager = manager;
    }


    /// <summary>
    /// 导出品类信息
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(FileStreamResult), (int)HttpStatusCode.OK)]
    public async Task ExportGoodsCategory()
    {
        await _manager.ExportGoodsCategory();
    }

    /// <summary>
    /// 添加产品分类
    /// </summary>
    /// <param name="request"></param>
    [HttpPost]
    public async Task AddGoodsCategory(AddGoodsCategoryRequest request)
    {
        await _manager.AddGoodsCategoryAsync(request);
    }

    /// <summary>
    /// 查询产品分类
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ResponseResult<List<GoodsCategory>>> QueryGoodsCategory()
    {
        return await _manager.QueryGoodsCategoryAsync();
    }

    /// <summary>
    /// 查询产品分类-分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ResponseResult<QueryGoodsCategoryByPageResponse>> QueryGoodsCategoryByPage(
        QueryCategoryByConditionRequest request)
    {
        return await _manager.QueryGoodsCategoryByPageAsync(request);
    }

    /// <summary>
    /// 更新产品分类
    /// </summary>
    /// <param name="request"></param>
    [HttpPost]
    public async Task UpdateGoodsCategory(UpdateGoodsCategoryRequest request)
    {
        await _manager.UpdateGoodsCategoryAsync(request);
    }

    /// <summary>
    /// 删除产品分类
    /// </summary>
    /// <param name="ids"></param>
    [HttpPost]
    public async Task<ResponseResult<List<string>>> BatchDelGoodsCategory(List<int> ids)
    {
        return await _manager.BatchDelGoodsCategoryAsync(ids);
    }
}