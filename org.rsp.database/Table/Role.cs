namespace org.rsp.database.Table;

public class Role : BaseTable
{
    public int RoleId { get; set; }
    
    public string RoleName { get; set; }
    public string UserId { get; set; }
    
    public virtual User User { get; set; }
}