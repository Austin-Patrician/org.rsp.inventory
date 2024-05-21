using org.rsp.entity.Common;

namespace org.rsp.entity.Request;

public class QueryStoreHouseRequest : Pager
{
    public string? StoreHouseName { get; set; }
    
    public string? Location { get; set; }
}