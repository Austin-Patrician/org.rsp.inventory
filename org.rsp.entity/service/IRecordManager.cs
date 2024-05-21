using org.rsp.database.Table;
using org.rsp.entity.Model;
using org.rsp.entity.Request;
using org.rsp.entity.Response;

namespace org.rsp.entity.service;

public interface IRecordManager
{
    Task<QueryRecordResponse> QueryRecordAsync(QueryRecordRequest request);
    Task<AddRecordResponse> AddRecordAsync(AddWareHouseRecordRequest request);

    Task BatchDelRecordAsync(List<int> ids);

    Task<byte[]> ExportRecord();

    Task UpdateRecordAsync(UpdateRecordRequest request);

}