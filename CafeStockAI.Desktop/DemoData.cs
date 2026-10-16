namespace CafeStockAI.Desktop;
internal static class DemoData
{
 public static void Initialize()
 {
  using var transaction=DataLock.Enter();string marker=Path.Combine(InventoryStore.DirectoryPath,"demo-v3.initialized");if(File.Exists(marker))return;
  Directory.CreateDirectory(InventoryStore.DirectoryPath);string backup=Path.Combine(InventoryStore.DirectoryPath,"KurulumOncesi-"+DateTime.Now.ToString("yyyyMMdd-HHmmss"));Directory.CreateDirectory(backup);foreach(var name in new[]{"inventory.json","products.json","accounts.json"})if(File.Exists(Path.Combine(InventoryStore.DirectoryPath,name)))File.Copy(Path.Combine(InventoryStore.DirectoryPath,name),Path.Combine(backup,name));
  var data=InventoryStore.Load();var products=ProductStore.Load();if(products.Count==0)products=ProductStore.ReadCatalog();
  foreach(var p in products){if(string.IsNullOrWhiteSpace(p.Unit))p.Unit=p.Name.Contains("ÇANTA")?"Adet":"Paket";if(p.MinimumStock==0)p.MinimumStock=p.Unit=="Kg"?2:5;}
  ProductStore.Save(products);
  if(data.Employees.Count==0)
  {
   var staff=new[]{("Aslıhan Erden","Şube Müdürü","Elazığ"),("Arda Görgün","Süpervizör","*"),("Ercan Erden","Genel Müdür","*"),("Canbora Baloğlu","Personel","Elazığ"),("Ayşegül Önder","Personel","İnegöl"),("Lütfü Önder","Depo Sorumlusu","İnegöl"),("Talha Önder","İşletme Sahibi","*"),("İrem Gözükara","İşletme Sahibi","*")};
   foreach(var (name,role,branch) in staff)data.Employees.Add(new Employee{Name=name,Role=role,Branches=branch=="*"?BranchStock.AllNames(data):new(){branch}});
  }
  int index=0;var random=new Random(374);
  foreach(string branch in BranchStock.AllNames(data))
  {
   foreach(var p in products)
   {
    // Preserve every existing stock history; only seed products without branch movements.
    var areas=data.Warehouses.Where(w=>w.BranchName==branch).Select(w=>w.Id).ToHashSet();if(data.Movements.Any(m=>m.ProductId==p.Id&&areas.Contains(m.WarehouseId)))continue;
    var bar=BranchStock.Area(data,branch,"Bar");var depot=BranchStock.Area(data,branch,"Depo");decimal scale=p.Unit=="Kg"?.15m:p.Unit=="Şişe"?.7m:1m;
    var daily=Enumerable.Range(0,35).Select(day=>Math.Round(scale*(1+day/70m)*(decimal)(.7+random.NextDouble()*.6)*(day%7 is 5 or 6?1.35m:1),3)).ToArray();if(index%17==0)daily[^1]*=3;
    decimal residual=index%9==0?1:12;decimal total=daily.Sum();
    data.Movements.Add(new StockMovement{ProductId=p.Id,WarehouseId=bar.Id,Date=DateTime.Today.AddDays(-36),Type="Giriş",Quantity=total+residual,Note="[ÖRNEK] Açılış stoğu"});
    data.Movements.Add(new StockMovement{ProductId=p.Id,WarehouseId=depot.Id,Date=DateTime.Today.AddDays(-36),Type="Giriş",Quantity=index%9==0?0:20,Note="[ÖRNEK] Depo açılış stoğu"});
    for(int day=0;day<35;day++)data.Movements.Add(new StockMovement{ProductId=p.Id,WarehouseId=bar.Id,Date=DateTime.Today.AddDays(-35+day).AddHours(20),Type="Çıkış",Quantity=-daily[day],Note="[ÖRNEK] Model denemesi için sentetik tüketim"});index++;
   }
  }
  foreach(var branch in BranchStock.AllNames(data)) if(!data.Counts.Any(c=>c.BranchName==branch)) data.Counts.Add(new StockCount{BranchName=branch,WarehouseId=BranchStock.Area(data,branch,"Depo").Id,Status="Onay bekliyor",Operator="[ÖRNEK] Deneme sayımı",Lines=products.Select((p,i)=>{decimal bar=BranchStock.Balance(data,p.Id,branch,"Bar"),depot=BranchStock.Balance(data,p.Id,branch,"Depo");return new CountLine{ProductId=p.Id,Expected=bar+depot,Counted=bar+depot+(i%11==0?1:0),BarExpected=bar,BarCounted=bar+(i%11==0?1:0),DepotExpected=depot,DepotCounted=depot};}).ToList()});
  InventoryStore.Save(data);File.WriteAllText(marker,DateTime.Now.ToString("O"));
 }
}
