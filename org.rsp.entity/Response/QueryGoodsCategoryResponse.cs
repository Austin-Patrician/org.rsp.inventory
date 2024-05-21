using org.rsp.database.Table;

namespace org.rsp.entity.Response;

public class QueryGoodsCategoryResponse
{
    public List<GoodsCategory> GoodsCategories { get; set; } = new();
    
    public int TotalCount { get; set; }
}