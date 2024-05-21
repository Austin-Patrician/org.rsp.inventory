using System.ComponentModel.DataAnnotations;

namespace org.rsp.entity.Request;

public class UpdateStoreHouseRequest
{
    public int StoreHouseId { get; set; }
    
    public string? StoreHouseName { get; set; }
    
    public string? Description { get; set; }
    
    public string? Location { get; set; }
    
    public string? Remark { get; set; }
    
    [Required]
    public string UpdateBy { get; set; }
    
}