using org.rsp.database.DbContext;
using org.rsp.database.Table;

namespace org.rsp.database.Repository.impl;

public class PermissionRepository : RepositoryBase<Permission>,IPermissionRepository
{
    public PermissionRepository(InventoryDbContext context) : base(context)
    {
    }
}