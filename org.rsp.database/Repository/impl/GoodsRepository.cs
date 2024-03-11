using org.rsp.database.DbContext;
using org.rsp.database.Table;

namespace org.rsp.database.Repository.impl;

public class GoodsRepository: RepositoryBase<Goods>,IGoodsRepository
{
    public GoodsRepository(InventoryDbContext context) : base(context)
    {
    }
}