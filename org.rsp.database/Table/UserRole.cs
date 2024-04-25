namespace org.rsp.database.Table;

public class UserRole : BaseTable
{
    public int Id { get; set; }
    
    /// <summary>
    /// 用户id
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// 角色id
    /// </summary>
    public int RoleId { get; set; }
    
}