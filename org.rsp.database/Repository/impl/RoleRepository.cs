using org.rsp.database.DbContext;
using org.rsp.database.Table;

namespace org.rsp.database.Repository.impl;

public class RoleRepository :  RepositoryBase<Role>,IRoleRepository
{
    public RoleRepository(InventoryDbContext context) : base(context)
    {
    }
}