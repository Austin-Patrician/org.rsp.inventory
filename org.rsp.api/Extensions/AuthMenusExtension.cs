using org.rsp.entity.Common;
using org.rsp.entity.Response;

namespace org.rsp.api.Extensions;

public class AuthMenusExtension
{
    private readonly IConfiguration _configuration;

    public AuthMenusExtension(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// 1.从配置文件里面读取，然后用户登录的时候拿到一个role名字集合，然后将其所有可用的page去重，返回给前端页面
    /// </summary>
    public void TryGetPage(List<string> roleName, ref LoginResponse response)
    {
        if (!roleName.Any())
        {
            return;
        }

        Dictionary<string, string[]> authMenus = new();
        var authMenusList = _configuration.GetSection("AuthMenusList").Get<List<AuthMenus>>();
        if (authMenusList != null && authMenusList.Any())
        {
            authMenus = authMenusList.ToDictionary(_ => _.Role, _ => _.Page);
        }

        //所有可以access的page
        HashSet<string> pageList = new();
        foreach (var role in roleName)
        {
            if (authMenus.TryGetValue(role, out var page))
            {
                foreach (var temp in page)
                {
                    pageList.Add(temp);
                }
            }
        }

        response.menus = pageList.ToArray();
        
        //当其只有一个role且是保洁的时候，
        if (roleName.Count == 1 && roleName.FirstOrDefault() == "Cleaner")
        {
            response.authList = ["export"];
        }
        else
        {
            response.authList = ["import", "export"];
        }
    }
}