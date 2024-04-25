using org.rsp.entity.Common;

namespace org.rsp.entity.Model;

public class RoleModel : BaseFiled
{
    public int Id { get; set; }
    
    /// <summary>
    /// 角色名称
    /// </summary>
    public string Name { get; set; }
}