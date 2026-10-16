namespace CafeStockAI.Desktop;
internal static class BranchMovements
{
    public static void Add(Guid productId,string branch,string area,decimal quantity,string type,string note)
    {
        Access.RequireMovement(branch);
        using var transaction = DataLock.Enter();
        var data = InventoryStore.Load(); var product = ProductStore.Load().FirstOrDefault(p => p.Id == productId);
        if (product is null || !BranchStock.AllNames(data).Contains(branch) || area is not ("Bar" or "Depo")) throw new InvalidOperationException("Ürün, şube ve Bar/Depo alanını seçiniz.");
        if (string.IsNullOrWhiteSpace(product.Unit)) throw new InvalidOperationException("Önce ürünün birimini belirleyiniz.");
        if (quantity <= 0 || type is not ("Giriş" or "Çıkış")) throw new InvalidOperationException("İşlem türü ve sıfırdan büyük miktar gereklidir.");
        if (type == "Çıkış" && BranchStock.Balance(data,productId,branch,area) < quantity) throw new InvalidOperationException("Seçilen şube alanında yeterli stok yok.");
        var target = BranchStock.Area(data,branch,area);
        void Movement(Guid id,decimal amount) => data.Movements.Add(new StockMovement { ProductId = productId, WarehouseId = id, Type = type, Quantity = amount, Note = note + (Access.Current is Employee actor ? " • " + actor.Name : "") });
        if (type == "Giriş") Movement(target.Id,quantity);
        else
        {
            decimal remaining = quantity;
            foreach (var source in data.Warehouses.Where(w => w.BranchName == branch && BranchStock.AreaOf(w) == area))
            {
                decimal take = Math.Min(remaining,Math.Max(0,data.Balance(productId,source.Id)));
                if (take > 0) { Movement(source.Id,-take); remaining -= take; }
                if (remaining == 0) break;
            }
            if (remaining != 0) throw new InvalidOperationException("Stok kayıtları tutarsız. İşlem kaydedilmedi.");
        }
        InventoryStore.Save(data);
    }
}
