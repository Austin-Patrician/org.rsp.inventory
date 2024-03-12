namespace org.rsp.entity.Request;

public class QueryStoreHouseByConditionRequest
{
    public string? StoreHouseName { get; set; }
    
    public string? Location { get; set; }
    
    public DateTime? StartCreateTime { get; set; }
    
    public DateTime? EndCreateTime { get; set; }
}