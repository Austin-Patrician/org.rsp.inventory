using org.rsp.database.DbContext;
using org.rsp.database.Table;

namespace org.rsp.database.Repository.impl;

public class RolePermissionRepository : RepositoryBase<RolePermission>,IRolePermissionRepository
{
    public RolePermissionRepository(InventoryDbContext context) : base(context)
    {
    }
}