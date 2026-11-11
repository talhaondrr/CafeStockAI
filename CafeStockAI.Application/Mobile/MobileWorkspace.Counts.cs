using CafeStockAI.Application.Models;

namespace CafeStockAI.Application.Mobile;
public sealed partial class MobileWorkspace
{
    public CountTicket Begin(Guid id, string area = "Both")
    {
        if (area is not ("Bar" or "Depo" or "Both"))
            throw new InvalidOperationException("Bar veya Depo seçin.");
        var a = Snapshot(id);
        if (a.Products.Any(p => string.IsNullOrWhiteSpace(p.Unit)))
            throw new InvalidOperationException("Eksik ürün birimleri yönetici tarafından tamamlanmalı.");
        if (a.Products.Count == 0)
            throw new InvalidOperationException("Sayılacak ürün bulunamadı.");
        return new(Guid.NewGuid(), id, DateTimeOffset.UtcNow.AddHours(24), a.Products.Select(p => new CountExpected(p.Id, p.Unit, Balance(a.Inventory, p.Id, "Bar"), Balance(a.Inventory, p.Id, "Depo"))).ToList(), area);
    }

    public Guid Submit(Guid id, CountTicket ticket, List<CountEntry> entries)
    {
        var a = Snapshot(id);
        var person = Person(a, id);
        if (ticket.EmployeeId != id || ticket.Expires < DateTimeOffset.UtcNow)
            throw new UnauthorizedAccessException("Sayım taslağının oturumu veya süresi geçersiz.");
        if (entries.Count == 0 || entries.Count != ticket.Products.Count || entries.Select(l => l.ProductId).Distinct().Count() != entries.Count)
            throw new InvalidOperationException("Bütün ürünlerin Bar ve Depo miktarlarını girin.");
        if (ticket.Area is not ("Bar" or "Depo" or "Both"))
            throw new InvalidOperationException("Geçersiz sayım alanı.");
        bool bar = ticket.Area != "Depo", depotArea = ticket.Area != "Bar";
        var entered = entries.ToDictionary(l => l.ProductId);
        foreach (var expected in ticket.Products)
        {
            if (!entered.TryGetValue(expected.ProductId, out var entry) || (bar && !entry.Bar.HasValue) || (depotArea && !entry.Depot.HasValue) || (!bar && entry.Bar.HasValue) || (!depotArea && entry.Depot.HasValue))
                throw new InvalidOperationException("Yalnız seçilen alanın bütün miktarlarını girin. Diğer alan boş kalmalı.");
            if (entry.Bar.HasValue)
                Quantity(entry.Bar.Value);
            if (entry.Depot.HasValue)
                Quantity(entry.Depot.Value);
        }

        var previous = a.Inventory.Counts.FirstOrDefault(c => c.Id == ticket.Id);
        if (previous is not null)
        {
            if (previous.Operator != person.Name || previous.Lines.Count != entries.Count || previous.Lines.Any(l => !entered.TryGetValue(l.ProductId, out var e) || e.Bar != l.BarCounted || e.Depot != l.DepotCounted))
                throw new InvalidOperationException("Bu gönderim numarası farklı bir sayımda kullanılmış.");
            return previous.Id;
        }

        if (!a.Products.Select(p => p.Id).Order().SequenceEqual(ticket.Products.Select(p => p.ProductId).Order()) || a.Products.Any(p => ticket.Products.Single(e => e.ProductId == p.Id).Unit != p.Unit))
            throw new InvalidOperationException("Ürünler veya birimler değişti. Yeni sayım başlatın.");
        if (ticket.Products.Any(e => (bar && e.Bar != Balance(a.Inventory, e.ProductId, "Bar")) || (depotArea && e.Depot != Balance(a.Inventory, e.ProductId, "Depo"))))
            throw new InvalidOperationException("Sayım sırasında stok değişti. Taslağınız korunuyor; güncel stokla yeni sayım başlatın.");
        var depot = a.Inventory.Warehouses.First(w => w.BranchName == "Elazığ" && w.StockArea == (ticket.Area == "Bar" ? "Bar" : "Depo"));
        a.Inventory.Counts.Add(new StockCount { Id = ticket.Id, WarehouseId = depot.Id, BranchName = "Elazığ", Operator = person.Name, Status = "Onay bekliyor", Date = DateTime.Now, Lines = ticket.Products.Select(e => new CountLine { ProductId = e.ProductId, BarExpected = bar ? e.Bar : null, DepotExpected = depotArea ? e.Depot : null, Expected = (bar ? e.Bar : 0) + (depotArea ? e.Depot : 0), BarCounted = entered[e.ProductId].Bar, DepotCounted = entered[e.ProductId].Depot, Counted = (entered[e.ProductId].Bar ?? 0) + (entered[e.ProductId].Depot ?? 0) }).ToList() });
        store.SaveInventory(a.Inventory);
        return ticket.Id;
    }

    public void Review(Guid id, Guid countId, bool approved, string? note)
    {
        var a = Snapshot(id);
        var person = Person(a, id);
        if (!Approver(person))
            throw new UnauthorizedAccessException("Sayım onaylama yetkiniz yok.");
        if (!a.Inventory.Counts.Any(c => c.Id == countId && c.BranchName == "Elazığ"))
            throw new InvalidOperationException("Sayım bulunamadı.");
        CountApproval.Apply(a.Inventory, a.Products, id, person.Name, countId, approved, Text(note, 2000));
        store.SaveInventory(a.Inventory);
    }
}
