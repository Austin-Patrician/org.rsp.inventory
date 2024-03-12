using System.ComponentModel.DataAnnotations;

namespace org.rsp.entity.Request;

public class UpdateGoodsCategoryRequest
{
    public int GoodCategoryId { get; set; }
    
    public string? GoodsCategoryName { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime UpdateTime { get; set; }
    
    [Required]
    public string UpdateBy { get; set; }
    
}