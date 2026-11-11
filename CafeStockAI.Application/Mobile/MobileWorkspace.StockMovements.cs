using CafeStockAI.Application.Models;

namespace CafeStockAI.Application.Mobile;
public sealed partial class MobileWorkspace
{
    public void AddMovement(Guid id, MovementEdit cmd)
    {
        var a = Snapshot(id);
        var p = Person(a, id);
        if (!Movement(p))
            throw new UnauthorizedAccessException("Stok hareketi yetkiniz yok.");
        Quantity(cmd.Quantity);
        if (cmd.Quantity == 0 || cmd.Id == Guid.Empty || cmd.Area is not ("Bar" or "Depo") || cmd.Type is not ("Giriş" or "Çıkış") || !a.Products.Any(p => p.Id == cmd.ProductId))
            throw new InvalidOperationException("Ürün, alan, işlem ve miktarı kontrol edin.");
        var area = a.Inventory.Warehouses.Single(w => w.BranchName == "Elazığ" && w.StockArea == cmd.Area);
        decimal amount = cmd.Type == "Çıkış" ? -cmd.Quantity : cmd.Quantity;
        string note = Text(cmd.Note, 1800) + " • " + p.Name;
        var old = a.Inventory.Movements.SingleOrDefault(m => m.Id == cmd.Id);
        if (old is not null)
        {
            if (old.ProductId == cmd.ProductId && old.WarehouseId == area.Id && old.Quantity == amount && old.ActorId == id)
                return;
            throw new InvalidOperationException("İşlem numarası kullanılmış.");
        }

        if (Balance(a.Inventory, cmd.ProductId, cmd.Area) + amount < 0)
            throw new InvalidOperationException("Yeterli stok yok.");
        a.Inventory.Movements.Add(new StockMovement { Id = cmd.Id, ProductId = cmd.ProductId, WarehouseId = area.Id, ActorId = id, Quantity = amount, Type = cmd.Type, Note = note });
        store.SaveInventory(a.Inventory);
    }
}
