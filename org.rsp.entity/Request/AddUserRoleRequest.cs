namespace org.rsp.entity.Request;

public class AddUserRoleRequest
{
    public int UserId { get; set; }
    
    public int[] roleList { get; set; }
    
    public string CreateBy { get; set; }
}