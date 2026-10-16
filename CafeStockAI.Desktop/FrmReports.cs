namespace CafeStockAI.Desktop;
public partial class FrmReports : Form, IInventoryPage
{
    private sealed class ReportRow
    {
        public DateTime? Tarih { get; set; }
        public string Ürün { get; set; } = "";
        public string Depo { get; set; } = "";
        public string İşlem { get; set; } = "";
        public decimal? Bar { get; set; }
        public decimal? DepoMiktar { get; set; }
        public decimal Miktar { get; set; }
        public string Birim { get; set; } = "";
        public string Açıklama { get; set; } = "";
    }
    public FrmReports()
    {
        InitializeComponent(); QuantityText.Setup(this);
        reportInput.Items.AddRange(new object[] { "Güncel şube stokları", "Stok hareketleri", "Sayım sonuçları" }); reportInput.SelectedIndex = 0;
        fromInput.Value = DateTime.Today.AddDays(-30); toInput.Value = DateTime.Today;
        reportInput.SelectedIndexChanged += (_,_) => UpdateDates(); UpdateDates();
    }
    private void UpdateDates() { fromInput.Enabled = toInput.Enabled = reportInput.SelectedIndex != 0; }
    public void RefreshData() => PageUi.Run(() =>
    {
        string previous = warehouseInput.SelectedItem as string ?? "Tüm şubeler";
        var data = Access.View(); var names = BranchStock.AllNames(data); names.Insert(0,"Tüm şubeler");
        if (data.Warehouses.Any(w => string.IsNullOrWhiteSpace(w.BranchName))) names.Add("Şubesiz eski kayıtlar");
        warehouseInput.DataSource = names; warehouseInput.SelectedItem = names.Contains(previous) ? previous : "Tüm şubeler"; Render();
    });
    private void Render()
    {
        if (reportInput.SelectedIndex != 0 && fromInput.Value.Date > toInput.Value.Date) throw new InvalidOperationException("Başlangıç tarihi bitişten sonra olamaz.");
        var data = Access.View(); var products = ProductStore.Load(); string selected = warehouseInput.SelectedItem as string ?? "Tüm şubeler";
        bool InBranch(string branch) => selected == "Tüm şubeler" || (selected == "Şubesiz eski kayıtlar" ? string.IsNullOrWhiteSpace(branch) : branch == selected);
        string BranchOf(Guid id) => data.Warehouses.FirstOrDefault(w => w.Id == id)?.BranchName ?? "";
        bool InDate(DateTime date) => date.Date >= fromInput.Value.Date && date.Date <= toInput.Value.Date;
        var rows = new List<ReportRow>();
        if (reportInput.SelectedIndex == 0)
        {
            var names = BranchStock.AllNames(data); if (data.Warehouses.Any(w => string.IsNullOrWhiteSpace(w.BranchName))) names.Add("");
            foreach (var branch in names.Where(InBranch))
            foreach (var product in products)
            {
                decimal bar = BranchStock.Balance(data,product.Id,branch,"Bar"), depot = BranchStock.Balance(data,product.Id,branch,"Depo");
                rows.Add(new ReportRow { Ürün = product.DisplayName, Depo = branch == "" ? "Şubesiz eski kayıtlar" : branch, İşlem = "Güncel stok", Bar = bar, DepoMiktar = depot, Miktar = bar+depot, Birim = product.UnitDisplay, Açıklama = bar+depot <= product.MinimumStock ? "Kritik stok" : "Normal" });
            }
        }
        else if (reportInput.SelectedIndex == 1)
            rows = data.Movements.Where(m => InBranch(BranchOf(m.WarehouseId)) && InDate(m.Date)).OrderByDescending(m => m.Date).Select(m => new ReportRow { Tarih = m.Date, Ürün = PageUi.ProductName(products,m.ProductId), Depo = PageUi.WarehouseName(data,m.WarehouseId), İşlem = m.Type, Miktar = m.Quantity, Birim = products.FirstOrDefault(p => p.Id == m.ProductId)?.Unit ?? "", Açıklama = m.Note }).ToList();
        else
            foreach (var count in data.Counts.Where(c => InBranch(string.IsNullOrWhiteSpace(c.BranchName) ? BranchOf(c.WarehouseId) : c.BranchName) && InDate(c.Date)).OrderByDescending(c => c.Date))
            foreach (var line in count.Lines)
                rows.Add(new ReportRow { Tarih = count.Date, Ürün = PageUi.ProductName(products,line.ProductId), Depo = string.IsNullOrWhiteSpace(count.BranchName) ? PageUi.WarehouseName(data,count.WarehouseId) : count.BranchName, İşlem = "Sayım farkı", Bar = line.BarCounted, DepoMiktar = line.DepotCounted, Miktar = line.Counted-line.Expected, Birim = products.FirstOrDefault(p => p.Id == line.ProductId)?.Unit ?? "", Açıklama = $"Sistem: {QuantityText.Format(line.Expected)} / Sayılan toplam: {QuantityText.Format(line.Counted)} / {count.Operator} / {count.Status}" });
        colBar.Visible = colDepotQuantity.Visible = reportInput.SelectedIndex != 1;
        colMiktar.HeaderText = reportInput.SelectedIndex == 0 ? "Toplam" : reportInput.SelectedIndex == 2 ? "Toplam Fark" : "Miktar";
        PageUi.Bind(grid,rows); statusLabel.Text = $"{rows.Count} satır. Şube stokları Bar ve Depo olarak ayrı gösterilir.";
    }
    private void Refresh_Click(object? sender,EventArgs e) => PageUi.Run(Render);
    private void Export_Click(object? sender,EventArgs e) => PageUi.Run(() => PageUi.ExportCsv(grid,this));
}
