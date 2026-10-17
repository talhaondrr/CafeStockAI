using System.Text.Json;
using CafeStockAI.Application;
using CafeStockAI.Application.Models;
namespace CafeStockAI.Mobile;
/// <summary>Explicit opt-in demo; never reads real personnel or stock files.</summary>
public sealed class DemoPersistence : IInventoryPersistence,IWorkspacePersistence
{
 private readonly object sync=new();private InventoryArchive state;private long revision=1;
 public DemoPersistence()
 {
  using var stream=typeof(DemoPersistence).Assembly.GetManifestResourceStream("Mobile.ProductCatalog.json")!;
  var products=JsonSerializer.Deserialize<List<Product>>(stream)??[];
  foreach(var product in products)if(string.IsNullOrWhiteSpace(product.Unit))product.Unit="Adet";
  var data=new InventoryData{BranchRegistryInitialized=true,Branches=[new Branch{Name="Elazığ"}],Settings=new(){BusinessName="CafeStock AI Demo"}};
  foreach(var area in new[]{"Bar","Depo"})data.Warehouses.Add(new Warehouse{BranchName="Elazığ",StockArea=area,Name="Elazığ / "+area});
  var users=new List<UserAccount>();
  foreach(var (name,role) in new[]{("Demo Personel","Personel"),("Demo Müdür","Şube Müdürü"),("Demo Sahip","İşletme Sahibi")})
  {
   var staff=new Employee{Name=name,Role=role,Branches=["Elazığ"],BranchName="Elazığ"};data.Employees.Add(staff);
   var user=new UserAccount{EmployeeId=staff.Id,Username=name};AccountCredentials.Set(user,"123456");users.Add(user);
  }
  foreach(var product in products)foreach(var area in data.Warehouses)data.Movements.Add(new StockMovement{ProductId=product.Id,WarehouseId=area.Id,Quantity=area.StockArea=="Bar"?4:12,Type="Giriş",Note="[ÖRNEK] Mobil demo"});
  // Local AI has 35 complete days of synthetic consumption, never mixed with production.
  foreach(var product in products.Take(8))for(int day=35;day>0;day--){var area=data.Warehouses[1];data.Movements.Add(new StockMovement{ProductId=product.Id,WarehouseId=area.Id,Quantity=2,Type="Giriş",Date=DateTime.Today.AddDays(-day),Note="[ÖRNEK]"});data.Movements.Add(new StockMovement{ProductId=product.Id,WarehouseId=area.Id,Quantity=-1,Type="Çıkış",Date=DateTime.Today.AddDays(-day),Note="[ÖRNEK]"});}
  state=new(data,products,users);
 }
 private static T Clone<T>(T value)=>JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(value))!;
 private void Check(byte[] version){if(!version.SequenceEqual(BitConverter.GetBytes(revision)))throw new InvalidOperationException("Kayıtlar başka bir işlemde değişti. Yenileyip tekrar deneyin.");}
 public InventoryArchive Export(){lock(sync){var a=Clone(state);a.Inventory.SqlVersion=BitConverter.GetBytes(revision);return a;}}
 public InventoryData LoadInventory()=>Export().Inventory;public List<Product> LoadProducts()=>Export().Products;public List<UserAccount> LoadAccounts()=>Export().Accounts;
 public void SaveInventory(InventoryData data){lock(sync){Check(data.SqlVersion);state=state with{Inventory=Clone(data)};revision++;}}
 public void SaveProducts(List<Product> products){lock(sync){state=state with{Products=Clone(products)};revision++;}}
 public void SaveAccounts(List<UserAccount> users){lock(sync){state=state with{Accounts=Clone(users)};revision++;}}
 public void Restore(InventoryArchive a){lock(sync){state=Clone(a);revision++;}}
 public void SaveStaffAndAccounts(InventoryData data,List<UserAccount> users){lock(sync){Check(data.SqlVersion);state=state with{Inventory=Clone(data),Accounts=Clone(users)};revision++;}}
 public void SaveProductsChecked(List<Product> products,byte[] version){lock(sync){Check(version);SaveProducts(products);}}
 public void SaveAccountsChecked(List<UserAccount> users,byte[] version){lock(sync){Check(version);SaveAccounts(users);}}
}

