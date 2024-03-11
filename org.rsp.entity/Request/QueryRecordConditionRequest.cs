using org.rsp.entity.Common;

namespace org.rsp.entity.Request;

public class QueryRecordConditionRequest : Pager
{
    //default in.
    public byte? Direction { get; set; } = 0;
    
    //供应商
    public string SupplierName { get; set; }
}