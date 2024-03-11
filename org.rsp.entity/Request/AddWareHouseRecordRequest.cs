using org.rsp.database.Table;

namespace org.rsp.entity.Request;

public class AddWareHouseRecordRequest
{
    public string SupplierName { get; set; }
    
    public List<AddRecordRequest> Records { get; set; }
}