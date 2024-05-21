using System.ComponentModel.DataAnnotations;

namespace org.rsp.entity.Request;

public class UpdateGoodsCategoryRequest
{
    [Required]
    public int GoodCategoryId { get; set; }
    
    public string? GoodsCategoryName { get; set; }
    
    public string? Description { get; set; }
    
    public string? Remark { get; set; }
    
    [Required]
    public string UpdateBy { get; set; }
    
}