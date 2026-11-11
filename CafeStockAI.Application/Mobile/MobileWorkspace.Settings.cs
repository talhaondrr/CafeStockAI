using CafeStockAI.Application.Models;

namespace CafeStockAI.Application.Mobile;
public sealed partial class MobileWorkspace
{
    public void SaveSettings(Guid id, string business)
    {
        var a = Snapshot(id);
        RequireGlobal(Person(a, id));
        business = Text(business);
        if (business.Length == 0)
            throw new InvalidOperationException("İşletme adı zorunlu.");
        a.Inventory.Settings.BusinessName = business;
        store.SaveInventory(a.Inventory);
    }
}
