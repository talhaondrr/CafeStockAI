using CafeStockAI.Application;
using CafeStockAI.Application.Mobile;
using CafeStockAI.Infrastructure;
internal static class MobilePersistenceChecks
{
 public static void Run(SqlInventoryPersistence store,List<string> checks)
 {
  void Assert(bool v,string m){if(!v)throw new Exception(m);checks.Add(m);}
  var a=store.Export();var manager=a.Inventory.Employees.First(e=>e.Role=="Genel Müdür");var person=a.Inventory.Employees.First(e=>e.Role=="Personel");var service=new MobileWorkspace(store);
  var ticket=service.Begin(person.Id);var original=store.LoadInventory().Movements.Count;
  var lines=ticket.Products.Select(p=>new CountEntry(p.ProductId,p.Bar+2,p.Depot)).ToList();
  Guid id=service.Submit(person.Id,ticket,lines);service.Submit(person.Id,ticket,lines);
  Assert(store.LoadInventory().Movements.Count==original&&store.LoadInventory().Counts.Count(c=>c.Id==id)==1,"Mobil SQL gönderimi bekleyen sayım oluşturur; tekrarında çoğalmaz ve stok değişmez");
  service.Review(manager.Id,id,true,"Mobil kontrol");Assert(store.LoadInventory().Movements.Count==original+lines.Count,"Mobil onay SQL'de durum ve fark hareketlerini birlikte kaydetti");
  var bt=service.Begin(person.Id,"Bar");var dt=service.Begin(person.Id,"Depo");
  var stockBefore=store.Export().Inventory;
  var bc=service.Submit(person.Id,bt,bt.Products.Select(p=>new CountEntry(p.ProductId,p.Bar+1,null)).ToList());
  var persisted=store.Export().Inventory.Counts.Single(c=>c.Id==bc);
  Assert(CountApproval.Area(persisted)=="Bar"&&persisted.Lines.All(l=>l.DepotCounted is null&&l.DepotExpected is null),"SQL Bar sayımında Depo alanları sıfır değil null olarak saklanır");
  service.Review(manager.Id,bc,true,"Bar ayrı sayıldı");
  var afterBar=store.Export().Inventory;
  Assert(bt.Products.All(p=>MobileWorkspace.Balance(afterBar,p.ProductId,"Bar")==p.Bar+1&&MobileWorkspace.Balance(afterBar,p.ProductId,"Depo")==p.Depot),"SQL Bar onayı Depo bakiyesini değiştirmedi");
  var dc=service.Submit(person.Id,dt,dt.Products.Select(p=>new CountEntry(p.ProductId,null,p.Depot+2)).ToList());
  service.Review(manager.Id,dc,true,"Depo ayrı sayıldı");
  var afterDepot=store.Export().Inventory;
  Assert(dt.Products.All(p=>MobileWorkspace.Balance(afterDepot,p.ProductId,"Bar")==MobileWorkspace.Balance(afterBar,p.ProductId,"Bar")&&MobileWorkspace.Balance(afterDepot,p.ProductId,"Depo")==p.Depot+2),"SQL Depo onayı Bar bakiyesini değiştirmedi; diğer alan değişse de bağımsız sayım onaylandı");
  service.SaveStaff(manager.Id,new(null,"Yeni Mobil Personel","Personel","5551234567",new DateTime(2026,10,17)));
  a=store.Export();var added=a.Inventory.Employees.Single(e=>e.Name=="Yeni Mobil Personel");var user=a.Accounts.Single(u=>u.EmployeeId==added.Id);
  Assert(added.Phone=="0555 123 45 67"&&AccountCredentials.Verify(user,"123456"),"Mobil personel ve PBKDF2 hesabı aynı SQL transaction'ında oluşturuldu");
  var before=store.Export();var invalid=store.Export();invalid.Inventory.Employees.Single(e=>e.Id==added.Id).Name="Geri alınmalı";invalid.Accounts.Single(u=>u.EmployeeId==added.Id).Salt="bozuk";
  try{store.SaveStaffAndAccounts(invalid.Inventory,invalid.Accounts);throw new Exception("Geçersiz hesap kaydedildi");}catch(Exception e) when(e is InvalidDataException or FormatException){}
  Assert(store.Export().Inventory.Employees.Single(e=>e.Id==added.Id).Name=="Yeni Mobil Personel","Hatalı hesap bilgisinde personel değişikliği de rollback oldu");
  var stale=store.Export();service.SaveSettings(manager.Id,"Mobil SQL Test");
  try{store.SaveProductsChecked(stale.Products,stale.Inventory.SqlVersion);throw new Exception("Eski ürün sürümü kaydedildi");}catch(InvalidOperationException){}
  Assert(store.Export().Inventory.Settings.BusinessName=="Mobil SQL Test","Eski mobil yönetim görüntüsü güncel SQL kayıtlarını ezemedi");
  var history=store.Export();service.RemoveStaff(manager.Id,person.Id);var removed=store.Export();
  Assert(!removed.Inventory.Employees.Single(e=>e.Id==person.Id).IsActive&&!removed.Accounts.Single(u=>u.EmployeeId==person.Id).Enabled&&removed.Inventory.Counts.Count==history.Inventory.Counts.Count&&removed.Inventory.Movements.Count==history.Inventory.Movements.Count,"Mobil personel kaldırma SQL geçmişini korudu ve hesabı aynı transaction içinde kapattı");

 }
}

