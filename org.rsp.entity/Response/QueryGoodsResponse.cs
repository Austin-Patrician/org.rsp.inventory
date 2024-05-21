namespace org.rsp.entity.Response;

public class QueryGoodsResponse
{
    public List<GoodsResponse> GoodsResponses { get; set; }= new();
    
    public int TotalCount { get; set; }
}