namespace CafeStockAI.Desktop;
partial class FrmDatabaseSetup
{
 private TextBox serverInput=null!,databaseInput=null!;private Button connectButton=null!;private Label statusLabel=null!;
 private void InitializeComponent()
 {
  Text="Elazığ • SQL bağlantısı";ClientSize=new Size(650,370);StartPosition=FormStartPosition.CenterParent;Font=new Font("Segoe UI",10);BackColor=Color.FromArgb(243,237,227);FormBorderStyle=FormBorderStyle.FixedDialog;MaximizeBox=false;MinimizeBox=false;
  var title=new Label{Text="Elazığ veritabanına geç",Location=new Point(24,24),AutoSize=true,Font=new Font("Segoe UI",20,FontStyle.Bold)};var hint=new Label{Text="Windows oturumunuz kullanılır. Mevcut dbo tabloları korunur.\nUygulamanın kayıtları cafestock şemasına aktarılır; bütün personeller kalır.",Location=new Point(24,75),Size=new Size(600,50)};
  var serverLabel=new Label{Text="SQL sunucusu",Location=new Point(24,140),AutoSize=true};serverInput=new TextBox{Name="serverInput",Text="localhost\\SQLEXPRESS",Location=new Point(190,136),Width=420};var databaseLabel=new Label{Text="Veritabanı",Location=new Point(24,185),AutoSize=true};databaseInput=new TextBox{Name="databaseInput",Text="CafeStockAI",Location=new Point(190,181),Width=420};
  connectButton=new Button{Name="connectButton",Text="Bağlan ve Elazığ'a aktar",Location=new Point(190,232),Size=new Size(260,42),BackColor=Color.FromArgb(112,74,45),ForeColor=Color.White,FlatStyle=FlatStyle.Flat};statusLabel=new Label{Name="statusLabel",Location=new Point(24,290),Size=new Size(600,65),Text="Aktarım öncesi yedek alınır. Diğer şubeler yedekte saklanır."};Controls.AddRange(new Control[]{title,hint,serverLabel,serverInput,databaseLabel,databaseInput,connectButton,statusLabel});AcceptButton=connectButton;
 }
}
