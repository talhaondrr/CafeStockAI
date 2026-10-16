namespace CafeStockAI.Desktop { partial class FrmStockMovements {
private System.ComponentModel.IContainer components = null;
protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            areaInput = new ComboBox();
            areaInputLabel = new Label();
            layout = new TableLayoutPanel();
            titleLabel = new Label();
            descriptionLabel = new Label();
            editor = new TableLayoutPanel();
            productInputLabel = new Label();
            productInput = new ComboBox();
            branchInputLabel = new Label();
            branchInput = new ComboBox();
            typeInputLabel = new Label();
            typeInput = new ComboBox();
            quantityInputLabel = new Label();
            quantityInput = new QuantityInput();
            noteInputLabel = new Label();
            noteInput = new TextBox();
            actions = new FlowLayoutPanel();
            saveButton = new Button();
            refreshButton = new Button();
            grid = new DataGridView();
            statusLabel = new Label();
            layout.SuspendLayout();
            editor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)quantityInput).BeginInit();
            actions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            SuspendLayout();
            //
            // areaInput
            //
            areaInput.Dock = DockStyle.Top;
            areaInput.DropDownStyle = ComboBoxStyle.DropDownList;
            areaInput.Items.AddRange(new object[] { "Bar", "Depo" });
            areaInput.Location = new Point(693, 103);
            areaInput.Margin = new Padding(3, 3, 16, 3);
            areaInput.Name = "areaInput";
            areaInput.Size = new Size(329, 25);
            areaInput.TabIndex = 11;
            //
            // areaInputLabel
            //
            areaInputLabel.AutoSize = true;
            areaInputLabel.Location = new Point(693, 74);
            areaInputLabel.Name = "areaInputLabel";
            areaInputLabel.Size = new Size(68, 19);
            areaInputLabel.TabIndex = 10;
            areaInputLabel.Text = "Stok alanı";
            //
            // layout
            //
            layout.ColumnCount = 1;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.Controls.Add(titleLabel, 0, 0);
            layout.Controls.Add(descriptionLabel, 0, 1);
            layout.Controls.Add(editor, 0, 2);
            layout.Controls.Add(actions, 0, 3);
            layout.Controls.Add(grid, 0, 4);
            layout.Controls.Add(statusLabel, 0, 5);
            layout.Dock = DockStyle.Fill;
            layout.Location = new Point(0, 0);
            layout.Name = "layout";
            layout.Padding = new Padding(20);
            layout.RowCount = 6;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 165F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            layout.Size = new Size(1084, 761);
            layout.TabIndex = 0;
            //
            // titleLabel
            //
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            titleLabel.Location = new Point(23, 20);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(226, 37);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Stok Hareketleri";
            //
            // descriptionLabel
            //
            descriptionLabel.Dock = DockStyle.Fill;
            descriptionLabel.ForeColor = Color.FromArgb(124, 102, 80);
            descriptionLabel.Location = new Point(23, 70);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new Size(1038, 52);
            descriptionLabel.TabIndex = 1;
            descriptionLabel.Text = "Ürün ve şube seçin. Stok giriş/çıkışını Bar veya Depo alanına kaydedin.";
            //
            // editor
            //
            editor.ColumnCount = 3;
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            editor.Controls.Add(productInputLabel, 0, 0);
            editor.Controls.Add(productInput, 0, 1);
            editor.Controls.Add(branchInputLabel, 1, 0);
            editor.Controls.Add(branchInput, 1, 1);
            editor.Controls.Add(typeInputLabel, 2, 0);
            editor.Controls.Add(typeInput, 2, 1);
            editor.Controls.Add(quantityInputLabel, 0, 2);
            editor.Controls.Add(quantityInput, 0, 3);
            editor.Controls.Add(noteInputLabel, 1, 2);
            editor.Controls.Add(noteInput, 1, 3);
            editor.Controls.Add(areaInputLabel, 2, 2);
            editor.Controls.Add(areaInput, 2, 3);
            editor.Dock = DockStyle.Fill;
            editor.Location = new Point(23, 125);
            editor.Name = "editor";
            editor.RowCount = 4;
            editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            editor.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            editor.Size = new Size(1038, 159);
            editor.TabIndex = 2;
            //
            // productInputLabel
            //
            productInputLabel.AutoSize = true;
            productInputLabel.Location = new Point(3, 0);
            productInputLabel.Name = "productInputLabel";
            productInputLabel.Size = new Size(40, 19);
            productInputLabel.TabIndex = 0;
            productInputLabel.Text = "Ürün";
            //
            // productInput
            //
            productInput.Dock = DockStyle.Top;
            productInput.DropDownStyle = ComboBoxStyle.DropDownList;
            productInput.Location = new Point(3, 29);
            productInput.Margin = new Padding(3, 3, 16, 3);
            productInput.Name = "productInput";
            productInput.Size = new Size(326, 25);
            productInput.TabIndex = 1;
            //
            // branchInputLabel
            //
            branchInputLabel.AutoSize = true;
            branchInputLabel.Location = new Point(348, 0);
            branchInputLabel.Name = "branchInputLabel";
            branchInputLabel.Size = new Size(39, 19);
            branchInputLabel.TabIndex = 2;
            branchInputLabel.Text = "Şube";
            //
            // branchInput
            //
            branchInput.Dock = DockStyle.Top;
            branchInput.DropDownStyle = ComboBoxStyle.DropDownList;
            branchInput.Location = new Point(348, 29);
            branchInput.Margin = new Padding(3, 3, 16, 3);
            branchInput.Name = "branchInput";
            branchInput.Size = new Size(326, 25);
            branchInput.TabIndex = 3;
            //
            // typeInputLabel
            //
            typeInputLabel.AutoSize = true;
            typeInputLabel.Location = new Point(693, 0);
            typeInputLabel.Name = "typeInputLabel";
            typeInputLabel.Size = new Size(41, 19);
            typeInputLabel.TabIndex = 4;
            typeInputLabel.Text = "İşlem";
            //
            // typeInput
            //
            typeInput.Dock = DockStyle.Top;
            typeInput.DropDownStyle = ComboBoxStyle.DropDownList;
            typeInput.Location = new Point(693, 29);
            typeInput.Margin = new Padding(3, 3, 16, 3);
            typeInput.Name = "typeInput";
            typeInput.Size = new Size(329, 25);
            typeInput.TabIndex = 5;
            //
            // quantityInputLabel
            //
            quantityInputLabel.AutoSize = true;
            quantityInputLabel.Location = new Point(3, 74);
            quantityInputLabel.Name = "quantityInputLabel";
            quantityInputLabel.Size = new Size(49, 19);
            quantityInputLabel.TabIndex = 6;
            quantityInputLabel.Text = "Miktar";
            //
            // quantityInput
            //
            quantityInput.DecimalPlaces = 3;
            quantityInput.Dock = DockStyle.Top;
            quantityInput.Location = new Point(3, 103);
            quantityInput.Margin = new Padding(3, 3, 16, 3);
            quantityInput.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            quantityInput.Name = "quantityInput";
            quantityInput.Size = new Size(326, 25);
            quantityInput.TabIndex = 7;
            //
            // noteInputLabel
            //
            noteInputLabel.AutoSize = true;
            noteInputLabel.Location = new Point(348, 74);
            noteInputLabel.Name = "noteInputLabel";
            noteInputLabel.Size = new Size(63, 19);
            noteInputLabel.TabIndex = 8;
            noteInputLabel.Text = "Açıklama";
            //
            // noteInput
            //
            noteInput.Dock = DockStyle.Top;
            noteInput.Location = new Point(348, 103);
            noteInput.Margin = new Padding(3, 3, 16, 3);
            noteInput.MaxLength = 160;
            noteInput.Name = "noteInput";
            noteInput.Size = new Size(326, 25);
            noteInput.TabIndex = 9;
            //
            // actions
            //
            actions.Controls.Add(saveButton);
            actions.Controls.Add(refreshButton);
            actions.Dock = DockStyle.Fill;
            actions.Location = new Point(23, 290);
            actions.Name = "actions";
            actions.Size = new Size(1038, 44);
            actions.TabIndex = 3;
            //
            // saveButton
            //
            saveButton.BackColor = Color.FromArgb(112, 74, 45);
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.FlatStyle = FlatStyle.Flat;
            saveButton.ForeColor = Color.White;
            saveButton.Location = new Point(3, 3);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(145, 36);
            saveButton.TabIndex = 0;
            saveButton.Text = "Hareketi kaydet";
            saveButton.UseVisualStyleBackColor = false;
            saveButton.Click += Save_Click;
            //
            // refreshButton
            //
            refreshButton.BackColor = Color.FromArgb(112, 74, 45);
            refreshButton.FlatAppearance.BorderSize = 0;
            refreshButton.FlatStyle = FlatStyle.Flat;
            refreshButton.ForeColor = Color.White;
            refreshButton.Location = new Point(154, 3);
            refreshButton.Name = "refreshButton";
            refreshButton.Size = new Size(145, 36);
            refreshButton.TabIndex = 1;
            refreshButton.Text = "Yenile";
            refreshButton.UseVisualStyleBackColor = false;
            refreshButton.Click += Refresh_Click;
            //
            // grid
            //
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(249, 245, 238);
            grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(98, 71, 50);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(234, 219, 199);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(98, 71, 50);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            grid.DefaultCellStyle = dataGridViewCellStyle3;
            grid.AutoGenerateColumns=false;
            grid.Columns.Clear();
grid.Columns.Add(new DataGridViewTextBoxColumn {DataPropertyName="Tarih",HeaderText="Tarih"});
grid.Columns.Add(new DataGridViewTextBoxColumn {DataPropertyName="Ürün",HeaderText="Ürün"});
grid.Columns.Add(new DataGridViewTextBoxColumn {DataPropertyName="Depo",HeaderText="Şube / alan"});
grid.Columns.Add(new DataGridViewTextBoxColumn {DataPropertyName="İşlem",HeaderText="İşlem"});
grid.Columns.Add(new DataGridViewTextBoxColumn {DataPropertyName="Miktar",HeaderText="Miktar"});
grid.Columns.Add(new DataGridViewTextBoxColumn {DataPropertyName="Birim",HeaderText="Birim"});
grid.Columns.Add(new DataGridViewTextBoxColumn {DataPropertyName="Açıklama",HeaderText="Açıklama"});
grid.Dock = DockStyle.Fill;
            grid.EnableHeadersVisualStyles = false;
            grid.Location = new Point(23, 340);
            grid.MultiSelect = false;
            grid.Name = "grid";
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowTemplate.Height = 34;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.Size = new Size(1038, 362);
            grid.TabIndex = 4;
            //
            // statusLabel
            //
            statusLabel.AutoSize = true;
            statusLabel.ForeColor = Color.FromArgb(124, 102, 80);
            statusLabel.Location = new Point(23, 705);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(110, 19);
            statusLabel.TabIndex = 5;
            statusLabel.Text = "Henüz kayıt yok.";
            //
            // FrmStockMovements
            //
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 237, 227);
            ClientSize = new Size(1084, 761);
            Controls.Add(layout);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(950, 650);
            Name = "FrmStockMovements";
            Text = "CafeStock AI - Stok Hareketleri";
            layout.ResumeLayout(false);
            layout.PerformLayout();
            editor.ResumeLayout(false);
            editor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)quantityInput).EndInit();
            actions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            ResumeLayout(false);
        }
        private ComboBox areaInput;
private Label areaInputLabel;
private TableLayoutPanel layout;
private Label titleLabel;
private Label descriptionLabel;
private TableLayoutPanel editor;
private FlowLayoutPanel actions;
private DataGridView grid;
private Label statusLabel;
private Label productInputLabel;
private ComboBox productInput;
private Label branchInputLabel;
private ComboBox branchInput;
private Label typeInputLabel;
private ComboBox typeInput;
private Label quantityInputLabel;
private NumericUpDown quantityInput;
private Label noteInputLabel;
private TextBox noteInput;
private Button saveButton;
private Button refreshButton;







} }
