using org.rsp.database.Table;
using org.rsp.entity.Request;

namespace org.rsp.entity.service;

public interface IRecordManager
{
    Task<List<Record>> QueryRecordAsync(QueryRecordConditionRequest request);
    Task AddWareHouseRecordAsync(AddWareHouseRecordRequest request);

    Task DelRecord(List<int> ids);
}