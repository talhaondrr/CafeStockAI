using System.Text.Json;

namespace CafeStockAI.Desktop;

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
    public List<Branch> Branches { get; set; } = new();
    public bool BranchRegistryInitialized { get; set; }
    public List<Warehouse> Warehouses { get; set; } = new();
    public List<Employee> Employees { get; set; } = new();
    public List<StockMovement> Movements { get; set; } = new();
    public List<StockCount> Counts { get; set; } = new();
    public AppSettings Settings { get; set; } = new();
    public decimal Balance(Guid product, Guid warehouse) => Movements.Where(m => m.ProductId == product && m.WarehouseId == warehouse).Sum(m => m.Quantity);
}
internal static class InventoryStore
{
    internal static string? TestDirectory { get; set; }
    public static string DirectoryPath => TestDirectory ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CafeStockAI");
    private static string FilePath => Path.Combine(DirectoryPath, "inventory.json");
    public static InventoryData Load()
    {
        if (!File.Exists(FilePath)) { var empty = new InventoryData(); BranchRegistry.Initialize(empty); return empty; }
        var data = JsonSerializer.Deserialize<InventoryData>(File.ReadAllText(FilePath)) ?? throw new InvalidDataException("İşletme kayıtları geçersiz.");
        if (data.Warehouses is null || data.Employees is null || data.Movements is null || data.Counts is null || data.Settings is null)
            throw new InvalidDataException("İşletme kayıt dosyası eksik alanlar içeriyor.");
        BranchRegistry.Initialize(data);
        return data;
    }
    public static void Save(InventoryData data)
    {
        Directory.CreateDirectory(DirectoryPath);
        if (File.Exists(FilePath) && !File.Exists(FilePath + ".before-branch-v2.bak")) File.Copy(FilePath,FilePath + ".before-branch-v2.bak");
        File.WriteAllText(FilePath + ".tmp", JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
        File.Move(FilePath + ".tmp", FilePath, true);
    }
}
internal static class InventoryService
{
    public static void AddMovement(Guid productId, Guid warehouseId, decimal quantity, string type, string note)
    {
        Access.RequireGlobal();
        using var transaction = DataLock.Enter();
        var data = InventoryStore.Load();
        if (!ProductStore.Load().Any(p => p.Id == productId) || !data.Warehouses.Any(w => w.Id == warehouseId))
            throw new InvalidOperationException("Geçerli ürün ve depo seçiniz.");
        if (string.IsNullOrWhiteSpace(ProductStore.Load().First(p => p.Id == productId).Unit)) throw new InvalidOperationException("Önce seçilen ürünün birimini belirleyiniz.");
        if (quantity <= 0) throw new InvalidOperationException("Miktar sıfırdan büyük olmalı.");
        if (type != "Giriş" && type != "Çıkış") throw new InvalidOperationException("İşlem türü seçiniz.");
        decimal delta = type == "Çıkış" ? -quantity : quantity;
        if (data.Balance(productId, warehouseId) + delta < 0) throw new InvalidOperationException("Depoda yeterli stok yok.");
        data.Movements.Add(new StockMovement { ProductId = productId, WarehouseId = warehouseId, Quantity = delta, Type = type, Note = note });
        InventoryStore.Save(data);
    }
    public static void CompleteCount(Guid warehouseId, List<CountLine> lines, string operatorName)
    {
        Access.RequireGlobal();
        using var transaction = DataLock.Enter();
        var data = InventoryStore.Load();
        if (!data.Warehouses.Any(w => w.Id == warehouseId) || lines.Count == 0) throw new InvalidOperationException("Depo ve sayım satırları gereklidir.");
        if (string.IsNullOrWhiteSpace(operatorName)) throw new InvalidOperationException("Sayımı yapan kişiyi yazınız.");
        var productIds = ProductStore.Load().Select(p => p.Id).OrderBy(id => id).ToArray();
        if (!productIds.SequenceEqual(lines.Select(l => l.ProductId).OrderBy(id => id)))
            throw new InvalidOperationException("Ürün listesi değişmiş. Sayımı yeniden başlatınız.");
        foreach (var line in lines)
        {
            if (line.Counted < 0) throw new InvalidOperationException("Sayım miktarı negatif olamaz.");
            if (data.Balance(line.ProductId, warehouseId) != line.Expected)
                throw new InvalidOperationException("Sayım sırasında stok değişmiş. Sayımı yeniden başlatınız.");
        }
        var count = new StockCount { WarehouseId = warehouseId, Operator = operatorName.Trim(), Lines = lines };
        foreach (var line in lines.Where(l => l.Counted != l.Expected))
            data.Movements.Add(new StockMovement { ProductId = line.ProductId, WarehouseId = warehouseId, Type = "Sayım düzeltmesi", Quantity = line.Counted - line.Expected, Note = $"Sayım: {count.Id} / {count.Operator}" });
        data.Counts.Add(count);
        InventoryStore.Save(data);
    }
}
public interface IInventoryPage { void RefreshData(); }
internal static class PageUi
{
    internal static bool Strict { get; set; }
    public static void Run(Action action)
    {
        try { action(); }
        catch (Exception ex) { if (Strict) throw; MessageBox.Show(ex.Message, "CafeStock AI", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }
    public static void Bind(DataGridView grid, object rows) { grid.DataSource = rows; grid.ClearSelection(); }
    public static void Choices<T>(ComboBox box, List<T> values, string display) { box.DisplayMember = display; box.ValueMember = "Id"; box.DataSource = values; box.SelectedIndex = -1; }
    public static string ProductName(List<Product> products, Guid id) => products.FirstOrDefault(p => p.Id == id)?.DisplayName ?? "Silinmiş ürün";
    public static string WarehouseName(InventoryData data, Guid id) => data.Warehouses.FirstOrDefault(w => w.Id == id) is Warehouse w ? (string.IsNullOrWhiteSpace(w.BranchName) ? w.Name : w.BranchName + " / " + BranchStock.AreaOf(w)) : "Silinmiş depo";
    public static object MovementRows(InventoryData data, List<Product> products) => data.Movements.OrderByDescending(m => m.Date).Select(m => new { Tarih = m.Date, Ürün = ProductName(products, m.ProductId), Depo = WarehouseName(data, m.WarehouseId), İşlem = m.Type, Miktar = m.Quantity, Birim = products.FirstOrDefault(p => p.Id == m.ProductId)?.Unit, Açıklama = m.Note }).ToList();
    public static void ExportCsv(DataGridView grid, IWin32Window owner)
    {
        using var dialog = new SaveFileDialog { Filter = "CSV dosyası (*.csv)|*.csv", FileName = "CafeStockAI-rapor.csv" };
        if (dialog.ShowDialog(owner) != DialogResult.OK) return;
        static string Cell(object? value)
        {
            var text = value is decimal number ? QuantityText.Format(number) : Convert.ToString(value) ?? "";
            if (text.Length > 0 && "=+-@\t\r".Contains(text[0])) text = "'" + text;
            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }
        var columns = grid.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToList();
        var lines = new List<string> { string.Join(";", columns.Select(c => Cell(c.HeaderText))) };
        lines.AddRange(grid.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).Select(r => string.Join(";", columns.Select(c => Cell(r.Cells[c.Index].Value)))));
        File.WriteAllLines(dialog.FileName, lines, new System.Text.UTF8Encoding(true));
        MessageBox.Show("Rapor kaydedildi.", "CafeStock AI");
    }
}
