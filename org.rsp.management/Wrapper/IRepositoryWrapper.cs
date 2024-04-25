using Microsoft.EntityFrameworkCore.Storage;
using org.rsp.database.DbContext;
using org.rsp.database.Repository;

namespace org.rsp.management.Wrapper;

public interface IRepositoryWrapper
{
    IGoodsCategoryRepository GoodsCategoryRepository { get; }
    IGoodsRepository GoodsRepository { get; }
    IStoreHouseRepository StoreHouseRepository { get; }
    IRecordRepository RecordRepository { get; }
    
    
    IRoleRepository Role { get; }
    IPermissionRepository Permission { get; }
    IUserRoleRepository UserRole { get; }
    
    IUserRepository User { get; }
    
    IRolePermissionRepository RolePermission { get; }
    
    
    Task<int> SaveChangeAsync();

    Task<IDbContextTransaction> StartTransactionAsync();

    Task ExecuteSqlRaw(string sql,params object[] parameter);
}