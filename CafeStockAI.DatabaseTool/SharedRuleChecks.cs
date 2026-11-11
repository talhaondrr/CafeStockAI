using CafeStockAI.Application;
using CafeStockAI.Application.Models;
using CafeStockAI.Infrastructure;
using Microsoft.Data.SqlClient;

internal static class SharedRuleChecks
{
    public static void Run()
    {
        void Check(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException(message);
            Console.WriteLine(message);
        }

        var account = new UserAccount();
        AccountCredentials.Set(account, "test-password");
        Check(AccountCredentials.Verify(account, "test-password") && !AccountCredentials.Verify(account, "wrong-password"), "Ortak parola doğrulaması doğru/yanlış şifreyi ayırıyor");
        account.Hash = "broken-hash";
        Check(!AccountCredentials.Verify(account, "test-password"), "Bozuk hesap hash değeri giriş açmıyor");
        Check(AccountCredentials.Key(" irem ") == AccountCredentials.Key("İREM"), "Türkçe kullanıcı adı eşleştirmesi korunuyor");
        foreach (var role in EmployeePermissions.Roles)
        {
            var employee = new Employee
            {
                Role = role
            };
            Check(EmployeePermissions.CanManage(employee) == (role is "İşletme Sahibi" or "Genel Müdür"), "Yönetim yetkisi: " + role);
            Check(EmployeePermissions.CanApprove(employee) == (role is "İşletme Sahibi" or "Genel Müdür" or "Şube Müdürü" or "Bölge Müdürü" or "Süpervizör"), "Sayım onay yetkisi: " + role);
        }

        Check(!EmployeePermissions.CanMoveStock(null) && !EmployeePermissions.CanManage(new Employee { Role = "Bilinmeyen" }), "Eksik/bilinmeyen rol yetki kazanmıyor");
        var local = new SqlConnectionStringBuilder(new SqlConnectionSettings("localhost\\SQLEXPRESS", "TestOnly").ConnectionString("Check"));
        var remote = new SqlConnectionStringBuilder(new SqlConnectionSettings("sql.example.invalid", "TestOnly").ConnectionString("Check"));
        Check(local.TrustServerCertificate && !remote.TrustServerCertificate && remote.IntegratedSecurity, "Ortak SQL ayarı uzak sunucuda sertifika doğrulamasını koruyor");
        Check(ProductImageKey.For(new Product { Name = "COLD BREW", Category = "TOZ VE KURUTULMUŞ" }) == "bottle", "Cold Brew görseli şişe olarak seçiliyor");
    }
}
