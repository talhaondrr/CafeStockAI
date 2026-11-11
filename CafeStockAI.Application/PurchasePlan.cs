namespace CafeStockAI.Application.Analysis;

public sealed record PurchasePlan(int Rank, string Priority, string Reason, decimal? Demand, decimal Reorder, string Remaining)
{
    public const int LeadDays = 2;

    public static PurchasePlan Create(string unit, decimal stock, decimal minimum, Forecast? forecast)
    {
        decimal leadDemand = forecast?.Series.Where(p => p.Predicted).Take(LeadDays).Sum(p => p.Quantity) ?? 0;
        bool deliveryRisk = forecast is not null && leadDemand > 0 && stock <= leadDemand;
        decimal reorder = StockPlanning.Reorder(unit, forecast?.SevenDays ?? 0, minimum, stock);
        int rank = (stock <= 0 && reorder > 0) || stock < minimum || deliveryRisk ? 0
            : reorder > 0 || forecast is null ? 1 : 2;
        string priority = rank switch { 0 => "Bugün sipariş ver", 1 => "Takip et", _ => "Stok yeterli" };
        string reason = stock <= 0 ? "Stok tükendi."
            : stock < minimum ? "Stok kritik sınırın altında."
            : deliveryRisk ? "Stok teslimata kadar tükenebilir."
            : forecast is null ? "Tahmin için veri yetersiz."
            : reorder > 0 ? "Hafta sonunda güvenlik stoğu azalabilir."
            : "Stok 7 günlük ihtiyacı ve güvenlik payını karşılıyor.";
        decimal? days = forecast?.Daily > 0 ? Math.Max(0, stock) / forecast.Daily : null;
        string remaining = stock <= 0 ? "Tükendi" : forecast is null ? "Veri yetersiz"
            : days is null || days > 30 ? "30 günden fazla" : FormattableString.Invariant($"{days:0.#} gün");
        return new PurchasePlan(rank, priority, reason, forecast is null ? null : StockPlanning.Demand(unit, forecast.SevenDays), reorder, remaining);
    }

    public string Explain(string unit, decimal stock, decimal minimum, Forecast? forecast)
    {
        string demand = forecast is null ? "Tahmin verisi yetersiz. Öneri kritik sınıra göre hesaplandı."
            : FormattableString.Invariant($"7 günlük model tahmini {forecast.SevenDays:0.####} {unit}.");
        return FormattableString.Invariant($"{demand} Sipariş hesabı: tahmin + güvenlik stoğu ({minimum:0.###} {unit}) − mevcut stok ({stock:0.###} {unit}). Önerilen sipariş {Reorder:0.####} {unit}.")
            + (StockPlanning.UsesWholeUnits(unit) ? " Pozitif sipariş miktarı tam birime yukarı yuvarlanır." : "");
    }
}
