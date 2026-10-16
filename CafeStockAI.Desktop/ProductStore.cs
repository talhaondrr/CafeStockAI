using System.Text.Json;
namespace CafeStockAI.Desktop;
public sealed class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Unit { get; set; } = "Adet";
    public decimal MinimumStock { get; set; }
    public string SourceCell { get; set; } = "";
    [System.Text.Json.Serialization.JsonIgnore]
    public string DisplayName => string.IsNullOrEmpty(Category) ? Name : $"{Category} / {Name}";
    [System.Text.Json.Serialization.JsonIgnore]
    public string UnitDisplay => string.IsNullOrWhiteSpace(Unit) ? "Birim seçiniz" : Unit;
}
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
        var products = File.Exists(FilePath) ? JsonSerializer.Deserialize<List<Product>>(File.ReadAllText(FilePath)) ?? throw new InvalidDataException("Ürün kayıt dosyası geçersiz.") : new List<Product>();
        if (InventoryStore.TestDirectory is null && !File.Exists(ImportMarker))
        {
            if (File.Exists(FilePath)) File.Copy(FilePath, FilePath + ".before-mackbear-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".bak", false);
            products = MergeCatalog(products); Save(products); File.WriteAllText(ImportMarker, DateTimeOffset.Now.ToString("O"));
        }
        return products;
    }
    public static void Save(List<Product> products)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        string temporary = FilePath + ".tmp";
        File.WriteAllText(temporary,JsonSerializer.Serialize(products,new JsonSerializerOptions { WriteIndented = true }));
        File.Move(temporary,FilePath,true);
    }
}
