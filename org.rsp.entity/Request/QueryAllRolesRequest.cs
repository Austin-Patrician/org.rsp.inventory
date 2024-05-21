using org.rsp.entity.Common;

namespace org.rsp.entity.Request;

public class QueryAllRolesRequest : Pager
{
    public string? RoleName { get; set; }
}