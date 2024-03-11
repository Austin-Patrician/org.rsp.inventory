using org.rsp.database.Table;
using org.rsp.entity.Request;
using org.rsp.entity.Response;

namespace org.rsp.entity.service;

public interface IGoodsCategoryManager
{
    Task<List<GoodsCategory>> QueryGoodsCategoryAsync();

    Task<List<(string, string)>> BatchDelGoodsCategoryAsync(List<int> ids);
    Task<QueryGoodsCategoryByPageResponse> QueryGoodsCategoryByPageAsync(QueryCategoryByConditionRequest request);

    Task UpdateGoodsCategoryAsync(UpdateGoodsCategoryRequest request);

    Task ExportGoodsCategory();

    Task AddGoodsCategoryAsync(AddGoodsCategoryRequest request);
}