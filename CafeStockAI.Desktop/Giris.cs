namespace CafeStockAI.Desktop;
public partial class FrmLogin : DevExpress.XtraEditors.XtraForm
{
    public FrmLogin()
    {
        InitializeComponent();
        showPassword.CheckedChanged += (_, _) => txtPassword.UseSystemPasswordChar = !showPassword.Checked;
        if(SqlMode.Configured&&!SqlMode.Enabled){errorLabel.Text="Veritabanına ulaşılamıyor. Giriş yaparken bağlantı yeniden denenecek.";}
        txtUsername.TextChanged += (_, _) => errorLabel.Text = "";
        txtPassword.TextChanged += (_, _) => errorLabel.Text = "";
    }
    private void btnLogin_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtUsername.Text)) { errorLabel.Text = "Lütfen kullanıcı adınızı giriniz."; txtUsername.Focus(); return; }
        if (string.IsNullOrWhiteSpace(txtPassword.Text)) { errorLabel.Text = "Lütfen şifrenizi giriniz."; txtPassword.Focus(); return; }
        try
        {
            if(SqlMode.Configured&&!SqlMode.Enabled)
            {
                SqlMode.Initialize();
                if(!SqlMode.Enabled)
                {
                    errorLabel.Text="Veritabanına ulaşılamadı. SQL Server hizmetini kontrol edin.";
                    if(MessageBox.Show("Veritabanına ulaşılamadı. Bağlantı ayarlarını açmak ister misiniz?",Text,MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes){using var setup=new FrmDatabaseSetup();setup.ShowDialog(this);}
                    if(!SqlMode.Enabled)return;
                }
            }
            if (!Accounts.Login(txtUsername.Text, txtPassword.Text)) { errorLabel.Text = "Kullanıcı adı veya şifre hatalı; hesap kapatılmış olabilir."; return; }
            var dashboard = new FrmDashboard();
            dashboard.FormClosed += (_, _) => { Access.EmployeeId=null; txtPassword.Clear(); Show(); };
            dashboard.Show(); Hide();
        }
        catch (Exception ex) { errorLabel.Text = "Panel açılamadı: " + ex.Message; }
    }
}
