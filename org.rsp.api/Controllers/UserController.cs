using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using org.rsp.api.Auth;
using org.rsp.api.Extensions;
using org.rsp.entity.Common;
using org.rsp.entity.Exception;
using org.rsp.entity.Helper;
using org.rsp.entity.Model;
using org.rsp.entity.Request;
using org.rsp.entity.Response;
using org.rsp.entity.service;

namespace org.rsp.api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserController: Controller 
{
    private readonly ILogger<UserController> _logger;
    private readonly IJwtService _jwtService;
    private readonly IUserManager _userManager;
    private readonly MyRedis<string,string> _redisManager;
    private readonly AuthMenusExtension _authMenusExtension;
    
    public UserController(IJwtService jwtService, IUserManager userManager, ILogger<UserController> logger, MyRedis<string, string> redisManager, AuthMenusExtension authMenusExtension)
    {
        _jwtService = jwtService;
        _userManager = userManager;
        _logger = logger;
        _redisManager = redisManager;
        _authMenusExtension = authMenusExtension;
    }

    /// <summary>
    /// 过期时间由redis控制，不再jwt里面设置，不好refresh 时间
    /// 而且避免token被破解修改过期时间。
    /// 登录成功需要拿到其可以授权的页面。返回到menus上面
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<LoginResponse> Login(LoginRequest request)
    {
        LoginResponse response = new LoginResponse();
        
        if (string.IsNullOrEmpty(request.Phone) || string.IsNullOrEmpty(request.PassWord))
            return response;
        
        string userId = "";
        try
        {
            //查询数据库校验参数
            var userInformation = await _userManager.QueryUserByPhone(request.Phone);
            var user= userInformation.Item1;
            if (user is null)
            {
                _logger.LogError($"Can't find the user by {request.Phone}");
                return response;
            }
            
            userId = user.Id.ToString();
            //密码加密
            var realPwd = PwdBCrypt.Encryption(request.PassWord);
            if (!user.Phone.Equals(request.Phone) || !user.PassWord.Equals(realPwd))
            {
                _logger.LogError($"{request.Phone} : Account or Password wrong.");
                return response;
            }

            if (_redisManager.TryGet(userId, out var token))
            {
                if (!string.IsNullOrEmpty(token))
                {
                    //校验token
                    await _jwtService.VerifyToken(token);
                    //刷新过期时间
                    await _jwtService.RefreshToken(userId);
                }
            }
            else
            {
                //首次登录 ，生成token
                token = await _jwtService.GetToken(user);
                await _redisManager.SetAsync(userId,token,TimeSpan.FromMinutes(60));
                //将token 放到请求头
                //Response.Headers.Add(new KeyValuePair<string, StringValues>("Token",token));
            }

            response.token = token;
            response.userName = user.UserName;
            
            _authMenusExtension.TryGetPage(userInformation.Item2,ref response);
            
            _logger.LogInformation($"{response.userName} Login success.");
            _logger.LogInformation($"response is {response.token},{response.menus},{response.userName}");
            
            return response;
        }
        catch (Exception _)
        {
            //删除token
            _redisManager.Remove(userId);
            return response;
        }
    }
    
    
    [HttpPost]
    public async Task<UserModel> AddUser(AddUserRequest request)
    {
        try
        {
            return await _userManager.AddUserAsync(request);
        }
        catch (UserException _)
        {
            _logger.LogError(
                $"Error in org.rsp.Service.UserController.UserManager.AddUser.");
            throw;
        }
        catch (Exception e)
        {
            var userException = new UserException(e.Message);
            _logger.LogError(
                $"Error in org.rsp.Service.UserController.UserManager.AddUser.");
            throw userException;
        }
    }


    [HttpPost]
    [Yes(Roles = "Administrator")]
    public async ValueTask BatchDeleteUser(List<int> ids)
    {
        try
        {
            await _userManager.DeleteBatchUserAsync(ids);
        }
        catch (Exception e)
        {
            _logger.LogError(
                $"Error in org.rsp.Service.UserController.UserManager.BatchDeleteUser. {e.InnerException}");
            throw;
        }
    }


    [HttpPost]
    [AllowAnonymous]
    public async ValueTask<IActionResult> LogOut(int id)
    {
        try
        {
            //移除token,页面跳转到登录界面
            _redisManager.Remove(id.ToString());
            return new RedirectResult("index/User/Login");
        }
        catch (Exception e)
        {
            _logger.LogError(
                $"Error in org.rsp.Service.UserController.UserManager.LogOut.{e.InnerException}");
            throw;
        }
    }


    [HttpPost]
    [Yes(Roles = "Administrator")]
    public async Task<QueryAllUserResponse> QueryAllUser(QueryAllUsersRequest request) 
    {
        try
        {
           return await _userManager.QueryAllUsersAsync(request);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error in org.rsp.Service.UserController.UserManager.QueryAllUser.{e.InnerException}");
            throw;
        }
    }

    [HttpPost]
    [Yes(Roles = "Administrator")]
    public async Task UpdateUser(UpdateUserRequest request)
    {
        try
        {
            await _userManager.UpdateUserAsync(request);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error in org.rsp.Service.UserController.UserManager.UpdateUser.{e.InnerException}");
            throw;
        }
    }

    [HttpPost]
    [Yes(Roles = "Administrator")]
    public async Task AddUserRole(AddUserRoleRequest request)
    {
        try
        {
            await _userManager.AddUserRoleAsync(request);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error in org.rsp.Service.UserController.UserManager.AddUserRole.{e.InnerException}");
            throw;  
        }
    }
    
    [HttpPost]
    [Yes(Roles = "Administrator")]
    public async Task RemoveUserRole(RemoveUserRoleRequest request)
    {
        try
        {
            await _userManager.RemoveUserRoleAsync(request);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error in org.rsp.Service.UserController.UserManager.RemoveUserRole.{e.InnerException}");
            throw;  
        }
    }
}