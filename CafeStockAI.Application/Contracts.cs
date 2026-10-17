namespace CafeStockAI.Application;
public sealed record AreaBalance(Guid ProductId,Guid AreaId,decimal Quantity);
public interface IStockReadRepository { Task<IReadOnlyList<AreaBalance>> ReadBalancesAsync(IReadOnlyCollection<Guid> authorizedBranchIds,CancellationToken cancellationToken=default); }
public interface IDatabaseProbe { Task<bool> CanConnectAsync(CancellationToken cancellationToken=default); }
