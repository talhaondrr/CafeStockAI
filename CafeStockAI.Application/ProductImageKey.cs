using CafeStockAI.Application.Models;

namespace CafeStockAI.Application;
public static class ProductImageKey
{
    public static string For(Product p)
    {
        var c = p.Category.ToUpperInvariant();
        var n = p.Name.ToUpperInvariant();
        if (string.IsNullOrWhiteSpace(c) && n.Contains("SÜT"))
            return "milk";
        if (n == "SU")
            return "water";
        if (n.Contains("POMPASI"))
            return "pump";
        if (n.Contains("KAPAK"))
            return "lid";
        if (n.Contains("PİPET"))
            return "straw";
        if (n == "COLD BREW")
            return "bottle";
        return c switch
        {
            "BARDAK/KAPAK/PİPET" => "cup",
            "BARDAK/TABAK" => "mug",
            "SÜT ÇEŞİTLERİ" => "milk",
            "BİTKİ ÇAYLARI" => "tea",
            "ANA MALZEME" => "coffee",
            "TOZ VE KURUTULMUŞ" => "powder",
            "KASA ÖNÜ" => "snack",
            "BAR SOSLAR" or "ŞURUPLAR" or "COOL SERIES" or "BOOM SERIES" or "LİMONATA" => "bottle",
            _ when p.Name.Contains("Kahve", StringComparison.CurrentCultureIgnoreCase) => "coffee",
            _ => "bag"
        };
    }
}
