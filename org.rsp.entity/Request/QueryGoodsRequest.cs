using Azure;
using org.rsp.entity.Common;

namespace org.rsp.entity.Request;

public class QueryGoodsRequest : Pager
{
    public string? GoodsName { get; set; }
}