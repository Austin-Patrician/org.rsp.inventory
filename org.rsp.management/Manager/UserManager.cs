using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using org.rsp.database.Table;
using org.rsp.entity.Exception;
using org.rsp.entity.Helper;
using org.rsp.entity.Model;
using org.rsp.entity.Request;
using org.rsp.entity.Response;
using org.rsp.entity.service;
using org.rsp.management.Wrapper;

namespace org.rsp.management.Manager;

public class UserManager : IUserManager,ITransient
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
        var user = _wrapper.User.FindByCondition(_=>_.Id==id).FirstOrDefault();
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
    public async Task<UserModel> QueryUserByPhone(string phone)
    {
        var user = _wrapper.User.FindByCondition(_=>_.Phone == phone).FirstOrDefault();
        if (user is null)
            return null;
        
        var userModel = _mapper.Map<UserModel>(user);
        return userModel;
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
            var existOrNot = _wrapper.User.FindByCondition(_ => _.Phone.Equals(request.Phone )&& _.UserName.Equals(request.UserName))
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
            
            var userModel = _mapper.Map<UserModel>(user);
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
        var user = _wrapper.User.FindByCondition(_ => _.Phone == request.Phone && _.IsDeleted == false).FirstOrDefault();
        if (user is null)
        {
            //user not exist
            return;
        }
        if (! string.IsNullOrEmpty(request.PassWord))
        {
            request.PassWord = PwdBCrypt.Encryption(request.PassWord);
        }
        
        if (string.Equals(user.UserName,request.UserName) && string.Equals(user.PassWord,request.PassWord) && string.Equals(user.Phone,request.Phone) && string.Equals(user.Remark,request.Remark))
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
        if (!string.IsNullOrEmpty(request.Phone))
        {
            user.Phone = request.Phone;
        }
        if (!string.IsNullOrEmpty(request.Remark))
        {
            user.Remark = request.Remark;
        }
        user.UpdateTime=DateTime.Now;
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
        var list =await _wrapper.User.FindByCondition(x=>ids.Contains(x.Id)).ToListAsync();
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
        var roleIds =await _wrapper.UserRole.FindByCondition(_=>_.UserId==userId && _.IsDeleted==false)
            .Select(_=>_.RoleId).ToListAsync();
        if (roleIds.Any())
        {
            foreach (var roleId in roleIds)
            {
                //拿到具体的角色
                var role = _wrapper.Role.FindByCondition(_=>_.Id==roleId && _.IsDeleted==false).FirstOrDefault();
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
    public async Task<List<UserModel>> QueryAllUsersAsync()
    {
        var response = new List<UserModel>();
        try
        {
            //查数据库，并设置redis；
            var users =await _wrapper.User.FindAll().Where(_ => _.IsDeleted == false).ToListAsync();
            response = _mapper.Map<List<UserModel>>(users);
            
        }
        catch (Exception e)
        {
            _logger.LogError($"QueryAllUsersAsync error: {e.Message}");
            throw;
        }

        return response;
    }
    
}