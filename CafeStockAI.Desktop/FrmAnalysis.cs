namespace CafeStockAI.Desktop;
public partial class FrmAnalysis : Form,IInventoryPage
{
 public FrmAnalysis() {InitializeComponent(); QuantityText.Setup(this);colÜrün.FillWeight=130;colÖneri.FillWeight=280;}
 public void RefreshData()=>PageUi.Run(()=>{string previous=warehouseInput.SelectedItem as string??"Tüm şubeler";var names=BranchStock.AllNames(Access.View());names.Insert(0,"Tüm şubeler");warehouseInput.DataSource=names;warehouseInput.SelectedItem=names.Contains(previous)?previous:"Tüm şubeler";Render();});
 private void Render()
 {
  var data=Access.View();var products=ProductStore.Load();string selected=warehouseInput.SelectedItem as string??"Tüm şubeler";
  var rows=(from branch in BranchStock.AllNames(data) where selected=="Tüm şubeler"||branch==selected from p in products
   let balance=BranchStock.Balance(data,p.Id,branch,"Bar")+BranchStock.Balance(data,p.Id,branch,"Depo")
   let forecast=DemandModel.Train(data,p.Id,branch)
   let days=forecast?.Daily>0?balance/forecast.Daily:(decimal?)null
   select new {Ürün=p.Name,Depo=branch,Stok=balance,Birim=p.UnitDisplay,Sınır=p.MinimumStock,Gün=days?.ToString("0.#",System.Globalization.CultureInfo.InvariantCulture)??"En az 14 gün veri gerekli",Tahmin=forecast?.SevenDays.ToString("0.##",System.Globalization.CultureInfo.InvariantCulture)??"—",Hata=forecast?.Error.ToString("0.###",System.Globalization.CultureInfo.InvariantCulture)??"—",
    Öneri=(balance<=p.MinimumStock?"Kritik stok. ":"")+(forecast is null?"Tahmin için tüketim verisi biriktirin.":$"7 gün için {QuantityText.Format(forecast.SevenDays)} {p.Unit}; önerilen tedarik {QuantityText.Format(Math.Max(0,forecast.SevenDays+p.MinimumStock-balance))} {p.Unit}."+(forecast.Anomaly>0.5m?$" Son gün tüketim beklenenden %{forecast.Anomaly*100:N0} fazla; sayımı kontrol edin.":""))}).OrderBy(r=>r.Stok>r.Sınır).ThenBy(r=>r.Depo).ToList();
  PageUi.Bind(grid,rows);statusLabel.Text=$"{rows.Count} ürün/şube • Trend ve hafta günü modeli yerel olarak eğitilir. Test hatası: eğitimden ayrılan son 7 günün ortalama mutlak hatası. Örnek hareketler [ÖRNEK] etiketlidir.";
 }
 private void Refresh_Click(object? sender,EventArgs e)=>PageUi.Run(Render);
 private void Export_Click(object? sender,EventArgs e)=>PageUi.Run(()=>PageUi.ExportCsv(grid,this));
}
