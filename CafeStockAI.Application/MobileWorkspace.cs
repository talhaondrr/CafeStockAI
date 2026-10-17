using CafeStockAI.Application.Models;
using CafeStockAI.Application.Analysis;
namespace CafeStockAI.Application.Mobile;

public sealed record CountExpected(Guid ProductId,string Unit,decimal Bar,decimal Depot);
public sealed record CountTicket(Guid Id,Guid EmployeeId,DateTimeOffset Expires,List<CountExpected> Products);
public sealed record CountEntry(Guid ProductId,decimal? Bar,decimal? Depot);
public sealed record ProductEdit(Guid? Id,string Name,string Category,string Unit,decimal MinimumStock);
public sealed record StaffEdit(Guid? Id,string Name,string Role,string Phone,DateTime? HireDate);
public sealed record AccountEdit(Guid EmployeeId,bool Enabled,string? Password);
public sealed record MovementEdit(Guid Id,Guid ProductId,string Area,string Type,decimal Quantity,string Note);

/// <summary>Request-scoped identity; no Desktop static session is shared with web users.</summary>
public sealed class MobileWorkspace(IInventoryPersistence store)
{
 public static readonly string[] Roles=["Personel","Şube Müdürü","Genel Müdür","Bölge Müdürü","İşletme Sahibi","Süpervizör","Depo Sorumlusu"];
 public static bool Global(Employee p)=>p.Role is "İşletme Sahibi" or "Genel Müdür";
 public static bool Approver(Employee p)=>Global(p)||p.Role is "Şube Müdürü" or "Bölge Müdürü" or "Süpervizör";
 public static bool Movement(Employee p)=>Approver(p)||p.Role=="Depo Sorumlusu";
 public static Employee Person(InventoryArchive data,Guid id)
 {
  var p=data.Inventory.Employees.SingleOrDefault(p=>p.Id==id&&p.IsActive);
  if(p is null||!data.Accounts.Any(a=>a.EmployeeId==id&&a.Enabled))throw new UnauthorizedAccessException("Hesabınız kapalı. Yeniden giriş yapın.");
  if(!Global(p)&&!p.AssignedBranches.Contains("Elazığ"))throw new UnauthorizedAccessException("Elazığ şubesine erişim yetkiniz yok.");
  return p;
 }
 private IWorkspacePersistence Checked=>store as IWorkspacePersistence??throw new InvalidOperationException("Depolama bu işlemi desteklemiyor.");
 public InventoryArchive Snapshot(Guid id){var a=store.Export();Person(a,id);return a;}
 public static decimal Balance(InventoryData d,Guid product,string area)=>d.Warehouses.Where(w=>w.BranchName=="Elazığ"&&w.StockArea==area).Sum(w=>d.Balance(product,w.Id));
 private static void Quantity(decimal n){if(n<0||n>=1000000000000m||decimal.Round(n,6)!=n)throw new InvalidOperationException("Miktar negatif olamaz; en fazla 6 ondalık basamak kullanılabilir.");}
 private static string Text(string? s,int max=160){s=(s??"").Trim();if(s.Length>max)throw new InvalidOperationException($"Metin en fazla {max} karakter olabilir.");return s;}
 private static void RequireGlobal(Employee p){if(!Global(p))throw new UnauthorizedAccessException("Genel yönetim yetkisi gerekiyor.");}
 public CountTicket Begin(Guid id)
 {
  var a=Snapshot(id);if(a.Products.Any(p=>string.IsNullOrWhiteSpace(p.Unit)))throw new InvalidOperationException("Eksik ürün birimleri yönetici tarafından tamamlanmalı.");if(a.Products.Count==0)throw new InvalidOperationException("Sayılacak ürün bulunamadı.");
  return new(Guid.NewGuid(),id,DateTimeOffset.UtcNow.AddHours(24),a.Products.Select(p=>new CountExpected(p.Id,p.Unit,Balance(a.Inventory,p.Id,"Bar"),Balance(a.Inventory,p.Id,"Depo"))).ToList());
 }
 public Guid Submit(Guid id,CountTicket ticket,List<CountEntry> entries)
 {
  var a=Snapshot(id);var person=Person(a,id);
  if(ticket.EmployeeId!=id||ticket.Expires<DateTimeOffset.UtcNow)throw new UnauthorizedAccessException("Sayım taslağının oturumu veya süresi geçersiz.");
  if(entries.Count==0||entries.Count!=ticket.Products.Count||entries.Select(l=>l.ProductId).Distinct().Count()!=entries.Count)throw new InvalidOperationException("Bütün ürünlerin Bar ve Depo miktarlarını girin.");
  var entered=entries.ToDictionary(l=>l.ProductId);
  foreach(var expected in ticket.Products){if(!entered.TryGetValue(expected.ProductId,out var entry)||!entry.Bar.HasValue||!entry.Depot.HasValue)throw new InvalidOperationException("Eksik ürün miktarı var. Boş alanlar sıfır kabul edilmez.");Quantity(entry.Bar.Value);Quantity(entry.Depot.Value);}
  var previous=a.Inventory.Counts.FirstOrDefault(c=>c.Id==ticket.Id);
  if(previous is not null)
  {
   if(previous.Operator!=person.Name||previous.Lines.Count!=entries.Count||previous.Lines.Any(l=>!entered.TryGetValue(l.ProductId,out var e)||e.Bar!=l.BarCounted||e.Depot!=l.DepotCounted))throw new InvalidOperationException("Bu gönderim numarası farklı bir sayımda kullanılmış.");
   return previous.Id;
  }
  if(!a.Products.Select(p=>p.Id).Order().SequenceEqual(ticket.Products.Select(p=>p.ProductId).Order())||a.Products.Any(p=>ticket.Products.Single(e=>e.ProductId==p.Id).Unit!=p.Unit))throw new InvalidOperationException("Ürünler veya birimler değişti. Yeni sayım başlatın.");
  if(ticket.Products.Any(e=>e.Bar!=Balance(a.Inventory,e.ProductId,"Bar")||e.Depot!=Balance(a.Inventory,e.ProductId,"Depo")))throw new InvalidOperationException("Sayım sırasında stok değişti. Taslağınız korunuyor; güncel stokla yeni sayım başlatın.");
  var depot=a.Inventory.Warehouses.First(w=>w.BranchName=="Elazığ"&&w.StockArea=="Depo");
  a.Inventory.Counts.Add(new StockCount{Id=ticket.Id,WarehouseId=depot.Id,BranchName="Elazığ",Operator=person.Name,Status="Onay bekliyor",Date=DateTime.Now,Lines=ticket.Products.Select(e=>new CountLine{ProductId=e.ProductId,BarExpected=e.Bar,DepotExpected=e.Depot,Expected=e.Bar+e.Depot,BarCounted=entered[e.ProductId].Bar,DepotCounted=entered[e.ProductId].Depot,Counted=entered[e.ProductId].Bar!.Value+entered[e.ProductId].Depot!.Value}).ToList()});
  store.SaveInventory(a.Inventory);return ticket.Id;
 }
 public void Review(Guid id,Guid countId,bool approved,string? note)
 {
  var a=Snapshot(id);var person=Person(a,id);if(!Approver(person))throw new UnauthorizedAccessException("Sayım onaylama yetkiniz yok.");
  var count=a.Inventory.Counts.SingleOrDefault(c=>c.Id==countId&&c.BranchName=="Elazığ")??throw new InvalidOperationException("Sayım bulunamadı.");
  if(count.Status!="Onay bekliyor")throw new InvalidOperationException("Sayım zaten sonuçlandırılmış.");
  note=Text(note,2000);if(!approved&&note.Length==0)throw new InvalidOperationException("Ret gerekçesi girin.");
  if(approved)
  {
   if(!a.Products.Select(p=>p.Id).Order().SequenceEqual(count.Lines.Select(l=>l.ProductId).Order()))throw new InvalidOperationException("Ürün listesi değişti. Yeni sayım gerekiyor.");
   foreach(var l in count.Lines)
   {
    if(l.BarExpected is null||l.DepotExpected is null||l.BarCounted is null||l.DepotCounted is null)throw new InvalidOperationException("Eski sayım alanları eksik; yeniden sayım gerekiyor.");
    if(Balance(a.Inventory,l.ProductId,"Bar")!=l.BarExpected||Balance(a.Inventory,l.ProductId,"Depo")!=l.DepotExpected)throw new InvalidOperationException("Stok değişti; bu sayım onaylanamaz.");
    foreach(var area in new[]{"Bar","Depo"})
    {
     decimal delta=area=="Bar"?l.BarCounted.Value-l.BarExpected.Value:l.DepotCounted.Value-l.DepotExpected.Value;
     if(delta!=0)a.Inventory.Movements.Add(new StockMovement{ActorId=id,CountId=count.Id,ProductId=l.ProductId,WarehouseId=a.Inventory.Warehouses.Single(w=>w.BranchName=="Elazığ"&&w.StockArea==area).Id,Quantity=delta,Type="Sayım düzeltmesi",Note=$"Mobil onay • {person.Name} • {area}"});
    }
   }
  }
  count.Status=approved?"Onaylandı":"Reddedildi";count.Reviewer=person.Name;count.ReviewedAt=DateTime.Now;count.ReviewNote=note;store.SaveInventory(a.Inventory);
 }
 public void AddMovement(Guid id,MovementEdit cmd)
 {
  var a=Snapshot(id);var p=Person(a,id);if(!Movement(p))throw new UnauthorizedAccessException("Stok hareketi yetkiniz yok.");
  Quantity(cmd.Quantity);if(cmd.Quantity==0||cmd.Id==Guid.Empty||cmd.Area is not("Bar" or "Depo")||cmd.Type is not("Giriş" or "Çıkış")||!a.Products.Any(p=>p.Id==cmd.ProductId))throw new InvalidOperationException("Ürün, alan, işlem ve miktarı kontrol edin.");
  var area=a.Inventory.Warehouses.Single(w=>w.BranchName=="Elazığ"&&w.StockArea==cmd.Area);decimal amount=cmd.Type=="Çıkış"?-cmd.Quantity:cmd.Quantity;
  string note=Text(cmd.Note,1800)+" • "+p.Name;var old=a.Inventory.Movements.SingleOrDefault(m=>m.Id==cmd.Id);if(old is not null){if(old.ProductId==cmd.ProductId&&old.WarehouseId==area.Id&&old.Quantity==amount&&old.ActorId==id)return;throw new InvalidOperationException("İşlem numarası kullanılmış.");}
  if(Balance(a.Inventory,cmd.ProductId,cmd.Area)+amount<0)throw new InvalidOperationException("Yeterli stok yok.");
  a.Inventory.Movements.Add(new StockMovement{Id=cmd.Id,ProductId=cmd.ProductId,WarehouseId=area.Id,ActorId=id,Quantity=amount,Type=cmd.Type,Note=note});store.SaveInventory(a.Inventory);
 }
 public void SaveProduct(Guid id,ProductEdit edit)
 {
  var a=Snapshot(id);RequireGlobal(Person(a,id));Quantity(edit.MinimumStock);
  string name=Text(edit.Name),category=Text(edit.Category),unit=Text(edit.Unit);if(name.Length==0||category.Length==0||unit.Length==0)throw new InvalidOperationException("Ürün adı, kategori ve birim zorunlu.");
  if(a.Products.Any(p=>p.Id!=edit.Id&&AccountCredentials.Key(p.Name)==AccountCredentials.Key(name)&&AccountCredentials.Key(p.Category)==AccountCredentials.Key(category)))throw new InvalidOperationException("Bu kategoride aynı isimde ürün var.");
  var old=a.Products.SingleOrDefault(p=>p.Id==edit.Id);
  if(edit.Id.HasValue&&old is null)throw new InvalidOperationException("Ürün bulunamadı.");
  if(old is not null&&old.Unit!=unit&&(a.Inventory.Movements.Any(m=>m.ProductId==old.Id)||a.Inventory.Counts.Any(c=>c.Lines.Any(l=>l.ProductId==old.Id))))throw new InvalidOperationException("Geçmişi olan ürünün birimini değiştirmek için yeni ürün açın.");
  var row=old??new Product{Code="MOB-"+Guid.NewGuid().ToString("N")[..8]};row.Name=name;row.Category=category;row.Unit=unit;row.MinimumStock=edit.MinimumStock;if(old is null)a.Products.Add(row);Checked.SaveProductsChecked(a.Products,a.Inventory.SqlVersion);
 }
 public void RemoveProduct(Guid id,Guid product)
 {
  var a=Snapshot(id);RequireGlobal(Person(a,id));if(!a.Products.Any(p=>p.Id==product))throw new InvalidOperationException("Ürün bulunamadı.");
  Checked.SaveProductsChecked(a.Products.Where(p=>p.Id!=product).ToList(),a.Inventory.SqlVersion);
 }
 public void SaveStaff(Guid id,StaffEdit edit)
 {
  var a=Snapshot(id);RequireGlobal(Person(a,id));string name=Text(edit.Name);if(name.Length==0||!Roles.Contains(edit.Role))throw new InvalidOperationException("Personel adı ve geçerli görev girin.");
  if(a.Inventory.Employees.Any(e=>e.Id!=edit.Id&&AccountCredentials.Key(e.Name)==AccountCredentials.Key(name)))throw new InvalidOperationException("Bu isimde personel var.");
  string digits=new string(Text(edit.Phone).Where(char.IsDigit).ToArray());if(digits.Length==12&&digits.StartsWith("90"))digits="0"+digits[2..];if(digits.Length==10)digits="0"+digits;if(digits.Length!=0&&(digits.Length!=11||digits[0]!='0'))throw new InvalidOperationException("11 haneli telefon girin.");
  var old=a.Inventory.Employees.SingleOrDefault(e=>e.Id==edit.Id);
  if(edit.Id.HasValue&&old is null)throw new InvalidOperationException("Personel bulunamadı.");
  var row=old??new Employee();row.Name=name;row.Role=edit.Role;row.HireDate=edit.HireDate;row.Phone=digits.Length==11?$"{digits[..4]} {digits[4..7]} {digits[7..9]} {digits[9..]}":"";row.Branches=["Elazığ"];row.BranchName="Elazığ";if(old is null)a.Inventory.Employees.Add(row);
  if(!a.Inventory.Employees.Any(e=>e.IsActive&&Global(e)&&a.Accounts.Any(u=>u.EmployeeId==e.Id&&u.Enabled)))throw new InvalidOperationException("Son aktif yönetici görevi değiştirilemez.");
  // Save staff and account changes together via an archive transaction.
  var account=a.Accounts.SingleOrDefault(u=>u.EmployeeId==row.Id);
  if(account is null){account=new UserAccount{EmployeeId=row.Id};AccountCredentials.Set(account,"123456");a.Accounts.Add(account);}account.Username=name;
  Checked.SaveStaffAndAccounts(a.Inventory,a.Accounts);
 }
 public void RemoveStaff(Guid id,Guid employeeId)
 {
  var a=Snapshot(id);RequireGlobal(Person(a,id));if(id==employeeId)throw new InvalidOperationException("Kendi personel kaydınızı kaldıramazsınız.");
  var target=a.Inventory.Employees.SingleOrDefault(e=>e.Id==employeeId)??throw new InvalidOperationException("Personel bulunamadı.");
  target.IsActive=false;var account=a.Accounts.FirstOrDefault(u=>u.EmployeeId==employeeId);if(account is not null)account.Enabled=false;
  if(!a.Inventory.Employees.Any(e=>e.IsActive&&Global(e)&&a.Accounts.Any(u=>u.EmployeeId==e.Id&&u.Enabled)))throw new InvalidOperationException("Son yönetici kaldırılamaz.");
  Checked.SaveStaffAndAccounts(a.Inventory,a.Accounts);
 }
 public void ChangeAccount(Guid id,AccountEdit edit)
 {
  var a=Snapshot(id);RequireGlobal(Person(a,id));var target=a.Accounts.SingleOrDefault(u=>u.EmployeeId==edit.EmployeeId)??throw new InvalidOperationException("Hesap bulunamadı.");
  if(edit.EmployeeId==id&&!edit.Enabled)throw new InvalidOperationException("Kendi hesabınızı kapatamazsınız.");
  target.Enabled=edit.Enabled;if(!string.IsNullOrEmpty(edit.Password))AccountCredentials.Set(target,edit.Password);
  if(!a.Inventory.Employees.Any(e=>e.IsActive&&Global(e)&&a.Accounts.Any(u=>u.EmployeeId==e.Id&&u.Enabled)))throw new InvalidOperationException("Son yönetici hesabı kapatılamaz.");
  Checked.SaveAccountsChecked(a.Accounts,a.Inventory.SqlVersion);
 }
 public void SaveSettings(Guid id,string business)
 {
  var a=Snapshot(id);RequireGlobal(Person(a,id));business=Text(business);if(business.Length==0)throw new InvalidOperationException("İşletme adı zorunlu.");a.Inventory.Settings.BusinessName=business;store.SaveInventory(a.Inventory);
 }
}



