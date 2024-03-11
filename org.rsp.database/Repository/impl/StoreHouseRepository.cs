using org.rsp.database.DbContext;
using org.rsp.database.Table;

namespace org.rsp.database.Repository.impl;

public class StoreHouseRepository: RepositoryBase<StoreHouse>,IStoreHouseRepository
{
    public StoreHouseRepository(InventoryDbContext context) : base(context)
    {
    }
}