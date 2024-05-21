using org.rsp.entity.Model;

namespace org.rsp.entity.Response;

public class QueryRecordResponse
{
    public List<RecordModel> RecordModels { get; set; } = new();
    
    public int TotalCount { get; set; }
}