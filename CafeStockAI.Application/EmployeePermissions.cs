using CafeStockAI.Application.Models;

namespace CafeStockAI.Application;
public static class EmployeePermissions
{
    public static readonly string[] Roles = ["Personel", "Şube Müdürü", "Genel Müdür", "Bölge Müdürü", "İşletme Sahibi", "Süpervizör", "Depo Sorumlusu"];
    public static bool CanManage(Employee? person) => person?.Role is "İşletme Sahibi" or "Genel Müdür";
    public static bool CanApprove(Employee? person) => CanManage(person) || person?.Role is "Şube Müdürü" or "Bölge Müdürü" or "Süpervizör";
    public static bool CanMoveStock(Employee? person) => CanApprove(person) || person?.Role == "Depo Sorumlusu";
}
