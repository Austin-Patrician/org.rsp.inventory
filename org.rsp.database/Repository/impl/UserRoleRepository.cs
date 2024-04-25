using org.rsp.database.DbContext;
using org.rsp.database.Table;

namespace org.rsp.database.Repository.impl;

public class UserRoleRepository : RepositoryBase<UserRole>,IUserRoleRepository
{
    public UserRoleRepository(InventoryDbContext context) : base(context)
    {
    }
}