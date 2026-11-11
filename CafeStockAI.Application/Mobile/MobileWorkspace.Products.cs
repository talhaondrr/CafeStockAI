using CafeStockAI.Application.Models;

namespace CafeStockAI.Application.Mobile;
public sealed partial class MobileWorkspace
{
    public void SaveProduct(Guid id, ProductEdit edit)
    {
        var a = Snapshot(id);
        RequireGlobal(Person(a, id));
        Quantity(edit.MinimumStock);
        string name = Text(edit.Name), category = Text(edit.Category), unit = Text(edit.Unit);
        if (name.Length == 0 || category.Length == 0 || unit.Length == 0)
            throw new InvalidOperationException("Ürün adı, kategori ve birim zorunlu.");
        if (a.Products.Any(p => p.Id != edit.Id && AccountCredentials.Key(p.Name) == AccountCredentials.Key(name) && AccountCredentials.Key(p.Category) == AccountCredentials.Key(category)))
            throw new InvalidOperationException("Bu kategoride aynı isimde ürün var.");
        var old = a.Products.SingleOrDefault(p => p.Id == edit.Id);
        if (edit.Id.HasValue && old is null)
            throw new InvalidOperationException("Ürün bulunamadı.");
        if (old is not null && old.Unit != unit && (a.Inventory.Movements.Any(m => m.ProductId == old.Id) || a.Inventory.Counts.Any(c => c.Lines.Any(l => l.ProductId == old.Id))))
            throw new InvalidOperationException("Geçmişi olan ürünün birimini değiştirmek için yeni ürün açın.");
        var row = old ?? new Product
        {
            Code = "MOB-" + Guid.NewGuid().ToString("N")[..8]
        };
        row.Name = name;
        row.Category = category;
        row.Unit = unit;
        row.MinimumStock = edit.MinimumStock;
        if (old is null)
            a.Products.Add(row);
        Checked.SaveProductsChecked(a.Products, a.Inventory.SqlVersion);
    }

    public void RemoveProduct(Guid id, Guid product)
    {
        var a = Snapshot(id);
        RequireGlobal(Person(a, id));
        if (!a.Products.Any(p => p.Id == product))
            throw new InvalidOperationException("Ürün bulunamadı.");
        Checked.SaveProductsChecked(a.Products.Where(p => p.Id != product).ToList(), a.Inventory.SqlVersion);
    }
}
