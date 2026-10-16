namespace CafeStockAI.Desktop;
public sealed class BranchCountLine
{
    public Guid ProductId { get; set; }
    public decimal ExpectedBar { get; set; }
    public decimal CountedBar { get; set; }
    public decimal ExpectedDepot { get; set; }
    public decimal CountedDepot { get; set; }
}
internal static class BranchStock
{

    public static string AreaOf(Warehouse warehouse) => warehouse.StockArea == "Bar" || (string.IsNullOrEmpty(warehouse.StockArea) && warehouse.Name.Equals("Bar", StringComparison.CurrentCultureIgnoreCase)) ? "Bar" : "Depo";
    public static decimal Balance(InventoryData data, Guid product, string branch, string area) => data.Warehouses.Where(w => w.BranchName == branch && AreaOf(w) == area).Sum(w => data.Balance(product,w.Id));
    public static List<string> AllNames(InventoryData data) { BranchRegistry.Initialize(data); return data.Branches.Select(b => b.Name).ToList(); }
    internal static Warehouse Area(InventoryData data, string branch, string area)
    {
        var existing = data.Warehouses.FirstOrDefault(w => w.BranchName == branch && AreaOf(w) == area);
        if (existing is not null) return existing;
        var warehouse = new Warehouse { BranchName = branch, StockArea = area, Name = branch + " / " + area, Code = "ALAN-" + Guid.NewGuid().ToString("N")[..10], Location = area };
        data.Warehouses.Add(warehouse); return warehouse;
    }
    public static void Complete(string branch, List<BranchCountLine> lines, string operatorName) => Commit(branch, lines, operatorName, false);
    internal static void Commit(string branch, List<BranchCountLine> lines, string operatorName, bool approval, Guid? pendingId = null, string reviewNote = "")
    {
        Access.RequireBranch(branch);
        using var transaction = DataLock.Enter();
        var data = InventoryStore.Load();
        if (!AllNames(data).Contains(branch)) throw new InvalidOperationException("Geçerli bir şube seçiniz.");
        if (string.IsNullOrWhiteSpace(operatorName)) throw new InvalidOperationException("Sayımı yapan kişiyi giriniz.");
        var products = ProductStore.Load();
        if (!products.Select(p => p.Id).OrderBy(id => id).SequenceEqual(lines.Select(l => l.ProductId).OrderBy(id => id))) throw new InvalidOperationException("Ürün listesi değişti. Sayımı yeniden başlatın.");
        if (lines.Count == 0 || products.Any(p => string.IsNullOrWhiteSpace(p.Unit))) throw new InvalidOperationException("Ürünleri ve eksik birimleri tamamlayınız.");
        foreach (var line in lines)
        {
            if (line.CountedBar < 0 || line.CountedDepot < 0) throw new InvalidOperationException("Bar ve depo miktarları negatif olamaz.");
            if (Balance(data,line.ProductId,branch,"Bar") != line.ExpectedBar || Balance(data,line.ProductId,branch,"Depo") != line.ExpectedDepot)
                throw new InvalidOperationException("Sayım sırasında şubenin Bar/Depo stoğu değişti. Sayımı yeniden başlatın.");
        }
        var bar = Area(data,branch,"Bar"); var depot = Area(data,branch,"Depo");
        var count = new StockCount { Id = pendingId ?? Guid.NewGuid(), BranchName = branch, WarehouseId = depot.Id, Operator = operatorName.Trim(), Lines = lines.Select(l => new CountLine { ProductId = l.ProductId, Expected = l.ExpectedBar+l.ExpectedDepot, Counted = l.CountedBar+l.CountedDepot, BarExpected = l.ExpectedBar, BarCounted = l.CountedBar, DepotExpected = l.ExpectedDepot, DepotCounted = l.CountedDepot }).ToList() };
        if (Access.Current is not null && !approval) { count.Status = "Onay bekliyor"; count.Operator = Access.Current.Name; data.Counts.Add(count); InventoryStore.Save(data); return; }
        foreach (var line in lines)
        {
            void Adjust(Warehouse target,decimal delta,string area)
            {
                void Movement(Guid id,decimal amount) => data.Movements.Add(new StockMovement { ProductId = line.ProductId, WarehouseId = id, Type = "Sayım düzeltmesi", Quantity = amount, Note = $"{branch} / {area} sayımı: {count.Id} / {count.Operator}" });
                if (delta > 0) Movement(target.Id,delta);
                if (delta < 0)
                {
                    decimal remaining = -delta;
                    foreach (var source in data.Warehouses.Where(w => w.BranchName == branch && AreaOf(w) == area))
                    {
                        decimal take = Math.Min(remaining,Math.Max(0,data.Balance(line.ProductId,source.Id)));
                        if (take > 0) { Movement(source.Id,-take); remaining -= take; }
                        if (remaining == 0) break;
                    }
                    if (remaining != 0) throw new InvalidOperationException("Eski stok kayıtları tutarsız. Sayım kaydedilmedi.");
                }
            }
            Adjust(bar,line.CountedBar-line.ExpectedBar,"Bar"); Adjust(depot,line.CountedDepot-line.ExpectedDepot,"Depo");
        }
        if (pendingId is Guid id) { var pending = data.Counts.Single(c=>c.Id==id); pending.Status="Onaylandı"; pending.Reviewer=Access.Current?.Name??"Test yöneticisi"; pending.ReviewNote=reviewNote; pending.ReviewedAt=DateTime.Now; } else data.Counts.Add(count); InventoryStore.Save(data);
    }
}
