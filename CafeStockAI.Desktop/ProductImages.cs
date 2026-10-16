using System.Reflection;
namespace CafeStockAI.Desktop;
internal static class ProductImages
{
    private static readonly Dictionary<string, Image> cache = new();
    internal static string Key(Product product)
    {
        string category = product.Category.ToUpperInvariant();
        string name = product.Name.ToUpperInvariant();
        if (name == "SU") return "water";
        if (string.IsNullOrWhiteSpace(category) && name.Contains("SÜT")) return "milk";
        if (name.Contains("POMPASI")) return "pump";
        if (name.Contains("KAPAK")) return "lid";
        if (name.Contains("PİPET")) return "straw";
        if (category == "BARDAK/KAPAK/PİPET") return "cup";
        if (category == "BARDAK/TABAK") return "mug";
        if (category == "SÜT ÇEŞİTLERİ") return "milk";
        if (category == "BİTKİ ÇAYLARI") return "tea";
        if (category == "ANA MALZEME") return "coffee";
        if (category == "TOZ VE KURUTULMUŞ") return "powder";
        if (category == "KASA ÖNÜ") return "snack";
        if (category is "BAR SOSLAR" or "ŞURUPLAR" or "COOL SERIES" or "BOOM SERIES" or "LİMONATA") return "bottle";
        if (product.Name.Contains("Kahve",StringComparison.CurrentCultureIgnoreCase)) return "coffee";
        return "bag";
    }
    public static Image Get(Product product)
    {
        string key = Key(product);
        if (cache.TryGetValue(key,out var image)) return image;
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"CafeStockAI.Desktop.Assets.Products.{key}.png")
            ?? throw new InvalidDataException($"Ürün görseli bulunamadı: {key}");
        using var source = Image.FromStream(stream);
        // Cache one small decoded thumbnail per asset instead of decoding a full photo for each row.
        var thumb = new Bitmap(96,96);
        using (var graphics = Graphics.FromImage(thumb))
        {
            graphics.Clear(Color.FromArgb(255,253,249));
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            float scale = Math.Min(96F/source.Width,96F/source.Height);
            float width = source.Width*scale, height = source.Height*scale;
            graphics.DrawImage(source,(96-width)/2,(96-height)/2,width,height);
        }
        cache[key] = thumb;
        return thumb;
    }
}
