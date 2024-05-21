namespace org.rsp.entity.Common;

public struct Auth
{
    public string RoleName { get; set; }
    
    public string[] Page{ get; set; }
}


public static class AuthMenu
{
    public static readonly Dictionary<string,string[]> Auths =new ()
    {
        {"Cleaner", ["/","/goods/","/record","/record/","/goods"] },
        {"Administrator", ["/","/storeHouse","/user","/role","/goodsCategory","/goods","/record","/user/","/goods/","/record/"] },
        {"Regular",["/","/storeHouse","/goodsCategory","/goods","/record","/goods/","/record/"]}
    };


    public static List<string> TryGetPage(List<string> roleList)
    {
        if (!roleList.Any())
        {
            return null;
        }

        var tempPage = new List<string>();
        foreach (var role in roleList)
        {
            if (AuthMenu.Auths.TryGetValue(role, out var page))
            {
                tempPage.AddRange(page);
            }
        }
        
        return tempPage.Distinct().ToList();
    }
}