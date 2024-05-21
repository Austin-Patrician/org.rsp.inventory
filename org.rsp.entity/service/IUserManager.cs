using org.rsp.entity.Model;
using org.rsp.entity.Request;
using org.rsp.entity.Response;

namespace org.rsp.entity.service;

public interface IUserManager
{
    Task<UserModel> AddUserAsync(AddUserRequest request);
    Task DeleteBatchUserAsync(List<int> ids);
    Task<QueryAllUserResponse>  QueryAllUsersAsync(QueryAllUsersRequest request);
    Task<UserModel> QueryUserById(int id);
    Task<(UserModel,List<string>)> QueryUserByPhone(string phone);
    
    Task<QueryRoleByUserIdResponse> QueryRoleByUserIdAsync(int userId);

    Task QueryUserAndRole(string phone);

    Task UpdateUserAsync(UpdateUserRequest request);

    Task AddUserRoleAsync(AddUserRoleRequest request);

    Task RemoveUserRoleAsync(RemoveUserRoleRequest request);

}