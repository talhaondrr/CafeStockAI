namespace CafeStockAI.Desktop;
public partial class FrmStockMovements : DevExpress.XtraEditors.XtraForm, IInventoryPage
{
    private readonly TextBox historySearch = new();
    private readonly Label historyCount = new();
    private List<PageUi.MovementRow> movementRows = [];

    public FrmStockMovements()
    {
        InitializeComponent();
        ConfigureAppearance();
        QuantityText.Setup(this);
        typeInput.Items.AddRange(new object[] { "Giriş", "Çıkış" });
        typeInput.SelectedIndex = 0;
        AddChoices(areaInput, 1, 1, "Bar", "Depo");
        AddChoices(typeInput, 2, 1, "Giriş", "Çıkış");
        productInput.SelectedIndexChanged += (_, _) => UpdateBalance();
        branchInput.SelectedIndexChanged += (_, _) => UpdateBalance();
        areaInput.SelectedIndexChanged += (_, _) => UpdateBalance();
    }

    public void RefreshData() => PageUi.Run(() =>
    {
        var data = Access.View();
        var products = ProductStore.Load();
        PageUi.Choices(productInput, products.OrderBy(p => p.Category).ThenBy(p => p.Name).ToList(), "DisplayName");
        branchInput.DataSource = BranchStock.AllNames(data);
        branchInput.SelectedIndex = branchInput.Items.Count == 1 ? 0 : -1;
        branchInput.Visible = branchInputLabel.Visible = branchInput.Items.Count != 1;
        if (branchInput.Items.Count == 1)
            descriptionLabel.Text = branchInput.Text;
        movementRows = PageUi.MovementRows(data, products);
        ApplySearch();
        UpdateBalance();
    });
    private void ConfigureAppearance()
    {
        layout.SuspendLayout();
        BackColor = Color.FromArgb(247, 241, 232);
        layout.RowStyles[0].Height = 45;
        layout.RowStyles[1].Height = 30;
        layout.RowStyles[2].Height = 248;
        layout.RowStyles[3].Height = 0;
        layout.RowStyles[5].Height = 0;
        titleLabel.ForeColor = Color.FromArgb(74, 41, 27);
        titleLabel.Font = new Font("Georgia", 21, FontStyle.Bold);

        var formCard = new CoffeeCard { Dock = DockStyle.Fill, Padding = new Padding(18, 12, 18, 10), Margin = new Padding(0, 0, 0, 12) };
        var form = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, BackColor = Color.Transparent };
        form.RowStyles.Add(new RowStyle(SizeType.Absolute, 33));
        form.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        form.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        form.Controls.Add(SectionTitle("Yeni stok hareketi"), 0, 0);
        form.Controls.Add(editor, 0, 1);
        form.Controls.Add(actions, 0, 2);
        formCard.Controls.Add(form);
        layout.Controls.Add(formCard, 0, 2);
        editor.Margin = Padding.Empty;
        editor.RowStyles[0].Height = editor.RowStyles[2].Height = 22;
        editor.RowStyles[1].Height = editor.RowStyles[3].Height = 36;

        editor.SetCellPosition(areaInputLabel, new TableLayoutPanelCellPosition(1, 0));
        editor.SetCellPosition(areaInput, new TableLayoutPanelCellPosition(1, 1));
        editor.SetCellPosition(branchInputLabel, new TableLayoutPanelCellPosition(2, 2));
        editor.SetCellPosition(branchInput, new TableLayoutPanelCellPosition(2, 3));

        saveButton.BackColor = Color.FromArgb(111, 55, 33);
        refreshButton.BackColor = Color.FromArgb(241, 230, 219);
        refreshButton.ForeColor = Color.FromArgb(83, 54, 39);
        statusLabel.Margin = new Padding(10, 10, 0, 0);
        actions.Controls.Add(statusLabel);

        var historyCard = new CoffeeCard { Dock = DockStyle.Fill, Padding = new Padding(16, 10, 16, 10), Margin = Padding.Empty };
        var history = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 3, BackColor = Color.Transparent };
        history.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52));
        history.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48));
        history.RowStyles.Add(new RowStyle(SizeType.Absolute, 43));
        history.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        history.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));
        history.Controls.Add(SectionTitle("Hareket geçmişi"), 0, 0);
        historySearch.PlaceholderText = "Ürün, işlem veya açıklama ara…";
        historySearch.AccessibleName = "Hareket geçmişinde ara";
        historySearch.Dock = DockStyle.Top;
        historySearch.Margin = new Padding(0, 5, 0, 0);
        historySearch.TextChanged += (_, _) => ApplySearch();
        history.Controls.Add(historySearch, 1, 0);
        history.Controls.Add(grid, 0, 1);
        history.SetColumnSpan(grid, 2);
        historyCount.Dock = DockStyle.Fill;
        historyCount.ForeColor = descriptionLabel.ForeColor;
        history.Controls.Add(historyCount, 0, 2);
        history.SetColumnSpan(historyCount, 2);
        historyCard.Controls.Add(history);
        layout.Controls.Add(historyCard, 0, 4);

        grid.Margin = Padding.Empty;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.GridColor = Color.FromArgb(230, 220, 209);
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(242, 233, 224);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(74, 41, 27);
        grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(7);
        grid.DefaultCellStyle.Padding = new Padding(7, 3, 7, 3);
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(253, 250, 246);
        grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
        float[] weights = [95, 150, 85, 100, 64, 52, 135];
        for (int index = 0; index < grid.Columns.Count; index++)
            grid.Columns[index].FillWeight = weights[index];
        grid.CellPainting += PaintMovementBadge;
        layout.ResumeLayout(true);
    }

    private void AddChoices(ComboBox input, int column, int row, params string[] labels)
    {
        var options = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = labels.Length, RowCount = 1, Margin = input.Margin };
        for (int index = 0; index < labels.Length; index++)
        {
            string label = labels[index];
            options.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / labels.Length));
            var choice = new RadioButton
            {
                Text = label,
                Appearance = System.Windows.Forms.Appearance.Button,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 4, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                Checked = input.Text == label,
                ForeColor = Color.FromArgb(74, 41, 27)
            };
            choice.FlatAppearance.BorderColor = Color.FromArgb(224, 205, 190);
            choice.FlatAppearance.CheckedBackColor = Color.FromArgb(243, 220, 205);
            choice.CheckedChanged += (_, _) =>
            {
                if (choice.Checked)
                    input.SelectedItem = label;
            };
            options.Controls.Add(choice, index, 0);
        }
        input.Visible = false;
        editor.Controls.Add(options, column, row);
    }

    private static Label SectionTitle(string title) => new()
    {
        Text = title,
        Dock = DockStyle.Fill,
        Font = new Font("Georgia", 14, FontStyle.Bold),
        ForeColor = Color.FromArgb(74, 41, 27),
        TextAlign = ContentAlignment.MiddleLeft
    };

    private void ApplySearch()
    {
        var comparison = System.Globalization.CultureInfo.GetCultureInfo("tr-TR").CompareInfo;
        string query = historySearch.Text.Trim();
        bool Matches(string value) => comparison.IndexOf(value, query, System.Globalization.CompareOptions.IgnoreCase) >= 0;
        var visible = query.Length == 0 ? movementRows : movementRows.Where(row =>
            Matches(row.Ürün) || Matches(row.Depo) || Matches(row.İşlem) || Matches(row.Açıklama)).ToList();
        PageUi.Bind(grid, visible);
        historyCount.Text = visible.Count == 0 ? "Eşleşen hareket yok." : $"{visible.Count} / {movementRows.Count} hareket";
    }

    private void PaintMovementBadge(object? sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex != 3 || e.Graphics is not { } graphics ||
            e.FormattedValue is not string type || e.CellStyle is not { Font: { } font })
            return;

        var colors = type switch
        {
            "Giriş" => (Color.FromArgb(225, 241, 224), Color.FromArgb(41, 99, 52)),
            "Çıkış" => (Color.FromArgb(250, 227, 222), Color.FromArgb(142, 55, 46)),
            _ => (Color.FromArgb(249, 236, 202), Color.FromArgb(109, 79, 30))
        };
        int height = Math.Min(e.CellBounds.Height - 4, TextRenderer.MeasureText(type, font).Height + 4);
        int width = Math.Min(e.CellBounds.Width - 8, TextRenderer.MeasureText(type, font).Width + 12);
        if (height < 4 || width < height)
            return;

        e.Paint(e.ClipBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
        var bounds = new Rectangle(e.CellBounds.Left + 4, e.CellBounds.Top + (e.CellBounds.Height - height) / 2, width, height);
        using var path = new System.Drawing.Drawing2D.GraphicsPath();
        path.AddArc(bounds.Left, bounds.Top, height, height, 90, 180);
        path.AddArc(bounds.Right - height, bounds.Top, height, height, 270, 180);
        path.CloseFigure();
        using var brush = new SolidBrush(colors.Item1);
        graphics.FillPath(brush, path);
        TextRenderer.DrawText(graphics, type, font, bounds, colors.Item2,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        e.Handled = true;
    }

    private void UpdateBalance() => PageUi.Run(() =>
    {
        statusLabel.Text = "";
        quantityInputLabel.Text = productInput.SelectedItem is Product selected ? $"Miktar · {selected.UnitDisplay}" : "Miktar";
        if (productInput.SelectedItem is Product product && branchInput.SelectedItem is string branch && areaInput.SelectedItem is string area)
            statusLabel.Text = $"{branch} / {area} mevcut stok: {QuantityText.Format(BranchStock.Balance(Access.View(), product.Id, branch, area))} {product.UnitDisplay}";
    });
    private void Refresh_Click(object? sender, EventArgs e) => RefreshData();
    private void Save_Click(object? sender, EventArgs e) => PageUi.Run(() =>
    {
        if (productInput.SelectedItem is not Product product || branchInput.SelectedItem is not string branch || areaInput.SelectedItem is not string area)
            throw new InvalidOperationException("Ürün, şube ve stok alanını seçiniz.");
        BranchMovements.Add(product.Id, branch, area, quantityInput.Value, typeInput.Text, noteInput.Text.Trim());
        quantityInput.Value = 0;
        noteInput.Clear();
        RefreshData();
        statusLabel.Text = "Stok hareketi kaydedildi.";
    });
}
