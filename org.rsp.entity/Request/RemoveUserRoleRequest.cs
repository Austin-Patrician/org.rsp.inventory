using System.ComponentModel.DataAnnotations;

namespace org.rsp.entity.Request;

public class RemoveUserRoleRequest
{
    [Required]
    public int UserId { get; set; }
    
    public int[] roleList { get; set; }
    
    [Required]
    public string UpdateBy { get; set; }
}