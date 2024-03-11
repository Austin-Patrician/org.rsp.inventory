using org.rsp.database.DbContext;
using org.rsp.database.Table;

namespace org.rsp.database.Repository.impl;

public class GoodsCategoryRepository : RepositoryBase<GoodsCategory>,IGoodsCategoryRepository
{
    public GoodsCategoryRepository(InventoryDbContext context) : base(context)
    {
    }
}