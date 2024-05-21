using org.rsp.entity.Common;

namespace org.rsp.entity.Model;

public class GoodsModel : BaseFiled
{
    public int GoodsId { get; set; }
    
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
    public string? Description { get; set; }
    
}