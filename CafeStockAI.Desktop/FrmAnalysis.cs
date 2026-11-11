namespace CafeStockAI.Desktop;
public partial class FrmAnalysis : DevExpress.XtraEditors.XtraForm, IInventoryPage
{
    private sealed record AnalysisRow(Product Product, string Depo, decimal Stok, Forecast? Forecast, PurchasePlan Plan)
    {
        public string Ürün => Product.Name;
        public string Birim => Product.UnitDisplay;
        public decimal Sınır => Product.MinimumStock;
        public string Gün => Plan.Remaining;
        public string Öncelik => Plan.Priority;
        public decimal? Tahmin => Plan.Demand is decimal demand ? Math.Round(demand, 4) : null;
        public decimal Sipariş => Math.Round(Plan.Reorder, 4);
        public string Öneri => Plan.Reason;
    }

    private static readonly string[] PriorityLabels = ["Bugün sipariş ver", "Takip et", "Stok yeterli"];
    private static readonly Color[] PriorityColors =
        [Color.FromArgb(137, 56, 45), Color.FromArgb(116, 78, 26), Color.FromArgb(36, 105, 79)];
    private static readonly Color[] PriorityBackgrounds =
        [Color.FromArgb(250, 231, 224), Color.FromArgb(251, 239, 214), Color.FromArgb(224, 241, 230)];
    private readonly Button[] priorityCards = new Button[3];
    private List<AnalysisRow> analysisRows = [];
    private int? priorityFilter;
    private string analysisNote = "";

    public FrmAnalysis()
    {
        InitializeComponent();
        CoffeePageStyle.Apply(this, titleLabel, refreshButton);
        QuantityText.Setup(this);
        colÜrün.FillWeight = 130;
        colÖneri.FillWeight = 220;
        AddPrioritySummary();
        CoffeePageStyle.Frame(layout, 5, new Padding(8));
        grid.CellFormatting += (_, e) =>
        {
            if (e.CellStyle is not { } style || e.RowIndex < 0 ||
                grid.Columns[e.ColumnIndex].DataPropertyName != "Öncelik" ||
                grid.Rows[e.RowIndex].DataBoundItem is not AnalysisRow row)
                return;

            int rank = row.Plan.Rank;
            style.BackColor = PriorityBackgrounds[rank];
            style.ForeColor = PriorityColors[rank];
        };
        grid.CellDoubleClick += (_, e) =>
        {
            if (e.RowIndex >= 0)
                ShowDetails(grid.Rows[e.RowIndex].DataBoundItem as AnalysisRow);
        };
        var details = new Button
        {
            Text = "Ürün detayı",
            Size = exportButton.Size,
            BackColor = exportButton.BackColor,
            ForeColor = exportButton.ForeColor,
            FlatStyle = FlatStyle.Flat
        };
        details.Click += (_, _) => ShowDetails(grid.CurrentRow?.DataBoundItem as AnalysisRow);
        actions.Controls.Add(details);
    }

    private void AddPrioritySummary()
    {
        var summary = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1 };
        for (int rank = 0; rank < priorityCards.Length; rank++)
        {
            int filter = rank;
            summary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 3));
            var card = new Button
            {
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                BackColor = PriorityBackgrounds[rank],
                ForeColor = PriorityColors[rank],
                Font = new Font(Font.FontFamily, 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(14, 0, 0, 0),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 2, 8, 8)
            };
            card.Click += (_, _) =>
            {
                priorityFilter = priorityFilter == filter ? null : filter;
                ShowPriorityRows();
            };
            priorityCards[rank] = card;
            summary.Controls.Add(card, rank, 0);
        }

        layout.RowCount = 7;
        layout.RowStyles.Insert(2, new RowStyle(SizeType.Absolute, 76));
        layout.SetRow(statusLabel, 6);
        layout.SetRow(grid, 5);
        layout.SetRow(actions, 4);
        layout.SetRow(editor, 3);
        layout.Controls.Add(summary, 0, 2);

        var clear = new Button { Text = "Tüm öncelikler", Size = exportButton.Size };
        clear.Click += (_, _) =>
        {
            priorityFilter = null;
            ShowPriorityRows();
        };
        actions.Controls.Add(clear);
    }

    private void ShowPriorityRows()
    {
        for (int rank = 0; rank < priorityCards.Length; rank++)
        {
            int count = analysisRows.Count(row => row.Plan.Rank == rank);
            var card = priorityCards[rank];
            card.Text = $"{count} ürün\n{PriorityLabels[rank]}";
            card.FlatAppearance.BorderColor = PriorityColors[rank];
            card.FlatAppearance.BorderSize = priorityFilter == rank ? 2 : 0;
            card.AccessibleDescription = priorityFilter == rank ? "Etkin filtre" : "Ürünleri filtrele";
        }
        var visible = analysisRows.Where(row => priorityFilter is null || row.Plan.Rank == priorityFilter).ToList();
        PageUi.Bind(grid, visible);
        statusLabel.Text = $"{visible.Count} / {analysisRows.Count} ürün/şube • {analysisNote}";
    }

    public void RefreshData() => PageUi.Run(() =>
    {
        string previous = warehouseInput.SelectedItem as string ?? "Tüm şubeler";
        var names = BranchStock.AllNames(Access.View());
        names.Insert(0, "Tüm şubeler");
        warehouseInput.DataSource = names;
        warehouseInput.SelectedItem = names.Contains(previous) ? previous : "Tüm şubeler";
        Render();
    });
    private void Render()
    {
        var data = Access.View();
        var products = ProductStore.Load();
        bool sample = !SqlMode.Configured
            && data.Movements.Any(DemandModel.IsExample)
            && !data.Movements.Any(m => m.Type == "Çıkış" && !DemandModel.IsExample(m));
        string selected = warehouseInput.SelectedItem as string ?? "Tüm şubeler";
        analysisRows = (
            from branch in BranchStock.AllNames(data)
            where selected == "Tüm şubeler" || branch == selected
            from p in products
            let balance = BranchStock.Balance(data, p.Id, branch, "Bar") + BranchStock.Balance(data, p.Id, branch, "Depo")
            let forecast = DemandModel.Train(data, p.Id, branch, sample)
            select new AnalysisRow(p, branch, balance, forecast, PurchasePlan.Create(p.Unit, balance, p.MinimumStock, forecast))
        ).OrderBy(r => r.Plan.Rank).ThenByDescending(r => r.Sipariş).ThenBy(r => r.Ürün).ToList();
        analysisNote = $"Teslim süresi varsayımı {PurchasePlan.LeadDays} gün." + (sample ? " Tahminler örnek tüketimden üretildi." : "");
        ShowPriorityRows();
    }

    private void ShowDetails(AnalysisRow? row)
    {
        if (row is null)
            return;
        using var dialog = new DevExpress.XtraEditors.XtraForm
        {
            Text = $"{row.Ürün} • {row.Depo}",
            StartPosition = FormStartPosition.CenterParent,
            ClientSize = new Size(760, 380),
            MinimumSize = new Size(600, 360),
            Padding = new Padding(16),
            BackColor = BackColor
        };
        var summary = new Label
        {
            Dock = DockStyle.Top,
            Height = 102,
            Text = row.Plan.Priority + " · " + row.Plan.Reason + "\n"
                + row.Plan.Explain(row.Product.Unit, row.Stok, row.Sınır, row.Forecast)
        };
        dialog.Controls.Add(summary);
        if (row.Forecast is { } forecast)
        {
            summary.Text += $"\nTest hatası (son 7 günün günlük MAE değeri) {QuantityText.Format(forecast.Error)} {row.Product.Unit}.";
            if (forecast.Anomaly > 0.5m)
                summary.Text += $" Son gün tüketim beklenenden %{forecast.Anomaly * 100:N0} fazla.";
            var chart = new Panel { Dock = DockStyle.Fill, BackColor = Color.FloralWhite };
            chart.Resize += (_, _) => chart.Invalidate();
            chart.Paint += (_, e) => DrawSeries(chart, e.Graphics, forecast.Series, row.Product.Unit);
            dialog.Controls.Add(chart);
            chart.BringToFront();
        }
        dialog.ShowDialog(this);
    }

    private static void DrawSeries(Control chart, Graphics graphics, IReadOnlyList<DemandPoint> points, string unit)
    {
        decimal max = points.Max(p => p.Quantity);
        float step = (chart.ClientSize.Width - 64f) / points.Count;
        float bottom = chart.ClientSize.Height - 42;
        using var actual = new SolidBrush(Color.SaddleBrown);
        using var predicted = new SolidBrush(Color.Teal);
        using var axis = new Pen(Color.Tan);
        graphics.DrawString($"■ Geçmiş tüketim ({unit})", chart.Font, actual, 12, 8);
        graphics.DrawString("■ 7 günlük tahmin", chart.Font, predicted, 240, 8);
        for (int tick = 0; tick <= 3; tick++)
        {
            float y = bottom - (bottom - 40) * tick / 3;
            graphics.DrawLine(axis, 54, y, chart.Width - 10, y);
            graphics.DrawString(QuantityText.Format(Math.Round(max * tick / 3, 3)), chart.Font, actual, 4, y - 8);
        }
        int forecastStart = points.Count - 7;
        float divider = 54 + forecastStart * step - 1;
        using var boundary = new Pen(Color.Teal) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
        graphics.DrawLine(boundary, divider, 40, divider, bottom);
        for (int i = 0; i < points.Count; i++)
        {
            var point = points[i];
            float height = max > 0 ? (float)(point.Quantity / max) * (bottom - 40) : 0;
            float x = 54 + i * step;
            graphics.FillRectangle(point.Predicted ? predicted : actual, x, bottom - height, Math.Max(1, step - 2), height);
            if (i == 0 || i == forecastStart || i == points.Count - 1)
                graphics.DrawString(point.Date.ToString("dd.MM"), chart.Font, actual, Math.Min(x, chart.Width - 48), bottom + 6);
        }
    }

    private void Refresh_Click(object? sender, EventArgs e) => PageUi.Run(Render);
    private void Export_Click(object? sender, EventArgs e) => PageUi.Run(() => PageUi.ExportCsv(grid, this));
}
