namespace org.rsp.entity.Request;

public class UpdateGoodsCategoryRequest
{
    public int Id { get; set; }
    
    public string Goods_Name { get; set; }
    
    public string Description { get; set; }
    
    public DateTime UpdateTime { get; set; }
    
    public string UpdateBy { get; set; }
}