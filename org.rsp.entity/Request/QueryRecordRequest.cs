using org.rsp.entity.Common;

namespace org.rsp.entity.Request;

public class QueryRecordRequest : Pager
{
    //default in.
    public byte? Direction { get; set; } = 0;
    
    public string? GoodsName { get; set; }
    
    public string? StoreHouseName { get; set; }
}