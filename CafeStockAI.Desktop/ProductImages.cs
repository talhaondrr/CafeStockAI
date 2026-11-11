using CafeStockAI.Application;
using System.Reflection;

namespace CafeStockAI.Desktop;
internal static class ProductImages
{
    private static readonly Dictionary<string, Image> cache = new();
    internal static string Key(Product product) => ProductImageKey.For(product);
    public static Image Get(Product product)
    {
        string key = Key(product);
        if (cache.TryGetValue(key, out var image))
            return image;
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"CafeStockAI.Desktop.Assets.Products.{key}.png") ?? throw new InvalidDataException($"Ürün görseli bulunamadı: {key}");
        using var source = Image.FromStream(stream);
        // Cache one small decoded thumbnail per asset instead of decoding a full photo for each row.
        var thumb = new Bitmap(96, 96);
        using (var graphics = Graphics.FromImage(thumb))
        {
            graphics.Clear(Color.FromArgb(255, 253, 249));
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            float scale = Math.Min(96F / source.Width, 96F / source.Height);
            float width = source.Width * scale, height = source.Height * scale;
            graphics.DrawImage(source, (96 - width) / 2, (96 - height) / 2, width, height);
        }

        cache[key] = thumb;
        return thumb;
    }
}
