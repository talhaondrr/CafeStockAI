namespace CafeStockAI.Desktop
{
    partial class FrmDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            pnlSidebar = new Panel();
            btnReports = new Button();
            btnSettings = new Button();
            btnAIAnalysis = new Button();
            btnEmployees = new Button();
            btnStockCount = new Button();
            buttbtnStockMovementson1 = new Button();
            btnWarehouses = new Button();
            btnProducts = new Button();
            btnDashboard = new Button();
            lblSidebarSubtitle = new Label();
            lblSidebarTitle = new Label();
            pnlContent = new Panel();
            pnlTodayMovements = new Panel();
            lblTodayMovementsValue = new Label();
            lblTodayMovementsTitle = new Label();
            pnlTotalWarehouses = new Panel();
            lblTotalWarehousesValue = new Label();
            lblTotalWarehousesTitle = new Label();
            pnlCriticalStock = new Panel();
            lblCriticalStockValue = new Label();
            lblCriticalStockTitle = new Label();
            pnlTotalProducts = new Panel();
            lblTotalProductsValue = new Label();
            lblTotalProductsTitle = new Label();
            lblDashboardDescription = new Label();
            lblDashboardTitle = new Label();
            lblTotalProductsIcon = new Label();
            lblCriticalStockIcon = new Label();
            lblTotalWarehousesIcon = new Label();
            lblTodayMovementsIcon = new Label();
            pnlRecentMovements = new Panel();
            lblRecentMovementsTitle = new Label();
            dgvRecentMovements = new DataGridView();
            colDate = new DataGridViewTextBoxColumn();
            colProduct = new DataGridViewTextBoxColumn();
            colMovementType = new DataGridViewTextBoxColumn();
            colQuantity = new DataGridViewTextBoxColumn();
            colWarehouse = new DataGridViewTextBoxColumn();
            pnlSidebar.SuspendLayout();
            pnlContent.SuspendLayout();
            pnlTodayMovements.SuspendLayout();
            pnlTotalWarehouses.SuspendLayout();
            pnlCriticalStock.SuspendLayout();
            pnlTotalProducts.SuspendLayout();
            pnlRecentMovements.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRecentMovements).BeginInit();
            SuspendLayout();
            // 
            // pnlSidebar
            // 
            pnlSidebar.AutoScroll = true;
            pnlSidebar.BackColor = Color.FromArgb(25, 35, 55);
            pnlSidebar.Controls.Add(btnReports);
            pnlSidebar.Controls.Add(btnSettings);
            pnlSidebar.Controls.Add(btnAIAnalysis);
            pnlSidebar.Controls.Add(btnEmployees);
            pnlSidebar.Controls.Add(btnStockCount);
            pnlSidebar.Controls.Add(buttbtnStockMovementson1);
            pnlSidebar.Controls.Add(btnWarehouses);
            pnlSidebar.Controls.Add(btnProducts);
            pnlSidebar.Controls.Add(btnDashboard);
            pnlSidebar.Controls.Add(lblSidebarSubtitle);
            pnlSidebar.Controls.Add(lblSidebarTitle);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 0);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new Size(250, 761);
            pnlSidebar.TabIndex = 0;
            // 
            // btnReports
            // 
            btnReports.BackColor = Color.FromArgb(35, 48, 70);
            btnReports.FlatAppearance.BorderSize = 0;
            btnReports.FlatStyle = FlatStyle.Flat;
            btnReports.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnReports.ForeColor = Color.White;
            btnReports.Location = new Point(10, 520);
            btnReports.Name = "btnReports";
            btnReports.Size = new Size(230, 45);
            btnReports.TabIndex = 10;
            btnReports.Text = "▥ Raporlar";
            btnReports.TextAlign = ContentAlignment.MiddleLeft;
            btnReports.UseVisualStyleBackColor = false;
            // 
            // btnSettings
            // 
            btnSettings.BackColor = Color.FromArgb(35, 48, 70);
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnSettings.ForeColor = Color.White;
            btnSettings.Location = new Point(10, 575);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(230, 45);
            btnSettings.TabIndex = 9;
            btnSettings.Text = "⚙ Ayarlar";
            btnSettings.TextAlign = ContentAlignment.MiddleLeft;
            btnSettings.UseVisualStyleBackColor = false;
            btnSettings.Click += button2_Click;
            // 
            // btnAIAnalysis
            // 
            btnAIAnalysis.BackColor = Color.FromArgb(35, 48, 70);
            btnAIAnalysis.FlatAppearance.BorderSize = 0;
            btnAIAnalysis.FlatStyle = FlatStyle.Flat;
            btnAIAnalysis.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnAIAnalysis.ForeColor = Color.White;
            btnAIAnalysis.Location = new Point(10, 465);
            btnAIAnalysis.Name = "btnAIAnalysis";
            btnAIAnalysis.Size = new Size(230, 45);
            btnAIAnalysis.TabIndex = 8;
            btnAIAnalysis.Text = "✦ Yapay Zekâ Analizi";
            btnAIAnalysis.TextAlign = ContentAlignment.MiddleLeft;
            btnAIAnalysis.UseVisualStyleBackColor = false;
            // 
            // btnEmployees
            // 
            btnEmployees.BackColor = Color.FromArgb(35, 48, 70);
            btnEmployees.FlatAppearance.BorderSize = 0;
            btnEmployees.FlatStyle = FlatStyle.Flat;
            btnEmployees.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnEmployees.ForeColor = Color.White;
            btnEmployees.Location = new Point(10, 410);
            btnEmployees.Name = "btnEmployees";
            btnEmployees.Size = new Size(230, 45);
            btnEmployees.TabIndex = 7;
            btnEmployees.Text = "♙ Personel Yönetimi";
            btnEmployees.TextAlign = ContentAlignment.MiddleLeft;
            btnEmployees.UseVisualStyleBackColor = false;
            // 
            // btnStockCount
            // 
            btnStockCount.BackColor = Color.FromArgb(35, 48, 70);
            btnStockCount.FlatAppearance.BorderSize = 0;
            btnStockCount.FlatStyle = FlatStyle.Flat;
            btnStockCount.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnStockCount.ForeColor = Color.White;
            btnStockCount.Location = new Point(10, 355);
            btnStockCount.Name = "btnStockCount";
            btnStockCount.Size = new Size(230, 45);
            btnStockCount.TabIndex = 6;
            btnStockCount.Text = "☑ Stok Sayımı";
            btnStockCount.TextAlign = ContentAlignment.MiddleLeft;
            btnStockCount.UseVisualStyleBackColor = false;
            // 
            // buttbtnStockMovementson1
            // 
            buttbtnStockMovementson1.BackColor = Color.FromArgb(35, 48, 70);
            buttbtnStockMovementson1.FlatAppearance.BorderSize = 0;
            buttbtnStockMovementson1.FlatStyle = FlatStyle.Flat;
            buttbtnStockMovementson1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            buttbtnStockMovementson1.ForeColor = Color.White;
            buttbtnStockMovementson1.Location = new Point(10, 300);
            buttbtnStockMovementson1.Name = "buttbtnStockMovementson1";
            buttbtnStockMovementson1.Size = new Size(230, 45);
            buttbtnStockMovementson1.TabIndex = 5;
            buttbtnStockMovementson1.Text = "⇄ Stok Hareketleri";
            buttbtnStockMovementson1.TextAlign = ContentAlignment.MiddleLeft;
            buttbtnStockMovementson1.UseVisualStyleBackColor = false;
            // 
            // btnWarehouses
            // 
            btnWarehouses.BackColor = Color.FromArgb(35, 48, 70);
            btnWarehouses.FlatAppearance.BorderSize = 0;
            btnWarehouses.FlatStyle = FlatStyle.Flat;
            btnWarehouses.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnWarehouses.ForeColor = Color.White;
            btnWarehouses.Location = new Point(10, 245);
            btnWarehouses.Name = "btnWarehouses";
            btnWarehouses.Size = new Size(230, 45);
            btnWarehouses.TabIndex = 4;
            btnWarehouses.Text = "▤ Depo Yönetimi";
            btnWarehouses.TextAlign = ContentAlignment.MiddleLeft;
            btnWarehouses.UseVisualStyleBackColor = false;
            btnWarehouses.Click += btnWarehouses_Click;
            // 
            // btnProducts
            // 
            btnProducts.BackColor = Color.FromArgb(35, 48, 70);
            btnProducts.FlatAppearance.BorderSize = 0;
            btnProducts.FlatStyle = FlatStyle.Flat;
            btnProducts.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnProducts.ForeColor = Color.White;
            btnProducts.Location = new Point(10, 190);
            btnProducts.Name = "btnProducts";
            btnProducts.Size = new Size(230, 45);
            btnProducts.TabIndex = 3;
            btnProducts.Text = "▣  Ürün Yönetimi";
            btnProducts.TextAlign = ContentAlignment.MiddleLeft;
            btnProducts.UseVisualStyleBackColor = false;
            // 
            // btnDashboard
            // 
            btnDashboard.BackColor = Color.FromArgb(29, 173, 145);
            btnDashboard.FlatAppearance.BorderSize = 0;
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnDashboard.ForeColor = Color.White;
            btnDashboard.Location = new Point(10, 135);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Size = new Size(230, 45);
            btnDashboard.TabIndex = 1;
            btnDashboard.Text = "⌂  Dashboard";
            btnDashboard.TextAlign = ContentAlignment.MiddleLeft;
            btnDashboard.UseVisualStyleBackColor = false;
            // 
            // lblSidebarSubtitle
            // 
            lblSidebarSubtitle.AutoSize = true;
            lblSidebarSubtitle.BackColor = Color.Transparent;
            lblSidebarSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 162);
            lblSidebarSubtitle.ForeColor = Color.FromArgb(180, 195, 215);
            lblSidebarSubtitle.Location = new Point(23, 75);
            lblSidebarSubtitle.Name = "lblSidebarSubtitle";
            lblSidebarSubtitle.Size = new Size(100, 15);
            lblSidebarSubtitle.TabIndex = 2;
            lblSidebarSubtitle.Text = "YÖNETİM PANELİ";
            // 
            // lblSidebarTitle
            // 
            lblSidebarTitle.AutoSize = true;
            lblSidebarTitle.BackColor = Color.Transparent;
            lblSidebarTitle.Font = new Font("Segoe UI", 18.75F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblSidebarTitle.ForeColor = Color.White;
            lblSidebarTitle.Location = new Point(20, 35);
            lblSidebarTitle.Name = "lblSidebarTitle";
            lblSidebarTitle.Size = new Size(165, 35);
            lblSidebarTitle.TabIndex = 1;
            lblSidebarTitle.Text = "CafeStock AI";
            // 
            // pnlContent
            // 
            pnlContent.AutoScroll = true;
            pnlContent.Controls.Add(pnlRecentMovements);
            pnlContent.Controls.Add(pnlTodayMovements);
            pnlContent.Controls.Add(pnlTotalWarehouses);
            pnlContent.Controls.Add(pnlCriticalStock);
            pnlContent.Controls.Add(pnlTotalProducts);
            pnlContent.Controls.Add(lblDashboardDescription);
            pnlContent.Controls.Add(lblDashboardTitle);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(250, 0);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1014, 761);
            pnlContent.TabIndex = 1;
            pnlContent.Paint += pnlContent_Paint;
            // 
            // pnlTodayMovements
            // 
            pnlTodayMovements.BackColor = Color.White;
            pnlTodayMovements.Controls.Add(lblTodayMovementsIcon);
            pnlTodayMovements.Controls.Add(lblTodayMovementsValue);
            pnlTodayMovements.Controls.Add(lblTodayMovementsTitle);
            pnlTodayMovements.Location = new Point(283, 300);
            pnlTodayMovements.Name = "pnlTodayMovements";
            pnlTodayMovements.Size = new Size(210, 130);
            pnlTodayMovements.TabIndex = 4;
            // 
            // lblTodayMovementsValue
            // 
            lblTodayMovementsValue.AutoSize = true;
            lblTodayMovementsValue.BackColor = Color.Transparent;
            lblTodayMovementsValue.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblTodayMovementsValue.ForeColor = Color.FromArgb(25, 35, 55);
            lblTodayMovementsValue.Location = new Point(20, 55);
            lblTodayMovementsValue.Name = "lblTodayMovementsValue";
            lblTodayMovementsValue.Size = new Size(43, 50);
            lblTodayMovementsValue.TabIndex = 1;
            lblTodayMovementsValue.Text = "0";
            // 
            // lblTodayMovementsTitle
            // 
            lblTodayMovementsTitle.AutoSize = true;
            lblTodayMovementsTitle.BackColor = Color.Transparent;
            lblTodayMovementsTitle.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            lblTodayMovementsTitle.ForeColor = Color.FromArgb(110, 120, 140);
            lblTodayMovementsTitle.Location = new Point(20, 20);
            lblTodayMovementsTitle.Name = "lblTodayMovementsTitle";
            lblTodayMovementsTitle.Size = new Size(122, 20);
            lblTodayMovementsTitle.TabIndex = 0;
            lblTodayMovementsTitle.Text = "Bugünkü İşlemler";
            // 
            // pnlTotalWarehouses
            // 
            pnlTotalWarehouses.BackColor = Color.White;
            pnlTotalWarehouses.Controls.Add(lblTotalWarehousesIcon);
            pnlTotalWarehouses.Controls.Add(lblTotalWarehousesValue);
            pnlTotalWarehouses.Controls.Add(lblTotalWarehousesTitle);
            pnlTotalWarehouses.Location = new Point(38, 300);
            pnlTotalWarehouses.Name = "pnlTotalWarehouses";
            pnlTotalWarehouses.Size = new Size(210, 130);
            pnlTotalWarehouses.TabIndex = 3;
            // 
            // lblTotalWarehousesValue
            // 
            lblTotalWarehousesValue.AutoSize = true;
            lblTotalWarehousesValue.BackColor = Color.Transparent;
            lblTotalWarehousesValue.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblTotalWarehousesValue.ForeColor = Color.FromArgb(25, 35, 55);
            lblTotalWarehousesValue.Location = new Point(20, 55);
            lblTotalWarehousesValue.Name = "lblTotalWarehousesValue";
            lblTotalWarehousesValue.Size = new Size(43, 50);
            lblTotalWarehousesValue.TabIndex = 1;
            lblTotalWarehousesValue.Text = "0";
            // 
            // lblTotalWarehousesTitle
            // 
            lblTotalWarehousesTitle.AutoSize = true;
            lblTotalWarehousesTitle.BackColor = Color.Transparent;
            lblTotalWarehousesTitle.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            lblTotalWarehousesTitle.ForeColor = Color.FromArgb(110, 120, 140);
            lblTotalWarehousesTitle.Location = new Point(20, 20);
            lblTotalWarehousesTitle.Name = "lblTotalWarehousesTitle";
            lblTotalWarehousesTitle.Size = new Size(100, 20);
            lblTotalWarehousesTitle.TabIndex = 0;
            lblTotalWarehousesTitle.Text = "Toplam Depo";
            lblTotalWarehousesTitle.Click += lblTotalWarehousesTitle_Click;
            // 
            // pnlCriticalStock
            // 
            pnlCriticalStock.BackColor = Color.White;
            pnlCriticalStock.Controls.Add(lblCriticalStockIcon);
            pnlCriticalStock.Controls.Add(lblCriticalStockValue);
            pnlCriticalStock.Controls.Add(lblCriticalStockTitle);
            pnlCriticalStock.Location = new Point(283, 150);
            pnlCriticalStock.Name = "pnlCriticalStock";
            pnlCriticalStock.Size = new Size(210, 130);
            pnlCriticalStock.TabIndex = 3;
            // 
            // lblCriticalStockValue
            // 
            lblCriticalStockValue.AutoSize = true;
            lblCriticalStockValue.BackColor = Color.Transparent;
            lblCriticalStockValue.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblCriticalStockValue.ForeColor = Color.FromArgb(220, 53, 69);
            lblCriticalStockValue.Location = new Point(20, 55);
            lblCriticalStockValue.Name = "lblCriticalStockValue";
            lblCriticalStockValue.Size = new Size(43, 50);
            lblCriticalStockValue.TabIndex = 1;
            lblCriticalStockValue.Text = "0";
            // 
            // lblCriticalStockTitle
            // 
            lblCriticalStockTitle.AutoSize = true;
            lblCriticalStockTitle.BackColor = Color.Transparent;
            lblCriticalStockTitle.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            lblCriticalStockTitle.ForeColor = Color.FromArgb(110, 120, 140);
            lblCriticalStockTitle.Location = new Point(20, 20);
            lblCriticalStockTitle.Name = "lblCriticalStockTitle";
            lblCriticalStockTitle.Size = new Size(76, 20);
            lblCriticalStockTitle.TabIndex = 0;
            lblCriticalStockTitle.Text = "Kritik Stok";
            // 
            // pnlTotalProducts
            // 
            pnlTotalProducts.BackColor = Color.White;
            pnlTotalProducts.Controls.Add(lblTotalProductsIcon);
            pnlTotalProducts.Controls.Add(lblTotalProductsValue);
            pnlTotalProducts.Controls.Add(lblTotalProductsTitle);
            pnlTotalProducts.Location = new Point(35, 150);
            pnlTotalProducts.Name = "pnlTotalProducts";
            pnlTotalProducts.Size = new Size(210, 130);
            pnlTotalProducts.TabIndex = 2;
            // 
            // lblTotalProductsValue
            // 
            lblTotalProductsValue.AutoSize = true;
            lblTotalProductsValue.BackColor = Color.Transparent;
            lblTotalProductsValue.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblTotalProductsValue.ForeColor = Color.FromArgb(25, 35, 55);
            lblTotalProductsValue.Location = new Point(20, 55);
            lblTotalProductsValue.Name = "lblTotalProductsValue";
            lblTotalProductsValue.Size = new Size(43, 50);
            lblTotalProductsValue.TabIndex = 1;
            lblTotalProductsValue.Text = "0";
            // 
            // lblTotalProductsTitle
            // 
            lblTotalProductsTitle.AutoSize = true;
            lblTotalProductsTitle.BackColor = Color.Transparent;
            lblTotalProductsTitle.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            lblTotalProductsTitle.ForeColor = Color.FromArgb(110, 120, 140);
            lblTotalProductsTitle.Location = new Point(20, 20);
            lblTotalProductsTitle.Name = "lblTotalProductsTitle";
            lblTotalProductsTitle.Size = new Size(94, 20);
            lblTotalProductsTitle.TabIndex = 0;
            lblTotalProductsTitle.Text = "Toplam Ürün";
            // 
            // lblDashboardDescription
            // 
            lblDashboardDescription.AutoSize = true;
            lblDashboardDescription.BackColor = Color.Transparent;
            lblDashboardDescription.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            lblDashboardDescription.ForeColor = Color.FromArgb(110, 120, 140);
            lblDashboardDescription.Location = new Point(38, 90);
            lblDashboardDescription.Name = "lblDashboardDescription";
            lblDashboardDescription.Size = new Size(431, 20);
            lblDashboardDescription.TabIndex = 1;
            lblDashboardDescription.Text = "İşletmenizin stok ve depo durumunu buradan takip edebilirsiniz.";
            // 
            // lblDashboardTitle
            // 
            lblDashboardTitle.AutoSize = true;
            lblDashboardTitle.BackColor = Color.Transparent;
            lblDashboardTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblDashboardTitle.ForeColor = Color.FromArgb(25, 35, 55);
            lblDashboardTitle.Location = new Point(35, 30);
            lblDashboardTitle.Name = "lblDashboardTitle";
            lblDashboardTitle.Size = new Size(193, 45);
            lblDashboardTitle.TabIndex = 0;
            lblDashboardTitle.Text = "Genel Bakış";
            // 
            // lblTotalProductsIcon
            // 
            lblTotalProductsIcon.AutoSize = true;
            lblTotalProductsIcon.Font = new Font("Segoe UI Symbol", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTotalProductsIcon.Location = new Point(165, 12);
            lblTotalProductsIcon.Name = "lblTotalProductsIcon";
            lblTotalProductsIcon.Size = new Size(33, 32);
            lblTotalProductsIcon.TabIndex = 2;
            lblTotalProductsIcon.Text = "⬡";
            // 
            // lblCriticalStockIcon
            // 
            lblCriticalStockIcon.AutoSize = true;
            lblCriticalStockIcon.Font = new Font("Segoe UI Symbol", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCriticalStockIcon.ForeColor = Color.FromArgb(220, 53, 69);
            lblCriticalStockIcon.Location = new Point(165, 12);
            lblCriticalStockIcon.Name = "lblCriticalStockIcon";
            lblCriticalStockIcon.Size = new Size(35, 32);
            lblCriticalStockIcon.TabIndex = 3;
            lblCriticalStockIcon.Text = "⚠";
            // 
            // lblTotalWarehousesIcon
            // 
            lblTotalWarehousesIcon.AutoSize = true;
            lblTotalWarehousesIcon.Font = new Font("Segoe UI Symbol", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTotalWarehousesIcon.ForeColor = Color.FromArgb(25, 35, 55);
            lblTotalWarehousesIcon.Location = new Point(165, 12);
            lblTotalWarehousesIcon.Name = "lblTotalWarehousesIcon";
            lblTotalWarehousesIcon.Size = new Size(35, 32);
            lblTotalWarehousesIcon.TabIndex = 4;
            lblTotalWarehousesIcon.Text = "▤";
            // 
            // lblTodayMovementsIcon
            // 
            lblTodayMovementsIcon.AutoSize = true;
            lblTodayMovementsIcon.Font = new Font("Segoe UI Symbol", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTodayMovementsIcon.Location = new Point(165, 12);
            lblTodayMovementsIcon.Name = "lblTodayMovementsIcon";
            lblTodayMovementsIcon.Size = new Size(36, 32);
            lblTodayMovementsIcon.TabIndex = 5;
            lblTodayMovementsIcon.Text = "⇄";
            // 
            // pnlRecentMovements
            // 
            pnlRecentMovements.BackColor = Color.White;
            pnlRecentMovements.Controls.Add(dgvRecentMovements);
            pnlRecentMovements.Controls.Add(lblRecentMovementsTitle);
            pnlRecentMovements.Location = new Point(35, 465);
            pnlRecentMovements.Name = "pnlRecentMovements";
            pnlRecentMovements.Size = new Size(890, 300);
            pnlRecentMovements.TabIndex = 5;
            // 
            // lblRecentMovementsTitle
            // 
            lblRecentMovementsTitle.AutoSize = true;
            lblRecentMovementsTitle.BackColor = Color.Transparent;
            lblRecentMovementsTitle.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblRecentMovementsTitle.ForeColor = Color.FromArgb(25, 35, 55);
            lblRecentMovementsTitle.Location = new Point(20, 20);
            lblRecentMovementsTitle.Name = "lblRecentMovementsTitle";
            lblRecentMovementsTitle.Size = new Size(215, 30);
            lblRecentMovementsTitle.TabIndex = 1;
            lblRecentMovementsTitle.Text = "Son Stok Hareketleri";
            // 
            // dgvRecentMovements
            // 
            dgvRecentMovements.AllowUserToAddRows = false;
            dgvRecentMovements.AllowUserToDeleteRows = false;
            dgvRecentMovements.AllowUserToResizeRows = false;
            dgvRecentMovements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRecentMovements.BackgroundColor = Color.White;
            dgvRecentMovements.BorderStyle = BorderStyle.None;
            dgvRecentMovements.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(25, 35, 55);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 162);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvRecentMovements.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvRecentMovements.ColumnHeadersHeight = 45;
            dgvRecentMovements.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvRecentMovements.Columns.AddRange(new DataGridViewColumn[] { colDate, colProduct, colMovementType, colQuantity, colWarehouse });
            dgvRecentMovements.EnableHeadersVisualStyles = false;
            dgvRecentMovements.GridColor = Color.FromArgb(230, 235, 240);
            dgvRecentMovements.Location = new Point(20, 70);
            dgvRecentMovements.MultiSelect = false;
            dgvRecentMovements.Name = "dgvRecentMovements";
            dgvRecentMovements.ReadOnly = true;
            dgvRecentMovements.RowHeadersVisible = false;
            dgvRecentMovements.RowTemplate.Height = 40;
            dgvRecentMovements.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecentMovements.Size = new Size(850, 205);
            dgvRecentMovements.TabIndex = 2;
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
            colWarehouse.HeaderText = "Depo";
            colWarehouse.Name = "colWarehouse";
            colWarehouse.ReadOnly = true;
            // 
            // FrmDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1264, 761);
            Controls.Add(pnlContent);
            Controls.Add(pnlSidebar);
            MinimumSize = new Size(1100, 700);
            Name = "FrmDashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CafeStock AI - Yönetim Paneli";
            WindowState = FormWindowState.Maximized;
            pnlSidebar.ResumeLayout(false);
            pnlSidebar.PerformLayout();
            pnlContent.ResumeLayout(false);
            pnlContent.PerformLayout();
            pnlTodayMovements.ResumeLayout(false);
            pnlTodayMovements.PerformLayout();
            pnlTotalWarehouses.ResumeLayout(false);
            pnlTotalWarehouses.PerformLayout();
            pnlCriticalStock.ResumeLayout(false);
            pnlCriticalStock.PerformLayout();
            pnlTotalProducts.ResumeLayout(false);
            pnlTotalProducts.PerformLayout();
            pnlRecentMovements.ResumeLayout(false);
            pnlRecentMovements.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRecentMovements).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlSidebar;
        private Label lblSidebarTitle;
        private Label lblSidebarSubtitle;
        private Button btnDashboard;
        private Button btnProducts;
        private Button btnWarehouses;
        private Button btnEmployees;
        private Button btnStockCount;
        private Button buttbtnStockMovementson1;
        private Button btnReports;
        private Button btnSettings;
        private Button btnAIAnalysis;
        private Panel pnlContent;
        private Label lblDashboardTitle;
        private Label lblDashboardDescription;
        private Panel pnlTotalProducts;
        private Label lblTotalProductsTitle;
        private Label lblTotalProductsValue;
        private Panel pnlTodayMovements;
        private Label lblTodayMovementsValue;
        private Label lblTodayMovementsTitle;
        private Panel pnlTotalWarehouses;
        private Label lblTotalWarehousesValue;
        private Label lblTotalWarehousesTitle;
        private Panel pnlCriticalStock;
        private Label lblCriticalStockValue;
        private Label lblCriticalStockTitle;
        private Label lblTotalProductsIcon;
        private Label lblTodayMovementsIcon;
        private Label lblTotalWarehousesIcon;
        private Label lblCriticalStockIcon;
        private Panel pnlRecentMovements;
        private Label lblRecentMovementsTitle;
        private DataGridView dgvRecentMovements;
        private DataGridViewTextBoxColumn colDate;
        private DataGridViewTextBoxColumn colProduct;
        private DataGridViewTextBoxColumn colMovementType;
        private DataGridViewTextBoxColumn colQuantity;
        private DataGridViewTextBoxColumn colWarehouse;
    }
}