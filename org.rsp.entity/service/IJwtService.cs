using System.IdentityModel.Tokens.Jwt;
using org.rsp.entity.Model;

namespace org.rsp.entity.service;

public interface IJwtService
{
    Task<string> GetToken(UserModel userModel);

    Task<List<string>> AnalysisToken(string token);

    Task<JwtSecurityTokenHandler> VerifyToken(string token);

    Task<bool> RefreshToken(string id);
}