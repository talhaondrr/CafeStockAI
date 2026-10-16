namespace CafeStockAI.Desktop;
public partial class FrmLogin : Form
{
    public FrmLogin()
    {
        InitializeComponent();
        showPassword.CheckedChanged += (_, _) => txtPassword.UseSystemPasswordChar = !showPassword.Checked;
        txtUsername.TextChanged += (_, _) => errorLabel.Text = "";
        txtPassword.TextChanged += (_, _) => errorLabel.Text = "";
    }
    private void btnLogin_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtUsername.Text)) { errorLabel.Text = "Lütfen kullanıcı adınızı giriniz."; txtUsername.Focus(); return; }
        if (string.IsNullOrWhiteSpace(txtPassword.Text)) { errorLabel.Text = "Lütfen şifrenizi giriniz."; txtPassword.Focus(); return; }
        try
        {
            if (!Accounts.Login(txtUsername.Text, txtPassword.Text)) { errorLabel.Text = "Kullanıcı adı veya şifre hatalı; hesap kapatılmış olabilir."; return; }
            var dashboard = new FrmDashboard();
            dashboard.FormClosed += (_, _) => { Access.EmployeeId=null; txtPassword.Clear(); Show(); };
            dashboard.Show(); Hide();
        }
        catch (Exception ex) { errorLabel.Text = "Panel açılamadı: " + ex.Message; }
    }
}
