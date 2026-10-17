using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace CafeStockAI.Infrastructure;
public sealed class CafeStockDesignFactory : IDesignTimeDbContextFactory<CafeStockDbContext>
{
 public CafeStockDbContext CreateDbContext(string[] args)=>new(new DbContextOptionsBuilder<CafeStockDbContext>().UseSqlServer(sql=>sql.MigrationsHistoryTable("__EFMigrationsHistory","cafestock")).Options);
}
