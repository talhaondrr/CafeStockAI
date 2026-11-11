using CafeStockAI.Application.Models;

namespace CafeStockAI.Application;
public sealed record InventoryArchive(InventoryData Inventory, List<Product> Products, List<UserAccount> Accounts, Dictionary<Guid, Guid>? LocationAliases = null);
public interface IInventoryPersistence
{
    InventoryData LoadInventory();
    void SaveInventory(InventoryData data);
    List<Product> LoadProducts();
    void SaveProducts(List<Product> products);
    List<UserAccount> LoadAccounts();
    void SaveAccounts(List<UserAccount> accounts);
    InventoryArchive Export();
    void Restore(InventoryArchive archive);
}

public static class SingleBranchImport
{
    public static InventoryArchive Prepare(InventoryArchive source)
    {
        // Work on a deep copy; source JSON and other branches remain in the pre-import backup.
        var copy = System.Text.Json.JsonSerializer.Deserialize<InventoryArchive>(System.Text.Json.JsonSerializer.Serialize(source)) ?? throw new InvalidDataException("Aktarım kaynağı okunamadı.");
        var data = copy.Inventory;
        var branch = data.Branches.SingleOrDefault(b => b.Name == "Elazığ") ?? new Branch
        {
            Name = "Elazığ",
            Code = "ELZ"
        };
        var locations = data.Warehouses.Where(w => w.BranchName == "Elazığ").ToList();
        string Area(Warehouse w) => w.StockArea == "Bar" || (string.IsNullOrEmpty(w.StockArea) && w.Name.Equals("Bar", StringComparison.OrdinalIgnoreCase)) ? "Bar" : "Depo";
        var canonical = new Dictionary<string, Warehouse>();
        foreach (string area in new[]
        {
            "Bar",
            "Depo"
        }

        )
        {
            var w = locations.FirstOrDefault(w => Area(w) == area) ?? new Warehouse
            {
                Id = Guid.NewGuid()
            };
            w.StockArea = area;
            w.BranchName = "Elazığ";
            w.Name = "Elazığ / " + area;
            canonical[area] = w;
        }

        var map = locations.ToDictionary(w => w.Id, w => canonical[Area(w)].Id);
        var kept = locations.Select(w => w.Id).ToHashSet();
        data.Counts = data.Counts.Where(c => c.BranchName == "Elazığ" || kept.Contains(c.WarehouseId)).ToList();
        foreach (var c in data.Counts)
        {
            c.BranchName = "Elazığ";
            c.WarehouseId = map.GetValueOrDefault(c.WarehouseId, canonical["Depo"].Id);
        }

        data.Movements = data.Movements.Where(m => kept.Contains(m.WarehouseId)).ToList();
        foreach (var movement in data.Movements)
            movement.WarehouseId = map[movement.WarehouseId];
        data.Warehouses = canonical.Values.ToList();
        data.Branches = new()
        {
            branch
        };
        data.BranchRegistryInitialized = true;
        foreach (var person in data.Employees)
        {
            person.Branches = new()
            {
                "Elazığ"
            };
            person.BranchName = "Elazığ";
        }

        var employees = data.Employees.Select(e => e.Id).ToHashSet();
        return copy with
        {
            LocationAliases = map,
            Accounts = copy.Accounts.Where(a => employees.Contains(a.EmployeeId)).ToList()
        };
    }
}
