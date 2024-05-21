using org.rsp.entity.Common;

namespace org.rsp.entity.Request;

public class QueryGoodsCategoryRequest : Pager
{
    public string? GoodsCategoryName { get; set; }
}