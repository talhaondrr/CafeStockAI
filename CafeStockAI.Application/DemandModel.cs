using CafeStockAI.Application.Models;

namespace CafeStockAI.Application.Analysis;
public sealed record Forecast(decimal SevenDays, decimal Daily, decimal Error, decimal? Anomaly, int Samples)
{
    public IReadOnlyList<DemandPoint> Series { get; init; } = [];
}
public sealed record DemandPoint(DateTime Date, decimal Quantity, bool Predicted);

public static class DemandModel
{
    // Ridge regression learns trend and weekday effects separately for each product/branch.
    private static double[] Features(int day, DateTime date)
    {
        var x = new double[8];
        x[0] = 1;
        x[1] = day / 30d;
        int week = (int)date.DayOfWeek;
        if (week > 0)
            x[week + 1] = 1;
        return x;
    }

    private static double[] Fit(double[] values, DateTime start, int count)
    {
        var a = new double[8, 9];
        for (int n = 0; n < count; n++)
        {
            var x = Features(n, start.AddDays(n));
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    a[i, j] += x[i] * x[j];
                a[i, 8] += x[i] * values[n];
            }
        }

        for (int i = 1; i < 8; i++)
            a[i, i] += .35;
        for (int k = 0; k < 8; k++)
        {
            int pivot = k;
            for (int i = k + 1; i < 8; i++)
                if (Math.Abs(a[i, k]) > Math.Abs(a[pivot, k]))
                    pivot = i;
            for (int j = k; j < 9; j++)
                (a[k, j], a[pivot, j]) = (a[pivot, j], a[k, j]);
            double d = a[k, k];
            if (Math.Abs(d) < 1e-10)
                continue;
            for (int j = k; j < 9; j++)
                a[k, j] /= d;
            for (int i = 0; i < 8; i++)
                if (i != k)
                {
                    double f = a[i, k];
                    for (int j = k; j < 9; j++)
                        a[i, j] -= f * a[k, j];
                }
        }

        return Enumerable.Range(0, 8).Select(i => a[i, 8]).ToArray();
    }

    private static double Predict(double[] weights, int day, DateTime start) => Math.Max(0, weights.Zip(Features(day, start.AddDays(day)), (a, b) => a * b).Sum());
    public static bool IsExample(StockMovement movement) => movement.Note?.StartsWith("[ÖRNEK]", StringComparison.Ordinal) == true;

    public static Forecast? Train(InventoryData data, Guid product, string branch, bool? sampleData = null)
    {
        var today = DateTime.Today;
        var areas = data.Warehouses.Where(w => w.BranchName == branch).Select(w => w.Id).ToHashSet();
        var exits = data.Movements.Where(m => m.ProductId == product && areas.Contains(m.WarehouseId) && m.Type == "Çıkış" && m.Quantity < 0 && m.Date.Date < today && (sampleData is null || IsExample(m) == sampleData)).ToList();
        if (exits.Count == 0)
            return null;
        var start = exits.Min(m => m.Date.Date);
        if (start < today.AddDays(-60))
            start = today.AddDays(-60);
        int count = (today - start).Days;
        if (count < 14)
            return null;
        var y = Enumerable.Range(0, count).Select(i => (double)-exits.Where(m => m.Date.Date == start.AddDays(i)).Sum(m => m.Quantity)).ToArray();
        var validation = Fit(y, start, count - 7);
        double error = Enumerable.Range(count - 7, 7).Average(i => Math.Abs(y[i] - Predict(validation, i, start)));
        var weights = Fit(y, start, count);
        var history = Enumerable.Range(Math.Max(0, count - 30), Math.Min(30, count))
            .Select(i => new DemandPoint(start.AddDays(i), (decimal)y[i], false));
        var future = Enumerable.Range(count, 7)
            .Select(i => new DemandPoint(start.AddDays(i), (decimal)Predict(weights, i, start), true))
            .ToArray();
        decimal forecast = future.Sum(point => point.Quantity);
        var prior = Fit(y, start, count - 1);
        double expected = Predict(prior, count - 1, start);
        decimal? anomaly = expected > .001 ? (decimal)(y[^1] / expected - 1) : null;
        return new Forecast(forecast, forecast / 7, Math.Round((decimal)error, 3), anomaly, count) { Series = [.. history, .. future] };
    }
}
