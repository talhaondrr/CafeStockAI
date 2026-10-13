namespace CafeStockAI.Desktop
{
    partial class FrmLogin
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlBrand = new Panel();
            lblBrandTitle = new Label();
            lblBrandSubtitle = new Label();
            lblLoginTitle = new Label();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnLogin = new Button();
            pnlBrand.SuspendLayout();
            SuspendLayout();
            // 
            // pnlBrand
            // 
            pnlBrand.BackColor = Color.FromArgb(25, 35, 55);
            pnlBrand.Controls.Add(lblBrandSubtitle);
            pnlBrand.Controls.Add(lblBrandTitle);
            pnlBrand.Dock = DockStyle.Left;
            pnlBrand.Location = new Point(0, 0);
            pnlBrand.Name = "pnlBrand";
            pnlBrand.Size = new Size(400, 611);
            pnlBrand.TabIndex = 0;
            // 
            // lblBrandTitle
            // 
            lblBrandTitle.AutoSize = true;
            lblBrandTitle.BackColor = Color.Transparent;
            lblBrandTitle.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBrandTitle.ForeColor = Color.White;
            lblBrandTitle.Location = new Point(55, 230);
            lblBrandTitle.Name = "lblBrandTitle";
            lblBrandTitle.Size = new Size(243, 50);
            lblBrandTitle.TabIndex = 0;
            lblBrandTitle.Text = "CafeStock AI";
            // 
            // lblBrandSubtitle
            // 
            lblBrandSubtitle.AutoSize = true;
            lblBrandSubtitle.BackColor = Color.Transparent;
            lblBrandSubtitle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 162);
            lblBrandSubtitle.ForeColor = Color.FromArgb(180, 195, 215);
            lblBrandSubtitle.Location = new Point(74, 294);
            lblBrandSubtitle.Name = "lblBrandSubtitle";
            lblBrandSubtitle.Size = new Size(204, 21);
            lblBrandSubtitle.TabIndex = 1;
            lblBrandSubtitle.Text = "Akıllı Depo ve Stok Yönetimi";
            // 
            // lblLoginTitle
            // 
            lblLoginTitle.AutoSize = true;
            lblLoginTitle.BackColor = Color.Transparent;
            lblLoginTitle.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblLoginTitle.ForeColor = Color.FromArgb(25, 35, 55);
            lblLoginTitle.Location = new Point(490, 150);
            lblLoginTitle.Name = "lblLoginTitle";
            lblLoginTitle.Size = new Size(319, 40);
            lblLoginTitle.TabIndex = 1;
            lblLoginTitle.Text = "Hesabınıza Giriş Yapın";
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblUsername.ForeColor = Color.FromArgb(65, 75, 95);
            lblUsername.Location = new Point(490, 235);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(96, 20);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Kullanıcı Adı";
            // 
            // txtUsername
            // 
            txtUsername.BackColor = Color.White;
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 162);
            txtUsername.ForeColor = Color.FromArgb(25, 35, 55);
            txtUsername.Location = new Point(490, 265);
            txtUsername.MaxLength = 50;
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(400, 29);
            txtUsername.TabIndex = 3;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblPassword.ForeColor = Color.FromArgb(65, 75, 95);
            lblPassword.Location = new Point(490, 335);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(41, 20);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Şifre";
            lblPassword.Click += label1_Click;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.White;
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 162);
            txtPassword.ForeColor = Color.FromArgb(25, 35, 55);
            txtPassword.Location = new Point(490, 365);
            txtPassword.MaxLength = 100;
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(400, 29);
            txtPassword.TabIndex = 5;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(29, 173, 145);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(490, 435);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(345, 45);
            btnLogin.TabIndex = 6;
            btnLogin.Text = "Giriş Yap";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // FrmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 611);
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            Controls.Add(txtUsername);
            Controls.Add(lblUsername);
            Controls.Add(lblLoginTitle);
            Controls.Add(pnlBrand);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimumSize = new Size(900, 600);
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CafeStock AI - Giriş";
            pnlBrand.ResumeLayout(false);
            pnlBrand.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlBrand;
        private Label lblBrandTitle;
        private Label lblBrandSubtitle;
        private Label lblLoginTitle;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnLogin;
    }
}
