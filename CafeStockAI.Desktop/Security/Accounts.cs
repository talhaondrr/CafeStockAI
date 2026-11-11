using CafeStockAI.Application;
using System.Globalization;
using System.Text.Json;

namespace CafeStockAI.Desktop;
internal static class Accounts
{
    private static string FilePath => Path.Combine(InventoryStore.DirectoryPath, "accounts.json");

    internal static string Key(string value) => AccountCredentials.Key(value);
    public static List<UserAccount> Load() => SqlMode.Store is { } sql ? sql.LoadAccounts() : File.Exists(FilePath) ? JsonSerializer.Deserialize<List<UserAccount>>(File.ReadAllText(FilePath)) ?? throw new InvalidDataException("Kullanıcı dosyası geçersiz.") : new();
    private static void Save(List<UserAccount> users)
    {
        if (SqlMode.Store is { } sql)
        {
            sql.SaveAccounts(users);
            return;
        }

        Directory.CreateDirectory(InventoryStore.DirectoryPath);
        File.WriteAllText(FilePath + ".tmp", JsonSerializer.Serialize(users));
        File.Move(FilePath + ".tmp", FilePath, true);
    }

    private static void SetPassword(UserAccount user, string password) => AccountCredentials.Set(user, password);
    public static void Sync()
    {
        using var transaction = DataLock.Enter();
        var users = Load();
        var staff = InventoryStore.Load().Employees;
        foreach (var person in staff)
        {
            var user = users.FirstOrDefault(u => u.EmployeeId == person.Id);
            if (user is null)
            {
                user = new UserAccount
                {
                    EmployeeId = person.Id
                };
                SetPassword(user, "123456");
                users.Add(user);
            }

            string name = person.Name.Trim();
            if (staff.Count(e => Key(e.Name) == Key(name)) > 1)
                name += " " + person.Id.ToString("N")[..6];
            if (string.IsNullOrWhiteSpace(user.Username) || Key(user.Username) == Key(name))
                user.Username = name.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].ToLower(new CultureInfo("tr-TR"));
        }

        foreach (var user in users.Where(u => !staff.Any(e => e.Id == u.EmployeeId)))
            user.Enabled = false;
        Save(users);
    }

    public static bool Login(string username, string password)
    {
        Access.EmployeeId = null;
        Sync();
        var user = Load().FirstOrDefault(u => u.Enabled && Key(u.Username) == Key(username));
        if (user is null)
            return false;
        bool valid = AccountCredentials.Verify(user, password);
        if (valid)
            Access.EmployeeId = user.EmployeeId;
        return valid;
    }

    public static void EnsureManager(InventoryData data)
    {
        if (InventoryStore.TestDirectory is not null && Access.EmployeeId is null)
            return;
        var users = Load();
        if (!data.Employees.Any(e => e.Role is "İşletme Sahibi" or "Genel Müdür" && (!users.Any(u => u.EmployeeId == e.Id) || users.Any(u => u.EmployeeId == e.Id && u.Enabled))))
            throw new InvalidOperationException("En az bir aktif genel müdür veya işletme sahibi bulunmalı.");
    }

    public static void Enable(Guid id, bool enabled)
    {
        Access.RequireGlobal();
        if (id == Access.EmployeeId && !enabled)
            throw new InvalidOperationException("Kendi hesabınızı kapatamazsınız.");
        using var transaction = DataLock.Enter();
        var users = Load();
        if (!enabled && !users.Any(u => u.EmployeeId != id && u.Enabled && InventoryStore.Load().Employees.Any(e => e.Id == u.EmployeeId && e.Role is "İşletme Sahibi" or "Genel Müdür")))
            throw new InvalidOperationException("Son yönetici hesabı kapatılamaz.");
        users.Single(u => u.EmployeeId == id).Enabled = enabled;
        Save(users);
    }

    public static void Change(Guid id, string password, bool? enabled = null)
    {
        if (id != Access.EmployeeId)
            Access.RequireGlobal();
        using var transaction = DataLock.Enter();
        var users = Load();
        var user = users.Single(u => u.EmployeeId == id);
        SetPassword(user, password);
        if (enabled.HasValue)
        {
            Access.RequireGlobal();
            if (id == Access.EmployeeId && !enabled.Value)
                throw new InvalidOperationException("Kendi hesabınızı kapatamazsınız.");
            user.Enabled = enabled.Value;
        }

        Save(users);
    }
}
