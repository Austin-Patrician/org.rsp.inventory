using System.ComponentModel.DataAnnotations;
using org.rsp.entity.Common;

namespace org.rsp.entity.Request;

public class AddRoleRequest : BaseFiled
{
    //新增角色名称
    [Required]
    public string Name { get; set; }
}