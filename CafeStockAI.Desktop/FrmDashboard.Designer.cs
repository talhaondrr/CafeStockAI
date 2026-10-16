namespace CafeStockAI.Desktop { partial class FrmDashboard { private System.ComponentModel.IContainer components = null; protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            pnlSidebar = new Panel();
            lblSidebarTitle = new Label();
            lblSidebarSubtitle = new Label();
            btnDashboard = new Button();
            btnProducts = new Button();
            btnWarehouses = new Button();
            buttbtnStockMovementson1 = new Button();
            btnStockCount = new Button();
            btnEmployees = new Button();
            btnAIAnalysis = new Button();
            btnReports = new Button();
            btnSettings = new Button();
            btnOperations = new Button();
            sidebarNote = new Panel();
            sidebarNoteText = new Label();
            pnlContent = new Panel();
            overviewLayout = new TableLayoutPanel();
            topBar = new TableLayoutPanel();
            quickSearch = new TextBox();
            operatorLabel = new Label();
            headingPanel = new Panel();
            lblDashboardTitle = new Label();
            lblDashboardDescription = new Label();
            statsLayout = new TableLayoutPanel();
            pnlTotalWarehouses = new CoffeeCard();
            lblTotalWarehousesTitle = new Label();
            lblTotalWarehousesValue = new Label();
            lblTotalWarehousesIcon = new Label();
            cardHint0 = new Label();
            pnlTotalProducts = new CoffeeCard();
            lblTotalProductsTitle = new Label();
            lblTotalProductsValue = new Label();
            lblTotalProductsIcon = new Label();
            cardHint1 = new Label();
            pnlTodayMovements = new CoffeeCard();
            lblTodayMovementsTitle = new Label();
            lblTodayMovementsValue = new Label();
            lblTodayMovementsIcon = new Label();
            cardHint2 = new Label();
            pnlCriticalStock = new CoffeeCard();
            lblCriticalStockTitle = new Label();
            lblCriticalStockValue = new Label();
            lblCriticalStockIcon = new Label();
            cardHint3 = new Label();
            middleLayout = new TableLayoutPanel();
            warehouseCard = new CoffeeCard();
            warehouseTitle = new Label();
            warehouseGrid = new DataGridView();
            whName = new DataGridViewTextBoxColumn();
            whStatus = new DataGridViewTextBoxColumn();
            whDate = new DataGridViewTextBoxColumn();
            whLines = new DataGridViewTextBoxColumn();
            chartCard = new CoffeeCard();
            chartTitle = new Label();
            weeklyChart = new CoffeeChart();
            lowerLayout = new TableLayoutPanel();
            criticalCard = new CoffeeCard();
            criticalTitle = new Label();
            criticalGrid = new DataGridView();
            criticalImage = new DataGridViewImageColumn();
            criticalProduct = new DataGridViewTextBoxColumn();
            criticalWarehouse = new DataGridViewTextBoxColumn();
            criticalQuantity = new DataGridViewTextBoxColumn();
            criticalMinimum = new DataGridViewTextBoxColumn();
            insightCard = new CoffeeCard();
            insightTitle = new Label();
            insightText = new Label();
            pnlRecentMovements = new CoffeeCard();
            lblRecentMovementsTitle = new Label();
            dgvRecentMovements = new DataGridView();
            colDate = new DataGridViewTextBoxColumn();
            colProduct = new DataGridViewTextBoxColumn();
            colMovementType = new DataGridViewTextBoxColumn();
            colQuantity = new DataGridViewTextBoxColumn();
            colWarehouse = new DataGridViewTextBoxColumn();
            sidebarNoteTitle = new Label();
            pnlSidebar.SuspendLayout();
            sidebarNote.SuspendLayout();
            pnlContent.SuspendLayout();
            overviewLayout.SuspendLayout();
            topBar.SuspendLayout();
            headingPanel.SuspendLayout();
            statsLayout.SuspendLayout();
            pnlTotalWarehouses.SuspendLayout();
            pnlTotalProducts.SuspendLayout();
            pnlTodayMovements.SuspendLayout();
            pnlCriticalStock.SuspendLayout();
            middleLayout.SuspendLayout();
            warehouseCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)warehouseGrid).BeginInit();
            chartCard.SuspendLayout();
            lowerLayout.SuspendLayout();
            criticalCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)criticalGrid).BeginInit();
            insightCard.SuspendLayout();
            pnlRecentMovements.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRecentMovements).BeginInit();
            SuspendLayout();
            //
            // pnlSidebar
            //
            pnlSidebar.BackColor = Color.FromArgb(35, 33, 30);
            pnlSidebar.Controls.Add(lblSidebarTitle);
            pnlSidebar.Controls.Add(lblSidebarSubtitle);
            pnlSidebar.Controls.Add(btnDashboard);
            pnlSidebar.Controls.Add(btnProducts);
            pnlSidebar.Controls.Add(btnWarehouses);
            pnlSidebar.Controls.Add(buttbtnStockMovementson1);
            pnlSidebar.Controls.Add(btnStockCount);
            pnlSidebar.Controls.Add(btnEmployees);
            pnlSidebar.Controls.Add(btnAIAnalysis);
            pnlSidebar.Controls.Add(btnReports);
            pnlSidebar.Controls.Add(btnSettings);
            pnlSidebar.Controls.Add(btnOperations);
            btnOperations.Text="Hesap • Onay • Transfer"; btnOperations.Name="btnOperations"; btnOperations.Location=new Point(16,559); btnOperations.Size=new Size(203,42); btnOperations.BackColor=Color.FromArgb(35,33,30); btnOperations.ForeColor=Color.FromArgb(239,232,222); btnOperations.FlatStyle=FlatStyle.Flat; btnOperations.FlatAppearance.BorderSize=0; btnOperations.TextAlign=ContentAlignment.MiddleLeft; btnOperations.Padding=new Padding(10,0,0,0);
            pnlSidebar.Controls.Add(sidebarNote);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 0);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new Size(235, 960);
            pnlSidebar.TabIndex = 1;
            //
            // lblSidebarTitle
            //
            lblSidebarTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblSidebarTitle.ForeColor = Color.FromArgb(250, 242, 229);
            lblSidebarTitle.Location = new Point(20, 28);
            lblSidebarTitle.Name = "lblSidebarTitle";
            lblSidebarTitle.Size = new Size(210, 40);
            lblSidebarTitle.TabIndex = 0;
            lblSidebarTitle.Text = "☕  CafeStock AI";
            //
            // lblSidebarSubtitle
            //
            lblSidebarSubtitle.AutoSize = true;
            lblSidebarSubtitle.Font = new Font("Segoe UI", 9F);
            lblSidebarSubtitle.ForeColor = Color.FromArgb(190, 178, 160);
            lblSidebarSubtitle.Location = new Point(25, 72);
            lblSidebarSubtitle.Name = "lblSidebarSubtitle";
            lblSidebarSubtitle.Size = new Size(153, 15);
            lblSidebarSubtitle.TabIndex = 1;
            lblSidebarSubtitle.Text = "Kahvenin arkasındaki düzen";
            //
            // btnDashboard
            //
            btnDashboard.BackColor = Color.FromArgb(98, 71, 50);
            btnDashboard.Cursor = Cursors.Hand;
            btnDashboard.FlatAppearance.BorderSize = 0;
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.ForeColor = Color.FromArgb(239, 232, 222);
            btnDashboard.Location = new Point(16, 118);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Padding = new Padding(10, 0, 0, 0);
            btnDashboard.Size = new Size(203, 42);
            btnDashboard.TabIndex = 2;
            btnDashboard.Text = "⌂   Genel Bakış";
            btnDashboard.TextAlign = ContentAlignment.MiddleLeft;
            btnDashboard.UseVisualStyleBackColor = false;
            //
            // btnProducts
            //
            btnProducts.BackColor = Color.FromArgb(35, 33, 30);
            btnProducts.Cursor = Cursors.Hand;
            btnProducts.FlatAppearance.BorderSize = 0;
            btnProducts.FlatStyle = FlatStyle.Flat;
            btnProducts.ForeColor = Color.FromArgb(239, 232, 222);
            btnProducts.Location = new Point(16, 167);
            btnProducts.Name = "btnProducts";
            btnProducts.Padding = new Padding(10, 0, 0, 0);
            btnProducts.Size = new Size(203, 42);
            btnProducts.TabIndex = 3;
            btnProducts.Text = "▣   Ürünler";
            btnProducts.TextAlign = ContentAlignment.MiddleLeft;
            btnProducts.UseVisualStyleBackColor = false;
            //
            // btnWarehouses
            //
            btnWarehouses.BackColor = Color.FromArgb(35, 33, 30);
            btnWarehouses.Cursor = Cursors.Hand;
            btnWarehouses.FlatAppearance.BorderSize = 0;
            btnWarehouses.FlatStyle = FlatStyle.Flat;
            btnWarehouses.ForeColor = Color.FromArgb(239, 232, 222);
            btnWarehouses.Location = new Point(16, 216);
            btnWarehouses.Name = "btnWarehouses";
            btnWarehouses.Padding = new Padding(10, 0, 0, 0);
            btnWarehouses.Size = new Size(203, 42);
            btnWarehouses.TabIndex = 4;
            btnWarehouses.Text = "▤   Şube Yönetimi";
            btnWarehouses.TextAlign = ContentAlignment.MiddleLeft;
            btnWarehouses.UseVisualStyleBackColor = false;
            btnWarehouses.Click += btnWarehouses_Click;
            //
            // buttbtnStockMovementson1
            //
            buttbtnStockMovementson1.BackColor = Color.FromArgb(35, 33, 30);
            buttbtnStockMovementson1.Cursor = Cursors.Hand;
            buttbtnStockMovementson1.FlatAppearance.BorderSize = 0;
            buttbtnStockMovementson1.FlatStyle = FlatStyle.Flat;
            buttbtnStockMovementson1.ForeColor = Color.FromArgb(239, 232, 222);
            buttbtnStockMovementson1.Location = new Point(16, 265);
            buttbtnStockMovementson1.Name = "buttbtnStockMovementson1";
            buttbtnStockMovementson1.Padding = new Padding(10, 0, 0, 0);
            buttbtnStockMovementson1.Size = new Size(203, 42);
            buttbtnStockMovementson1.TabIndex = 5;
            buttbtnStockMovementson1.Text = "⇄   Stok Hareketleri";
            buttbtnStockMovementson1.TextAlign = ContentAlignment.MiddleLeft;
            buttbtnStockMovementson1.UseVisualStyleBackColor = false;
            //
            // btnStockCount
            //
            btnStockCount.BackColor = Color.FromArgb(35, 33, 30);
            btnStockCount.Cursor = Cursors.Hand;
            btnStockCount.FlatAppearance.BorderSize = 0;
            btnStockCount.FlatStyle = FlatStyle.Flat;
            btnStockCount.ForeColor = Color.FromArgb(239, 232, 222);
            btnStockCount.Location = new Point(16, 314);
            btnStockCount.Name = "btnStockCount";
            btnStockCount.Padding = new Padding(10, 0, 0, 0);
            btnStockCount.Size = new Size(203, 42);
            btnStockCount.TabIndex = 6;
            btnStockCount.Text = "☑   Sayım Yönetimi";
            btnStockCount.TextAlign = ContentAlignment.MiddleLeft;
            btnStockCount.UseVisualStyleBackColor = false;
            //
            // btnEmployees
            //
            btnEmployees.BackColor = Color.FromArgb(35, 33, 30);
            btnEmployees.Cursor = Cursors.Hand;
            btnEmployees.FlatAppearance.BorderSize = 0;
            btnEmployees.FlatStyle = FlatStyle.Flat;
            btnEmployees.ForeColor = Color.FromArgb(239, 232, 222);
            btnEmployees.Location = new Point(16, 363);
            btnEmployees.Name = "btnEmployees";
            btnEmployees.Padding = new Padding(10, 0, 0, 0);
            btnEmployees.Size = new Size(203, 42);
            btnEmployees.TabIndex = 7;
            btnEmployees.Text = "♙   Personel";
            btnEmployees.TextAlign = ContentAlignment.MiddleLeft;
            btnEmployees.UseVisualStyleBackColor = false;
            //
            // btnAIAnalysis
            //
            btnAIAnalysis.BackColor = Color.FromArgb(35, 33, 30);
            btnAIAnalysis.Cursor = Cursors.Hand;
            btnAIAnalysis.FlatAppearance.BorderSize = 0;
            btnAIAnalysis.FlatStyle = FlatStyle.Flat;
            btnAIAnalysis.ForeColor = Color.FromArgb(239, 232, 222);
            btnAIAnalysis.Location = new Point(16, 412);
            btnAIAnalysis.Name = "btnAIAnalysis";
            btnAIAnalysis.Padding = new Padding(10, 0, 0, 0);
            btnAIAnalysis.Size = new Size(203, 42);
            btnAIAnalysis.TabIndex = 8;
            btnAIAnalysis.Text = "✦   Stok Analizi";
            btnAIAnalysis.TextAlign = ContentAlignment.MiddleLeft;
            btnAIAnalysis.UseVisualStyleBackColor = false;
            //
            // btnReports
            //
            btnReports.BackColor = Color.FromArgb(35, 33, 30);
            btnReports.Cursor = Cursors.Hand;
            btnReports.FlatAppearance.BorderSize = 0;
            btnReports.FlatStyle = FlatStyle.Flat;
            btnReports.ForeColor = Color.FromArgb(239, 232, 222);
            btnReports.Location = new Point(16, 461);
            btnReports.Name = "btnReports";
            btnReports.Padding = new Padding(10, 0, 0, 0);
            btnReports.Size = new Size(203, 42);
            btnReports.TabIndex = 9;
            btnReports.Text = "▥   Raporlar";
            btnReports.TextAlign = ContentAlignment.MiddleLeft;
            btnReports.UseVisualStyleBackColor = false;
            //
            // btnSettings
            //
            btnSettings.BackColor = Color.FromArgb(35, 33, 30);
            btnSettings.Cursor = Cursors.Hand;
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.ForeColor = Color.FromArgb(239, 232, 222);
            btnSettings.Location = new Point(16, 510);
            btnSettings.Name = "btnSettings";
            btnSettings.Padding = new Padding(10, 0, 0, 0);
            btnSettings.Size = new Size(203, 42);
            btnSettings.TabIndex = 10;
            btnSettings.Text = "⚙   Ayarlar";
            btnSettings.TextAlign = ContentAlignment.MiddleLeft;
            btnSettings.UseVisualStyleBackColor = false;
            btnSettings.Click += button2_Click;
            //
            // sidebarNote
            //
            sidebarNote.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            sidebarNote.BackColor = Color.FromArgb(74, 54, 39);
            sidebarNote.Controls.Add(sidebarNoteTitle);
            sidebarNote.Controls.Add(sidebarNoteText);
            sidebarNote.Location = new Point(18, 780);
            sidebarNote.Name = "sidebarNote";
            sidebarNote.Size = new Size(199, 145);
            sidebarNote.TabIndex = 11;
            //
            // sidebarNoteText
            //
            sidebarNoteText.AutoSize = true;
            sidebarNoteText.Font = new Font("Segoe UI", 9F);
            sidebarNoteText.ForeColor = Color.FromArgb(218, 199, 175);
            sidebarNoteText.Location = new Point(15, 105);
            sidebarNoteText.Name = "sidebarNoteText";
            sidebarNoteText.Size = new Size(140, 15);
            sidebarNoteText.TabIndex = 1;
            sidebarNoteText.Text = "Örnek hareketler: [ÖRNEK]";
            //
            // pnlContent
            //
            pnlContent.AutoScroll = true;
            pnlContent.BackColor = Color.FromArgb(243, 237, 227);
            pnlContent.Controls.Add(overviewLayout);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(235, 0);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1205, 960);
            pnlContent.TabIndex = 0;
            //
            // overviewLayout
            //
            overviewLayout.ColumnCount = 1;
            overviewLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            overviewLayout.Controls.Add(topBar, 0, 0);
            overviewLayout.Controls.Add(headingPanel, 0, 1);
            overviewLayout.Controls.Add(statsLayout, 0, 2);
            overviewLayout.Controls.Add(middleLayout, 0, 3);
            overviewLayout.Controls.Add(lowerLayout, 0, 4);
            overviewLayout.Controls.Add(pnlRecentMovements, 0, 5);
            overviewLayout.Dock = DockStyle.Top;
            overviewLayout.Location = new Point(0, 0);
            overviewLayout.Name = "overviewLayout";
            overviewLayout.Padding = new Padding(22, 16, 22, 20);
            overviewLayout.RowCount = 7;
            overviewLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            overviewLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            overviewLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 118F));
            overviewLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 250F));
            overviewLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 230F));
            overviewLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 220F));
            overviewLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
            overviewLayout.Size = new Size(1188, 1000);
            overviewLayout.TabIndex = 0;
            //
            // topBar
            //
            topBar.ColumnCount = 2;
            topBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            topBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            topBar.Controls.Add(quickSearch, 0, 0);
            topBar.Controls.Add(operatorLabel, 1, 0);
            topBar.Dock = DockStyle.Fill;
            topBar.Location = new Point(25, 19);
            topBar.Name = "topBar";
            topBar.Padding = new Padding(6, 10, 6, 10);
            topBar.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            topBar.Size = new Size(1138, 58);
            topBar.TabIndex = 0;
            //
            // quickSearch
            //
            quickSearch.BackColor = Color.FromArgb(255, 253, 249);
            quickSearch.BorderStyle = BorderStyle.FixedSingle;
            quickSearch.Dock = DockStyle.Top;
            quickSearch.Location = new Point(9, 13);
            quickSearch.Name = "quickSearch";
            quickSearch.PlaceholderText = "Ürün kodu veya adı yazın ve Enter tuşuna basın...";
            quickSearch.Size = new Size(669, 25);
            quickSearch.TabIndex = 0;
            //
            // operatorLabel
            //
            operatorLabel.Dock = DockStyle.Fill;
            operatorLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            operatorLabel.ForeColor = Color.FromArgb(93, 66, 44);
            operatorLabel.Location = new Point(684, 10);
            operatorLabel.Name = "operatorLabel";
            operatorLabel.Size = new Size(445, 38);
            operatorLabel.TabIndex = 1;
            operatorLabel.Text = "CafeStock AI  /  Yönetim Paneli";
            operatorLabel.TextAlign = ContentAlignment.MiddleRight;
            //
            // headingPanel
            //
            headingPanel.Controls.Add(lblDashboardTitle);
            headingPanel.Controls.Add(lblDashboardDescription);
            headingPanel.Dock = DockStyle.Fill;
            headingPanel.Location = new Point(25, 83);
            headingPanel.Name = "headingPanel";
            headingPanel.Size = new Size(1138, 58);
            headingPanel.TabIndex = 1;
            //
            // lblDashboardTitle
            //
            lblDashboardTitle.AutoSize = true;
            lblDashboardTitle.Font = new Font("Segoe UI", 21F, FontStyle.Bold);
            lblDashboardTitle.ForeColor = Color.FromArgb(56, 43, 31);
            lblDashboardTitle.Location = new Point(6, 0);
            lblDashboardTitle.Name = "lblDashboardTitle";
            lblDashboardTitle.Size = new Size(322, 38);
            lblDashboardTitle.TabIndex = 0;
            lblDashboardTitle.Text = "İşletmenize genel bakış";
            //
            // lblDashboardDescription
            //
            lblDashboardDescription.AutoSize = true;
            lblDashboardDescription.Font = new Font("Segoe UI", 10F);
            lblDashboardDescription.ForeColor = Color.FromArgb(130, 111, 91);
            lblDashboardDescription.Location = new Point(8, 38);
            lblDashboardDescription.Name = "lblDashboardDescription";
            lblDashboardDescription.Size = new Size(367, 19);
            lblDashboardDescription.TabIndex = 1;
            lblDashboardDescription.Text = "Şubelerinizin, sayımlarınızın ve stoklarınızın güncel durumu.";
            //
            // statsLayout
            //
            statsLayout.ColumnCount = 4;
            statsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsLayout.Controls.Add(pnlTotalWarehouses, 0, 0);
            statsLayout.Controls.Add(pnlTotalProducts, 1, 0);
            statsLayout.Controls.Add(pnlTodayMovements, 2, 0);
            statsLayout.Controls.Add(pnlCriticalStock, 3, 0);
            statsLayout.Dock = DockStyle.Fill;
            statsLayout.Location = new Point(25, 147);
            statsLayout.Name = "statsLayout";
            statsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            statsLayout.Size = new Size(1138, 112);
            statsLayout.TabIndex = 2;
            //
            // pnlTotalWarehouses
            //
            pnlTotalWarehouses.BackColor = Color.FromArgb(255, 253, 249);
            pnlTotalWarehouses.Controls.Add(lblTotalWarehousesTitle);
            pnlTotalWarehouses.Controls.Add(lblTotalWarehousesValue);
            pnlTotalWarehouses.Controls.Add(lblTotalWarehousesIcon);
            pnlTotalWarehouses.Controls.Add(cardHint0);
            pnlTotalWarehouses.Dock = DockStyle.Fill;
            pnlTotalWarehouses.Location = new Point(6, 6);
            pnlTotalWarehouses.Margin = new Padding(6);
            pnlTotalWarehouses.Name = "pnlTotalWarehouses";
            pnlTotalWarehouses.Padding = new Padding(18);
            pnlTotalWarehouses.Size = new Size(272, 100);
            pnlTotalWarehouses.TabIndex = 0;
            //
            // lblTotalWarehousesTitle
            //
            lblTotalWarehousesTitle.AutoSize = true;
            lblTotalWarehousesTitle.Font = new Font("Segoe UI", 10F);
            lblTotalWarehousesTitle.ForeColor = Color.FromArgb(122, 104, 85);
            lblTotalWarehousesTitle.Location = new Point(64, 15);
            lblTotalWarehousesTitle.Name = "lblTotalWarehousesTitle";
            lblTotalWarehousesTitle.Size = new Size(87, 19);
            lblTotalWarehousesTitle.TabIndex = 0;
            lblTotalWarehousesTitle.Text = "Toplam Şube";
            //
            // lblTotalWarehousesValue
            //
            lblTotalWarehousesValue.AutoSize = true;
            lblTotalWarehousesValue.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblTotalWarehousesValue.ForeColor = Color.FromArgb(64, 45, 31);
            lblTotalWarehousesValue.Location = new Point(62, 36);
            lblTotalWarehousesValue.Name = "lblTotalWarehousesValue";
            lblTotalWarehousesValue.Size = new Size(40, 47);
            lblTotalWarehousesValue.TabIndex = 1;
            lblTotalWarehousesValue.Text = "0";
            //
            // lblTotalWarehousesIcon
            //
            lblTotalWarehousesIcon.AutoSize = true;
            lblTotalWarehousesIcon.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTotalWarehousesIcon.ForeColor = Color.FromArgb(148, 101, 63);
            lblTotalWarehousesIcon.Location = new Point(18, 28);
            lblTotalWarehousesIcon.Name = "lblTotalWarehousesIcon";
            lblTotalWarehousesIcon.Size = new Size(49, 45);
            lblTotalWarehousesIcon.TabIndex = 2;
            lblTotalWarehousesIcon.Text = "▤";
            //
            // cardHint0
            //
            cardHint0.AutoSize = true;
            cardHint0.Font = new Font("Segoe UI", 8F);
            cardHint0.ForeColor = Color.FromArgb(147, 129, 109);
            cardHint0.Location = new Point(64, 83);
            cardHint0.Name = "cardHint0";
            cardHint0.Size = new Size(76, 13);
            cardHint0.TabIndex = 3;
            cardHint0.Text = "Şube kayıtları";
            //
            // pnlTotalProducts
            //
            pnlTotalProducts.BackColor = Color.FromArgb(255, 253, 249);
            pnlTotalProducts.Controls.Add(lblTotalProductsTitle);
            pnlTotalProducts.Controls.Add(lblTotalProductsValue);
            pnlTotalProducts.Controls.Add(lblTotalProductsIcon);
            pnlTotalProducts.Controls.Add(cardHint1);
            pnlTotalProducts.Dock = DockStyle.Fill;
            pnlTotalProducts.Location = new Point(290, 6);
            pnlTotalProducts.Margin = new Padding(6);
            pnlTotalProducts.Name = "pnlTotalProducts";
            pnlTotalProducts.Padding = new Padding(18);
            pnlTotalProducts.Size = new Size(272, 100);
            pnlTotalProducts.TabIndex = 1;
            //
            // lblTotalProductsTitle
            //
            lblTotalProductsTitle.AutoSize = true;
            lblTotalProductsTitle.Font = new Font("Segoe UI", 10F);
            lblTotalProductsTitle.ForeColor = Color.FromArgb(122, 104, 85);
            lblTotalProductsTitle.Location = new Point(64, 15);
            lblTotalProductsTitle.Name = "lblTotalProductsTitle";
            lblTotalProductsTitle.Size = new Size(80, 19);
            lblTotalProductsTitle.TabIndex = 0;
            lblTotalProductsTitle.Text = "Kayıtlı Ürün";
            //
            // lblTotalProductsValue
            //
            lblTotalProductsValue.AutoSize = true;
            lblTotalProductsValue.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblTotalProductsValue.ForeColor = Color.FromArgb(64, 45, 31);
            lblTotalProductsValue.Location = new Point(62, 36);
            lblTotalProductsValue.Name = "lblTotalProductsValue";
            lblTotalProductsValue.Size = new Size(40, 47);
            lblTotalProductsValue.TabIndex = 1;
            lblTotalProductsValue.Text = "0";
            //
            // lblTotalProductsIcon
            //
            lblTotalProductsIcon.AutoSize = true;
            lblTotalProductsIcon.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTotalProductsIcon.ForeColor = Color.FromArgb(148, 101, 63);
            lblTotalProductsIcon.Location = new Point(18, 28);
            lblTotalProductsIcon.Name = "lblTotalProductsIcon";
            lblTotalProductsIcon.Size = new Size(49, 45);
            lblTotalProductsIcon.TabIndex = 2;
            lblTotalProductsIcon.Text = "▣";
            //
            // cardHint1
            //
            cardHint1.AutoSize = true;
            cardHint1.Font = new Font("Segoe UI", 8F);
            cardHint1.ForeColor = Color.FromArgb(147, 129, 109);
            cardHint1.Location = new Point(64, 83);
            cardHint1.Name = "cardHint1";
            cardHint1.Size = new Size(75, 13);
            cardHint1.TabIndex = 3;
            cardHint1.Text = "Ürün çeşitleri";
            //
            // pnlTodayMovements
            //
            pnlTodayMovements.BackColor = Color.FromArgb(255, 253, 249);
            pnlTodayMovements.Controls.Add(lblTodayMovementsTitle);
            pnlTodayMovements.Controls.Add(lblTodayMovementsValue);
            pnlTodayMovements.Controls.Add(lblTodayMovementsIcon);
            pnlTodayMovements.Controls.Add(cardHint2);
            pnlTodayMovements.Dock = DockStyle.Fill;
            pnlTodayMovements.Location = new Point(574, 6);
            pnlTodayMovements.Margin = new Padding(6);
            pnlTodayMovements.Name = "pnlTodayMovements";
            pnlTodayMovements.Padding = new Padding(18);
            pnlTodayMovements.Size = new Size(272, 100);
            pnlTodayMovements.TabIndex = 2;
            //
            // lblTodayMovementsTitle
            //
            lblTodayMovementsTitle.AutoSize = true;
            lblTodayMovementsTitle.Font = new Font("Segoe UI", 10F);
            lblTodayMovementsTitle.ForeColor = Color.FromArgb(122, 104, 85);
            lblTodayMovementsTitle.Location = new Point(64, 15);
            lblTodayMovementsTitle.Name = "lblTodayMovementsTitle";
            lblTodayMovementsTitle.Size = new Size(115, 19);
            lblTodayMovementsTitle.TabIndex = 0;
            lblTodayMovementsTitle.Text = "Bugünkü İşlemler";
            //
            // lblTodayMovementsValue
            //
            lblTodayMovementsValue.AutoSize = true;
            lblTodayMovementsValue.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblTodayMovementsValue.ForeColor = Color.FromArgb(64, 45, 31);
            lblTodayMovementsValue.Location = new Point(62, 36);
            lblTodayMovementsValue.Name = "lblTodayMovementsValue";
            lblTodayMovementsValue.Size = new Size(40, 47);
            lblTodayMovementsValue.TabIndex = 1;
            lblTodayMovementsValue.Text = "0";
            //
            // lblTodayMovementsIcon
            //
            lblTodayMovementsIcon.AutoSize = true;
            lblTodayMovementsIcon.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTodayMovementsIcon.ForeColor = Color.FromArgb(148, 101, 63);
            lblTodayMovementsIcon.Location = new Point(18, 28);
            lblTodayMovementsIcon.Name = "lblTodayMovementsIcon";
            lblTodayMovementsIcon.Size = new Size(44, 45);
            lblTodayMovementsIcon.TabIndex = 2;
            lblTodayMovementsIcon.Text = "↗";
            //
            // cardHint2
            //
            cardHint2.AutoSize = true;
            cardHint2.Font = new Font("Segoe UI", 8F);
            cardHint2.ForeColor = Color.FromArgb(147, 129, 109);
            cardHint2.Location = new Point(64, 83);
            cardHint2.Name = "cardHint2";
            cardHint2.Size = new Size(110, 13);
            cardHint2.TabIndex = 3;
            cardHint2.Text = "Stok giriş ve çıkışları";
            //
            // pnlCriticalStock
            //
            pnlCriticalStock.BackColor = Color.FromArgb(255, 253, 249);
            pnlCriticalStock.Controls.Add(lblCriticalStockTitle);
            pnlCriticalStock.Controls.Add(lblCriticalStockValue);
            pnlCriticalStock.Controls.Add(lblCriticalStockIcon);
            pnlCriticalStock.Controls.Add(cardHint3);
            pnlCriticalStock.Dock = DockStyle.Fill;
            pnlCriticalStock.Location = new Point(858, 6);
            pnlCriticalStock.Margin = new Padding(6);
            pnlCriticalStock.Name = "pnlCriticalStock";
            pnlCriticalStock.Padding = new Padding(18);
            pnlCriticalStock.Size = new Size(274, 100);
            pnlCriticalStock.TabIndex = 3;
            //
            // lblCriticalStockTitle
            //
            lblCriticalStockTitle.AutoSize = true;
            lblCriticalStockTitle.Font = new Font("Segoe UI", 10F);
            lblCriticalStockTitle.ForeColor = Color.FromArgb(122, 104, 85);
            lblCriticalStockTitle.Location = new Point(64, 15);
            lblCriticalStockTitle.Name = "lblCriticalStockTitle";
            lblCriticalStockTitle.Size = new Size(71, 19);
            lblCriticalStockTitle.TabIndex = 0;
            lblCriticalStockTitle.Text = "Kritik Stok";
            //
            // lblCriticalStockValue
            //
            lblCriticalStockValue.AutoSize = true;
            lblCriticalStockValue.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblCriticalStockValue.ForeColor = Color.FromArgb(183, 65, 48);
            lblCriticalStockValue.Location = new Point(62, 36);
            lblCriticalStockValue.Name = "lblCriticalStockValue";
            lblCriticalStockValue.Size = new Size(40, 47);
            lblCriticalStockValue.TabIndex = 1;
            lblCriticalStockValue.Text = "0";
            //
            // lblCriticalStockIcon
            //
            lblCriticalStockIcon.AutoSize = true;
            lblCriticalStockIcon.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblCriticalStockIcon.ForeColor = Color.FromArgb(148, 101, 63);
            lblCriticalStockIcon.Location = new Point(18, 28);
            lblCriticalStockIcon.Name = "lblCriticalStockIcon";
            lblCriticalStockIcon.Size = new Size(30, 45);
            lblCriticalStockIcon.TabIndex = 2;
            lblCriticalStockIcon.Text = "!";
            //
            // cardHint3
            //
            cardHint3.AutoSize = true;
            cardHint3.Font = new Font("Segoe UI", 8F);
            cardHint3.ForeColor = Color.FromArgb(147, 129, 109);
            cardHint3.Location = new Point(64, 83);
            cardHint3.Name = "cardHint3";
            cardHint3.Size = new Size(156, 13);
            cardHint3.TabIndex = 3;
            cardHint3.Text = "Eşiğinde veya altında ürünler";
            //
            // middleLayout
            //
            middleLayout.ColumnCount = 2;
            middleLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            middleLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            middleLayout.Controls.Add(warehouseCard, 0, 0);
            middleLayout.Controls.Add(chartCard, 1, 0);
            middleLayout.Dock = DockStyle.Fill;
            middleLayout.Location = new Point(25, 265);
            middleLayout.Name = "middleLayout";
            middleLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            middleLayout.Size = new Size(1138, 244);
            middleLayout.TabIndex = 3;
            //
            // warehouseCard
            //
            warehouseCard.BackColor = Color.FromArgb(255, 253, 249);
            warehouseCard.Controls.Add(warehouseTitle);
            warehouseCard.Controls.Add(warehouseGrid);
            warehouseCard.Dock = DockStyle.Fill;
            warehouseCard.Location = new Point(6, 6);
            warehouseCard.Margin = new Padding(6);
            warehouseCard.Name = "warehouseCard";
            warehouseCard.Padding = new Padding(18);
            warehouseCard.Size = new Size(670, 232);
            warehouseCard.TabIndex = 0;
            //
            // warehouseTitle
            //
            warehouseTitle.Dock = DockStyle.Top;
            warehouseTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            warehouseTitle.ForeColor = Color.FromArgb(62, 46, 32);
            warehouseTitle.Location = new Point(18, 18);
            warehouseTitle.Name = "warehouseTitle";
            warehouseTitle.Size = new Size(634, 36);
            warehouseTitle.TabIndex = 0;
            warehouseTitle.Text = "Şubelerin bugünkü sayım durumu";
            //
            // warehouseGrid
            //
            warehouseGrid.AllowUserToAddRows = false;
            warehouseGrid.AllowUserToDeleteRows = false;
            warehouseGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            warehouseGrid.BackgroundColor = Color.FromArgb(255, 253, 249);
            warehouseGrid.BorderStyle = BorderStyle.None;
            warehouseGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(247, 242, 234);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(111, 94, 76);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(247, 242, 234);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(111, 94, 76);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            warehouseGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            warehouseGrid.ColumnHeadersHeight = 34;
            warehouseGrid.Columns.AddRange(new DataGridViewColumn[] { whName, whStatus, whDate, whLines });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(255, 253, 249);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(52, 44, 36);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(234, 219, 199);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(62, 43, 28);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            warehouseGrid.DefaultCellStyle = dataGridViewCellStyle2;
            warehouseGrid.Dock = DockStyle.Fill;
            warehouseGrid.EnableHeadersVisualStyles = false;
            warehouseGrid.GridColor = Color.FromArgb(239, 232, 222);
            warehouseGrid.Location = new Point(18, 18);
            warehouseGrid.MultiSelect = false;
            warehouseGrid.Name = "warehouseGrid";
            warehouseGrid.ReadOnly = true;
            warehouseGrid.RowHeadersVisible = false;
            warehouseGrid.RowTemplate.Height = 35;
            warehouseGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            warehouseGrid.Size = new Size(634, 196);
            warehouseGrid.TabIndex = 1;
            //
            // whName
            //
            whName.HeaderText = "Şube";
            whName.Name = "whName";
            whName.ReadOnly = true;
            //
            // whStatus
            //
            whStatus.HeaderText = "Durum";
            whStatus.Name = "whStatus";
            whStatus.ReadOnly = true;
            //
            // whDate
            //
            whDate.HeaderText = "Son Sayım";
            whDate.Name = "whDate";
            whDate.ReadOnly = true;
            //
            // whLines
            //
            whLines.HeaderText = "Ürün";
            whLines.Name = "whLines";
            whLines.ReadOnly = true;
            //
            // chartCard
            //
            chartCard.BackColor = Color.FromArgb(255, 253, 249);
            chartCard.Controls.Add(chartTitle);
            chartCard.Controls.Add(weeklyChart);
            chartCard.Dock = DockStyle.Fill;
            chartCard.Location = new Point(688, 6);
            chartCard.Margin = new Padding(6);
            chartCard.Name = "chartCard";
            chartCard.Padding = new Padding(18);
            chartCard.Size = new Size(444, 232);
            chartCard.TabIndex = 1;
            //
            // chartTitle
            //
            chartTitle.Dock = DockStyle.Top;
            chartTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            chartTitle.ForeColor = Color.FromArgb(62, 46, 32);
            chartTitle.Location = new Point(18, 18);
            chartTitle.Name = "chartTitle";
            chartTitle.Size = new Size(408, 36);
            chartTitle.TabIndex = 0;
            chartTitle.Text = "Haftalık stok hareketleri";
            //
            // weeklyChart
            //
            weeklyChart.BackColor = Color.FromArgb(255, 253, 249);
            weeklyChart.Dock = DockStyle.Fill;
            weeklyChart.Location = new Point(18, 18);
            weeklyChart.Name = "weeklyChart";
            weeklyChart.Size = new Size(408, 196);
            weeklyChart.TabIndex = 1;
            //
            // lowerLayout
            //
            lowerLayout.ColumnCount = 2;
            lowerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            lowerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            lowerLayout.Controls.Add(criticalCard, 0, 0);
            lowerLayout.Controls.Add(insightCard, 1, 0);
            lowerLayout.Dock = DockStyle.Fill;
            lowerLayout.Location = new Point(25, 515);
            lowerLayout.Name = "lowerLayout";
            lowerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            lowerLayout.Size = new Size(1138, 224);
            lowerLayout.TabIndex = 4;
            //
            // criticalCard
            //
            criticalCard.BackColor = Color.FromArgb(255, 253, 249);
            criticalCard.Controls.Add(criticalTitle);
            criticalCard.Controls.Add(criticalGrid);
            criticalCard.Dock = DockStyle.Fill;
            criticalCard.Location = new Point(6, 6);
            criticalCard.Margin = new Padding(6);
            criticalCard.Name = "criticalCard";
            criticalCard.Padding = new Padding(18);
            criticalCard.Size = new Size(557, 212);
            criticalCard.TabIndex = 0;
            //
            // criticalTitle
            //
            criticalTitle.Dock = DockStyle.Top;
            criticalTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            criticalTitle.ForeColor = Color.FromArgb(62, 46, 32);
            criticalTitle.Location = new Point(18, 18);
            criticalTitle.Name = "criticalTitle";
            criticalTitle.Size = new Size(521, 34);
            criticalTitle.TabIndex = 0;
            criticalTitle.Text = "Kritik stoklar";
            //
            // criticalGrid
            //
            criticalGrid.AllowUserToAddRows = false;
            criticalGrid.AllowUserToDeleteRows = false;
            criticalGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            criticalGrid.BackgroundColor = Color.FromArgb(255, 253, 249);
            criticalGrid.BorderStyle = BorderStyle.None;
            criticalGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(247, 242, 234);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(111, 94, 76);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(247, 242, 234);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(111, 94, 76);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            criticalGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            criticalGrid.ColumnHeadersHeight = 34;
            criticalGrid.Columns.AddRange(new DataGridViewColumn[] { criticalImage, criticalProduct, criticalWarehouse, criticalQuantity, criticalMinimum });
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(255, 253, 249);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(52, 44, 36);
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(234, 219, 199);
            dataGridViewCellStyle5.SelectionForeColor = Color.FromArgb(62, 43, 28);
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            criticalGrid.DefaultCellStyle = dataGridViewCellStyle5;
            criticalGrid.Dock = DockStyle.Fill;
            criticalGrid.EnableHeadersVisualStyles = false;
            criticalGrid.GridColor = Color.FromArgb(239, 232, 222);
            criticalGrid.Location = new Point(18, 18);
            criticalGrid.MultiSelect = false;
            criticalGrid.Name = "criticalGrid";
            criticalGrid.ReadOnly = true;
            criticalGrid.RowHeadersVisible = false;
            criticalGrid.RowTemplate.Height = 60;
            criticalGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            criticalGrid.Size = new Size(521, 176);
            criticalGrid.TabIndex = 1;
            //
            // criticalImage
            //
            criticalImage.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.NullValue = null;
            dataGridViewCellStyle4.Padding = new Padding(5);
            criticalImage.DefaultCellStyle = dataGridViewCellStyle4;
            criticalImage.HeaderText = "";
            criticalImage.ImageLayout = DataGridViewImageCellLayout.Zoom;
            criticalImage.Name = "criticalImage";
            criticalImage.ReadOnly = true;
            criticalImage.Width = 65;
            //
            // criticalProduct
            //
            criticalProduct.HeaderText = "Ürün";
            criticalProduct.Name = "criticalProduct";
            criticalProduct.ReadOnly = true;
            //
            // criticalWarehouse
            //
            criticalWarehouse.HeaderText = "Şube";
            criticalWarehouse.Name = "criticalWarehouse";
            criticalWarehouse.ReadOnly = true;
            //
            // criticalQuantity
            //
            criticalQuantity.HeaderText = "Stok";
            criticalQuantity.Name = "criticalQuantity";
            criticalQuantity.ReadOnly = true;
            //
            // criticalMinimum
            //
            criticalMinimum.HeaderText = "Alt Sınır";
            criticalMinimum.Name = "criticalMinimum";
            criticalMinimum.ReadOnly = true;
            //
            // insightCard
            //
            insightCard.BackColor = Color.FromArgb(255, 253, 249);
            insightCard.Controls.Add(insightTitle);
            insightCard.Controls.Add(insightText);
            insightCard.Dock = DockStyle.Fill;
            insightCard.Location = new Point(575, 6);
            insightCard.Margin = new Padding(6);
            insightCard.Name = "insightCard";
            insightCard.Padding = new Padding(18);
            insightCard.Size = new Size(557, 212);
            insightCard.TabIndex = 1;
            //
            // insightTitle
            //
            insightTitle.Dock = DockStyle.Top;
            insightTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            insightTitle.ForeColor = Color.FromArgb(62, 46, 32);
            insightTitle.Location = new Point(18, 18);
            insightTitle.Name = "insightTitle";
            insightTitle.Size = new Size(521, 35);
            insightTitle.TabIndex = 0;
            insightTitle.Text = "Stok önerileri";
            //
            // insightText
            //
            insightText.Dock = DockStyle.Fill;
            insightText.Font = new Font("Segoe UI", 10F);
            insightText.ForeColor = Color.FromArgb(118, 94, 70);
            insightText.Location = new Point(18, 18);
            insightText.Name = "insightText";
            insightText.Padding = new Padding(0, 8, 0, 0);
            insightText.Size = new Size(521, 176);
            insightText.TabIndex = 1;
            insightText.Text = "Kayıtlarınızı ekledikçe stok önerileri burada görünecek.";
            //
            // pnlRecentMovements
            //
            pnlRecentMovements.BackColor = Color.FromArgb(255, 253, 249);
            pnlRecentMovements.Controls.Add(lblRecentMovementsTitle);
            pnlRecentMovements.Controls.Add(dgvRecentMovements);
            pnlRecentMovements.Dock = DockStyle.Fill;
            pnlRecentMovements.Location = new Point(28, 748);
            pnlRecentMovements.Margin = new Padding(6);
            pnlRecentMovements.Name = "pnlRecentMovements";
            pnlRecentMovements.Padding = new Padding(18);
            pnlRecentMovements.Size = new Size(1132, 208);
            pnlRecentMovements.TabIndex = 5;
            //
            // lblRecentMovementsTitle
            //
            lblRecentMovementsTitle.Dock = DockStyle.Top;
            lblRecentMovementsTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblRecentMovementsTitle.ForeColor = Color.FromArgb(62, 46, 32);
            lblRecentMovementsTitle.Location = new Point(18, 18);
            lblRecentMovementsTitle.Name = "lblRecentMovementsTitle";
            lblRecentMovementsTitle.Size = new Size(1096, 34);
            lblRecentMovementsTitle.TabIndex = 0;
            lblRecentMovementsTitle.Text = "Son stok hareketleri";
            //
            // dgvRecentMovements
            //
            dgvRecentMovements.AllowUserToAddRows = false;
            dgvRecentMovements.AllowUserToDeleteRows = false;
            dgvRecentMovements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRecentMovements.BackgroundColor = Color.FromArgb(255, 253, 249);
            dgvRecentMovements.BorderStyle = BorderStyle.None;
            dgvRecentMovements.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.FromArgb(247, 242, 234);
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(111, 94, 76);
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(247, 242, 234);
            dataGridViewCellStyle6.SelectionForeColor = Color.FromArgb(111, 94, 76);
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dgvRecentMovements.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dgvRecentMovements.ColumnHeadersHeight = 34;
            dgvRecentMovements.Columns.AddRange(new DataGridViewColumn[] { colDate, colProduct, colMovementType, colQuantity, colWarehouse });
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = Color.FromArgb(255, 253, 249);
            dataGridViewCellStyle7.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle7.ForeColor = Color.FromArgb(52, 44, 36);
            dataGridViewCellStyle7.SelectionBackColor = Color.FromArgb(234, 219, 199);
            dataGridViewCellStyle7.SelectionForeColor = Color.FromArgb(62, 43, 28);
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.False;
            dgvRecentMovements.DefaultCellStyle = dataGridViewCellStyle7;
            dgvRecentMovements.Dock = DockStyle.Fill;
            dgvRecentMovements.EnableHeadersVisualStyles = false;
            dgvRecentMovements.GridColor = Color.FromArgb(239, 232, 222);
            dgvRecentMovements.Location = new Point(18, 18);
            dgvRecentMovements.MultiSelect = false;
            dgvRecentMovements.Name = "dgvRecentMovements";
            dgvRecentMovements.ReadOnly = true;
            dgvRecentMovements.RowHeadersVisible = false;
            dgvRecentMovements.RowTemplate.Height = 35;
            dgvRecentMovements.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecentMovements.Size = new Size(1096, 172);
            dgvRecentMovements.TabIndex = 1;
            //
            // colDate
            //
            colDate.HeaderText = "Tarih";
            colDate.Name = "colDate";
            colDate.ReadOnly = true;
            //
            // colProduct
            //
            colProduct.HeaderText = "Ürün";
            colProduct.Name = "colProduct";
            colProduct.ReadOnly = true;
            //
            // colMovementType
            //
            colMovementType.HeaderText = "İşlem";
            colMovementType.Name = "colMovementType";
            colMovementType.ReadOnly = true;
            //
            // colQuantity
            //
            colQuantity.HeaderText = "Miktar";
            colQuantity.Name = "colQuantity";
            colQuantity.ReadOnly = true;
            //
            // colWarehouse
            //
            colWarehouse.HeaderText = "Şube / Alan";
            colWarehouse.Name = "colWarehouse";
            colWarehouse.ReadOnly = true;
            //
            // sidebarNoteTitle
            //
            sidebarNoteTitle.AutoSize = true;
            sidebarNoteTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            sidebarNoteTitle.ForeColor = Color.FromArgb(251, 243, 230);
            sidebarNoteTitle.Location = new Point(15, 20);
            sidebarNoteTitle.Name = "sidebarNoteTitle";
            sidebarNoteTitle.Size = new Size(190, 50);
            sidebarNoteTitle.TabIndex = 0;
            sidebarNoteTitle.Text = "Daha düzenli stok.\nDaha verimli işletme.";
            //
            // FrmDashboard
            //
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 237, 227);
            ClientSize = new Size(1440, 960);
            Controls.Add(pnlContent);
            Controls.Add(pnlSidebar);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(1180, 780);
            Name = "FrmDashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CafeStock AI - Yönetim Paneli";
            WindowState = FormWindowState.Maximized;
            pnlSidebar.ResumeLayout(false);
            pnlSidebar.PerformLayout();
            sidebarNote.ResumeLayout(false);
            sidebarNote.PerformLayout();
            pnlContent.ResumeLayout(false);
            overviewLayout.ResumeLayout(false);
            topBar.ResumeLayout(false);
            topBar.PerformLayout();
            headingPanel.ResumeLayout(false);
            headingPanel.PerformLayout();
            statsLayout.ResumeLayout(false);
            pnlTotalWarehouses.ResumeLayout(false);
            pnlTotalWarehouses.PerformLayout();
            pnlTotalProducts.ResumeLayout(false);
            pnlTotalProducts.PerformLayout();
            pnlTodayMovements.ResumeLayout(false);
            pnlTodayMovements.PerformLayout();
            pnlCriticalStock.ResumeLayout(false);
            pnlCriticalStock.PerformLayout();
            middleLayout.ResumeLayout(false);
            warehouseCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)warehouseGrid).EndInit();
            chartCard.ResumeLayout(false);
            lowerLayout.ResumeLayout(false);
            criticalCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)criticalGrid).EndInit();
            insightCard.ResumeLayout(false);
            pnlRecentMovements.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvRecentMovements).EndInit();
            ResumeLayout(false);
        }
        private Panel pnlSidebar;
private Label lblSidebarTitle;
private Label lblSidebarSubtitle;
private Button btnDashboard;
private Button btnProducts;
private Button btnWarehouses;
private Button buttbtnStockMovementson1;
private Button btnStockCount;
private Button btnEmployees;
private Button btnAIAnalysis;
private Button btnReports;
private Button btnSettings;
private Button btnOperations;
private Panel sidebarNote;
private Label sidebarNoteText;
private Panel pnlContent;
private TableLayoutPanel overviewLayout;
private TableLayoutPanel topBar;
private TextBox quickSearch;
private Label operatorLabel;
private Panel headingPanel;
private Label lblDashboardTitle;
private Label lblDashboardDescription;
private TableLayoutPanel statsLayout;
private CoffeeCard pnlTotalWarehouses;
private Label lblTotalWarehousesTitle;
private Label lblTotalWarehousesValue;
private Label lblTotalWarehousesIcon;
private Label cardHint0;
private CoffeeCard pnlTotalProducts;
private Label lblTotalProductsTitle;
private Label lblTotalProductsValue;
private Label lblTotalProductsIcon;
private Label cardHint1;
private CoffeeCard pnlTodayMovements;
private Label lblTodayMovementsTitle;
private Label lblTodayMovementsValue;
private Label lblTodayMovementsIcon;
private Label cardHint2;
private CoffeeCard pnlCriticalStock;
private Label lblCriticalStockTitle;
private Label lblCriticalStockValue;
private Label lblCriticalStockIcon;
private Label cardHint3;
private TableLayoutPanel middleLayout;
private CoffeeCard warehouseCard;
private Label warehouseTitle;
private DataGridView warehouseGrid;
private DataGridViewTextBoxColumn whName;
private DataGridViewTextBoxColumn whStatus;
private DataGridViewTextBoxColumn whDate;
private DataGridViewTextBoxColumn whLines;
private CoffeeCard chartCard;
private Label chartTitle;
private CoffeeChart weeklyChart;
private TableLayoutPanel lowerLayout;
private DataGridViewImageColumn criticalImage;
private CoffeeCard criticalCard;
private Label criticalTitle;
private DataGridView criticalGrid;
private DataGridViewTextBoxColumn criticalProduct;
private DataGridViewTextBoxColumn criticalWarehouse;
private DataGridViewTextBoxColumn criticalQuantity;
private DataGridViewTextBoxColumn criticalMinimum;
private CoffeeCard insightCard;
private Label insightTitle;
private Label insightText;
private CoffeeCard pnlRecentMovements;
private Label lblRecentMovementsTitle;
private DataGridView dgvRecentMovements;
private DataGridViewTextBoxColumn colDate;
private DataGridViewTextBoxColumn colProduct;
private DataGridViewTextBoxColumn colMovementType;
private DataGridViewTextBoxColumn colQuantity;
private DataGridViewTextBoxColumn colWarehouse;
        private Label sidebarNoteTitle;
    } }
