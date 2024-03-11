namespace org.rsp.entity.Request;

public class AddGoodsRequest
{
    public string GoodsName { get; set; }
    
    //数量
    public double Number { get; set; }
    
    //哪个物品种类
    public int GoodsCategoryId { get; set; }
    
    //哪个仓库
    public int StoreHouseId { get; set; }
    
    //单价
    public double Price { get; set; }
    
    //描述
    public string Description { get; set; }
    
    //CreateBy
    
    public string CreateBy { get; set; }
}