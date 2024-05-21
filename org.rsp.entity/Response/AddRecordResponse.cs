using System.Net;

namespace org.rsp.entity.Response;

public class AddRecordResponse
{
    public string Message { get; set; }
    
    public HttpStatusCode StatusCode { get; set; }
}