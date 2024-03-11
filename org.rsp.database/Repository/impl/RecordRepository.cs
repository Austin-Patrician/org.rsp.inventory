using org.rsp.database.DbContext;
using org.rsp.database.Table;

namespace org.rsp.database.Repository.impl;

public class RecordRepository : RepositoryBase<Record>,IRecordRepository   
{
    public RecordRepository(InventoryDbContext context) : base(context)
    {
    }
}