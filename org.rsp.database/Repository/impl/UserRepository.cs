using org.rsp.database.DbContext;
using org.rsp.database.Table;

namespace org.rsp.database.Repository.impl;

public class UserRepository: RepositoryBase<User>,IUserRepository
{
    public UserRepository(InventoryDbContext context) : base(context)
    {
    }
}