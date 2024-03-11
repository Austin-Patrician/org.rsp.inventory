namespace org.rsp.database.Table;

public class GoodsCategory : BaseTable
{
    public int GoodsCategoryId { get; set; }
    
    public string GoodsCategoryName { get; set; }
    
    public string? Description { get; set; }
    
    //image save address.
    public string? ImageCode { get; set; }
}