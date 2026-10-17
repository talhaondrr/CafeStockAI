using Microsoft.EntityFrameworkCore;
namespace CafeStockAI.Infrastructure;
public static class DatabaseOptions
{
 public static CafeStockDbContext Create(string connection) => new(new DbContextOptionsBuilder<CafeStockDbContext>().UseSqlServer(connection,sql=>sql.MigrationsHistoryTable("__EFMigrationsHistory","cafestock").CommandTimeout(30)).Options);
}
