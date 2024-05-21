using org.rsp.entity.Model;
using org.rsp.entity.Request;
using org.rsp.entity.Response;

namespace org.rsp.entity.service;

public interface IRoleManager
{
    Task<QueryAllRolesResponse> QueryAllRolesAsync(QueryAllRolesRequest request);

    Task AddRoleAsync(AddRoleRequest request);

    Task<QueryAllRolesResponse> QueryUserNotHasRoleAsync(QueryUserNotHasRoleRequest request);

    Task UpdateRoleAsync(UpdateRoleRequest request);

    Task BatchDeleteRoleAsync(List<int> ids);
    
    Task<QueryAllRolesResponse> QueryUserHaveRoleAsync(QueryUserNotHasRoleRequest request);
}