﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using org.rsp.entity.Helper;
using org.rsp.entity.Model;
using org.rsp.entity.service;
using StackExchange.Redis;

namespace org.rsp.management.Service;

public class JwtService : IJwtService,ITransient
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserManager _userManager;
    private readonly ILogger<JwtService> _logger;
    private readonly MyRedis<string, string> _redis;

    public JwtService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor,
        IUserManager userManager, ILogger<JwtService> logger, MyRedis<string, string> redis)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _logger = logger;
        _redis = redis;
    }

    public async Task<string> GetToken(UserModel dto)
    {
        var jwtTokenOptions = _configuration.GetSection("JWTTokenOptions").Get<JWTTokenOptions>();

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenOptions.SecurityKey));

        //查询用户的角色
        var role = await _userManager.QueryRoleByUserIdAsync(dto.Id);
        var roleNames = role.RoleModels.Select(_ => _.Name).ToList();
        var roleString = string.Join(",", roleNames);
        //new Claim("Expiration", expiresAt.ToString())
        IEnumerable<Claim> claims = new Claim[]
        {
            new Claim("UserName", dto.UserName),
            new Claim("Role", roleString)
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //expires: expiresAt, //60分钟有效期
        var token = new JwtSecurityToken(
            issuer: jwtTokenOptions.Isuser,
            audience: jwtTokenOptions.Audience,
            claims: claims,
            notBefore: DateTime.Now, //立即生效  DateTime.Now.AddMilliseconds(30),//30s后有效
            signingCredentials: creds);

        string returnToken = new JwtSecurityTokenHandler().WriteToken(token);

        //存到redis当中
        await _redis.SetAsync(dto.Id.ToString(), returnToken, TimeSpan.FromMinutes(60));
        return returnToken;
    }

    /// <summary>
    /// 每次登录后刷新token有效时间。
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<bool> RefreshToken(string id)
    {
        if(_redis.TryGet(id,out var value))
        {
            await _redis.SetAsync(id, value, TimeSpan.FromMinutes(60));
            return true;
        }
        return false;
    }

    

    /// <summary>
    /// 解析token，拿到权限信息。
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<List<string>> AnalysisToken(string token)
    {
        try
        {
            //校验token
            var claimsPrincipal = await VerifyToken(token);

            //拿到claims
            var jwtSecurityToken = claimsPrincipal.ReadJwtToken(token);
            //拿到权限列表
            var roleString = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var roleList = roleString!.Split(",").ToList();
            return roleList;
        }
        catch (Exception e)
        {
            _logger.LogError($"AnalysisToken occur exception:{e.Message}");
            throw;
        }
    }

    /// <summary>
    /// 校验token 的有效性
    /// </summary>
    /// <param name="token"></param>
    public async Task<JwtSecurityTokenHandler> VerifyToken(string token)
    {
        var jwtTokenOptions = _configuration.GetSection("JWTTokenOptions").Get<JWTTokenOptions>();
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenOptions!.SecurityKey));
        //校验token  ValidateLifetime = true,ClockSkew = TimeSpan.Zero //校验过期时间必须加此属性
        var validateParameter = new TokenValidationParameters()
        {
            ValidateLifetime = false,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtTokenOptions.Isuser,
            ValidAudience = jwtTokenOptions.Audience,
            IssuerSigningKey = securityKey,
        };

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            //校验是否合法
            tokenHandler.ValidateToken(token, validateParameter, out SecurityToken _);
            return tokenHandler;
        }
        catch (Exception e)
        {
            _logger.LogError("Token invalid.");
            throw;
        }
    }
}