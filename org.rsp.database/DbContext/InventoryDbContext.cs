using System.Reflection;
using Microsoft.EntityFrameworkCore;
using org.rsp.database.Table;

namespace org.rsp.database.DbContext;

public class InventoryDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    //set the table using DbSet<Table>
    public DbSet<GoodsCategory> GoodsCategories { get; set; }
    public DbSet<Goods> Goods { get; set; }
    public DbSet<StoreHouse> StoreHouses { get; set; }
    public DbSet<Record> Records { get; set; }
    public DbSet<WareHouseRecord> WareHouseRecord { get; set; }
    
    public InventoryDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //sql语句加全局过滤
        modelBuilder.Entity<GoodsCategory>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Goods>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<StoreHouse>().HasQueryFilter(e => !e.IsDeleted);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}