namespace org.rsp.database.Table;

public class StoreHouse : BaseTable
{
    public int StoreHouseId { get; set; }
    
    public string StoreHouseName { get; set; }
    
    public string? Description { get; set; }
    
    public string? Location { get; set; }
}