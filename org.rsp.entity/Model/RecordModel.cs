
namespace org.rsp.entity.Model;

public class RecordModel
{
    public int RecordId { get; set; }
    
    //出库/进库
    public byte Direction { get; set; }
    
    public string DirectionName { get; set; }
    
    //产品名称
    public int GoodsId { get; set; }
    
    public string GoodsName { get; set; }
    
    //数量
    public double Quantity { get; set; }
    
    //入库到哪个仓库/从哪个仓库出库
    public int StoreHouseId { get; set; }
    
    public string StoreHouseName { get; set; }
    
    //入库/出库时间
    public DateTime TradeTime { get; set; }
    
    //用途：出库==》出库去向  入库==> 购买目的
    public string Use { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public string? CreateBy { get; set; }
    
}