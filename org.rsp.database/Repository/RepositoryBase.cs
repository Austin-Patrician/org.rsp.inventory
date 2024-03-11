using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using org.rsp.database.DbContext;

namespace org.rsp.database.Repository;

public class RepositoryBase<T> : IRepositoryBase<T> where T: class
{
    private InventoryDbContext _context;

    public RepositoryBase(InventoryDbContext context)
    {
        _context = context;
    }

    public IQueryable<T> FindAll()
    {
        return _context.Set<T>().AsNoTracking();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression).AsNoTracking();
    }
    
    public IQueryable<T> ExecuteSql(string sql)
    {
        return _context.Set<T>().FromSqlRaw(sql);
    }

    public void Create(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}