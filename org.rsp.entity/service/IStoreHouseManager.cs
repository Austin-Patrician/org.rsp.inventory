using org.rsp.entity.Request;

namespace org.rsp.entity.service;

public interface IStoreHouseManager
{
    Task AddStoreHouseAsync(AddStoreHouseRequest request);

    Task UpdateStoreHouseAsync(UpdateStoreHouseRequest request);

    Task BatchDeleteStoreHouseAsync(List<int> ids);
}