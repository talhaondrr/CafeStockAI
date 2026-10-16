namespace CafeStockAI.Desktop;
public partial class FrmSettings : Form, IInventoryPage
{
    public FrmSettings() { InitializeComponent(); QuantityText.Setup(this); }
    public void RefreshData() => PageUi.Run(() =>
    {
        var settings = InventoryStore.Load().Settings; businessInput.Text = settings.BusinessName; operatorInput.Text = settings.OperatorName;
        PageUi.Bind(grid, new[] { new { Ayar = "Kayıt klasörü", Değer = InventoryStore.DirectoryPath }, new { Ayar = "Kayıt biçimi", Değer = "Bu bilgisayarda JSON dosyaları" }, new { Ayar = "Sayım", Değer = "Sayım onaylandıktan sonra stok güncellenir." }, new { Ayar = "Personel", Değer = "Personel adıyla giriş; görev ve şube kapsamına göre erişim." } });
        statusLabel.Text = "İşletme adı ana pencere başlığında kullanılır.";
    });
    private void Save_Click(object? sender, EventArgs e) => PageUi.Run(() =>
    {
        Access.RequireGlobal();
        if (string.IsNullOrWhiteSpace(businessInput.Text)) throw new InvalidOperationException("İşletme adı boş olamaz.");
        var data = InventoryStore.Load(); data.Settings.BusinessName = businessInput.Text.Trim(); data.Settings.OperatorName = operatorInput.Text.Trim(); InventoryStore.Save(data);
        if (TopLevelControl is Form host) host.Text = data.Settings.BusinessName + " - Yönetim Paneli";
        statusLabel.Text = "Ayarlar kaydedildi.";
    });
}
