using org.rsp.database.Table;

namespace org.rsp.entity.Response;

public class QueryStoreHouseResponse
{
    public List<StoreHouse> StoreHouses { get; set; }
    
    public int TotalCount { get; set; }
}