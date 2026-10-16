namespace CafeStockAI.Desktop { partial class FrmReports {
private System.ComponentModel.IContainer components = null;
protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            layout = new TableLayoutPanel();
            titleLabel = new Label();
            descriptionLabel = new Label();
            editor = new TableLayoutPanel();
            reportInputLabel = new Label();
            reportInput = new ComboBox();
            warehouseInputLabel = new Label();
            warehouseInput = new ComboBox();
            fromInputLabel = new Label();
            fromInput = new DateTimePicker();
            toInputLabel = new Label();
            toInput = new DateTimePicker();
            actions = new FlowLayoutPanel();
            refreshButton = new Button();
            exportButton = new Button();
            grid = new DataGridView();
            statusLabel = new Label();
            layout.SuspendLayout();
            editor.SuspendLayout();
            actions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            SuspendLayout();
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
            titleLabel.Size = new Size(129, 37);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Raporlar";
            //
            // descriptionLabel
            //
            descriptionLabel.Dock = DockStyle.Fill;
            descriptionLabel.ForeColor = Color.FromArgb(124, 102, 80);
            descriptionLabel.Location = new Point(23, 70);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new Size(1038, 52);
            descriptionLabel.TabIndex = 1;
            descriptionLabel.Text = "Şube stoklarını, hareketleri ve tamamlanan sayımları inceleyin; görünen tabloyu CSV olarak kaydedin.";
            //
            // editor
            //
            editor.ColumnCount = 3;
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            editor.Controls.Add(reportInputLabel, 0, 0);
            editor.Controls.Add(reportInput, 0, 1);
            editor.Controls.Add(warehouseInputLabel, 1, 0);
            editor.Controls.Add(warehouseInput, 1, 1);
            editor.Controls.Add(fromInputLabel, 2, 0);
            editor.Controls.Add(fromInput, 2, 1);
            editor.Controls.Add(toInputLabel, 0, 2);
            editor.Controls.Add(toInput, 0, 3);
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
            // reportInputLabel
            //
            reportInputLabel.AutoSize = true;
            reportInputLabel.Location = new Point(3, 0);
            reportInputLabel.Name = "reportInputLabel";
            reportInputLabel.Size = new Size(75, 19);
            reportInputLabel.TabIndex = 0;
            reportInputLabel.Text = "Rapor türü";
            //
            // reportInput
            //
            reportInput.Dock = DockStyle.Top;
            reportInput.DropDownStyle = ComboBoxStyle.DropDownList;
            reportInput.Location = new Point(3, 29);
            reportInput.Margin = new Padding(3, 3, 16, 3);
            reportInput.Name = "reportInput";
            reportInput.Size = new Size(326, 25);
            reportInput.TabIndex = 1;
            //
            // warehouseInputLabel
            //
            warehouseInputLabel.AutoSize = true;
            warehouseInputLabel.Location = new Point(348, 0);
            warehouseInputLabel.Name = "warehouseInputLabel";
            warehouseInputLabel.Size = new Size(79, 19);
            warehouseInputLabel.TabIndex = 2;
            warehouseInputLabel.Text = "Şube filtresi";
            //
            // warehouseInput
            //
            warehouseInput.Dock = DockStyle.Top;
            warehouseInput.DropDownStyle = ComboBoxStyle.DropDownList;
            warehouseInput.Location = new Point(348, 29);
            warehouseInput.Margin = new Padding(3, 3, 16, 3);
            warehouseInput.Name = "warehouseInput";
            warehouseInput.Size = new Size(326, 25);
            warehouseInput.TabIndex = 3;
            //
            // fromInputLabel
            //
            fromInputLabel.AutoSize = true;
            fromInputLabel.Location = new Point(693, 0);
            fromInputLabel.Name = "fromInputLabel";
            fromInputLabel.Size = new Size(100, 19);
            fromInputLabel.TabIndex = 4;
            fromInputLabel.Text = "Başlangıç tarihi";
            //
            // fromInput
            //
            fromInput.Dock = DockStyle.Top;
            fromInput.Format = DateTimePickerFormat.Short;
            fromInput.Location = new Point(693, 29);
            fromInput.Margin = new Padding(3, 3, 16, 3);
            fromInput.Name = "fromInput";
            fromInput.Size = new Size(329, 25);
            fromInput.TabIndex = 5;
            //
            // toInputLabel
            //
            toInputLabel.AutoSize = true;
            toInputLabel.Location = new Point(3, 74);
            toInputLabel.Name = "toInputLabel";
            toInputLabel.Size = new Size(69, 19);
            toInputLabel.TabIndex = 6;
            toInputLabel.Text = "Bitiş tarihi";
            //
            // toInput
            //
            toInput.Dock = DockStyle.Top;
            toInput.Format = DateTimePickerFormat.Short;
            toInput.Location = new Point(3, 103);
            toInput.Margin = new Padding(3, 3, 16, 3);
            toInput.Name = "toInput";
            toInput.Size = new Size(326, 25);
            toInput.TabIndex = 7;
            //
            // actions
            //
            actions.Controls.Add(refreshButton);
            actions.Controls.Add(exportButton);
            actions.Dock = DockStyle.Fill;
            actions.Location = new Point(23, 290);
            actions.Name = "actions";
            actions.Size = new Size(1038, 44);
            actions.TabIndex = 3;
            //
            // refreshButton
            //
            refreshButton.BackColor = Color.FromArgb(112, 74, 45);
            refreshButton.FlatAppearance.BorderSize = 0;
            refreshButton.FlatStyle = FlatStyle.Flat;
            refreshButton.ForeColor = Color.White;
            refreshButton.Location = new Point(3, 3);
            refreshButton.Name = "refreshButton";
            refreshButton.Size = new Size(145, 36);
            refreshButton.TabIndex = 0;
            refreshButton.Text = "Raporu getir";
            refreshButton.UseVisualStyleBackColor = false;
            refreshButton.Click += Refresh_Click;
            //
            // exportButton
            //
            exportButton.BackColor = Color.FromArgb(112, 74, 45);
            exportButton.FlatAppearance.BorderSize = 0;
            exportButton.FlatStyle = FlatStyle.Flat;
            exportButton.ForeColor = Color.White;
            exportButton.Location = new Point(154, 3);
            exportButton.Name = "exportButton";
            exportButton.Size = new Size(145, 36);
            exportButton.TabIndex = 1;
            exportButton.Text = "CSV dışa aktar";
            exportButton.UseVisualStyleBackColor = false;
            exportButton.Click += Export_Click;
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
colTarih=new DataGridViewTextBoxColumn {DataPropertyName="Tarih",HeaderText="Tarih"};
colUrun=new DataGridViewTextBoxColumn {DataPropertyName="Ürün",HeaderText="Ürün"};
colSube=new DataGridViewTextBoxColumn {DataPropertyName="Depo",HeaderText="Şube / alan"};
colIslem=new DataGridViewTextBoxColumn {DataPropertyName="İşlem",HeaderText="İşlem"};
colBar=new DataGridViewTextBoxColumn {DataPropertyName="Bar",HeaderText="Bar"};
colDepotQuantity=new DataGridViewTextBoxColumn {DataPropertyName="DepoMiktar",HeaderText="Depo"};
colMiktar=new DataGridViewTextBoxColumn {DataPropertyName="Miktar",HeaderText="Miktar"};
colBirim=new DataGridViewTextBoxColumn {DataPropertyName="Birim",HeaderText="Birim"};
colAciklama=new DataGridViewTextBoxColumn {DataPropertyName="Açıklama",HeaderText="Açıklama"};
grid.Columns.AddRange(new DataGridViewColumn[]{colTarih,colUrun,colSube,colIslem,colBar,colDepotQuantity,colMiktar,colBirim,colAciklama});
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
            // FrmReports
            //
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 237, 227);
            ClientSize = new Size(1084, 761);
            Controls.Add(layout);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(950, 650);
            Name = "FrmReports";
            Text = "CafeStock AI - Raporlar";
            layout.ResumeLayout(false);
            layout.PerformLayout();
            editor.ResumeLayout(false);
            editor.PerformLayout();
            actions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            ResumeLayout(false);
        }
        private DataGridViewTextBoxColumn colBar, colDepotQuantity;
private TableLayoutPanel layout;
private Label titleLabel;
private Label descriptionLabel;
private TableLayoutPanel editor;
private FlowLayoutPanel actions;
private DataGridView grid;
private Label statusLabel;
private Label reportInputLabel;
private ComboBox reportInput;
private Label warehouseInputLabel;
private ComboBox warehouseInput;
private Label fromInputLabel;
private DateTimePicker fromInput;
private Label toInputLabel;
private DateTimePicker toInput;
private Button refreshButton;
private Button exportButton;




private DataGridViewTextBoxColumn colMiktar;


private DataGridViewTextBoxColumn colTarih;
private DataGridViewTextBoxColumn colUrun;
private DataGridViewTextBoxColumn colSube;
private DataGridViewTextBoxColumn colIslem;
private DataGridViewTextBoxColumn colBirim;
private DataGridViewTextBoxColumn colAciklama;
} }
