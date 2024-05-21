using System.ComponentModel.DataAnnotations;

namespace org.rsp.entity.Request;

public class UpdateGoodsRequest
{
    [Required]
    public int GoodsId { get; set; }
    
    public double? Price { get; set; }
    
    public string? Remark { get; set; }
    
    public string? Description { get; set; }
    
    [Required]
    public string UpdateBy { get; set; }
    
}