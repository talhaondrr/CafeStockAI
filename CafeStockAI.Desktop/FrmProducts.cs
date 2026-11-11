namespace CafeStockAI.Desktop;
public partial class FrmProducts : DevExpress.XtraEditors.XtraForm, IInventoryPage
{
    private List<Product> products = new();
    private Guid? selectedId;
    private bool loaded;
    private bool settingCategoryFilter;
    public FrmProducts()
    {
        InitializeComponent();
        ConfigureAppearance();
        QuantityText.Setup(this);
        productSheet.ProductSelected += PopulateProduct;
        categoryFilter.SelectedIndexChanged += (_, _) =>
        {
            if (!settingCategoryFilter)
                RefreshGrid();
        };
        grid.CellFormatting += (_, e) =>
        {
            if (e.RowIndex < 0 || grid.Columns[e.ColumnIndex].Name != "imageColumn")
                return;
            if (grid.Rows[e.RowIndex].DataBoundItem is Product product)
            {
                e.Value = ProductImages.Get(product);
                e.FormattingApplied = true;
            }
        };
    }

    private void ConfigureAppearance()
    {
        var espresso = Color.FromArgb(74, 41, 27);
        BackColor = Color.FromArgb(247, 241, 232);
        title.Text = "Ürünler ve Stok";
        title.Font = new Font("Georgia", 21, FontStyle.Bold);
        title.ForeColor = espresso;
        mode.Font = new Font("Georgia", 14, FontStyle.Bold);
        mode.ForeColor = espresso;

        layout.SuspendLayout();
        layout.RowStyles[0].Height = 50;
        layout.RowStyles[1].Height = 0;
        layout.RowStyles[2].Height = 157;
        layout.RowStyles[3].Height = 0;
        layout.RowStyles[5].Height = 0;

        var editorCard = new CoffeeCard { Dock = DockStyle.Fill, Padding = new Padding(16, 10, 16, 8), Margin = new Padding(0, 0, 0, 12) };
        var form = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, BackColor = Color.Transparent };
        form.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        form.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        form.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        mode.Dock = DockStyle.Fill;
        form.Controls.Add(mode, 0, 0);

        var fields = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 2, BackColor = Color.Transparent };
        fields.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));
        fields.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var inputs = new (Label Label, Control Input, float Width)[]
        {
            (nameLabel, productName, 35),
            (categoryLabel, categoryInput, 29),
            (unitLabel, unit, 15),
            (minimumLabel, minimum, 21)
        };
        for (int column = 0; column < inputs.Length; column++)
        {
            var (label, input, width) = inputs[column];
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, width));
            label.ForeColor = espresso;
            input.Dock = DockStyle.Top;
            input.Margin = new Padding(3, 0, column == inputs.Length - 1 ? 0 : 12, 0);
            fields.Controls.Add(label, column, 0);
            fields.Controls.Add(input, column, 1);
        }
        form.Controls.Add(fields, 0, 1);
        form.Controls.Add(actions, 0, 2);
        editor.Visible = false;
        editorCard.Controls.Add(editor);
        editorCard.Controls.Add(form);
        layout.Controls.Add(editorCard, 0, 2);
        saveButton.BackColor = Color.FromArgb(111, 55, 33);
        newButton.BackColor = Color.FromArgb(241, 230, 219);
        newButton.ForeColor = espresso;
        deleteButton.BackColor = Color.FromArgb(250, 231, 225);
        deleteButton.ForeColor = Color.FromArgb(142, 55, 46);

        var catalogCard = new CoffeeCard { Dock = DockStyle.Fill, Padding = new Padding(14, 8, 14, 8), Margin = Padding.Empty };
        var catalog = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 3, BackColor = Color.Transparent };
        catalog.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        catalog.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 39));
        catalog.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26));
        catalog.RowStyles.Add(new RowStyle(SizeType.Absolute, 41));
        catalog.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        catalog.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));
        catalog.Controls.Add(new Label
        {
            Text = "Ürün kataloğu",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Georgia", 14, FontStyle.Bold),
            ForeColor = espresso
        }, 0, 0);
        search.PlaceholderText = "Ürün veya kategori ara…";
        search.AccessibleName = "Ürün kataloğunda ara";
        categoryFilter.AccessibleName = "Kategori filtresi";
        search.Anchor = categoryFilter.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        catalog.Controls.Add(search, 1, 0);
        catalog.Controls.Add(categoryFilter, 2, 0);
        catalog.Controls.Add(productSheet, 0, 1);
        catalog.SetColumnSpan(productSheet, 3);
        status.Dock = DockStyle.Fill;
        catalog.Controls.Add(status, 0, 2);
        catalog.SetColumnSpan(status, 3);
        catalogCard.Controls.Add(catalog);
        layout.Controls.Add(catalogCard, 0, 4);
        layout.ResumeLayout(true);
    }

    private void NewButton_Click(object? sender, EventArgs e) => ClearEditor();
    private void SaveButton_Click(object? sender, EventArgs e) => SaveProduct();
    private void DeleteButton_Click(object? sender, EventArgs e) => DeleteProduct();
    private void Grid_SelectionChanged(object? sender, EventArgs e) => SelectProduct();
    private void Search_TextChanged(object? sender, EventArgs e) => RefreshGrid();
    private void FrmProducts_Shown(object? sender, EventArgs e) => LoadProducts();
    public void RefreshData() => LoadProducts();
    internal void SearchProducts(string query)
    {
        search.Text = query.Trim();
        RefreshGrid();
    }

    private void LoadProducts()
    {
        try
        {
            products = ProductStore.Load();
            loaded = true;
            saveButton.Enabled = Access.Global;
            settingCategoryFilter = true;
            try
            {
                string selected = categoryFilter.SelectedItem as string ?? "Tüm kategoriler";
                var categories = products.Select(p => string.IsNullOrWhiteSpace(p.Category) ? "Kategorisiz" : p.Category).Distinct().OrderBy(n => n).ToList();
                categories.Insert(0, "Tüm kategoriler");
                categoryFilter.DataSource = categories;
                categoryFilter.SelectedItem = categories.Contains(selected) ? selected : "Tüm kategoriler";
            }
            finally
            {
                settingCategoryFilter = false;
            }

            RefreshGrid();
            saveButton.Enabled = true;
        }
        catch (Exception ex)
        {
            saveButton.Enabled = false;
            deleteButton.Enabled = false;
            MessageBox.Show("Ürünler okunamadı. Kayıtlar korunmak için düzenleme kapatıldı.\n" + ex.Message, Text);
        }
    }

    private void RefreshGrid()
    {
        string query = search.Text.Trim();
        string category = categoryFilter.SelectedItem as string ?? "Tüm kategoriler";
        grid.DataSource = products.Where(p => (category == "Tüm kategoriler" || (string.IsNullOrWhiteSpace(p.Category) ? "Kategorisiz" : p.Category) == category) && (p.Name.Contains(query, StringComparison.CurrentCultureIgnoreCase) || p.Code.Contains(query, StringComparison.OrdinalIgnoreCase) || p.Category.Contains(query, StringComparison.CurrentCultureIgnoreCase))).ToList();
        productSheet.SetRows(((List<Product>)grid.DataSource).Select(p => new CountDraft { Product = p, ProductId = p.Id, Name = p.Name, Category = p.Category, Unit = p.Unit, ExpectedDepot = p.MinimumStock, IsPreview = true }), false, true);
        grid.ClearSelection();
        ClearEditor();
        status.Text = $"Toplam {products.Count} ürün • Listelenen {grid.Rows.Count} • Birimi eksik {products.Count(p => string.IsNullOrWhiteSpace(p.Unit))}";
    }

    private void ClearEditor()
    {
        selectedId = null;
        mode.Text = "Yeni ürün";
        saveButton.Text = "Ürün Ekle";
        deleteButton.Enabled = false;
        code.Clear();
        productName.Clear();
        categoryInput.Clear();
        unit.SelectedIndex = 0;
        minimum.Value = 0;
        grid.ClearSelection();
    }

    private void SelectProduct()
    {
        if (grid.SelectedRows.Count == 0 || grid.SelectedRows[0].DataBoundItem is not Product product)
            return;
        PopulateProduct(product);
    }

    private void PopulateProduct(Product product)
    {
        mode.Text = "Ürünü düzenle";
        saveButton.Text = "Güncelle";
        deleteButton.Enabled = loaded && Access.Global;
        selectedId = product.Id;
        code.Text = product.Code;
        productName.Text = product.Name;
        categoryInput.Text = product.Category;
        unit.SelectedItem = product.Unit;
        minimum.Value = Math.Clamp(product.MinimumStock, minimum.Minimum, minimum.Maximum);
    }

    private void SaveProduct()
    {
        if (!loaded)
            return;
        if (!Access.Global)
        {
            MessageBox.Show("Ürün düzenleme yetkiniz yok.");
            return;
        }

        if (string.IsNullOrWhiteSpace(code.Text))
            code.Text = "URN-" + Guid.NewGuid().ToString("N")[..8];
        if (string.IsNullOrWhiteSpace(productName.Text) || unit.SelectedItem is null)
        {
            MessageBox.Show("Ürün kodu, ürün adı ve birim zorunludur.", Text);
            return;
        }

        if (products.Any(p => p.Id != selectedId && string.Equals(p.Code, code.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show("Bu ürün kodu zaten kullanılıyor.", Text);
            return;
        }

        var existing = products.FirstOrDefault(p => p.Id == selectedId);
        try
        {
            if (existing is not null && existing.Unit != unit.Text && (InventoryStore.Load().Movements.Any(m => m.ProductId == existing.Id) || InventoryStore.Load().Counts.Any(c => c.Lines.Any(l => l.ProductId == existing.Id))))
            {
                if (MessageBox.Show("Bu ürünün stok veya sayım geçmişi var. Eski kayıt ve miktarlar korunarak yeni birimle ayrı ürün açılsın mı? Yeni ürünün stoğu sıfırdan başlar; eski miktarlar dönüştürülmez.", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                selectedId = null;
                code.Text = "URN-" + Guid.NewGuid().ToString("N")[..8];
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, Text);
            return;
        }

        var candidate = new Product
        {
            Id = selectedId ?? Guid.NewGuid(),
            Code = code.Text.Trim(),
            Name = productName.Text.Trim(),
            Category = categoryInput.Text.Trim(),
            SourceCell = existing?.SourceCell ?? "",
            Unit = unit.SelectedItem.ToString()!,
            MinimumStock = minimum.Value
        };
        var updated = products.Where(p => p.Id != candidate.Id).Append(candidate).OrderBy(p => p.Name).ToList();
        Persist(updated);
    }

    private void DeleteProduct()
    {
        if (!loaded || selectedId is null)
            return;
        if (!Access.Global)
        {
            MessageBox.Show("Ürün silme yetkiniz yok.");
            return;
        }

        if (MessageBox.Show("Seçili ürün aktif ürün listesinden kaldırılsın mı? Yeni stok ve sayım işlemlerinde görünmez. Eski stok hareketleri ve sayım kayıtları korunur. İşlemden önce yedek alınır.", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        try
        {
            Operations.CreateBackup();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Yedek alınamadığı için ürün kaldırılmadı.\n" + ex.Message, Text);
            return;
        }

        Persist(products.Where(p => p.Id != selectedId).ToList());
    }

    private void Persist(List<Product> updated)
    {
        try
        {
            ProductStore.Save(updated);
            products = updated;
            LoadProducts();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Kayıt yapılamadı.\n" + ex.Message, Text);
        }
    }
}
