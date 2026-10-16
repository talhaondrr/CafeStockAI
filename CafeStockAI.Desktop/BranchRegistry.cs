namespace CafeStockAI.Desktop;
public sealed class Branch
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
}
internal static class BranchRegistry
{
    public static void Initialize(InventoryData data)
    {
        if (data.BranchRegistryInitialized) return;
        data.Branches ??= new();
        var names = new[] { "İnegöl", "Ankara", "Bursa", "Elazığ" }.Concat(data.Warehouses.Select(w => w.BranchName)).Concat(data.Employees.SelectMany(p => p.AssignedBranches)).Concat(data.Counts.Select(c => c.BranchName));
        foreach (string name in names.Where(n => !string.IsNullOrWhiteSpace(n)).Select(n => n.Trim()).Distinct(StringComparer.CurrentCultureIgnoreCase))
        {
            if (data.Branches.Any(b => string.Equals(b.Name,name,StringComparison.CurrentCultureIgnoreCase))) continue;
            var hash = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes("cafestock-branch:" + name));
            int suffix = data.Branches.Count+1;
            while (data.Branches.Any(b => b.Code == $"SUB-{suffix:000}")) suffix++;
            data.Branches.Add(new Branch { Id = new Guid(hash.AsSpan(0,16)), Code = $"SUB-{suffix:000}", Name = name });
        }
        data.BranchRegistryInitialized = true;
    }
    public static void SaveBranch(Guid? id,string code,string name,string address)
    {
        Access.RequireGlobal();
        using var transaction = DataLock.Enter();
        var data = InventoryStore.Load(); code = code.Trim(); name = name.Trim();
        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name)) throw new InvalidOperationException("Şube kodu ve adı zorunludur.");
        if (data.Branches.Any(b => b.Id != id && (string.Equals(b.Code,code,StringComparison.OrdinalIgnoreCase) || string.Equals(b.Name,name,StringComparison.CurrentCultureIgnoreCase)))) throw new InvalidOperationException("Bu şube kodu veya adı zaten kullanılıyor.");
        var existing = id.HasValue ? data.Branches.FirstOrDefault(b => b.Id == id.Value) ?? throw new InvalidOperationException("Şube artık bulunamıyor. Listeyi yenileyin.") : null;
        if (existing is not null && existing.Name != name)
        {
            string previous = existing.Name;
            foreach (var warehouse in data.Warehouses.Where(w => w.BranchName == previous)) { warehouse.BranchName = name; if (warehouse.StockArea is "Bar" or "Depo") warehouse.Name = name + " / " + warehouse.StockArea; }
            foreach (var count in data.Counts.Where(c => c.BranchName == previous)) count.BranchName = name;
            foreach (var employee in data.Employees)
            {
                employee.Branches = employee.AssignedBranches.Select(n => n == previous ? name : n).Distinct().ToList();
                if (employee.BranchName == previous) employee.BranchName = name;
            }
        }
        var row = existing ?? new Branch(); row.Code = code; row.Name = name; row.Address = address.Trim();
        if (existing is null) data.Branches.Add(row);
        InventoryStore.Save(data);
    }
    public static void SaveNamedBranch(Guid? id,string name)
    {
        var data = InventoryStore.Load(); var existing = data.Branches.FirstOrDefault(b => b.Id == id);
        if (id.HasValue && existing is null) throw new InvalidOperationException("Şube artık bulunamıyor.");
        int suffix = 1; while (data.Branches.Any(b => b.Code == $"SUB-{suffix:000}")) suffix++;
        SaveBranch(id,existing?.Code ?? $"SUB-{suffix:000}",name,existing?.Address ?? "");
    }
    public static void Delete(Guid id)
    {
        Access.RequireGlobal();
        using var transaction = DataLock.Enter();
        var data = InventoryStore.Load(); var branch = data.Branches.FirstOrDefault(b => b.Id == id) ?? throw new InvalidOperationException("Şube bulunamadı.");
        var areas = data.Warehouses.Where(w => w.BranchName == branch.Name).Select(w => w.Id).ToHashSet();
        if (data.Employees.Any(p => p.AssignedBranches.Contains(branch.Name)) || data.Counts.Any(c => c.BranchName == branch.Name || areas.Contains(c.WarehouseId)) || data.Movements.Any(m => areas.Contains(m.WarehouseId)))
            throw new InvalidOperationException("Personel ataması veya işlem geçmişi olan şube silinemez.");
        data.Branches.RemoveAll(b => b.Id == id); data.Warehouses.RemoveAll(w => areas.Contains(w.Id)); InventoryStore.Save(data);
    }
}
