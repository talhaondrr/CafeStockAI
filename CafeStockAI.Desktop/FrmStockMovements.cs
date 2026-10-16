namespace CafeStockAI.Desktop;
public partial class FrmStockMovements : Form, IInventoryPage
{
    public FrmStockMovements()
    {
        InitializeComponent(); QuantityText.Setup(this);
        typeInput.Items.AddRange(new object[] { "Giriş", "Çıkış" }); typeInput.SelectedIndex = 0;
        productInput.SelectedIndexChanged += (_,_) => UpdateBalance(); branchInput.SelectedIndexChanged += (_,_) => UpdateBalance(); areaInput.SelectedIndexChanged += (_,_) => UpdateBalance();
    }
    public void RefreshData() => PageUi.Run(() =>
    {
        var data = Access.View(); var products = ProductStore.Load();
        PageUi.Choices(productInput,products.OrderBy(p => p.Category).ThenBy(p => p.Name).ToList(),"DisplayName"); branchInput.DataSource = BranchStock.AllNames(data); branchInput.SelectedIndex = -1;
        PageUi.Bind(grid,PageUi.MovementRows(data,products)); statusLabel.Text = $"{data.Movements.Count} stok hareketi. Ürün, şube ve stok alanı seçiniz.";
    });
    private void UpdateBalance() => PageUi.Run(() =>
    {
        if (productInput.SelectedItem is Product product && branchInput.SelectedItem is string branch && areaInput.SelectedItem is string area)
            statusLabel.Text = $"{branch} / {area} mevcut stok: {QuantityText.Format(BranchStock.Balance(Access.View(),product.Id,branch,area))} {product.UnitDisplay}";
    });
    private void Refresh_Click(object? sender,EventArgs e) => RefreshData();
    private void Save_Click(object? sender,EventArgs e) => PageUi.Run(() =>
    {
        if (productInput.SelectedItem is not Product product || branchInput.SelectedItem is not string branch || areaInput.SelectedItem is not string area) throw new InvalidOperationException("Ürün, şube ve stok alanını seçiniz.");
        BranchMovements.Add(product.Id,branch,area,quantityInput.Value,typeInput.Text,noteInput.Text.Trim()); quantityInput.Value = 0; noteInput.Clear(); RefreshData(); statusLabel.Text = "Stok hareketi kaydedildi.";
    });
}
