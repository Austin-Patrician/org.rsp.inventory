using org.rsp.entity.Common;

namespace org.rsp.entity.Model;

public class RolePermissionModel : BaseFiled
{
    public int? Id { get; set; }
    
    /// <summary>
    /// 角色id
    /// </summary>
    public int RoleId { get; set; }
    
    /// <summary>
    /// 权限id
    /// </summary>
    public int PermissionId { get; set; }
}