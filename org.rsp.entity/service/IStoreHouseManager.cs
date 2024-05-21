using org.rsp.database.Table;
using org.rsp.entity.Request;
using org.rsp.entity.Response;

namespace org.rsp.entity.service;

public interface IStoreHouseManager
{
    Task AddStoreHouseAsync(AddStoreHouseRequest request);

    Task UpdateStoreHouseAsync(UpdateStoreHouseRequest request);

    Task BatchDeleteStoreHouseAsync(List<int> ids);

    Task<QueryStoreHouseResponse> QueryStoreHouseAsync(QueryStoreHouseRequest request);
}