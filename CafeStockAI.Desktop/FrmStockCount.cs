using System.ComponentModel;
namespace CafeStockAI.Desktop;
public sealed class CountDraft : INotifyPropertyChanged
{
    [Browsable(false)] public Product? Product { get; set; }
    public Guid ProductId { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Unit { get; set; } = "";
    public decimal ExpectedBar { get; set; }
    public decimal ExpectedDepot { get; set; }
    private decimal? countedBar, countedDepot;
    public decimal? BarCounted { get => countedBar; set { countedBar = value; Changed(nameof(BarCounted)); } }
    public decimal? DepotCounted { get => countedDepot; set { countedDepot = value; Changed(nameof(DepotCounted)); } }
    public bool IsPreview { get; set; }
    public decimal? Total => IsPreview ? ExpectedBar + ExpectedDepot : BarCounted + DepotCounted;
    public decimal? Difference => Total - ExpectedBar - ExpectedDepot;
    private void Changed(string property) { PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(property)); PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(Total))); PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(Difference))); }
    public event PropertyChangedEventHandler? PropertyChanged;
}
public partial class FrmStockCount : Form, IInventoryPage
{
    private BindingList<CountDraft> draft = new();
    private string? activeBranch;
    private List<CountDraft> previewRows = new();
    private bool settingCategories;
    private string previousCategory = "Tüm kategoriler";
    public FrmStockCount()
    {
        InitializeComponent(); QuantityText.Setup(this); completeButton.Enabled = false; cancelButton.Enabled = false;
        grid.CellFormatting += (_,e) =>
        {
            if (e.RowIndex < 0 || grid.Columns[e.ColumnIndex].Name != "imageColumn") return;
            if (grid.Rows[e.RowIndex].DataBoundItem is CountDraft row && row.Product is not null) { e.Value = ProductImages.Get(row.Product); e.FormattingApplied = true; }
        };
        categoryInput.SelectedIndexChanged += (_,_) => ApplyCategory();
        branchInput.SelectedIndexChanged += (_,_) => Preview();
        grid.DataError += (_,e) => { e.ThrowException = false; e.Cancel = true; statusLabel.Text = "Bar/Depo miktarı için geçerli bir sayı girin."; };
        grid.CellValidating += (_,e) =>
        {
            if (grid.Columns[e.ColumnIndex].DataPropertyName is not ("BarCounted" or "DepotCounted")) return;
            string? text = Convert.ToString(e.FormattedValue);
            if (!string.IsNullOrWhiteSpace(text) && (!QuantityText.TryParse(text,out decimal value) || value < 0)) { e.Cancel = true; statusLabel.Text = "Miktarlar sıfır veya pozitif olmalı."; }
        };
    }
    public void RefreshData() => PageUi.Run(() =>
    {
        if (activeBranch is not null) return;
        var selected = branchInput.SelectedItem as string; var data = Access.View(); branchInput.DataSource = BranchStock.AllNames(data); branchInput.SelectedItem = selected; if (selected is null) branchInput.SelectedIndex = -1; operatorInput.Text = data.Settings.OperatorName;
        if (branchInput.SelectedItem is not null) Preview();
        statusLabel.Text = $"{data.Counts.Count} tamamlanmış sayım. Şube seçin; Bar ve Depo miktarlarını aynı tabloda girin.";
    });
    private void Preview() => PageUi.Run(() =>
    {
        if (activeBranch is not null || branchInput.SelectedItem is not string branch) return;
        var data = Access.View();
        previewRows = ProductStore.Load().OrderBy(p => p.Category).ThenBy(p => p.Name).Select(p => new CountDraft { Product = p, ProductId = p.Id, Name = p.Name, Category = p.Category, Code = p.Code, Unit = p.UnitDisplay, ExpectedBar = BranchStock.Balance(data,p.Id,branch,"Bar"), ExpectedDepot = BranchStock.Balance(data,p.Id,branch,"Depo"), IsPreview = true }).ToList();
        FillCategories(previewRows); ApplyCategory();
        grid.ReadOnly = true; colCounted.Visible = colDepotCounted.Visible = colDifference.Visible = false;
        statusLabel.Text = branch + " şubesinin güncel Bar ve Depo miktarları. Yeni miktarlar için sayımı başlatın.";
    });
    private void FillCategories(IEnumerable<CountDraft> source)
    {
        settingCategories = true;
        try
        {
            string selected = categoryInput.SelectedItem as string ?? "Tüm kategoriler";
            var names = source.Select(p => string.IsNullOrWhiteSpace(p.Category) ? "Kategorisiz" : p.Category).Distinct().OrderBy(n => n).ToList(); names.Insert(0,"Tüm kategoriler");
            categoryInput.DataSource = names; categoryInput.SelectedItem = names.Contains(selected) ? selected : "Tüm kategoriler";
        }
        finally { settingCategories = false; }
    }
    private void ApplyCategory()
    {
        if (settingCategories) return;
        if (!grid.EndEdit())
        {
            settingCategories = true; categoryInput.SelectedItem = previousCategory; settingCategories = false; return;
        }
        string category = categoryInput.SelectedItem as string ?? "Tüm kategoriler";
        IEnumerable<CountDraft> source = activeBranch is null ? previewRows : draft;
        grid.DataSource = new BindingList<CountDraft>(source.Where(p => category == "Tüm kategoriler" || (string.IsNullOrWhiteSpace(p.Category) ? "Kategorisiz" : p.Category) == category).ToList());
        previousCategory = category;
    }
    private void Start_Click(object? sender,EventArgs e) => PageUi.Run(() =>
    {
        if (branchInput.SelectedItem is not string branch) throw new InvalidOperationException("Sayılacak şubeyi seçiniz.");
        var products = ProductStore.Load(); var data = Access.View();
        if (products.Count == 0) throw new InvalidOperationException("Önce ürün ekleyiniz.");
        if (products.Any(p => string.IsNullOrWhiteSpace(p.Unit))) throw new InvalidOperationException("Önce ürün listesindeki eksik birimleri tamamlayınız.");
        activeBranch = branch;
        draft = new BindingList<CountDraft>(products.OrderBy(p => p.Category).ThenBy(p => p.Name).Select(p => new CountDraft { Product = p, ProductId = p.Id, Code = p.Code, Name = p.Name, Category = p.Category, Unit = p.Unit, ExpectedBar = BranchStock.Balance(data,p.Id,branch,"Bar"), ExpectedDepot = BranchStock.Balance(data,p.Id,branch,"Depo") }).ToList());
        grid.ReadOnly = false; colCounted.Visible = colDepotCounted.Visible = colDifference.Visible = true;
        FillCategories(draft); ApplyCategory(); branchInput.Enabled = false; startButton.Enabled = false; completeButton.Enabled = true; cancelButton.Enabled = true;
        statusLabel.Text = branch + " şubesi: Bar ve Depo sayılan miktarlarını doldurun. Bulunmayan ürün için 0 yazın.";
    });
    private void Complete_Click(object? sender,EventArgs e) => PageUi.Run(() =>
    {
        if (activeBranch is null) return;
        if (!grid.EndEdit()) throw new InvalidOperationException("Geçersiz sayım hücresini düzeltin.");
        if (draft.Any(l => l.BarCounted is null || l.DepotCounted is null)) throw new InvalidOperationException("Her ürünün Bar ve Depo miktarlarını giriniz.");
        if (MessageBox.Show(activeBranch + " şubesinin sayımı yönetici onayına gönderilsin mi?",Text,MessageBoxButtons.YesNo,MessageBoxIcon.Question) != DialogResult.Yes) return;
        BranchStock.Complete(activeBranch,draft.Select(l => new BranchCountLine { ProductId = l.ProductId, ExpectedBar = l.ExpectedBar, ExpectedDepot = l.ExpectedDepot, CountedBar = l.BarCounted!.Value, CountedDepot = l.DepotCounted!.Value }).ToList(),operatorInput.Text);
        Reset(); RefreshData(); statusLabel.Text = "Sayım yönetici onayına gönderildi. Onaylanınca stok güncellenecek.";
    });
    private void Cancel_Click(object? sender,EventArgs e)
    {
        if (MessageBox.Show("Kaydedilmemiş şube sayımı iptal edilsin mi?",Text,MessageBoxButtons.YesNo) != DialogResult.Yes) return;
        Reset(); RefreshData();
    }
    private void Reset() { activeBranch = null; grid.DataSource = null; branchInput.Enabled = true; startButton.Enabled = true; completeButton.Enabled = false; cancelButton.Enabled = false; }
}
