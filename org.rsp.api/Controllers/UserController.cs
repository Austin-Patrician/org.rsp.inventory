using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using org.rsp.entity.Exception;
using org.rsp.entity.Helper;
using org.rsp.entity.Model;
using org.rsp.entity.Request;
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
    
    public UserController(IJwtService jwtService, IUserManager userManager, ILogger<UserController> logger, MyRedis<string, string> redisManager)
    {
        _jwtService = jwtService;
        _userManager = userManager;
        _logger = logger;
        _redisManager = redisManager;
    }

    /// <summary>
    /// 过期时间由redis控制，不再jwt里面设置，不好refresh 时间
    /// 而且避免token被破解修改过期时间。
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<string> Login(LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Phone) || string.IsNullOrEmpty(request.PassWord))
            return "Account and Password is must.";

        string userId = "";
        try
        {
            //查询数据库校验参数
            var user = await _userManager.QueryUserByPhone(request.Phone);
            if (user is null)
            {
                return "Please register.";
            }
            
            userId = user.Id.ToString();
            //密码加密
            var realPwd = PwdBCrypt.Encryption(request.PassWord);
            if (!user.Phone.Equals(request.Phone) || !user.PassWord.Equals(realPwd))
            {
                return "Account or Password wrong.";
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
                await _redisManager.SetAsync(userId,token,TimeSpan.FromSeconds(120));
            }
            
            //将token 放到请求头
            Response.Headers.Add(new KeyValuePair<string, StringValues>("Token",token));
            
            return "Login success.";
        }
        catch (Exception _)
        {
            //删除token
            _redisManager.Remove(userId);
            return "Login credentials have expired, please log in again";
        }
    }
    
    
    [HttpPost]
    public async Task<UserModel> AddUser(AddUserRequest request)
    {
        try
        {
            return await _userManager.AddUserAsync(request);
        }
        catch (UserException ex)
        {
            _logger.LogError(
                $"Error in org.huage.Service.UserController.UserManager.AddUser.");
            throw;
        }
        catch (Exception e)
        {
            var userException = new UserException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.UserController.UserManager.AddUser.");
            throw userException;
        }
    }
}