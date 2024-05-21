using System.ComponentModel.DataAnnotations;

namespace org.rsp.entity.Request;

public class UpdateRecordRequest
{
    [Required]
    public int RecordId { get; set; }
    public string? Use { get; set; }
    
    [Required]
    public string UpdateBy { get; set; }
}