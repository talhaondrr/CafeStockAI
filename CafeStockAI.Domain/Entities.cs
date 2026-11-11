namespace CafeStockAI.Domain;
public abstract class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}

public sealed class Role : Entity
{
    public string Name { get; set; } = "";
}

public sealed class Employee : Entity
{
    public string Name { get; set; } = "";
    public string Phone { get; set; } = "";
    public DateTime? HireDate { get; set; }
    public Guid RoleId { get; set; }
    public bool IsActive { get; set; } = true;
}

public sealed class UserAccount
{
    public Guid EmployeeId { get; set; }
    public string Username { get; set; } = "";
    public string UsernameKey { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string PasswordSalt { get; set; } = "";
    public bool IsEnabled { get; set; } = true;
}

public sealed class Branch : Entity
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public bool IsActive { get; set; } = true;
}

public sealed class EmployeeBranch
{
    public Guid EmployeeId { get; set; }
    public Guid BranchId { get; set; }
}

public sealed class Category : Entity
{
    public string Name { get; set; } = "";
}

public sealed class Unit : Entity
{
    public string Name { get; set; } = "";
}

public sealed class Product : Entity
{
    private Product()
    {
    }

    public Product(string name, Guid categoryId, Guid unitId, decimal minimumStock) => ChangeDetails(name, categoryId, unitId, minimumStock);
    public string Name { get; private set; } = "";
    public Guid CategoryId { get; private set; }
    public Guid UnitId { get; private set; }
    public decimal MinimumStock { get; private set; }
    public string Code { get; set; } = "";
    public string SourceCell { get; set; } = "";
    public string ImageKey { get; set; } = "";
    public bool IsActive { get; set; } = true;

    public void ChangeDetails(string name, Guid categoryId, Guid unitId, decimal minimumStock)
    {
        if (string.IsNullOrWhiteSpace(name) || categoryId == Guid.Empty || unitId == Guid.Empty || minimumStock < 0)
            throw new ArgumentException("Ürün adı, kategori, birim ve negatif olmayan stok eşiği gereklidir.");
        Name = name.Trim();
        CategoryId = categoryId;
        UnitId = unitId;
        MinimumStock = minimumStock;
    }
}

public enum StockAreaKind
{
    Bar = 1,
    Depot = 2
}

public sealed class StockArea : Entity
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Location { get; set; } = "";
    public Guid BranchId { get; set; }
    public StockAreaKind Area { get; set; }
}

public enum CountStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3
}

public sealed class StockCount : Entity
{
    public Guid SourceWarehouseId { get; set; }
    public string ReviewedName { get; set; } = "";
    public Guid BranchId { get; set; }
    public Guid? SubmittedById { get; set; }
    public string SubmittedName { get; set; } = "";
    public DateTime SubmittedAt { get; set; }
    public CountStatus Status { get; private set; } = CountStatus.Pending;
    public Guid? ReviewedById { get; private set; }
    public DateTime? ReviewedAt { get; private set; }
    public string ReviewNote { get; private set; } = "";
    public bool IsDemo { get; set; }
    public byte[] Version { get; set; } = Array.Empty<byte>();

    public static StockCount FromHistory(Guid id, Guid branchId, Guid warehouseId, string submitter, DateTime date, CountStatus status, string reviewer, DateTime? reviewedAt, string note, Guid? submitterId = null, Guid? reviewerId = null)
    {
        if (!Enum.IsDefined(status))
            throw new ArgumentException("Geçersiz sayım durumu.");
        return new StockCount
        {
            Id = id,
            BranchId = branchId,
            SourceWarehouseId = warehouseId,
            SubmittedById = submitterId,
            ReviewedById = reviewerId,
            SubmittedName = submitter,
            SubmittedAt = date,
            Status = status,
            ReviewedName = reviewer,
            ReviewedAt = reviewedAt,
            ReviewNote = note
        };
    }

    public void Review(Guid reviewer, bool approved, string note, DateTime now)
    {
        if (Status != CountStatus.Pending)
            throw new InvalidOperationException("Sayım zaten sonuçlandırılmış.");
        if (reviewer == Guid.Empty || (!approved && string.IsNullOrWhiteSpace(note)))
            throw new ArgumentException("İnceleyen kişi ve ret gerekçesi gereklidir.");
        Status = approved ? CountStatus.Approved : CountStatus.Rejected;
        ReviewedById = reviewer;
        ReviewNote = note.Trim();
        ReviewedAt = now;
    }
}

public sealed class StockCountLine
{
    public Guid CountId { get; set; }
    public Guid ProductId { get; set; }
    public decimal? ExpectedBar { get; set; }
    public decimal? CountedBar { get; set; }
    public decimal? ExpectedDepot { get; set; }
    public decimal? CountedDepot { get; set; }
    public decimal Expected { get; set; }
    public decimal Counted { get; set; }
}

public sealed class Transfer : Entity
{
    public Guid SourceAreaId { get; set; }
    public Guid TargetAreaId { get; set; }
    public Guid? CreatedById { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Note { get; set; } = "";
    public bool IsDemo { get; set; }
}

public sealed class TransferLine
{
    public Guid TransferId { get; set; }
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
}

public sealed class StockMovement : Entity
{
    public Guid ProductId { get; set; }
    public Guid AreaId { get; set; }
    public DateTime OccurredAt { get; set; }
    public string Type { get; set; } = "";
    public decimal Quantity { get; set; }
    public string Note { get; set; } = "";
    public Guid? ActorId { get; set; }
    public Guid? CountId { get; set; }
    public Guid? TransferId { get; set; }
    public bool IsDemo { get; set; }
}

public sealed class BusinessSettings : Entity
{
    public string OperatorName { get; set; } = "";
    public byte[] Version { get; set; } = Array.Empty<byte>();
    public string BusinessName { get; set; } = "";
}

public sealed class ImportHistory : Entity
{
    public string SourceFingerprint { get; set; } = "";
    public DateTime AppliedAt { get; set; }
    public int ProductCount { get; set; }
    public int EmployeeCount { get; set; }
    public int MovementCount { get; set; }
    public int CountCount { get; set; }
}

public sealed class ImportedLocation
{
    public Guid ImportId { get; set; }
    public Guid SourceWarehouseId { get; set; }
    public Guid AreaId { get; set; }
}
