using System.Security.Cryptography;
using CafeStockAI.Application;
using CafeStockAI.Application.Models;
using CafeStockAI.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using D=CafeStockAI.Domain;
internal static class PersistenceChecks
{
 public static void Run(string folder)
 {
  Directory.CreateDirectory(folder);using var connection=new SqliteConnection("Data Source=:memory:");connection.Open();
  TestContext Factory()=>new(new DbContextOptionsBuilder<CafeStockDbContext>().UseSqlite(connection).Options);
  using(var setup=Factory())setup.Database.EnsureCreated();var store=new SqlInventoryPersistence(Factory);var checks=new List<string>();
  void Assert(bool value,string text){if(!value)throw new Exception(text);checks.Add(text);}
  foreach(string unit in new[]{"Şişe","ŞİŞE","Koli","Paket","Fişek","Adet"})Assert(CafeStockAI.Application.Analysis.StockPlanning.Demand(unit,6.81m)==7&&CafeStockAI.Application.Analysis.StockPlanning.Demand(unit,6m)==6,"Tam birimde tahmin yukarı yuvarlanır: "+unit);
  Assert(CafeStockAI.Application.Analysis.StockPlanning.Demand("Kg",6.81m)==6.81m,"Kg tahmini kesirli korunur");Assert(CafeStockAI.Application.Analysis.StockPlanning.Reorder("Şişe",6.81m,5,6)==6&&CafeStockAI.Application.Analysis.StockPlanning.Reorder("Şişe",6.81m,5,50)==0,"Tedarik ihtiyacı gerçek miktarlardan hesaplanıp bir kez yuvarlanır");
  var elazig=new Branch{Name="Elazığ"};var other=new Branch{Name="İnegöl"};var bar=new Warehouse{BranchName="Elazığ",StockArea="Bar"};var depot=new Warehouse{BranchName="Elazığ",StockArea="Depo"};var legacyBar=new Warehouse{BranchName="Elazığ",StockArea="Bar"};var remote=new Warehouse{BranchName="İnegöl",StockArea="Bar"};
  var person=new Employee{Name="Test Personeli",Role="Personel",Phone="0 (532) 123 45 67",HireDate=new DateTime(2024,3,4),Branches=new(){"İnegöl"}};var manager=new Employee{Name="Test Müdürü",Role="Genel Müdür",Branches=new(){"Elazığ","İnegöl"}};var product=new Product{Name="Kahve",Code="K-1",Category="Kahve",Unit="Kg",MinimumStock=2};
  var data=new InventoryData{BranchRegistryInitialized=true,Branches=new(){elazig,other},Warehouses=new(){bar,depot,legacyBar,remote},Employees=new(){person,manager},Settings=new(){BusinessName="Test Kafe"},Movements=new(){new(){ProductId=product.Id,WarehouseId=bar.Id,Quantity=12,Type="Giriş"},new(){ProductId=product.Id,WarehouseId=legacyBar.Id,Quantity=3,Type="Giriş"},new(){ProductId=product.Id,WarehouseId=remote.Id,Quantity=200,Type="Giriş"}}};
  var salt=Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));var hash=Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));var accounts=new List<UserAccount>{new(){EmployeeId=person.Id,Username=person.Name,Salt=salt,Hash=hash},new(){EmployeeId=manager.Id,Username=manager.Name,Salt=salt,Hash=hash}};
  var source=new InventoryArchive(data,new(){product},accounts);var prepared=SingleBranchImport.Prepare(source);Assert(prepared.Inventory.Branches.Single().Name=="Elazığ"&&prepared.Inventory.Employees.Count==2&&prepared.Inventory.Employees.All(e=>e.AssignedBranches.SequenceEqual(new[]{"Elazığ"})),"Tek Elazığ; bütün personeller ve görevleri korundu");Assert(source.Inventory.Branches.Count==2&&source.Inventory.Employees[0].Branches[0]=="İnegöl","Kaynak kayıtlar değişmedi");Assert(prepared.Inventory.Balance(product.Id,bar.Id)==15&&prepared.Inventory.Movements.Count==2,"Eski Bar alanları toplandı; diğer şube stoğu Elazığ'a eklenmedi");
  store.Import(prepared,"test-source");SqlInventoryPersistence.Verify(prepared,store.Export());checks.Add("İlişkisel veritabanına aktarım, bakiye ve hesap hash/salt karşılaştırması geçti");store.Import(prepared,"test-source");Assert(store.LoadInventory().Movements.Count==2,"Tekrarlı aktarım kopya hareket oluşturmadı");
  var current=store.LoadInventory();var stale=store.LoadInventory();current.Employees[0].Phone="0 (532) 111 22 33";store.SaveInventory(current);try{store.SaveInventory(stale);throw new Exception("Eski sürüm kaydedildi");}catch(InvalidOperationException){checks.Add("Eski verinin yeni SQL kayıtlarını ezmesi engellendi");}
  var loaded=store.LoadProducts();loaded[0].Name="Espresso";store.SaveProducts(loaded);Assert(store.LoadProducts()[0].Name=="Espresso","Ürün güncellemesi SQL üzerinden okundu");
  current=store.LoadInventory();current.Counts.Add(new(){BranchName="Elazığ",WarehouseId=depot.Id,Operator=person.Name,Status="Onay bekliyor",Lines=new(){new(){ProductId=product.Id,Expected=15,Counted=16,BarExpected=15,BarCounted=16,DepotExpected=0,DepotCounted=0}}});store.SaveInventory(current);current=store.LoadInventory();Assert(current.Counts.Single().Lines.Single().BarCounted==16&&current.Balance(product.Id,bar.Id)==15,"Sayım satırları SQL'de; bekleyen sayım stoğu değiştirmedi");current.Counts[0].Status="Onaylandı";current.Counts[0].Reviewer=manager.Name;current.Counts[0].ReviewedAt=DateTime.Now;current.Movements.Add(new(){ProductId=product.Id,WarehouseId=bar.Id,Type="Sayım düzeltmesi",Quantity=1});store.SaveInventory(current);Assert(store.LoadInventory().Balance(product.Id,bar.Id)==16,"Onay ve fark hareketi birlikte kaydedildi");
  current=store.LoadInventory();int before=current.Movements.Count;current.Movements.Add(new(){ProductId=Guid.NewGuid(),WarehouseId=bar.Id,Quantity=1,Type="Giriş"});try{store.SaveInventory(current);throw new Exception("FK ihlali kabul edildi");}catch(DbUpdateException){Assert(store.LoadInventory().Movements.Count==before,"FK hatasında transaction geri alındı");}
  var backup=store.Export();current=store.LoadInventory();current.Employees.RemoveAll(e=>e.Id==person.Id);store.SaveInventory(current);accounts=store.LoadAccounts();accounts.First(a=>a.EmployeeId==person.Id).Enabled=false;store.SaveAccounts(accounts);var afterDeletion=store.Export();Assert(afterDeletion.Inventory.Employees.Single(e=>e.Id==person.Id).IsActive==false,"Personel geçmişi pasifleştirilerek korundu");store.Restore(backup);SqlInventoryPersistence.Verify(backup,store.Export());checks.Add("SQL yedeğinden bütün tablolar transaction içinde geri yüklendi");
  var original=store.Export();try{store.Restore(source);throw new Exception("Çok şubeli yedek kabul edildi");}catch(InvalidOperationException){SqlInventoryPersistence.Verify(original,store.Export());checks.Add("Uyumsuz yedek reddedildi; silme işlemleri rollback ile geri alındı");}
  File.WriteAllLines(Path.Combine(folder,"result.txt"),checks.Append("GEÇTİ — SQLite ilişkisel test sağlayıcısı kullanıldı. SQL Server canlı testi ayrıca gereklidir."));Console.WriteLine(string.Join(Environment.NewLine,checks));
 }
 private sealed class TestContext(DbContextOptions<CafeStockDbContext> options):CafeStockDbContext(options)
 {
  protected override void OnModelCreating(ModelBuilder model){base.OnModelCreating(model);foreach(var type in model.Model.GetEntityTypes())if(type.FindProperty("Version") is { } p)p.ValueGenerated=ValueGenerated.Never;}
  public override int SaveChanges(){foreach(var entry in ChangeTracker.Entries().Where(e=>e.State is EntityState.Added or EntityState.Modified))if(entry.Metadata.FindProperty("Version") is not null)entry.Property("Version").CurrentValue=RandomNumberGenerator.GetBytes(8);return base.SaveChanges();}
 }
}

