namespace CafeStockAI.Desktop;

public partial class FrmProducts : DevExpress.XtraEditors.XtraForm, IInventoryPage
{
    private List<Product> products = new();
    private Guid? selectedId;
    private bool loaded;
    private bool settingCategoryFilter;

    public FrmProducts()
    {
        InitializeComponent(); QuantityText.Setup(this); productSheet.ProductSelected += PopulateProduct;
        categoryFilter.SelectedIndexChanged += (_,_) => { if (!settingCategoryFilter) RefreshGrid(); };
        grid.CellFormatting += (_, e) =>
        {
            if (e.RowIndex < 0 || grid.Columns[e.ColumnIndex].Name != "imageColumn") return;
            if (grid.Rows[e.RowIndex].DataBoundItem is Product product) { e.Value = ProductImages.Get(product); e.FormattingApplied = true; }
        };
    }

    private void NewButton_Click(object? sender, EventArgs e) => ClearEditor();
    private void SaveButton_Click(object? sender, EventArgs e) => SaveProduct();
    private void DeleteButton_Click(object? sender, EventArgs e) => DeleteProduct();
    private void Grid_SelectionChanged(object? sender, EventArgs e) => SelectProduct();
    private void Search_TextChanged(object? sender, EventArgs e) => RefreshGrid();
    private void FrmProducts_Shown(object? sender, EventArgs e) => LoadProducts();
    public void RefreshData() => LoadProducts();
    internal void SearchProducts(string query) { search.Text = query.Trim(); RefreshGrid(); }

    private void LoadProducts()
    {
        try { products = ProductStore.Load(); loaded = true; saveButton.Enabled=Access.Global;
            settingCategoryFilter = true;
            try
            {
                string selected = categoryFilter.SelectedItem as string ?? "Tüm kategoriler";
                var categories = products.Select(p => string.IsNullOrWhiteSpace(p.Category) ? "Kategorisiz" : p.Category).Distinct().OrderBy(n => n).ToList(); categories.Insert(0,"Tüm kategoriler");
                categoryFilter.DataSource = categories; categoryFilter.SelectedItem = categories.Contains(selected) ? selected : "Tüm kategoriler";
            }
            finally { settingCategoryFilter = false; }
            RefreshGrid(); saveButton.Enabled = true; }
        catch (Exception ex) { saveButton.Enabled = false; deleteButton.Enabled = false; MessageBox.Show("Ürünler okunamadı. Kayıtlar korunmak için düzenleme kapatıldı.\n" + ex.Message, Text); }
    }

    private void RefreshGrid()
    {
        string query = search.Text.Trim();
        string category = categoryFilter.SelectedItem as string ?? "Tüm kategoriler";
        grid.DataSource = products.Where(p => (category == "Tüm kategoriler" || (string.IsNullOrWhiteSpace(p.Category) ? "Kategorisiz" : p.Category) == category) && (p.Name.Contains(query,StringComparison.CurrentCultureIgnoreCase) || p.Code.Contains(query,StringComparison.OrdinalIgnoreCase) || p.Category.Contains(query,StringComparison.CurrentCultureIgnoreCase))).ToList();
        productSheet.SetRows(((List<Product>)grid.DataSource).Select(p=>new CountDraft{Product=p,ProductId=p.Id,Name=p.Name,Category=p.Category,Unit=p.Unit,ExpectedDepot=p.MinimumStock,IsPreview=true}),false,true);
        grid.ClearSelection();
        ClearEditor();
        status.Text = $"Toplam {products.Count} ürün • Listelenen {grid.Rows.Count} • Birimi eksik {products.Count(p => string.IsNullOrWhiteSpace(p.Unit))}";
    }

    private void ClearEditor()
    {
        selectedId = null;
        mode.Text = "Yeni ürün"; saveButton.Text = "Ürün Ekle"; deleteButton.Enabled = false;
        code.Clear(); productName.Clear(); categoryInput.Clear(); unit.SelectedIndex = 0; minimum.Value = 0;
        grid.ClearSelection();
    }

    private void SelectProduct()
    {
        if (grid.SelectedRows.Count == 0 || grid.SelectedRows[0].DataBoundItem is not Product product) return;
        PopulateProduct(product);
    }

    private void PopulateProduct(Product product)
    {
        mode.Text = "Ürünü düzenle"; saveButton.Text = "Güncelle"; deleteButton.Enabled = loaded && Access.Global;
        selectedId = product.Id; code.Text = product.Code; productName.Text = product.Name; categoryInput.Text = product.Category;
        unit.SelectedItem = product.Unit;
        minimum.Value = Math.Clamp(product.MinimumStock, minimum.Minimum, minimum.Maximum);
    }

    private void SaveProduct()
    {
        if (!loaded) return;
        if (!Access.Global) { MessageBox.Show("Ürün düzenleme yetkiniz yok."); return; }
        if (string.IsNullOrWhiteSpace(code.Text)) code.Text = "URN-" + Guid.NewGuid().ToString("N")[..8];
        if (string.IsNullOrWhiteSpace(productName.Text) || unit.SelectedItem is null)
        { MessageBox.Show("Ürün kodu, ürün adı ve birim zorunludur.", Text); return; }
        if (products.Any(p => p.Id != selectedId && string.Equals(p.Code, code.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
        { MessageBox.Show("Bu ürün kodu zaten kullanılıyor.", Text); return; }
        var existing = products.FirstOrDefault(p => p.Id == selectedId);
        try
        {
            if (existing is not null && existing.Unit != unit.Text && (InventoryStore.Load().Movements.Any(m => m.ProductId == existing.Id) || InventoryStore.Load().Counts.Any(c=>c.Lines.Any(l=>l.ProductId==existing.Id))))
            { if(MessageBox.Show("Bu ürünün stok veya sayım geçmişi var. Eski kayıt ve miktarlar korunarak yeni birimle ayrı ürün açılsın mı? Yeni ürünün stoğu sıfırdan başlar; eski miktarlar dönüştürülmez.",Text,MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes)return; selectedId=null; code.Text="URN-"+Guid.NewGuid().ToString("N")[..8]; }
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, Text); return; }
        var candidate = new Product { Id = selectedId ?? Guid.NewGuid(), Code = code.Text.Trim(), Name = productName.Text.Trim(), Category = categoryInput.Text.Trim(), SourceCell = existing?.SourceCell ?? "", Unit = unit.SelectedItem.ToString()!, MinimumStock = minimum.Value };
        var updated = products.Where(p => p.Id != candidate.Id).Append(candidate).OrderBy(p => p.Name).ToList();
        Persist(updated);
    }

    private void DeleteProduct()
    {
        if (!loaded || selectedId is null) return;
        if (!Access.Global) { MessageBox.Show("Ürün silme yetkiniz yok."); return; }
        if (MessageBox.Show("Seçili ürün aktif ürün listesinden kaldırılsın mı? Yeni stok ve sayım işlemlerinde görünmez. Eski stok hareketleri ve sayım kayıtları korunur. İşlemden önce yedek alınır.", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try { Operations.CreateBackup(); }
        catch(Exception ex) { MessageBox.Show("Yedek alınamadığı için ürün kaldırılmadı.\n"+ex.Message,Text); return; }
        Persist(products.Where(p => p.Id != selectedId).ToList());
    }

    private void Persist(List<Product> updated)
    {
        try { ProductStore.Save(updated); products = updated; LoadProducts(); }
        catch (Exception ex) { MessageBox.Show("Kayıt yapılamadı.\n" + ex.Message, Text); }
    }
}
