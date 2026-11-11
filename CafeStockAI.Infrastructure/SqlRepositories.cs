using CafeStockAI.Application;
using Microsoft.EntityFrameworkCore;

namespace CafeStockAI.Infrastructure;
public sealed class SqlStockReadRepository(CafeStockDbContext context) : IStockReadRepository
{
    public async Task<IReadOnlyList<AreaBalance>> ReadBalancesAsync(IReadOnlyCollection<Guid> authorizedBranchIds, CancellationToken cancellationToken = default)
    {
        if (authorizedBranchIds.Count == 0)
            return Array.Empty<AreaBalance>();
        return await (
            from movement in context.StockMovements.AsNoTracking()join area in context.StockAreas.AsNoTracking()on movement.AreaId equals area.Id
            where authorizedBranchIds.Contains(area.BranchId)group movement by new
            {
                movement.ProductId,
                movement.AreaId
            }

                into rows
                select new AreaBalance(rows.Key.ProductId, rows.Key.AreaId, rows.Sum(x => x.Quantity))).ToListAsync(cancellationToken);
    }
}

public sealed class SqlDatabaseProbe(CafeStockDbContext context) : IDatabaseProbe
{
    public Task<bool> CanConnectAsync(CancellationToken cancellationToken = default) => context.Database.CanConnectAsync(cancellationToken);
}
