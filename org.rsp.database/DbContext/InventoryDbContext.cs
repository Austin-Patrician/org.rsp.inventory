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
    //权限控制
    public DbSet<Role> Role { get; set; }
    public DbSet<User> User { get; set; }
    //public DbSet<Permission> Permission { get; set; }
    public DbSet<UserRole> UserRole { get; set; }
    public DbSet<RolePermission> RolePermission { get; set; }
    public InventoryDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //sql语句加全局过滤
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}