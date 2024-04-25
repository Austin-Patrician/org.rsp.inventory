using org.rsp.entity.Model;
using org.rsp.entity.Request;
using org.rsp.entity.Response;

namespace org.rsp.entity.service;

public interface IRoleManager
{
    Task<QueryAllRolesResponse> QueryAllRolesAsync(QueryAllRolesRequest request);

    Task AddRoleAsync(AddRoleRequest request);
}