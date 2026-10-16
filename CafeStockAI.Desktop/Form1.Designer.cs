namespace CafeStockAI.Desktop
{
    partial class FrmLogin
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            root = new TableLayoutPanel();
            pnlBrand = new Panel();
            brandLayout = new TableLayoutPanel();
            lblBrandTitle = new Label();
            lblBrandSubtitle = new Label();
            brandHeadline = new Label();
            brandDescription = new Label();
            art = new CoffeeArt();
            brandFooter = new Label();
            rightLayout = new TableLayoutPanel();
            loginCard = new CoffeeCard();
            formLayout = new TableLayoutPanel();
            badge = new Label();
            lblLoginTitle = new Label();
            loginSubtitle = new Label();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            showPassword = new CheckBox();
            btnLogin = new Button();
            errorLabel = new Label();
            footer = new Label();
            root.SuspendLayout();
            pnlBrand.SuspendLayout();
            brandLayout.SuspendLayout();
            rightLayout.SuspendLayout();
            loginCard.SuspendLayout();
            formLayout.SuspendLayout();
            SuspendLayout();
            //
            // root
            //
            root.ColumnCount = 2;
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43F));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57F));
            root.Controls.Add(pnlBrand, 0, 0);
            root.Controls.Add(rightLayout, 1, 0);
            root.Dock = DockStyle.Fill;
            root.Location = new Point(0, 0);
            root.Margin = new Padding(0);
            root.Name = "root";
            root.RowCount = 1;
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.Size = new Size(1180, 780);
            root.TabIndex = 0;
            //
            // pnlBrand
            //
            pnlBrand.BackColor = Color.FromArgb(35, 33, 30);
            pnlBrand.Controls.Add(brandLayout);
            pnlBrand.Dock = DockStyle.Fill;
            pnlBrand.Location = new Point(0, 0);
            pnlBrand.Margin = new Padding(0);
            pnlBrand.Name = "pnlBrand";
            pnlBrand.Padding = new Padding(40, 38, 40, 32);
            pnlBrand.Size = new Size(507, 780);
            pnlBrand.TabIndex = 0;
            //
            // brandLayout
            //
            brandLayout.ColumnCount = 1;
            brandLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            brandLayout.Controls.Add(lblBrandTitle, 0, 0);
            brandLayout.Controls.Add(lblBrandSubtitle, 0, 1);
            brandLayout.Controls.Add(brandHeadline, 0, 2);
            brandLayout.Controls.Add(brandDescription, 0, 3);
            brandLayout.Controls.Add(art, 0, 4);
            brandLayout.Controls.Add(brandFooter, 0, 5);
            brandLayout.Dock = DockStyle.Fill;
            brandLayout.Location = new Point(40, 38);
            brandLayout.Name = "brandLayout";
            brandLayout.RowCount = 6;
            brandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            brandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            brandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 105F));
            brandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            brandLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            brandLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            brandLayout.Size = new Size(427, 710);
            brandLayout.TabIndex = 0;
            //
            // lblBrandTitle
            //
            lblBrandTitle.AutoSize = true;
            lblBrandTitle.Font = new Font("Segoe UI", 25F, FontStyle.Bold);
            lblBrandTitle.ForeColor = Color.FromArgb(249, 239, 222);
            lblBrandTitle.Location = new Point(3, 0);
            lblBrandTitle.Name = "lblBrandTitle";
            lblBrandTitle.Size = new Size(221, 45);
            lblBrandTitle.TabIndex = 0;
            lblBrandTitle.Text = "CafeStock AI";
            //
            // lblBrandSubtitle
            //
            lblBrandSubtitle.AutoSize = true;
            lblBrandSubtitle.Font = new Font("Segoe UI", 9F);
            lblBrandSubtitle.ForeColor = Color.FromArgb(188, 160, 122);
            lblBrandSubtitle.Location = new Point(3, 45);
            lblBrandSubtitle.Name = "lblBrandSubtitle";
            lblBrandSubtitle.Size = new Size(183, 15);
            lblBrandSubtitle.TabIndex = 1;
            lblBrandSubtitle.Text = "KAHVENİN ARKASINDAKİ DÜZEN";
            //
            // brandHeadline
            //
            brandHeadline.Dock = DockStyle.Fill;
            brandHeadline.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            brandHeadline.ForeColor = Color.FromArgb(249, 239, 222);
            brandHeadline.Location = new Point(3, 109);
            brandHeadline.Name = "brandHeadline";
            brandHeadline.Size = new Size(421, 105);
            brandHeadline.TabIndex = 2;
            brandHeadline.Text = "İyi kahvenin ardında,\niyi bir düzen vardır.";
            //
            // brandDescription
            //
            brandDescription.Dock = DockStyle.Fill;
            brandDescription.Font = new Font("Segoe UI", 11F);
            brandDescription.ForeColor = Color.FromArgb(195, 180, 158);
            brandDescription.Location = new Point(3, 214);
            brandDescription.Name = "brandDescription";
            brandDescription.Size = new Size(421, 65);
            brandDescription.TabIndex = 3;
            brandDescription.Text = "Stoklarınızı takip edin, ekibinizi yönetin.\nHer sayımda işletmenizi daha iyi tanıyın.";
            //
            // art
            //
            art.BackColor = Color.FromArgb(35, 33, 30);
            art.Dock = DockStyle.Fill;
            art.Location = new Point(3, 282);
            art.Name = "art";
            art.Size = new Size(421, 381);
            art.TabIndex = 4;
            //
            // brandFooter
            //
            brandFooter.Dock = DockStyle.Fill;
            brandFooter.Font = new Font("Segoe UI", 9F);
            brandFooter.ForeColor = Color.FromArgb(188, 160, 122);
            brandFooter.Location = new Point(3, 666);
            brandFooter.Name = "brandFooter";
            brandFooter.Size = new Size(421, 44);
            brandFooter.TabIndex = 5;
            brandFooter.Text = "STOK   /   SAYIM   /   EKİP";
            brandFooter.TextAlign = ContentAlignment.BottomLeft;
            //
            // rightLayout
            //
            rightLayout.ColumnCount = 3;
            rightLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9F));
            rightLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 82F));
            rightLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9F));
            rightLayout.Controls.Add(loginCard, 1, 1);
            rightLayout.Dock = DockStyle.Fill;
            rightLayout.Location = new Point(507, 0);
            rightLayout.Margin = new Padding(0);
            rightLayout.Name = "rightLayout";
            rightLayout.RowCount = 3;
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 555F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            rightLayout.Size = new Size(673, 780);
            rightLayout.TabIndex = 1;
            //
            // loginCard
            //
            loginCard.BackColor = Color.FromArgb(255, 253, 249);
            loginCard.Controls.Add(formLayout);
            loginCard.Dock = DockStyle.Fill;
            loginCard.Location = new Point(63, 115);
            loginCard.Name = "loginCard";
            loginCard.Padding = new Padding(35);
            loginCard.Size = new Size(545, 549);
            loginCard.TabIndex = 0;
            //
            // formLayout
            //
            formLayout.ColumnCount = 1;
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            formLayout.Controls.Add(badge, 0, 0);
            formLayout.Controls.Add(lblLoginTitle, 0, 1);
            formLayout.Controls.Add(loginSubtitle, 0, 2);
            formLayout.Controls.Add(lblUsername, 0, 3);
            formLayout.Controls.Add(txtUsername, 0, 4);
            formLayout.Controls.Add(lblPassword, 0, 5);
            formLayout.Controls.Add(txtPassword, 0, 6);
            formLayout.Controls.Add(showPassword, 0, 7);
            formLayout.Controls.Add(btnLogin, 0, 8);
            formLayout.Controls.Add(errorLabel, 0, 9);
            formLayout.Controls.Add(footer, 0, 11);
            formLayout.Dock = DockStyle.Fill;
            formLayout.Location = new Point(35, 35);
            formLayout.Name = "formLayout";
            formLayout.RowCount = 12;
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 53F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 39F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            formLayout.Size = new Size(475, 479);
            formLayout.TabIndex = 0;
            //
            // badge
            //
            badge.AutoSize = true;
            badge.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            badge.ForeColor = Color.FromArgb(154, 111, 70);
            badge.Location = new Point(3, 0);
            badge.Name = "badge";
            badge.Size = new Size(102, 15);
            badge.TabIndex = 0;
            badge.Text = "YÖNETİM PANELİ";
            //
            // lblLoginTitle
            //
            lblLoginTitle.AutoSize = true;
            lblLoginTitle.Font = new Font("Segoe UI", 23F, FontStyle.Bold);
            lblLoginTitle.Location = new Point(3, 36);
            lblLoginTitle.Name = "lblLoginTitle";
            lblLoginTitle.Size = new Size(302, 42);
            lblLoginTitle.TabIndex = 1;
            lblLoginTitle.Text = "Tekrar hoş geldiniz.";
            //
            // loginSubtitle
            //
            loginSubtitle.Dock = DockStyle.Fill;
            loginSubtitle.ForeColor = Color.FromArgb(130, 111, 91);
            loginSubtitle.Location = new Point(3, 84);
            loginSubtitle.Name = "loginSubtitle";
            loginSubtitle.Size = new Size(469, 53);
            loginSubtitle.TabIndex = 2;
            loginSubtitle.Text = "İşletmenizin güncel durumuna birlikte bakalım.";
            //
            // lblUsername
            //
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblUsername.Location = new Point(3, 137);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(90, 19);
            lblUsername.TabIndex = 3;
            lblUsername.Text = "Kullanıcı adı";
            //
            // txtUsername
            //
            txtUsername.Dock = DockStyle.Top;
            txtUsername.Font = new Font("Segoe UI", 13F);
            txtUsername.Location = new Point(3, 167);
            txtUsername.MaxLength = 50;
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Kullanıcı adınızı yazın";
            txtUsername.Size = new Size(469, 31);
            txtUsername.TabIndex = 0;
            //
            // lblPassword
            //
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPassword.Location = new Point(3, 216);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(40, 19);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Şifre";
            //
            // txtPassword
            //
            txtPassword.Dock = DockStyle.Top;
            txtPassword.Font = new Font("Segoe UI", 13F);
            txtPassword.Location = new Point(3, 246);
            txtPassword.MaxLength = 100;
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Şifrenizi yazın";
            txtPassword.Size = new Size(469, 31);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;
            //
            // showPassword
            //
            showPassword.AutoSize = true;
            showPassword.ForeColor = Color.FromArgb(130, 111, 91);
            showPassword.Location = new Point(3, 291);
            showPassword.Name = "showPassword";
            showPassword.Size = new Size(107, 23);
            showPassword.TabIndex = 2;
            showPassword.Text = "Şifreyi göster";
            //
            // btnLogin
            //
            btnLogin.BackColor = Color.FromArgb(112, 74, 45);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Dock = DockStyle.Top;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(3, 330);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(469, 44);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "Giriş yap  →";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            //
            // errorLabel
            //
            errorLabel.Dock = DockStyle.Fill;
            errorLabel.Font = new Font("Segoe UI", 9F);
            errorLabel.ForeColor = Color.FromArgb(177, 67, 49);
            errorLabel.Location = new Point(3, 379);
            errorLabel.Name = "errorLabel";
            errorLabel.Size = new Size(469, 48);
            errorLabel.TabIndex = 5;
            //
            // footer
            //
            footer.AutoSize = true;
            footer.Font = new Font("Segoe UI", 9F);
            footer.ForeColor = Color.FromArgb(146, 130, 110);
            footer.Location = new Point(3, 453);
            footer.Name = "footer";
            footer.Size = new Size(211, 15);
            footer.TabIndex = 6;
            footer.Text = "CafeStock AI • Daha düzenli bir işletme";
            //
            // FrmLogin
            //
            AcceptButton = btnLogin;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 237, 227);
            ClientSize = new Size(1180, 780);
            Controls.Add(root);
            Font = new Font("Segoe UI", 10F);
            ForeColor = Color.FromArgb(62, 43, 28);
            MinimumSize = new Size(1080, 720);
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CafeStock AI - Giriş";
            root.ResumeLayout(false);
            pnlBrand.ResumeLayout(false);
            brandLayout.ResumeLayout(false);
            brandLayout.PerformLayout();
            rightLayout.ResumeLayout(false);
            loginCard.ResumeLayout(false);
            formLayout.ResumeLayout(false);
            formLayout.PerformLayout();
            ResumeLayout(false);
        }
        private TableLayoutPanel root, brandLayout, rightLayout, formLayout;
        private Panel pnlBrand;
        private CoffeeCard loginCard;
        private CoffeeArt art;
        private Label lblBrandTitle, lblBrandSubtitle, brandHeadline, brandDescription, brandFooter, badge, lblLoginTitle, loginSubtitle, lblUsername, lblPassword, errorLabel, footer;
        private TextBox txtUsername, txtPassword;
        private CheckBox showPassword;
        private Button btnLogin;
    }
}
