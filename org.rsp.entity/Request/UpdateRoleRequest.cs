using System.ComponentModel.DataAnnotations;

namespace org.rsp.entity.Request;

public class UpdateRoleRequest
{
    public string? Name { get; set; }
    
    [Required]
    public int Id { get; set; }
    
    public string? Remark { get; set; }
    
    [Required]
    public string UpdateBy { get; set; }
}