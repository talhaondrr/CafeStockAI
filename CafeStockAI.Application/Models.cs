using System.Text.Json;
namespace CafeStockAI.Application.Models;
public sealed class Warehouse
{
    public string StockArea { get; set; } = "";
    public string BranchName { get; set; } = "";
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Location { get; set; } = "";
}
public sealed class Employee
{
    public bool IsActive { get; set; } = true;
    public DateTime? HireDate { get; set; }
    [System.Text.Json.Serialization.JsonIgnore] public string HireDateDisplay => HireDate?.ToString("dd.MM.yyyy") ?? "Belirtilmedi";
    public List<string> Branches { get; set; } = new();
    [System.Text.Json.Serialization.JsonIgnore]
    public List<string> AssignedBranches => Branches is { Count: > 0 } ? Branches : string.IsNullOrWhiteSpace(BranchName) ? new List<string>() : new List<string> { BranchName };
    [System.Text.Json.Serialization.JsonIgnore]
    public string BranchesDisplay => string.Join(", ",AssignedBranches);
    public string BranchName { get; set; } = "";
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Role { get; set; } = "";
    public string Phone { get; set; } = "";
}
public sealed class StockMovement
{
    public Guid? ActorId { get; set; }
    public Guid? CountId { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public string Type { get; set; } = "";
    public decimal Quantity { get; set; }
    public string Note { get; set; } = "";
}
public sealed class StockCount
{
    public string Status { get; set; } = "Onaylandı";
    public string Reviewer { get; set; } = "";
    public string ReviewNote { get; set; } = "";
    public DateTime? ReviewedAt { get; set; }
    public string BranchName { get; set; } = "";
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid WarehouseId { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public string Operator { get; set; } = "";
    public List<CountLine> Lines { get; set; } = new();
}
public sealed class CountLine
{
    public decimal? BarExpected { get; set; }
    public decimal? BarCounted { get; set; }
    public decimal? DepotExpected { get; set; }
    public decimal? DepotCounted { get; set; }
    public Guid ProductId { get; set; }
    public decimal Expected { get; set; }
    public decimal Counted { get; set; }
}
public sealed class AppSettings
{
    public string BusinessName { get; set; } = "CafeStock AI";
    public string OperatorName { get; set; } = "";
}
public sealed class InventoryData
{
    [System.Text.Json.Serialization.JsonIgnore] public byte[] SqlVersion { get; set; } = Array.Empty<byte>();
    public List<Branch> Branches { get; set; } = new();
    public bool BranchRegistryInitialized { get; set; }
    public List<Warehouse> Warehouses { get; set; } = new();
    public List<Employee> Employees { get; set; } = new();
    public List<StockMovement> Movements { get; set; } = new();
    public List<StockCount> Counts { get; set; } = new();
    public AppSettings Settings { get; set; } = new();
    public decimal Balance(Guid product, Guid warehouse) => Movements.Where(m => m.ProductId == product && m.WarehouseId == warehouse).Sum(m => m.Quantity);
}

public sealed class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Unit { get; set; } = "Adet";
    public decimal MinimumStock { get; set; }
    public string SourceCell { get; set; } = "";
    [System.Text.Json.Serialization.JsonIgnore]
    public string DisplayName => string.IsNullOrEmpty(Category) ? Name : $"{Category} / {Name}";
    [System.Text.Json.Serialization.JsonIgnore]
    public string UnitDisplay => string.IsNullOrWhiteSpace(Unit) ? "Birim seçiniz" : Unit;
}

public sealed class Branch
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
}

public sealed class UserAccount
{
 public Guid EmployeeId { get; set; }
 public string Username { get; set; } = "";
 public string Salt { get; set; } = "";
 public string Hash { get; set; } = "";
 public bool Enabled { get; set; } = true;
}
