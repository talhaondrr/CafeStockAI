namespace CafeStockAI.Desktop
{
    partial class FrmProducts
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            layout = new TableLayoutPanel();
            title = new Label();
            mode = new Label();
            editor = new Panel();
            categoryLabel = new Label();
            categoryInput = new TextBox();
            codeLabel = new Label();
            code = new TextBox();
            nameLabel = new Label();
            productName = new TextBox();
            unitLabel = new Label();
            unit = new ComboBox();
            minimumLabel = new Label();
            minimum = new QuantityInput();
            actions = new FlowLayoutPanel();
            newButton = new Button();
            saveButton = new Button();
            deleteButton = new Button();
            search = new TextBox();
            categoryFilter = new ComboBox();
            grid = new DataGridView();
            imageColumn = new DataGridViewImageColumn();
            status = new Label();
            layout.SuspendLayout();
            editor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)minimum).BeginInit();
            actions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            SuspendLayout();
            //
            // layout
            //
            layout.ColumnCount = 1;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.Controls.Add(title, 0, 0);
            layout.Controls.Add(mode, 0, 1);
            layout.Controls.Add(editor, 0, 2);
            layout.Controls.Add(actions, 0, 3);
            layout.Controls.Add(grid, 0, 4);
            layout.Controls.Add(status, 0, 5);
            layout.Dock = DockStyle.Fill;
            layout.Location = new Point(0, 0);
            layout.Name = "layout";
            layout.Padding = new Padding(20);
            layout.RowCount = 6;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            layout.Size = new Size(1084, 711);
            layout.TabIndex = 0;
            //
            // title
            //
            title.AutoSize = true;
            title.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            title.Location = new Point(23, 20);
            title.Name = "title";
            title.Size = new Size(201, 37);
            title.TabIndex = 0;
            title.Text = "Ürün Yönetimi";
            //
            // mode
            //
            mode.AutoSize = true;
            mode.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            mode.Location = new Point(23, 72);
            mode.Name = "mode";
            mode.Size = new Size(75, 20);
            mode.TabIndex = 1;
            mode.Text = "Yeni ürün";
            //
            // editor
            //
            editor.Controls.Add(categoryLabel);
            editor.Controls.Add(categoryInput);
            editor.Controls.Add(codeLabel);
            editor.Controls.Add(code);
            editor.Controls.Add(nameLabel);
            editor.Controls.Add(productName);
            editor.Controls.Add(unitLabel);
            editor.Controls.Add(unit);
            editor.Controls.Add(minimumLabel);
            editor.Controls.Add(minimum);
            editor.Dock = DockStyle.Fill;
            editor.Location = new Point(23, 109);
            editor.Name = "editor";
            editor.Size = new Size(1038, 154);
            editor.TabIndex = 2;
            //
            // categoryLabel
            //
            categoryLabel.AutoSize = true;
            categoryLabel.Location = new Point(0, 78);
            categoryLabel.Name = "categoryLabel";
            categoryLabel.Size = new Size(60, 19);
            categoryLabel.TabIndex = 0;
            categoryLabel.Text = "Kategori";
            //
            // categoryInput
            //
            categoryInput.Location = new Point(0, 106);
            categoryInput.MaxLength = 160;
            categoryInput.Name = "categoryInput";
            categoryInput.Size = new Size(430, 25);
            categoryInput.TabIndex = 1;
            //
            // codeLabel
            //
            codeLabel.AutoSize = true;
            codeLabel.Location = new Point(0, 8);
            codeLabel.Name = "codeLabel";
            codeLabel.Size = new Size(75, 19);
            codeLabel.TabIndex = 0;
            codeLabel.Text = "Ürün kodu"; codeLabel.Visible = false; code.Visible = false;
            //
            // code
            //
            code.Location = new Point(0, 36);
            code.MaxLength = 64;
            code.Name = "code";
            code.Size = new Size(150, 25);
            code.TabIndex = 1;
            //
            // nameLabel
            //
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(0, 8);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(62, 19);
            nameLabel.TabIndex = 2;
            nameLabel.Text = "Ürün adı";
            //
            // productName
            //
            productName.Location = new Point(0, 36);
            productName.MaxLength = 160;
            productName.Name = "productName";
            productName.Size = new Size(260, 25);
            productName.TabIndex = 3;
            //
            // unitLabel
            //
            unitLabel.AutoSize = true;
            unitLabel.Location = new Point(280, 8);
            unitLabel.Name = "unitLabel";
            unitLabel.Size = new Size(40, 19);
            unitLabel.TabIndex = 4;
            unitLabel.Text = "Birim";
            //
            // unit
            //
            unit.DropDownStyle = ComboBoxStyle.DropDownList;
            unit.Items.AddRange(new object[] { "Adet", "Kg", "Gram", "Litre", "Ml", "Paket", "Koli", "Şişe", "Fişek" });
            unit.Location = new Point(280, 36);
            unit.Name = "unit";
            unit.Size = new Size(110, 25);
            unit.TabIndex = 5;
            //
            // minimumLabel
            //
            minimumLabel.AutoSize = true;
            minimumLabel.Location = new Point(410, 8);
            minimumLabel.Name = "minimumLabel";
            minimumLabel.Size = new Size(102, 19);
            minimumLabel.TabIndex = 6;
            minimumLabel.Text = "Kritik stok sınırı";
            //
            // minimum
            //
            minimum.DecimalPlaces = 3;
            minimum.Location = new Point(410, 36);
            minimum.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            minimum.Name = "minimum";
            minimum.Size = new Size(160, 25);
            minimum.TabIndex = 7;
            //
            // actions
            //
            actions.Controls.Add(newButton);
            actions.Controls.Add(saveButton);
            actions.Controls.Add(deleteButton);
            actions.Controls.Add(search);
            actions.Controls.Add(categoryFilter);
            actions.Dock = DockStyle.Fill;
            actions.Location = new Point(23, 269);
            actions.Name = "actions";
            actions.Size = new Size(1038, 44);
            actions.TabIndex = 3;
            //
            // newButton
            //
            newButton.BackColor = Color.FromArgb(124, 102, 80);
            newButton.FlatAppearance.BorderSize = 0;
            newButton.FlatStyle = FlatStyle.Flat;
            newButton.ForeColor = Color.White;
            newButton.Location = new Point(3, 3);
            newButton.Name = "newButton";
            newButton.Size = new Size(100, 36);
            newButton.TabIndex = 0;
            newButton.Text = "Yeni";
            newButton.UseVisualStyleBackColor = false;
            newButton.Click += NewButton_Click;
            //
            // saveButton
            //
            saveButton.BackColor = Color.FromArgb(112, 74, 45);
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.FlatStyle = FlatStyle.Flat;
            saveButton.ForeColor = Color.White;
            saveButton.Location = new Point(109, 3);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(130, 36);
            saveButton.TabIndex = 1;
            saveButton.Text = "Ürün Ekle";
            saveButton.UseVisualStyleBackColor = false;
            saveButton.Click += SaveButton_Click;
            //
            // deleteButton
            //
            deleteButton.BackColor = Color.FromArgb(180, 55, 65);
            deleteButton.Enabled = false;
            deleteButton.FlatAppearance.BorderSize = 0;
            deleteButton.FlatStyle = FlatStyle.Flat;
            deleteButton.ForeColor = Color.White;
            deleteButton.Location = new Point(245, 3);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(110, 36);
            deleteButton.TabIndex = 2;
            deleteButton.Text = "Ürünü Sil";
            deleteButton.UseVisualStyleBackColor = false;
            deleteButton.Click += DeleteButton_Click;
            //
            // search
            //
            search.Location = new Point(361, 3);
            search.Name = "search";
            search.PlaceholderText = "Ürün, kod veya kategori ara...";
            search.Size = new Size(260, 25);
            search.TabIndex = 3;
            search.TextChanged += Search_TextChanged;
            //
            // categoryFilter
            //
            categoryFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            categoryFilter.Location = new Point(627, 3);
            categoryFilter.Name = "categoryFilter";
            categoryFilter.Size = new Size(200, 25);
            categoryFilter.TabIndex = 4;
            //
            // grid
            //
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(249, 245, 238);
            grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            grid.AutoGenerateColumns = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(98, 71, 50);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            grid.ColumnHeadersHeight = 42;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.Columns.AddRange(new DataGridViewColumn[] { imageColumn,
                new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Ürün adı", FillWeight = 180 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Category", HeaderText = "Kategori", FillWeight = 120 },
                new DataGridViewTextBoxColumn { DataPropertyName = "UnitDisplay", HeaderText = "Birim", FillWeight = 65 },
                new DataGridViewTextBoxColumn { DataPropertyName = "MinimumStock", HeaderText = "Kritik stok", FillWeight = 65 } });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Window;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(234, 219, 199);
            dataGridViewCellStyle4.SelectionForeColor = Color.FromArgb(98, 71, 50);
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            grid.DefaultCellStyle = dataGridViewCellStyle4;
            grid.Dock = DockStyle.Fill;
            grid.EnableHeadersVisualStyles = false;
            grid.GridColor = Color.FromArgb(235, 225, 211);
            grid.Location = new Point(23, 319);
            grid.MultiSelect = false;
            grid.Name = "grid";
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowTemplate.Height = 68;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.Size = new Size(1038, 334);
            grid.TabIndex = 4;
            grid.SelectionChanged += Grid_SelectionChanged;
            //
            // imageColumn
            //
            imageColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewCellStyle3.Padding = new Padding(6);
            imageColumn.DefaultCellStyle = dataGridViewCellStyle3;
            imageColumn.HeaderText = "";
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imageColumn.Name = "imageColumn";
            imageColumn.ReadOnly = true;
            imageColumn.Width = 76;
            //
            // status
            //
            status.AutoSize = true;
            status.ForeColor = Color.FromArgb(124, 102, 80);
            status.Location = new Point(20, 666);
            status.Margin = new Padding(0, 10, 0, 0);
            status.Name = "status";
            status.Size = new Size(98, 19);
            status.TabIndex = 5;
            status.Text = "Toplam 0 ürün";
            //
            // FrmProducts
            //
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 237, 227);
            ClientSize = new Size(1084, 711);
            Controls.Add(layout);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(950, 650);
            Name = "FrmProducts";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CafeStock AI - Ürün Yönetimi";
            Shown += FrmProducts_Shown;
            layout.ResumeLayout(false);
            layout.PerformLayout();
            editor.ResumeLayout(false);
            editor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)minimum).EndInit();
            actions.ResumeLayout(false);
            actions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            ResumeLayout(false);
        }
        private TableLayoutPanel layout;
        private Label title;
        private Label mode;
        private ComboBox categoryFilter;
        private DataGridViewImageColumn imageColumn;
        private TextBox categoryInput;
        private Label categoryLabel;

        private Panel editor;
        private FlowLayoutPanel actions;
        private Label codeLabel;
        private Label nameLabel;
        private Label unitLabel;
        private Label minimumLabel;
        private TextBox code;
        private TextBox productName;
        private ComboBox unit;
        private NumericUpDown minimum;
        private Button newButton;
        private Button saveButton;
        private Button deleteButton;
        private TextBox search;
        private DataGridView grid;
        private Label status;




    }
}
