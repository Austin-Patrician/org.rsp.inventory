using org.rsp.entity.Model;
using org.rsp.entity.Request;
using org.rsp.entity.Response;

namespace org.rsp.entity.service;

public interface IUserManager
{
    Task<UserModel> AddUserAsync(AddUserRequest request);
    Task DeleteBatchUserAsync(List<int> ids);
    Task<List<UserModel>>  QueryAllUsersAsync();
    Task<UserModel> QueryUserById(int id);
    Task<UserModel> QueryUserByPhone(string phone);
    
    Task<QueryRoleByUserIdResponse> QueryRoleByUserIdAsync(int userId);

}