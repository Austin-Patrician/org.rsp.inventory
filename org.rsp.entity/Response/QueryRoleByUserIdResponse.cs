using org.rsp.entity.Model;

namespace org.rsp.entity.Response;

public class QueryRoleByUserIdResponse
{
    public List<RoleModel> RoleModels { get; set; } = new ();
}