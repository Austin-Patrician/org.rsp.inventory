namespace org.rsp.entity.Response;

public class LoginResponse
{
    public string token { get; set; }

    public string[] menus { get; set; }
    
    public string userName { get; set; }
    
    public string[] authList { get; set; }
}