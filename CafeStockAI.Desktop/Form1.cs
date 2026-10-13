
using System;
using System.Windows.Forms;

namespace CafeStockAI.Desktop
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtUsername.Text.Trim();
            string sifre = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(kullaniciAdi))
            {
                MessageBox.Show(
                    "Lütfen kullanýcý adýnýzý giriniz.",
                    "CafeStock AI",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show(
                    "Lütfen þifrenizi giriniz.",
                    "CafeStock AI",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtPassword.Focus();
                return;
            }

            // Dashboard ekranýný aç
            FrmDashboard dashboard = new FrmDashboard();

            // Dashboard kapatýldýðýnda uygulamayý kapat
            dashboard.FormClosed += (s, args) => this.Close();

            dashboard.Show();

            // Giriþ ekranýný gizle
            this.Hide();
        }
    }
}
