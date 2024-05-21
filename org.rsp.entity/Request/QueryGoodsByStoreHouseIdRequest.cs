using org.rsp.entity.Common;

namespace org.rsp.entity.Request;

public class QueryGoodsByStoreHouseIdRequest : Pager
{
    public int? storeHouseId { get; set; }
}