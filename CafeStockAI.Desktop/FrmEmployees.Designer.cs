namespace CafeStockAI.Desktop
{
    partial class FrmEmployees
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container(); hireDateInput = new DateTimePicker(); hireDateLabel = new Label(); colHireDate = new DataGridViewTextBoxColumn();
            layout = new TableLayoutPanel(); header = new Panel(); titleLabel = new Label(); descriptionLabel = new Label();
            summary = new FlowLayoutPanel(); totalLabel = new Label(); totalValue = new Label(); branchLabel = new Label(); branchValue = new Label();
            body = new TableLayoutPanel(); editCard = new CoffeeCard(); listCard = new CoffeeCard(); editor = new TableLayoutPanel();
            editorTitle = new Label(); editorHint = new Label(); nameInputLabel = new Label(); nameInput = new TextBox(); roleInputLabel = new Label(); roleInput = new ComboBox(); branchInputLabel = new Label(); branchInput = new CheckedListBox(); phoneInputLabel = new Label(); phoneInput = new TextBox(); phoneHint = new Label();
            saveButton = new Button(); newButton = new Button(); deleteButton = new Button(); actions = new FlowLayoutPanel();
            listLayout = new TableLayoutPanel(); listTitle = new Label(); searchInput = new TextBox(); grid = new DataGridView(); statusLabel = new Label();
            colName = new DataGridViewTextBoxColumn(); colRole = new DataGridViewTextBoxColumn(); colBranch = new DataGridViewTextBoxColumn(); colPhone = new DataGridViewTextBoxColumn();
            errors = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errors).BeginInit();
            SuspendLayout();
            layout.Name = "layout"; layout.Dock = DockStyle.Fill; layout.Padding = new Padding(22); layout.ColumnCount = 1; layout.RowCount = 4;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute,78F)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute,58F)); layout.RowStyles.Add(new RowStyle(SizeType.Percent,100F)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute,32F));
            layout.Controls.Add(header,0,0); layout.Controls.Add(summary,0,1); layout.Controls.Add(body,0,2); layout.Controls.Add(statusLabel,0,3);
            header.Name = "header"; header.Dock = DockStyle.Fill; header.Controls.Add(titleLabel); header.Controls.Add(descriptionLabel);
            titleLabel.Text = "Ekibinizi yönetin"; titleLabel.Font = new Font("Segoe UI",23F,FontStyle.Bold); titleLabel.AutoSize = true; titleLabel.Location = new Point(0,0);
            descriptionLabel.Text = "Personel bilgileri, görevler ve şubeler tek bir yerde."; descriptionLabel.ForeColor = Color.FromArgb(130,111,91); descriptionLabel.AutoSize = true; descriptionLabel.Location = new Point(3,47);
            summary.Dock = DockStyle.Fill;
            totalLabel.Text = "Kayıtlı personel"; totalLabel.AutoSize = true; totalLabel.Margin = new Padding(0,10,12,0);
            totalValue.Text = "0"; totalValue.Font = new Font("Segoe UI",19F,FontStyle.Bold); totalValue.AutoSize = true; totalValue.Margin = new Padding(0,0,38,0);
            branchLabel.Text = "Personeli olan şube"; branchLabel.AutoSize = true; branchLabel.Margin = new Padding(0,10,12,0);
            branchValue.Text = "0"; branchValue.Font = new Font("Segoe UI",19F,FontStyle.Bold); branchValue.AutoSize = true;
            summary.Controls.Add(totalLabel); summary.Controls.Add(totalValue); summary.Controls.Add(branchLabel); summary.Controls.Add(branchValue);
            body.Dock = DockStyle.Fill; body.ColumnCount = 2; body.RowCount = 1; body.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,330F)); body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100F)); body.RowStyles.Add(new RowStyle(SizeType.Percent,100F)); body.Controls.Add(editCard,0,0); body.Controls.Add(listCard,1,0);
            editCard.Dock = DockStyle.Fill; editCard.Padding = new Padding(20); editCard.Margin = new Padding(0,0,14,0); editCard.AutoScroll = true; editCard.Controls.Add(editor);
            editor.Name = "editor"; editor.Dock = DockStyle.Top; editor.Height = 509; editor.ColumnCount = 1; editor.RowCount = 15; editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100F));
            editor.RowStyles.Add(new RowStyle(SizeType.Absolute,26F)); editor.RowStyles.Add(new RowStyle(SizeType.Absolute,25F));
            editor.RowStyles.Add(new RowStyle(SizeType.Absolute,24F)); editor.RowStyles.Add(new RowStyle(SizeType.Absolute,34F));
            editor.RowStyles.Add(new RowStyle(SizeType.Absolute,24F)); editor.RowStyles.Add(new RowStyle(SizeType.Absolute,34F));
            editor.RowStyles.Add(new RowStyle(SizeType.Absolute,24F)); editor.RowStyles.Add(new RowStyle(SizeType.Absolute,100F));
            editor.RowStyles.Add(new RowStyle(SizeType.Absolute,24F)); editor.RowStyles.Add(new RowStyle(SizeType.Absolute,32F));
            editor.RowStyles.Add(new RowStyle(SizeType.Absolute,26F)); editor.RowStyles.Add(new RowStyle(SizeType.Absolute,24F)); editor.RowStyles.Add(new RowStyle(SizeType.Absolute,32F)); editor.RowStyles.Add(new RowStyle(SizeType.Absolute,40F)); editor.RowStyles.Add(new RowStyle(SizeType.Absolute,40F));
            editorTitle.Text = "Yeni personel ekle"; editorTitle.Font = new Font("Segoe UI",13F,FontStyle.Bold); editorTitle.AutoSize = true; editor.Controls.Add(editorTitle,0,0);
            editorHint.Text = "Ekip üyenizin bilgilerini aşağıya yazın."; editorHint.Dock = DockStyle.Fill; editorHint.ForeColor = Color.FromArgb(130,111,91); editorHint.Font = new Font("Segoe UI",9F); editor.Controls.Add(editorHint,0,1);
            nameInputLabel.Text = "Ad soyad *"; nameInputLabel.AutoSize = true; editor.Controls.Add(nameInputLabel,0,2);
            nameInput.Name = "nameInput"; nameInput.MaxLength = 160; nameInput.PlaceholderText = "Adı ve soyadı"; nameInput.Dock = DockStyle.Top; nameInput.Margin = new Padding(3,3,16,3); editor.Controls.Add(nameInput,0,3);
            roleInputLabel.Text = "Görev *"; roleInputLabel.AutoSize = true; editor.Controls.Add(roleInputLabel,0,4);
            roleInput.Name = "roleInput"; roleInput.Dock = DockStyle.Top; roleInput.Margin = new Padding(3,3,16,3); roleInput.DropDownStyle = ComboBoxStyle.DropDownList; roleInput.Items.AddRange(new object[] { "Personel", "Şube Müdürü", "Genel Müdür", "Bölge Müdürü", "İşletme Sahibi", "Süpervizör", "Depo Sorumlusu" }); editor.Controls.Add(roleInput,0,5);
            branchInputLabel.Text = "Görevli olduğu şubeler *"; branchInputLabel.AutoSize = true; editor.Controls.Add(branchInputLabel,0,6);
            branchInput.Name = "branchInput"; branchInput.Dock = DockStyle.Top; branchInput.Margin = new Padding(3,3,16,3); branchInput.CheckOnClick = true; branchInput.IntegralHeight = false; branchInput.Height = 90; branchInput.BorderStyle = BorderStyle.FixedSingle;  editor.Controls.Add(branchInput,0,7);
            phoneInputLabel.Text = "Telefon"; phoneInputLabel.AutoSize = true; editor.Controls.Add(phoneInputLabel,0,8);
            phoneInput.Name = "phoneInput"; phoneInput.Dock = DockStyle.Top; phoneInput.Margin = new Padding(3,3,16,3); phoneInput.MaxLength = 32; phoneInput.PlaceholderText = "0 (5xx) xxx xx xx"; editor.Controls.Add(phoneInput,0,9);
            phoneHint.Text = "Türkiye numarası • Telefon isteğe bağlı"; phoneHint.ForeColor = Color.FromArgb(130,111,91); phoneHint.Font = new Font("Segoe UI",8.5F); phoneHint.AutoSize = true; editor.Controls.Add(phoneHint,0,10);
            hireDateLabel.Text = "İşe giriş tarihi"; hireDateLabel.AutoSize = true; editor.Controls.Add(hireDateLabel,0,11); hireDateInput.Name = "hireDateInput"; hireDateInput.Format = DateTimePickerFormat.Short; hireDateInput.ShowCheckBox = true; hireDateInput.Checked = false; hireDateInput.Dock = DockStyle.Top; editor.Controls.Add(hireDateInput,0,12); colHireDate.Name = "colHireDate"; colHireDate.HeaderText = "İşe Giriş"; colHireDate.DataPropertyName = "HireDateDisplay";
            saveButton.Name = "saveButton"; saveButton.Text = "Personeli kaydet"; saveButton.Dock = DockStyle.Top; saveButton.Height = 36; saveButton.BackColor = Color.FromArgb(112,74,45); saveButton.ForeColor = Color.White; saveButton.FlatStyle = FlatStyle.Flat; saveButton.FlatAppearance.BorderSize = 0; saveButton.Cursor = Cursors.Hand; saveButton.Click += Save_Click; editor.Controls.Add(saveButton,0,13);
            actions.Dock = DockStyle.Fill; actions.WrapContents = false; editor.Controls.Add(actions,0,14);
            newButton.Name = "newButton"; newButton.Text = "Yeni kayıt"; newButton.Size = new Size(120,32); newButton.BackColor = Color.FromArgb(241,232,218); newButton.ForeColor = Color.FromArgb(98,71,50); newButton.FlatStyle = FlatStyle.Flat; newButton.FlatAppearance.BorderSize = 0; newButton.Click += New_Click; actions.Controls.Add(newButton);
            deleteButton.Name = "deleteButton"; deleteButton.Text = "Personeli sil"; deleteButton.Size = new Size(120,32); deleteButton.BackColor = Color.FromArgb(251,235,229); deleteButton.ForeColor = Color.FromArgb(172,67,49); deleteButton.FlatStyle = FlatStyle.Flat; deleteButton.FlatAppearance.BorderSize = 0; deleteButton.Enabled = false; deleteButton.Click += Delete_Click; actions.Controls.Add(deleteButton);
            listCard.Dock = DockStyle.Fill; listCard.Padding = new Padding(20); listCard.Margin = new Padding(0); listCard.Controls.Add(listLayout);
            listLayout.Dock = DockStyle.Fill; listLayout.ColumnCount = 1; listLayout.RowCount = 3; listLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100F)); listLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,25F)); listLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,46F)); listLayout.RowStyles.Add(new RowStyle(SizeType.Percent,100F));
            listTitle.Text = "Personel listesi"; listTitle.Font = new Font("Segoe UI",13F,FontStyle.Bold); listTitle.AutoSize = true; listLayout.Controls.Add(listTitle,0,0);
            searchInput.Name = "searchInput"; searchInput.PlaceholderText = "İsim, görev, şube veya telefon ile ara..."; searchInput.Dock = DockStyle.Top; searchInput.MaxLength = 160; listLayout.Controls.Add(searchInput,0,1);
            grid.Name = "grid"; grid.Dock = DockStyle.Fill; grid.AutoGenerateColumns = false; grid.ReadOnly = true; grid.AllowUserToAddRows = false; grid.AllowUserToDeleteRows = false; grid.MultiSelect = false; grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; grid.RowHeadersVisible = false; grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; grid.BorderStyle = BorderStyle.None; grid.BackgroundColor = Color.FromArgb(255,253,249); grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; grid.GridColor = Color.FromArgb(239,232,222); grid.EnableHeadersVisualStyles = false; grid.ColumnHeadersHeight = 38; grid.RowTemplate.Height = 42; grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(247,242,234); grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(111,94,76); grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(247,242,234); grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(111,94,76); grid.DefaultCellStyle.BackColor = Color.FromArgb(255,253,249); grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(234,219,199); grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(62,43,28); grid.DefaultCellStyle.Padding = new Padding(4,8,4,8);
            colName.Name = "colName"; colName.HeaderText = "Ad Soyad"; colName.DataPropertyName = "Name"; colName.FillWeight = 120F;
            colRole.Name = "colRole"; colRole.HeaderText = "Görev"; colRole.DataPropertyName = "Role"; colRole.FillWeight = 115F;
            colBranch.Name = "colBranch"; colBranch.HeaderText = "Şubeler"; colBranch.DataPropertyName = "BranchesDisplay"; colBranch.FillWeight = 80F;
            colPhone.Name = "colPhone"; colPhone.HeaderText = "Telefon"; colPhone.DataPropertyName = "Phone"; colPhone.FillWeight = 140F;
            grid.Columns.AddRange(new DataGridViewColumn[] { colName, colRole, colBranch, colPhone, colHireDate }); listLayout.Controls.Add(grid,0,2);
            statusLabel.Text = "Yeni kayıt ekleyin veya düzenlemek için listeden bir personel seçin."; statusLabel.Dock = DockStyle.Fill; statusLabel.TextAlign = ContentAlignment.MiddleLeft; statusLabel.ForeColor = Color.FromArgb(124,102,80);
            errors.ContainerControl = this; errors.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            AutoScaleDimensions = new SizeF(7F,15F); AutoScaleMode = AutoScaleMode.Font; Font = new Font("Segoe UI",10F); BackColor = Color.FromArgb(243,237,227); ForeColor = Color.FromArgb(62,43,28); ClientSize = new Size(1180,780); MinimumSize = new Size(950,650); Name = "FrmEmployees"; Text = "CafeStock AI - Personel Yönetimi"; Controls.Add(layout);
            ((System.ComponentModel.ISupportInitialize)grid).EndInit(); ((System.ComponentModel.ISupportInitialize)errors).EndInit(); ResumeLayout(false);
        }
        private TableLayoutPanel layout, body, editor, listLayout;
        private Panel header;
        private FlowLayoutPanel summary, actions;
        private CoffeeCard editCard, listCard;
        private Label titleLabel, descriptionLabel, totalLabel, totalValue, branchLabel, branchValue, editorTitle, editorHint, nameInputLabel, roleInputLabel, branchInputLabel, phoneInputLabel, phoneHint, listTitle, statusLabel;
        private TextBox nameInput, phoneInput, searchInput;
        private ComboBox roleInput;
        private CheckedListBox branchInput;
        private Button saveButton, newButton, deleteButton;
        private DataGridView grid;
        private DataGridViewTextBoxColumn colName, colRole, colBranch, colPhone;
        private ErrorProvider errors;
        private DateTimePicker hireDateInput; private Label hireDateLabel; private DataGridViewTextBoxColumn colHireDate;
    }
}
