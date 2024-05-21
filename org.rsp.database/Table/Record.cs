using System.ComponentModel.DataAnnotations;

namespace org.rsp.database.Table;

public class Record
{
    public int RecordId { get; set; }
    
    //出库/进库
    [Required]
    public byte Direction { get; set; }
    
    //产品名称
    [Required]
    public int GoodsId { get; set; }
    
    //数量
    [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than or equal to 0")]
    public double Quantity { get; set; }
    
    //入库到哪个仓库/从哪个仓库出库
    public int StoreHouseId { get; set; }
    
    //入库/出库时间
    public DateTime TradeTime { get; set; }
    
    //用途：出库==》出库去向  入库==> 购买目的
    public string Use { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public string? CreateBy { get; set; }
    
    //外键
    public virtual Goods Goods { get; set; }
    public virtual StoreHouse StoreHouse { get; set; }
}