using CafeStockAI.Application.Models;
namespace CafeStockAI.Application;
public static class CountApproval
{
 public static string Area(StockCount count)
 {
  if(count.Lines.Count>0&&count.Lines.All(l=>l.BarExpected.HasValue&&l.BarCounted.HasValue&&l.DepotExpected is null&&l.DepotCounted is null))return "Bar";
  if(count.Lines.Count>0&&count.Lines.All(l=>l.DepotExpected.HasValue&&l.DepotCounted.HasValue&&l.BarExpected is null&&l.BarCounted is null))return "Depo";
  return "Bar ve Depo";
 }
 public static void Apply(InventoryData data,List<Product> products,Guid? actor,string reviewer,Guid countId,bool approved,string note)
 {
  var count=data.Counts.SingleOrDefault(c=>c.Id==countId)??throw new InvalidOperationException("Sayım bulunamadı.");
  if(count.Status!="Onay bekliyor")throw new InvalidOperationException("Sayım zaten sonuçlandırılmış.");
  if(note.Length>2000||(!approved&&string.IsNullOrWhiteSpace(note)))throw new InvalidOperationException("Ret gerekçesi girin; not en fazla 2000 karakter olabilir.");
  string selected=Area(count);var areas=selected=="Bar ve Depo"?new[]{"Bar","Depo"}:new[]{selected};
  if(approved)
  {
   if(products.Count==0||!products.Select(p=>p.Id).Order().SequenceEqual(count.Lines.Select(l=>l.ProductId).Order()))throw new InvalidOperationException("Ürün listesi değişti. Yeni sayım gerekiyor.");
   foreach(var line in count.Lines)foreach(string area in areas)
   {
    decimal? expected=area=="Bar"?line.BarExpected:line.DepotExpected,counted=area=="Bar"?line.BarCounted:line.DepotCounted;
    if(expected is null||counted is null||counted<0)throw new InvalidOperationException("Sayım alanları eksik veya geçersiz.");
    var locations=data.Warehouses.Where(w=>w.BranchName==count.BranchName&&w.StockArea==area).ToList();
    decimal current=locations.Sum(w=>data.Balance(line.ProductId,w.Id));
    if(current!=expected)throw new InvalidOperationException(area+" stoğu değişti; bu sayım onaylanamaz.");
    decimal delta=counted.Value-expected.Value;
    if(delta>0)data.Movements.Add(new StockMovement{ActorId=actor,CountId=count.Id,ProductId=line.ProductId,WarehouseId=locations.First().Id,Quantity=delta,Type="Sayım düzeltmesi",Note=$"{area} sayımı • {reviewer}"});
    if(delta<0)
    {
     decimal remaining=-delta;
     foreach(var source in locations){decimal take=Math.Min(remaining,Math.Max(0,data.Balance(line.ProductId,source.Id)));if(take>0){data.Movements.Add(new StockMovement{ActorId=actor,CountId=count.Id,ProductId=line.ProductId,WarehouseId=source.Id,Quantity=-take,Type="Sayım düzeltmesi",Note=$"{area} sayımı • {reviewer}"});remaining-=take;}}
     if(remaining!=0)throw new InvalidOperationException("Stok alanları tutarsız; onay kaydedilmedi.");
    }
   }
  }
  count.Status=approved?"Onaylandı":"Reddedildi";count.Reviewer=reviewer;count.ReviewedAt=DateTime.Now;count.ReviewNote=note.Trim();
 }
}
