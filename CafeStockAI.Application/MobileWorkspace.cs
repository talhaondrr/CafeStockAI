using CafeStockAI.Application.Models;

namespace CafeStockAI.Application.Mobile;
/// <summary>Request-scoped identity; no Desktop static session is shared with web users.</summary>
public sealed partial class MobileWorkspace(IInventoryPersistence store)
{
    public static readonly string[] Roles = EmployeePermissions.Roles;
    public static bool Global(Employee p) => EmployeePermissions.CanManage(p);
    public static bool Approver(Employee p) => EmployeePermissions.CanApprove(p);
    public static bool Movement(Employee p) => EmployeePermissions.CanMoveStock(p);
    public static Employee Person(InventoryArchive data, Guid id)
    {
        var p = data.Inventory.Employees.SingleOrDefault(p => p.Id == id && p.IsActive);
        if (p is null || !data.Accounts.Any(a => a.EmployeeId == id && a.Enabled))
            throw new UnauthorizedAccessException("Hesabınız kapalı. Yeniden giriş yapın.");
        if (!Global(p) && !p.AssignedBranches.Contains("Elazığ"))
            throw new UnauthorizedAccessException("Elazığ şubesine erişim yetkiniz yok.");
        return p;
    }

    private IWorkspacePersistence Checked => store as IWorkspacePersistence ?? throw new InvalidOperationException("Depolama bu işlemi desteklemiyor.");

    public InventoryArchive Snapshot(Guid id)
    {
        var a = store.Export();
        Person(a, id);
        return a;
    }

    public static decimal Balance(InventoryData d, Guid product, string area) => d.Warehouses.Where(w => w.BranchName == "Elazığ" && w.StockArea == area).Sum(w => d.Balance(product, w.Id));
    private static void Quantity(decimal n)
    {
        if (n < 0 || n >= 1000000000000m || decimal.Round(n, 6) != n)
            throw new InvalidOperationException("Miktar negatif olamaz; en fazla 6 ondalık basamak kullanılabilir.");
    }

    private static string Text(string? s, int max = 160)
    {
        s = (s ?? "").Trim();
        if (s.Length > max)
            throw new InvalidOperationException($"Metin en fazla {max} karakter olabilir.");
        return s;
    }

    private static void RequireGlobal(Employee p)
    {
        if (!Global(p))
            throw new UnauthorizedAccessException("Genel yönetim yetkisi gerekiyor.");
    }
}
