using System.Globalization;

namespace CafeStockAI.Application.Analysis;
public static class StockPlanning
{
    public static bool UsesWholeUnits(string unit) => unit.Trim().ToLower(CultureInfo.GetCultureInfo("tr-TR"))is "adet" or "şişe" or "koli" or "paket" or "fişek";
    public static decimal Demand(string unit, decimal quantity) => UsesWholeUnits(unit) ? decimal.Ceiling(Math.Max(0, quantity)) : Math.Max(0, quantity);
    public static decimal Reorder(string unit, decimal forecast, decimal minimum, decimal stock) => Demand(unit, Math.Max(0, forecast + minimum - stock));
}
