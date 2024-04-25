using org.rsp.entity.Model;

namespace org.rsp.entity.Response;

public class QueryAllRolesResponse
{
    public List<RoleModel> RoleModels { get; set; } = new ();

}