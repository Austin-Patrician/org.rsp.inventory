using org.rsp.database.Table;
using org.rsp.entity.Request;

namespace org.rsp.entity.service;

public interface IGoodsManager
{
    Task<List<Goods>> QueryGoodsAsync();

    Task<bool> BatchDelGoodsCategoryAsync(List<int> ids);

    Task<bool> UpdateGoodsAsync(UpdateGoodsRequest request);
    
    Task ExportGoodsAsync();

    Task AddGoodsAsync(AddGoodsRequest request);
}