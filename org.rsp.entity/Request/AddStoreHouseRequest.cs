using org.rsp.entity.Common;

namespace org.rsp.entity.Request;

public class AddStoreHouseRequest : BaseFiled
{
    public string StoreHouseName { get; set; }
    
    public string? Description { get; set; }
    
    public string Location { get; set; }
}