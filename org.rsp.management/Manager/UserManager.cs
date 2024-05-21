using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using org.rsp.database.Expressions;
using org.rsp.database.Table;
using org.rsp.entity.Exception;
using org.rsp.entity.Helper;
using org.rsp.entity.Model;
using org.rsp.entity.Request;
using org.rsp.entity.Response;
using org.rsp.entity.service;
using org.rsp.management.Wrapper;

namespace org.rsp.management.Manager;

public class UserManager : IUserManager, ITransient
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly ILogger<UserManager> _logger;
    private readonly IMapper _mapper;

    public UserManager(IMapper mapper, ILogger<UserManager> logger, IRepositoryWrapper wrapper)
    {
        _mapper = mapper;
        _logger = logger;
        _wrapper = wrapper;
    }


    /// <summary>
    /// 根据id查询用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<UserModel> QueryUserById(int id)
    {
        var user = _wrapper.User.FindByCondition(_ => _.Id == id).FirstOrDefault();
        if (user is null)
            return null;

        var userModel = _mapper.Map<UserModel>(user);
        return userModel;
    }

    /// <summary>
    /// 根据手机号查询用户
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    public async Task<(UserModel, List<string>)> QueryUserByPhone(string phone)
    {
        var roleList = new List<string>();

        var user = await _wrapper.User.FindByCondition(_ => _.Phone == phone).FirstOrDefaultAsync();
        if (user is null)
            return (null, roleList);

        //用户可以有多个角色
        var roles = await _wrapper.Role
            .ExecuteSql($"select r.* from Role r join UserRole ur on ur.RoleId=r.Id where ur.UserId= {user.Id}")
            .ToListAsync();

        var userModel = _mapper.Map<UserModel>(user);

        //可以使用mapper.
        if (!roles.Any())
        {
            return (userModel, roleList);
        }

        roles.ForEach(item => { roleList.Add(item.Name); });

        return (userModel, roleList);
    }


    public async Task QueryUserAndRole(string phone)
    {
        var information = await _wrapper.User
            .ExecuteSql(
                $"select u.UserName,u.PassWord,r.Name from [User] u join UserRole ur on ur.Id=u.Id join  Role r on r.Id=ur.Id where u.Phone='{phone}' ")
            .FirstOrDefaultAsync();
        Console.WriteLine(information);
    }

    /// <summary>
    /// 新增用户,用户至少归属于一个组织
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="Exception"></exception>
    public async Task<UserModel> AddUserAsync(AddUserRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.PassWord))
                throw new UserException("Account and pwd is must.");
            if (string.IsNullOrEmpty(request.Phone))
                throw new UserException("Phone is required");

            //判断用户表是否存在该用户
            var existOrNot = _wrapper.User
                .FindByCondition(_ => _.Phone.Equals(request.Phone) && _.UserName.Equals(request.UserName))
                .FirstOrDefault();
            if (existOrNot is not null)
            {
                //说明该用户已经存在
                throw new UserException("该用户已存在");
            }

            //密码加密
            request.PassWord = PwdBCrypt.Encryption(request.PassWord);


            var user = _mapper.Map<User>(request);
            _wrapper.User.Create(user);
            await _wrapper.SaveChangeAsync();

            //添加角色信息：
            if (request.RoleList != null && request.RoleList.Any())
            {
                //中间表添加数据
                for (int i = request.RoleList.Length - 1; i >= 0; i--)
                {
                    _wrapper.UserRole.Create(new UserRole
                    {
                        UpdateBy = "",
                        UserId = user.Id,
                        RoleId = request.RoleList[i],
                        Remark = request.Remark ?? "",
                        CreateTime = request.CreateTime,
                        UpdateTime = request.UpdateTime,
                        IsDeleted = request.IsDeleted,
                        CreateBy = request.CreateBy ?? "Administration"
                    });
                }
            }

            var userModel = _mapper.Map<UserModel>(user);
            await _wrapper.SaveChangeAsync();
            return userModel;
        }
        catch (Exception _)
        {
            _logger.LogError($"Excuse AddUserAsync error: {_.Message}");
            throw;
        }
    }


    /// <summary>
    /// 更新用户信息
    /// </summary>
    /// <param name="request"></param>
    public async Task UpdateUserAsync(UpdateUserRequest request)
    {
        var user = _wrapper.User.FindByCondition(_ => _.Id == request.Id && _.IsDeleted == false)
            .FirstOrDefault();
        if (user is null)
        {
            //user not exist
            return;
        }

        if (!string.IsNullOrEmpty(request.PassWord))
        {
            request.PassWord = PwdBCrypt.Encryption(request.PassWord);
        }

        if (string.Equals(user.UserName, request.UserName) && string.Equals(user.PassWord, request.PassWord) &&
            string.Equals(user.Remark, request.Remark))
        {
            //data not change
            return;
        }

        //set value
        if (!string.IsNullOrEmpty(request.UserName))
        {
            user.UserName = request.UserName;
        }

        if (!string.IsNullOrEmpty(request.PassWord))
        {
            user.PassWord = request.PassWord;
        }

        if (!string.IsNullOrEmpty(request.Remark))
        {
            user.Remark = request.Remark;
        }

        user.UpdateTime = DateTime.Now;
        _wrapper.User.Update(user);
        await _wrapper.SaveChangeAsync();
    }

    /// <summary>
    /// 批量删除用户
    /// </summary>
    /// <param name="ids"></param>
    /// <exception cref="Exception"></exception>
    public async Task DeleteBatchUserAsync(List<int> ids)
    {
        var list = await _wrapper.User.FindByCondition(x => ids.Contains(x.Id)).ToListAsync();
        foreach (var item in list)
        {
            item.IsDeleted = true;
            _wrapper.User.Update(item);
        }

        await _wrapper.SaveChangeAsync();
    }

    /// <summary>
    /// 查询用户拥有的角色
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <returns></returns>
    public async Task<QueryRoleByUserIdResponse> QueryRoleByUserIdAsync(int userId)
    {
        var response = new QueryRoleByUserIdResponse();
        //拿到角色列表
        var roleIds = await _wrapper.UserRole.FindByCondition(_ => _.UserId == userId && _.IsDeleted == false)
            .Select(_ => _.RoleId).ToListAsync();
        if (roleIds.Any())
        {
            foreach (var roleId in roleIds)
            {
                //拿到具体的角色
                var role = _wrapper.Role.FindByCondition(_ => _.Id == roleId && _.IsDeleted == false).FirstOrDefault();
                var roleModel = _mapper.Map<RoleModel>(role);
                response.RoleModels.Add(roleModel);
            }
        }

        return response;
    }


    /// <summary>
    /// 查询所有的用户
    /// </summary>
    /// <returns></returns>
    public async Task<QueryAllUserResponse> QueryAllUsersAsync(QueryAllUsersRequest request)
    {
        var response = new QueryAllUserResponse();
        try
        {
            Expression<Func<User, bool>> expression = ExpressionExtension.True<User>();
            if (!string.IsNullOrEmpty(request.UserName))
            {
                expression = expression.And(p => p.UserName.Contains(request.UserName));
            }

            if (!string.IsNullOrEmpty(request.Phone))
            {
                expression = expression.And(p => p.Phone.Contains(request.Phone));
            }

            expression = expression.And(p => p.IsDeleted == false);


            //查数据库，
            var users = await _wrapper.User.FindByCondition(expression)
                .OrderByDescending(_ => _.UpdateTime)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize).OrderByDescending(_ => _.UpdateTime).ToListAsync();

            if (users.Any())
            {
                response.UserModels = _mapper.Map<List<UserModel>>(users);
            }

            response.TotalCount = await _wrapper.User.FindByCondition(expression).CountAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"QueryAllUsersAsync error: {e.Message}");
            throw;
        }

        return response;
    }


    /// <summary>
    /// 添加UseRole
    /// </summary>
    /// <param name="request"></param>
    public async Task AddUserRoleAsync(AddUserRoleRequest request)
    {
        if (request.roleList.Any())
        {
            foreach (var roleId in request.roleList)
            {
                var userRole = new UserRole
                {
                    CreateBy = request.CreateBy,
                    CreateTime = DateTime.UtcNow,
                    IsDeleted = false,
                    RoleId = roleId,
                    UserId = request.UserId,
                    UpdateBy = request.CreateBy,
                    UpdateTime = DateTime.UtcNow
                };

                _wrapper.UserRole.Create(userRole);
            }

            await _wrapper.SaveChangeAsync();
        }
    }


    /// <summary>
    /// 移除UserRole
    /// </summary>
    /// <param name="request"></param>
    public async Task RemoveUserRoleAsync(RemoveUserRoleRequest request)
    {
        if (!request.roleList.Any())
            return;

        foreach (var id in request.roleList)
        {
            var userRole = await _wrapper.UserRole
                .FindByCondition(_ => _.UserId == request.UserId && _.RoleId == id && _.IsDeleted == false)
                .FirstOrDefaultAsync();
            if (userRole != null)
            {
                userRole.IsDeleted = true;
                _wrapper.UserRole.Update(userRole);
            }
        }

        await _wrapper.SaveChangeAsync();
    }
}