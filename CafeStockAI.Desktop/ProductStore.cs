using System.Text.Json;
namespace CafeStockAI.Desktop;
internal static class ProductStore
{
    private static string FilePath => Path.Combine(InventoryStore.DirectoryPath, "products.json");
    private static string ImportMarker => Path.Combine(InventoryStore.DirectoryPath, "mackbear-catalog-v1.imported");
    public static List<Product> ReadCatalog()
    {
        using var stream = typeof(ProductStore).Assembly.GetManifestResourceStream("CafeStockAI.Desktop.ProductCatalog.json")
            ?? throw new InvalidDataException("Ürün kataloğu bulunamadı.");
        return JsonSerializer.Deserialize<List<Product>>(stream) ?? throw new InvalidDataException("Ürün kataloğu okunamadı.");
    }
    internal static List<Product> MergeCatalog(List<Product> products)
    {
        var result = products.ToList();
        foreach (var product in ReadCatalog())
        {
            if (result.Any(p => p.Id == product.Id || (string.Equals(p.Name.Trim(),product.Name,StringComparison.OrdinalIgnoreCase) && string.Equals(p.Category.Trim(),product.Category,StringComparison.OrdinalIgnoreCase)))) continue;
            if (result.Any(p => string.Equals(p.Code,product.Code,StringComparison.OrdinalIgnoreCase)))
            {
                int suffix = 1; string prefix = product.Code;
                while (result.Any(p => string.Equals(p.Code, product.Code, StringComparison.OrdinalIgnoreCase))) product.Code = prefix + "-" + suffix++;
            }
            result.Add(product);
        }
        return result;
    }
    public static List<Product> Load()
    {
        if (SqlMode.Store is { } sql) return sql.LoadProducts();
        var products = File.Exists(FilePath) ? JsonSerializer.Deserialize<List<Product>>(File.ReadAllText(FilePath)) ?? throw new InvalidDataException("Ürün kayıt dosyası geçersiz.") : new List<Product>();
        if (InventoryStore.TestDirectory is null && !File.Exists(ImportMarker))
        {
            if (File.Exists(FilePath)) File.Copy(FilePath, FilePath + ".before-mackbear-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".bak", false);
            products = MergeCatalog(products); Save(products); File.WriteAllText(ImportMarker, DateTimeOffset.Now.ToString("O"));
        }
        return products;
    }
    internal static void ApplyColdBrewCorrection()
    {
        if (InventoryStore.TestDirectory is not null) return;
        string marker=Path.Combine(InventoryStore.DirectoryPath,"cold-brew-bottle-v1.corrected");
        if(File.Exists(marker))return;
        var products=Load();
        var original=products.FirstOrDefault(p=>p.Id==Guid.Parse("16bba727-da1d-5b5e-b5fe-2a0834db17cf"));
        if(original is null)return;
        var duplicateId=Guid.Parse("9c552c48-ec8f-47e3-ba8e-994e5b2f3a8d");
        var data=InventoryStore.Load();
        if(data.Movements.Any(m=>m.ProductId==duplicateId)||data.Counts.Any(c=>c.Lines.Any(l=>l.ProductId==duplicateId)))
            throw new InvalidOperationException("Yeni Cold Brew kaydına işlem eklenmiş. Kayıtlar korunması için otomatik düzeltme durduruldu.");
        Operations.CreateBackup();
        original.Unit="Şişe";
        Save(products.Where(p=>p.Id!=duplicateId).ToList());
        File.WriteAllText(marker,DateTimeOffset.UtcNow.ToString("O"));
    }
    public static void Save(List<Product> products)
    {
        if (SqlMode.Store is { } sql) { sql.SaveProducts(products); return; }
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        string temporary = FilePath + ".tmp";
        File.WriteAllText(temporary,JsonSerializer.Serialize(products,new JsonSerializerOptions { WriteIndented = true }));
        File.Move(temporary,FilePath,true);
    }
}
