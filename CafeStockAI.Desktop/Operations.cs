using System.IO.Compression;
using System.Text.Json;
namespace CafeStockAI.Desktop;
internal static class Operations
{
 public static void Review(Guid id,bool approve,string note)
 {
  if(!Access.Approver) throw new UnauthorizedAccessException("Sayım onaylama yetkiniz yok.");
  using var transaction=DataLock.Enter(); var data=InventoryStore.Load(); var count=data.Counts.Single(c=>c.Id==id); Access.RequireBranch(count.BranchName);
  if(count.Status!="Onay bekliyor") throw new InvalidOperationException("Bu sayım zaten sonuçlandırılmış.");
  if(!approve && string.IsNullOrWhiteSpace(note)) throw new InvalidOperationException("Ret gerekçesi yazınız.");
  if(approve)
  {
   BranchStock.Commit(count.BranchName,count.Lines.Select(l=>new BranchCountLine { ProductId=l.ProductId, ExpectedBar=l.BarExpected??0, ExpectedDepot=l.DepotExpected??0, CountedBar=l.BarCounted??0, CountedDepot=l.DepotCounted??0 }).ToList(),count.Operator,true,id,note.Trim());
   return;
  }
  count.Status=approve?"Onaylandı":"Reddedildi"; count.Reviewer=Access.Current?.Name??"Test yöneticisi"; count.ReviewNote=note.Trim(); count.ReviewedAt=DateTime.Now; InventoryStore.Save(data);
 }
 public static void Transfer(Guid product,string source,string sourceArea,string target,string targetArea,decimal quantity)
 {
  Access.RequireMovement(source); Access.RequireMovement(target);
  if(quantity<=0 || (source==target&&sourceArea==targetArea)) throw new InvalidOperationException("Farklı kaynak/hedef ve pozitif miktar seçiniz.");
  if(sourceArea is not("Bar" or "Depo") || targetArea is not("Bar" or "Depo")) throw new InvalidOperationException("Bar/Depo alanını seçiniz.");
  using var transaction=DataLock.Enter(); var data=InventoryStore.Load();
  if(!BranchStock.AllNames(data).Contains(source)||!BranchStock.AllNames(data).Contains(target)||!ProductStore.Load().Any(p=>p.Id==product&&!string.IsNullOrWhiteSpace(p.Unit))) throw new InvalidOperationException("Geçerli ürün ve şubeler gereklidir.");
  if(BranchStock.Balance(data,product,source,sourceArea)<quantity) throw new InvalidOperationException("Kaynakta yeterli stok yok.");
  string note=$"Transfer {Guid.NewGuid():N} • {source}/{sourceArea} → {target}/{targetArea} • {Access.Current?.Name}";
  decimal remaining=quantity;
  foreach(var w in data.Warehouses.Where(w=>w.BranchName==source&&BranchStock.AreaOf(w)==sourceArea).ToList())
  { decimal take=Math.Min(remaining,Math.Max(0,data.Balance(product,w.Id))); if(take>0) data.Movements.Add(new StockMovement { ProductId=product,WarehouseId=w.Id,Type="Transfer çıkışı",Quantity=-take,Note=note }); remaining-=take; if(remaining==0) break; }
  if(remaining!=0) throw new InvalidOperationException("Kaynak stok tutarsız; transfer kaydedilmedi.");
  data.Movements.Add(new StockMovement {ProductId=product,WarehouseId=BranchStock.Area(data,target,targetArea).Id,Type="Transfer girişi",Quantity=quantity,Note=note}); InventoryStore.Save(data);
 }
 public static string Backup()
 {
  Access.RequireGlobal(); return CreateBackup();
 }
 internal static string CreateBackup()
 {
  using var transaction=DataLock.Enter(); string folder=Path.Combine(InventoryStore.DirectoryPath,"Yedekler"); Directory.CreateDirectory(folder); string file=Path.Combine(folder,$"CafeStock-{DateTime.Now:yyyyMMdd-HHmmss}-{Guid.NewGuid().ToString("N")[..4]}.zip");
  using(var zip=ZipFile.Open(file,ZipArchiveMode.Create)) foreach(string name in new[]{"inventory.json","products.json","accounts.json"}) if(File.Exists(Path.Combine(InventoryStore.DirectoryPath,name))) zip.CreateEntryFromFile(Path.Combine(InventoryStore.DirectoryPath,name),name);
  return file;
 }

 public static void Restore(string file)
 {
  Access.RequireGlobal(); using var transaction=DataLock.Enter(); using var archive=ZipFile.OpenRead(file);
  string[] names={"inventory.json","products.json","accounts.json"};var content=new Dictionary<string,string>();
  foreach(string name in names) { var entries=archive.Entries.Where(e=>e.FullName==name).ToList();if(entries.Count!=1||entries[0].Length>50000000)throw new InvalidDataException("Yedek dosyası eksik veya geçersiz.");using var reader=new StreamReader(entries[0].Open());content[name]=reader.ReadToEnd(); }
  var data=JsonSerializer.Deserialize<InventoryData>(content[names[0]])??throw new InvalidDataException("Stok verisi geçersiz.");
  var products=JsonSerializer.Deserialize<List<Product>>(content[names[1]])??throw new InvalidDataException("Ürün verisi geçersiz.");
  var users=JsonSerializer.Deserialize<List<UserAccount>>(content[names[2]])??throw new InvalidDataException("Hesap verisi geçersiz.");
  if(data.Branches is null||data.Employees is null||data.Counts is null||data.Warehouses is null||data.Movements is null||data.Settings is null)throw new InvalidDataException("Yedek alanları eksik.");
  bool Unique(IEnumerable<Guid> ids)=>ids.Count()==ids.Distinct().Count();
  var pids=products.Select(p=>p.Id).ToHashSet();var wids=data.Warehouses.Select(w=>w.Id).ToHashSet();
  if(!Unique(pids)||!Unique(products.Select(p=>p.Id))||!Unique(data.Warehouses.Select(w=>w.Id))||!Unique(data.Employees.Select(e=>e.Id))||!Unique(data.Counts.Select(c=>c.Id))||!Unique(users.Select(u=>u.EmployeeId))||users.Where(u=>u.Enabled).GroupBy(u=>Accounts.Key(u.Username)).Any(g=>g.Count()>1)||data.Movements.Any(m=>!pids.Contains(m.ProductId)||!wids.Contains(m.WarehouseId))||data.Counts.Any(c=>c.Lines is null||c.Lines.Any(l=>!pids.Contains(l.ProductId)||l.Counted<0))||users.Any(u=>Convert.FromBase64String(u.Salt).Length!=16||Convert.FromBase64String(u.Hash).Length!=32)||!users.Any(u=>u.Enabled&&data.Employees.Any(e=>e.Id==u.EmployeeId&&e.Role is "İşletme Sahibi" or "Genel Müdür")))throw new InvalidDataException("Yedek kayıtları tutarsız veya yönetici hesabı eksik.");
  string before=Backup();var originals=names.ToDictionary(n=>n,n=>File.Exists(Path.Combine(InventoryStore.DirectoryPath,n))?File.ReadAllText(Path.Combine(InventoryStore.DirectoryPath,n)):null);
  try { foreach(string name in names)File.WriteAllText(Path.Combine(InventoryStore.DirectoryPath,name+".restore"),content[name]);foreach(string name in names)File.Move(Path.Combine(InventoryStore.DirectoryPath,name+".restore"),Path.Combine(InventoryStore.DirectoryPath,name),true); }
  catch { foreach(var pair in originals){string path=Path.Combine(InventoryStore.DirectoryPath,pair.Key);if(pair.Value is null){if(File.Exists(path))File.Delete(path);}else File.WriteAllText(path,pair.Value);}throw new IOException("Geri yükleme başarısız; önceki kayıtlar korundu. Yedek: "+before); }
  Access.EmployeeId=null;
 }
}
