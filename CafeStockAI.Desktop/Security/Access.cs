using CafeStockAI.Application;
using System.Globalization;
using System.Text.Json;

namespace CafeStockAI.Desktop;
internal static class Access
{
    internal static Guid? EmployeeId;
    public static Employee? Current => EmployeeId is Guid id && Accounts.Load().Any(u => u.EmployeeId == id && u.Enabled) ? InventoryStore.Load().Employees.FirstOrDefault(e => e.Id == id) : null;
    private static bool Test => InventoryStore.TestDirectory is not null && EmployeeId is null;
    public static bool Global => Test || EmployeePermissions.CanManage(Current);
    public static bool Approver => Test || EmployeePermissions.CanApprove(Current);

    public static void RequireGlobal()
    {
        if (!Global)
            throw new UnauthorizedAccessException("Bu işlem için genel yönetim yetkisi gerekiyor.");
    }

    public static void RequireBranch(string branch)
    {
        if (!Global && Current?.AssignedBranches.Contains(branch) != true)
            throw new UnauthorizedAccessException("Bu şubeye erişim yetkiniz yok.");
    }

    public static void RequireMovement(string branch)
    {
        RequireBranch(branch);
        if (!Test && !EmployeePermissions.CanMoveStock(Current))
            throw new UnauthorizedAccessException("Stok hareketi düzenleme yetkiniz yok.");
    }

    public static InventoryData View() => View(out _);

    public static InventoryData View(out Employee? person)
    {
        var data = InventoryStore.Load();
        person = null;
        if (Test)
            return data;

        if (EmployeeId is not Guid id || !Accounts.Load().Any(u => u.EmployeeId == id && u.Enabled))
            throw new UnauthorizedAccessException("Önce sisteme giriş yapınız.");
        person = data.Employees.FirstOrDefault(e => e.Id == id)
            ?? throw new UnauthorizedAccessException("Önce sisteme giriş yapınız.");
        if (EmployeePermissions.CanManage(person))
            return data;

        var allowed = person.AssignedBranches.ToHashSet();
        data.Branches.RemoveAll(b => !allowed.Contains(b.Name));
        data.Warehouses.RemoveAll(w => !allowed.Contains(w.BranchName));
        var areas = data.Warehouses.Select(w => w.Id).ToHashSet();
        data.Movements.RemoveAll(m => !areas.Contains(m.WarehouseId));
        data.Counts.RemoveAll(c => !allowed.Contains(c.BranchName));
        data.Employees.RemoveAll(e => !e.AssignedBranches.Any(allowed.Contains));
        foreach (var e in data.Employees)
        {
            e.Branches = e.AssignedBranches.Where(allowed.Contains).ToList();
            e.BranchName = e.Branches.FirstOrDefault() ?? "";
        }

        data.Settings.OperatorName = person.Name;
        return data;
    }
}
