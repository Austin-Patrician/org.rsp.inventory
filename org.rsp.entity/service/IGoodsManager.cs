using org.rsp.database.Table;
using org.rsp.entity.Model;
using org.rsp.entity.Request;
using org.rsp.entity.Response;

namespace org.rsp.entity.service;

public interface IGoodsManager
{
    Task<QueryGoodsResponse> QueryGoodsAsync(QueryGoodsRequest request);

    Task<bool> BatchDelGoodsAsync(List<int> ids);

    Task UpdateGoodsAsync(UpdateGoodsRequest request);
    
    Task ExportGoodsAsync();

    Task AddGoodsAsync(AddGoodsRequest request);

    Task<List<GoodsResponse>> QueryGoodsByStoreHouseIdAsync(QueryGoodsByStoreHouseIdRequest request);
}