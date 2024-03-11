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