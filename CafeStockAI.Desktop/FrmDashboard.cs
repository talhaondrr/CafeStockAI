namespace CafeStockAI.Desktop;
public partial class FrmDashboard : Form
{
    private readonly Dictionary<Type, Form> pages = new();
    private Control[] overviewControls = Array.Empty<Control>();
    private Button[] navigation = Array.Empty<Button>();
    public FrmDashboard()
    {
        InitializeComponent(); QuantityText.Setup(this);
        overviewControls = pnlContent.Controls.Cast<Control>().ToArray();
        navigation = new[] { btnDashboard, btnProducts, btnWarehouses, buttbtnStockMovementson1, btnStockCount, btnEmployees, btnReports, btnAIAnalysis, btnSettings, btnOperations };
        btnProducts.Click += (_, _) => ShowProducts();
        btnDashboard.Click += (_, _) => ShowOverview();
        buttbtnStockMovementson1.Click += (_, _) => OpenPage<FrmStockMovements>(buttbtnStockMovementson1);
        btnStockCount.Click += (_, _) => OpenPage<FrmStockCount>(btnStockCount);
        btnEmployees.Click += (_, _) => OpenPage<FrmEmployees>(btnEmployees);
        btnReports.Click += (_, _) => OpenPage<FrmReports>(btnReports);
        btnAIAnalysis.Click += (_, _) => OpenPage<FrmAnalysis>(btnAIAnalysis);
        quickSearch.KeyDown += (_, e) =>
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            ShowProducts();
            if (pages.TryGetValue(typeof(FrmProducts), out var page)) ((FrmProducts)page).SearchProducts(quickSearch.Text);
        };
        btnOperations.Click += (_,_) => OpenPage<FrmOperations>(btnOperations);
        Shown += (_, _) => RefreshOverview();
    }
    private void MarkActive(Button active)
    {
        foreach (var button in navigation) button.BackColor = button == active ? Color.FromArgb(98,71,50) : Color.FromArgb(35,33,30);
    }
    internal void OpenPage<T>(Button active) where T : Form, IInventoryPage, new()
    {
        PageUi.Run(() =>
        {
            // Create the next page before hiding the current view so construction failures leave navigation usable.
            if (!pages.TryGetValue(typeof(T), out var page) || page.IsDisposed)
            {
                page = new T { TopLevel = false, FormBorderStyle = FormBorderStyle.None, MinimumSize = Size.Empty, Dock = DockStyle.Fill };
                pages[typeof(T)] = page; pnlContent.Controls.Add(page);
            }
            pnlContent.SuspendLayout();
            try
            {
                foreach (var control in overviewControls) control.Hide();
                foreach (var existing in pages.Values) existing.Hide();
                pnlContent.AutoScrollPosition = Point.Empty; pnlContent.AutoScroll = false;
                page.Show(); page.BringToFront(); ((IInventoryPage)page).RefreshData(); MarkActive(active);
            }
            finally { pnlContent.ResumeLayout(true); }
        });
    }
    internal void ShowProducts() => OpenPage<FrmProducts>(btnProducts);
    internal void ShowOverview()
    {
        pnlContent.SuspendLayout();
        try
        {
            foreach (var page in pages.Values) page.Hide();
            pnlContent.AutoScroll = true;
            foreach (var control in overviewControls) control.Show();
            MarkActive(btnDashboard); RefreshOverview();
        }
        finally { pnlContent.ResumeLayout(true); }
    }
    private void RefreshOverview() => PageUi.Run(() =>
    {
        var data = Access.View(); var products = ProductStore.Load();
        Text = data.Settings.BusinessName + " - Yönetim Paneli";
        lblTotalProductsValue.Text = products.Count.ToString();
        lblTotalWarehousesValue.Text = BranchStock.AllNames(data).Count.ToString();
        lblTodayMovementsValue.Text = data.Movements.Count(m => m.Date.Date == DateTime.Today).ToString();
        lblCriticalStockValue.Text = products.Count(p => BranchStock.AllNames(data).Any(b => BranchStock.Balance(data,p.Id,b,"Bar")+BranchStock.Balance(data,p.Id,b,"Depo") <= p.MinimumStock)).ToString();
        operatorLabel.Text = Access.Current is Employee signedIn ? signedIn.Name + " / " + signedIn.Role : string.IsNullOrWhiteSpace(data.Settings.OperatorName) ? data.Settings.BusinessName + "  /  Yönetim Paneli" : data.Settings.OperatorName + "  /  " + data.Settings.BusinessName;
        warehouseGrid.Rows.Clear();
        foreach (var branch in BranchStock.AllNames(data))
        {
            var last = data.Counts.Where(c => c.BranchName == branch || (string.IsNullOrWhiteSpace(c.BranchName) && data.Warehouses.Any(w => w.Id == c.WarehouseId && w.BranchName == branch))).OrderByDescending(c => c.Date).FirstOrDefault();
            warehouseGrid.Rows.Add(branch,last?.Date.Date == DateTime.Today ? last.Status : "Sayım bekleniyor",last?.Date.ToString("dd.MM HH:mm") ?? "Henüz sayılmadı",last?.Lines.Count.ToString() ?? "—");
        }
        weeklyChart.Values = Enumerable.Range(0,7).Select(i => data.Movements.Count(m => m.Date.Date == DateTime.Today.AddDays(i - 6))).ToArray();
        criticalGrid.Rows.Clear();
        var criticalRows = (from branch in BranchStock.AllNames(data) from product in products
                            let balance = BranchStock.Balance(data,product.Id,branch,"Bar") + BranchStock.Balance(data,product.Id,branch,"Depo")
                            where balance <= product.MinimumStock orderby balance
                            select new { Product = product, Branch = branch, Balance = balance }).ToList();
        foreach (var item in criticalRows.Take(8))
            criticalGrid.Rows.Add(ProductImages.Get(item.Product),item.Product.Name,item.Branch,$"{QuantityText.Format(item.Balance)} {item.Product.UnitDisplay}",$"{QuantityText.Format(item.Product.MinimumStock)} {item.Product.UnitDisplay}");
        var todayCounts = data.Counts.Count(c => c.Date.Date == DateTime.Today && c.Status=="Onaylandı");
        var recentOutputs = data.Movements.Count(m => m.Type == "Çıkış" && m.Date >= DateTime.Today.AddDays(-6));
        insightText.Text = products.Count == 0 || data.Warehouses.Count == 0
            ? "Başlamak için ürünlerinizi ve şubelerinizi tanımlayın.\n\nArdından stok girişi yapıp ilk sayımınızı başlatabilirsiniz."
            : $"STOK TAKİBİ\n{criticalRows.Count} ürün/şube kaydı kritik stok seviyesinde.\n\nSAYIM PLANI\nBugün {todayCounts} sayım tamamlandı.\n\nHAREKET ÖZETİ\nSon 7 günde {recentOutputs} stok çıkışı kaydedildi.";
        dgvRecentMovements.Rows.Clear();
        foreach (var movement in data.Movements.OrderByDescending(m => m.Date).Take(15))
            dgvRecentMovements.Rows.Add(movement.Date.ToString("g"), PageUi.ProductName(products, movement.ProductId), movement.Type, movement.Quantity.ToString("0.############################",System.Globalization.CultureInfo.InvariantCulture), PageUi.WarehouseName(data, movement.WarehouseId));
        warehouseGrid.ClearSelection(); criticalGrid.ClearSelection(); dgvRecentMovements.ClearSelection();
    });
    internal void CheckAllPages()
    {
        OpenPage<FrmWarehouses>(btnWarehouses); OpenPage<FrmEmployees>(btnEmployees);
        OpenPage<FrmStockMovements>(buttbtnStockMovementson1); OpenPage<FrmStockCount>(btnStockCount);
        OpenPage<FrmReports>(btnReports); OpenPage<FrmAnalysis>(btnAIAnalysis); OpenPage<FrmSettings>(btnSettings); OpenPage<FrmOperations>(btnOperations);
        ShowProducts(); ShowOverview();
    }
    private void btnWarehouses_Click(object sender, EventArgs e) => OpenPage<FrmWarehouses>(btnWarehouses);
    private void button2_Click(object sender, EventArgs e) => OpenPage<FrmSettings>(btnSettings);
    private void lblTotalWarehousesTitle_Click(object sender, EventArgs e) { }
    private void pnlContent_Paint(object sender, PaintEventArgs e) { }
}
