using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NPOI.SS.Formula.Functions;
using org.rsp.database.DbContext;
using org.rsp.database.Repository;
using org.rsp.database.Repository.impl;

namespace org.rsp.management.Wrapper.impl;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly InventoryDbContext _context;

    private IGoodsCategoryRepository _goodsCategoryRepository;
    private IGoodsRepository _goodsRepository;

    private IStoreHouseRepository _storeHouseRepository;
    private IRecordRepository _recordRepository;

    
    private  IRoleRepository _roleRepository;
    private  IPermissionRepository _permissionRepository;
    private  IUserRoleRepository _userRoleRepository;
    private  IUserRepository _userRepository;
    private  IRolePermissionRepository _rolePermissionRepository;
    
    public IRoleRepository Role
    {
        get { return _roleRepository ??= new RoleRepository(_context); }
    }
    
    public IRolePermissionRepository RolePermission
    {
        get { return _rolePermissionRepository ??= new RolePermissionRepository(_context); }
    }
    
    
    public IPermissionRepository Permission
    {
        get { return _permissionRepository ??= new PermissionRepository(_context); }
    }
    
    public IUserRepository User
    {
        get { return _userRepository ??= new UserRepository(_context); }
    }
    
    public IUserRoleRepository UserRole
    {
        get { return _userRoleRepository ??= new UserRoleRepository(_context); }
    }
    
    public IGoodsCategoryRepository GoodsCategoryRepository
    {
        get { return _goodsCategoryRepository ??= new GoodsCategoryRepository(_context); }
    }

    public IRecordRepository RecordRepository
    {
        get { return _recordRepository ??= new RecordRepository(_context); }
    }

    public IStoreHouseRepository StoreHouseRepository
    {
        get { return _storeHouseRepository ??= new StoreHouseRepository(_context); }
    }

    public IGoodsRepository GoodsRepository
    {
        get { return _goodsRepository ??= new GoodsRepository(_context); }
    }

    public RepositoryWrapper(InventoryDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangeAsync()
    {
        return _context.SaveChangesAsync();
    }

    public Task<IDbContextTransaction> StartTransactionAsync()
    {
        return _context.Database.BeginTransactionAsync();
    }

    public Task ExecuteSqlRaw(string sql,params object[] parameter)
    {
        return _context.Database.ExecuteSqlRawAsync(sql,parameter);
    }
}