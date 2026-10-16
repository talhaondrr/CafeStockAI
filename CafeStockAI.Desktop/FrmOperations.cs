namespace CafeStockAI.Desktop;
public partial class FrmOperations : Form, IInventoryPage
{
 public FrmOperations()
 {
  InitializeComponent(); QuantityText.Setup(this); if(!Access.Global)tabs.TabPages.Remove(accountTab); counts.SelectionChanged+=(_,_)=>ShowLines();
  restore.Click+=(_,_)=>PageUi.Run(()=>{Access.RequireGlobal();using var dialog=new OpenFileDialog{Filter="CafeStock yedeği (*.zip)|*.zip"};if(dialog.ShowDialog(this)!=DialogResult.OK)return;if(MessageBox.Show("Seçilen yedek geri yüklenecek. Mevcut kayıtlar önce yedeklenecek ve oturum kapanacak. Devam edilsin mi?",Text,MessageBoxButtons.YesNo)!=DialogResult.Yes)return;Operations.Restore(dialog.FileName);(TopLevelControl as Form)?.Close();});
  logout.Click+=(_,_)=> (TopLevelControl as Form)?.Close();
  approve.Click+=(_,_)=>Review(true); reject.Click+=(_,_)=>Review(false);
  transfer.Click+=(_,_)=>PageUi.Run(()=>{ if(product.SelectedItem is not Product p) throw new InvalidOperationException("Ürün seçiniz."); Operations.Transfer(p.Id,source.Text,sourceArea.Text,target.Text,targetArea.Text,quantity.Value); RefreshData(); MessageBox.Show("Transfer tamamlandı. Kaynak ve hedef stok birlikte güncellendi."); });
  reset.Click+=(_,_)=>PageUi.Run(()=>{if(users.SelectedRows.Count==0)return; var id=(Guid)users.SelectedRows[0].Cells["EmployeeId"].Value; Accounts.Change(id,"123456"); MessageBox.Show("Şifre 123456 olarak yenilendi.");});
  toggle.Click+=(_,_)=>PageUi.Run(()=>{if(users.SelectedRows.Count==0)return; var id=(Guid)users.SelectedRows[0].Cells["EmployeeId"].Value; var account=Accounts.Load().Single(a=>a.EmployeeId==id); Accounts.Enable(id,!account.Enabled); RefreshData();});
  change.Click+=(_,_)=>PageUi.Run(()=>{if(Access.EmployeeId is not Guid id)return; Accounts.Change(id,password.Text); password.Clear(); MessageBox.Show("Şifreniz değiştirildi.");});
  backup.Click+=(_,_)=>PageUi.Run(()=>MessageBox.Show("Yedek oluşturuldu:\n"+Operations.Backup()));
 }
 public void RefreshData() => PageUi.Run(()=>
 {
  var data=Access.View(); var branches=BranchStock.AllNames(data); source.DataSource=branches.ToList(); target.DataSource=branches.ToList(); sourceArea.DataSource=new[]{"Bar","Depo"}; targetArea.DataSource=new[]{"Depo","Bar"}; product.DisplayMember="DisplayName"; product.DataSource=ProductStore.Load().OrderBy(p=>p.Name).ToList();
  counts.DataSource=data.Counts.OrderByDescending(c=>c.Date).Select(c=>new { c.Id, Tarih=c.Date, Şube=c.BranchName, SayımıYapan=c.Operator, Durum=c.Status, Ürün=c.Lines.Count, Onaylayan=c.Reviewer, Not=c.ReviewNote }).ToList(); if(counts.Columns.Contains("Id")) counts.Columns["Id"].Visible=false;
  foreach(var pair in new[]{("SayımıYapan","Sayımı yapan"),("Ürün","Ürün sayısı")}) if(counts.Columns.Contains(pair.Item1))counts.Columns[pair.Item1].HeaderText=pair.Item2;
  if(Access.Global) { Accounts.Sync(); users.DataSource=Accounts.Load().Select(a=>new {a.EmployeeId,KullanıcıAdı=a.Username,Görev=data.Employees.FirstOrDefault(e=>e.Id==a.EmployeeId)?.Role??"Ayrılmış",Şubeler=data.Employees.FirstOrDefault(e=>e.Id==a.EmployeeId)?.BranchesDisplay??"",Durum=a.Enabled?"Aktif":"Kapalı"}).ToList(); if(users.Columns.Contains("EmployeeId"))users.Columns["EmployeeId"].Visible=false;users.Columns["KullanıcıAdı"].HeaderText="Kullanıcı adı"; }
  users.Visible=reset.Visible=toggle.Visible=backup.Visible=restore.Visible=Access.Global; approve.Enabled=reject.Enabled=Access.Approver;
 });
 private void ShowLines() => PageUi.Run(()=>
 {
  if(counts.SelectedRows.Count==0||!counts.Columns.Contains("Id")){lines.DataSource=null;return;}var id=(Guid)counts.SelectedRows[0].Cells["Id"].Value;var count=Access.View().Counts.FirstOrDefault(c=>c.Id==id);var products=ProductStore.Load();lines.DataSource=count?.Lines.Select(l=>new{Ürün=products.FirstOrDefault(p=>p.Id==l.ProductId)?.Name,Birim=products.FirstOrDefault(p=>p.Id==l.ProductId)?.Unit,MevcutBar=l.BarExpected,SayılanBar=l.BarCounted,MevcutDepo=l.DepotExpected,SayılanDepo=l.DepotCounted,Fark=l.Counted-l.Expected}).ToList();
  foreach(var pair in new[]{("MevcutBar","Mevcut Bar"),("SayılanBar","Sayılan Bar"),("MevcutDepo","Mevcut Depo"),("SayılanDepo","Sayılan Depo")})if(lines.Columns.Contains(pair.Item1))lines.Columns[pair.Item1].HeaderText=pair.Item2;
 });
 private void Review(bool yes)=>PageUi.Run(()=>{if(counts.SelectedRows.Count==0)throw new InvalidOperationException("Bir sayım seçiniz."); Operations.Review((Guid)counts.SelectedRows[0].Cells["Id"].Value,yes,note.Text); RefreshData(); note.Clear();});
}
