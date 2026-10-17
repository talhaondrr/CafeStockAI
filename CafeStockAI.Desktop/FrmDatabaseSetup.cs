namespace CafeStockAI.Desktop;
public partial class FrmDatabaseSetup : DevExpress.XtraEditors.XtraForm
{
 private bool connecting;
 public FrmDatabaseSetup(){InitializeComponent();connectButton.Click+=Connect_Click;FormClosing+=(_,e)=>e.Cancel=connecting;}
 private async void Connect_Click(object? sender,EventArgs e)
 {
  connecting=true;connectButton.Enabled=false;serverInput.Enabled=databaseInput.Enabled=false;statusLabel.Text="Bağlantı ve Elazığ aktarımı kontrol ediliyor…";
  try
  {
   var source=SqlMode.ReadSource();Operations.CreateBackup();string server=serverInput.Text,database=databaseInput.Text;
   await Task.Run(()=>SqlMode.ConnectAndImport(server,database,source));statusLabel.Text="SQL etkin. Bütün personeller Elazığ'a atandı; görevleri ve ürünler korundu.";
   MessageBox.Show("SQL bağlantısı hazır. Kullanıcı adınızla giriş yapabilirsiniz.",Text);connecting=false;DialogResult=DialogResult.OK;Close();
  }
  catch(Exception ex){statusLabel.Text=ex is InvalidOperationException or InvalidDataException?ex.Message:"SQL kurulumu tamamlanamadı. Sunucu erişimi veya şema çakışması kontrol edilmeli. Mevcut kaynak dosyaları yedekte korunuyor.";}
  finally{connecting=false;if(!IsDisposed){connectButton.Enabled=true;serverInput.Enabled=databaseInput.Enabled=true;}}
 }
}
