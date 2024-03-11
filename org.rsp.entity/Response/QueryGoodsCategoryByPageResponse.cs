using org.rsp.database.Table;

namespace org.rsp.entity.Response;

public class QueryGoodsCategoryByPageResponse
{
    public List<GoodsCategory> GoodsCategories { get; set; } = new();
}