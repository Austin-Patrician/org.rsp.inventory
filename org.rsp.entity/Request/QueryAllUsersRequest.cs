using org.rsp.entity.Common;

namespace org.rsp.entity.Request;

public class QueryAllUsersRequest : Pager
{
    public string? UserName { get; set; }
    
    public string? Phone { get; set; }
}