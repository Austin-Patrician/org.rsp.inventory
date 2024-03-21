namespace org.rsp.database.Table;

public class User : BaseTable
{
    public int UserId { get; set; }
    
    public string UserName { get; set; }
    public string PassWord { get; set; }
    
    public virtual IEnumerable<Role> Roles { get; set; }
}