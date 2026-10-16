namespace CafeStockAI.Desktop;
internal sealed record Forecast(decimal SevenDays, decimal Daily, decimal Error, decimal? Anomaly, int Samples);
internal static class DemandModel
{
 // Ridge regression learns trend and weekday effects separately for each product/branch.
 private static double[] Features(int day,DateTime date) { var x=new double[8];x[0]=1;x[1]=day/30d;int week=(int)date.DayOfWeek; if(week>0)x[week+1]=1;return x; }
 private static double[] Fit(double[] values,DateTime start,int count)
 {
  var a=new double[8,9]; for(int n=0;n<count;n++) {var x=Features(n,start.AddDays(n));for(int i=0;i<8;i++){for(int j=0;j<8;j++)a[i,j]+=x[i]*x[j];a[i,8]+=x[i]*values[n];}}
  for(int i=1;i<8;i++)a[i,i]+=.35;
  for(int k=0;k<8;k++){int pivot=k;for(int i=k+1;i<8;i++)if(Math.Abs(a[i,k])>Math.Abs(a[pivot,k]))pivot=i;for(int j=k;j<9;j++)(a[k,j],a[pivot,j])=(a[pivot,j],a[k,j]);double d=a[k,k];if(Math.Abs(d)<1e-10)continue;for(int j=k;j<9;j++)a[k,j]/=d;for(int i=0;i<8;i++)if(i!=k){double f=a[i,k];for(int j=k;j<9;j++)a[i,j]-=f*a[k,j];}}
  return Enumerable.Range(0,8).Select(i=>a[i,8]).ToArray();
 }
 private static double Predict(double[] weights,int day,DateTime start)=>Math.Max(0,weights.Zip(Features(day,start.AddDays(day)),(a,b)=>a*b).Sum());
 public static Forecast? Train(InventoryData data,Guid product,string branch)
 {
  var areas=data.Warehouses.Where(w=>w.BranchName==branch).Select(w=>w.Id).ToHashSet();var exits=data.Movements.Where(m=>m.ProductId==product&&areas.Contains(m.WarehouseId)&&m.Type=="Çıkış"&&m.Quantity<0&&m.Date.Date<DateTime.Today).ToList();
  if(exits.Count==0)return null; var start=exits.Min(m=>m.Date.Date);if(start<DateTime.Today.AddDays(-60))start=DateTime.Today.AddDays(-60);int count=(DateTime.Today-start).Days;if(count<14)return null;
  var y=Enumerable.Range(0,count).Select(i=>(double)-exits.Where(m=>m.Date.Date==start.AddDays(i)).Sum(m=>m.Quantity)).ToArray();var validation=Fit(y,start,count-7);double error=Enumerable.Range(count-7,7).Average(i=>Math.Abs(y[i]-Predict(validation,i,start)));var weights=Fit(y,start,count);double forecast=Enumerable.Range(count,7).Sum(i=>Predict(weights,i,start));
  var prior=Fit(y,start,count-1);double expected=Predict(prior,count-1,start);decimal? anomaly=expected>.001?(decimal)(y[^1]/expected-1):null;
  return new Forecast(Math.Round((decimal)forecast,2),Math.Round((decimal)forecast/7,3),Math.Round((decimal)error,3),anomaly,count);
 }
}
