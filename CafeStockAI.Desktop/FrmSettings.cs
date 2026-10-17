namespace CafeStockAI.Desktop;
public partial class FrmSettings : DevExpress.XtraEditors.XtraForm, IInventoryPage
{
    public FrmSettings() { InitializeComponent(); QuantityText.Setup(this); }
    public void RefreshData() => PageUi.Run(() =>
    {
        var settings = InventoryStore.Load().Settings; businessInput.Text = settings.BusinessName; operatorInput.Text = settings.OperatorName;
        PageUi.Bind(grid, new[] { new { Ayar = "Kayıt klasörü", Değer = InventoryStore.DirectoryPath }, new { Ayar = "Kayıt biçimi", Değer = SqlMode.Enabled ? "SQL Server • Elazığ • cafestock şeması" : "JSON • SQL geçişi henüz etkin değil" }, new { Ayar = "Sayım", Değer = "Sayım onaylandıktan sonra stok güncellenir." }, new { Ayar = "Personel", Değer = "Personel adıyla giriş; görev ve şube kapsamına göre erişim." } });
        statusLabel.Text = "İşletme adı ana pencere başlığında kullanılır.";
    });
    private async void CheckDatabase_Click(object? sender,EventArgs e)
    {
        checkDatabaseButton.Enabled=false;statusLabel.Text="SQL bağlantısı ve kayıtlar okunuyor…";
        try
        {
            Access.RequireGlobal();
            string result=await Task.Run(()=>{
                SqlMode.Initialize();
                if(!SqlMode.Enabled)return "SQL doğrulanamadı. SQL Server hizmetini ve bağlantı ayarını kontrol edin.";
                var archive=SqlMode.Store!.Export();
                return $"SQL Server bağlantısı başarılı. {archive.Inventory.Branches.Count} şube, {archive.Products.Count} ürün, {archive.Inventory.Employees.Count} personel, {archive.Accounts.Count} hesap, {archive.Inventory.Counts.Count} sayım, {archive.Inventory.Movements.Count} stok hareketi SQL'den okundu. Bu kontrol veri değiştirmez.";
            });
            statusLabel.Text=result;MessageBox.Show(result,"Veritabanı kontrolü");
        }
        catch(Exception ex){statusLabel.Text="SQL kontrolü başarısız: "+ex.Message;}
        finally{if(!IsDisposed)checkDatabaseButton.Enabled=true;}
    }
    private void Database_Click(object? sender,EventArgs e)=>PageUi.Run(()=>
    {
        Access.RequireGlobal();
        using var setup=new FrmDatabaseSetup();if(setup.ShowDialog(this)==DialogResult.OK){if(TopLevelControl is FrmDashboard dashboard)dashboard.Close();return;}RefreshData();
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
