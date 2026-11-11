using System.Security.Cryptography;
using System.Text.Json;
using CafeStockAI.Application;
using CafeStockAI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CafeStockAI.Desktop;
internal static class SqlMode
{
    private static IInventoryPersistence? active;
    private static string FilePath => Path.Combine(InventoryStore.DirectoryPath, "connection.local.json");
    public static IInventoryPersistence? Store => InventoryStore.TestDirectory is null ? active : null;
    public static bool Configured => InventoryStore.TestDirectory is null && File.Exists(FilePath);
    public static bool Enabled => Store is not null;

    private static string Connection(string server, string database) => new SqlConnectionSettings(server, database).ConnectionString("CafeStockAI.Elazig");
    public static void Initialize()
    {
        if (!File.Exists(FilePath))
            return;
        var config = SqlConnectionSettings.Load(FilePath);
        var connection = Connection(config.Server, config.Database);
        using var db = DatabaseOptions.Create(connection);
        if (!db.Database.CanConnect())
            return;
        var store = new SqlInventoryPersistence(() => DatabaseOptions.Create(connection));
        try
        {
            if (store.IsInitialized())
                active = store;
        }
        catch
        {
            active = null;
        }
    }

    public static void ConnectAndImport(string server, string database, InventoryArchive source)
    {
        string connection = Connection(server, database);
        using var db = DatabaseOptions.Create(connection);
        if (!db.Database.CanConnect())
            throw new InvalidOperationException("SQL bağlantısı kurulamadı. Sunucu adı ve Windows erişimini kontrol edin.");
        var prepared = SingleBranchImport.Prepare(source);
        // Correct the user-confirmed duplicate on the copied import, never on the source files.
        var original = prepared.Products.FirstOrDefault(p => p.Id == Guid.Parse("16bba727-da1d-5b5e-b5fe-2a0834db17cf"));
        var duplicateId = Guid.Parse("9c552c48-ec8f-47e3-ba8e-994e5b2f3a8d");
        if (original is not null)
        {
            if (prepared.Inventory.Movements.Any(m => m.ProductId == duplicateId) || prepared.Inventory.Counts.Any(c => c.Lines.Any(l => l.ProductId == duplicateId)))
                throw new InvalidDataException("İkinci Cold Brew kaydında işlem bulundu. Aktarım öncesinde kayıtların kontrol edilmesi gerekiyor.");
            original.Unit = "Şişe";
            prepared.Products.RemoveAll(p => p.Id == duplicateId);
        }

        var duplicate = prepared.Products.GroupBy(p => (Category: p.Category.Trim().ToUpperInvariant(), Name: p.Name.Trim().ToUpperInvariant())).FirstOrDefault(g => g.Count() > 1);
        if (duplicate is not null)
            throw new InvalidDataException("Aynı kategori ve isimle birden fazla ürün var: " + duplicate.First().Category + " / " + duplicate.First().Name + ". Ürünler sayfasında fazla kaydı kaldırıp yeniden deneyin.");
        string fingerprint = Convert.ToHexString(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(prepared))));
        db.Database.Migrate();
        var store = new SqlInventoryPersistence(() => DatabaseOptions.Create(connection));
        if (!store.IsInitialized())
            store.Import(prepared, fingerprint);
        // Never silently overwrite an already initialized SQL database on reconnect.
        var loaded = store.Export();
        if (loaded.Inventory.Branches.Count != 1 || loaded.Inventory.Branches[0].Name != "Elazığ")
            throw new InvalidDataException("SQL kayıtları Elazığ tek şube düzeninde değil.");
        string temp = FilePath + ".tmp";
        File.WriteAllText(temp, JsonSerializer.Serialize(new SqlConnectionSettings(server.Trim(), database.Trim())));
        File.Move(temp, FilePath, true);
        active = store;
        Access.EmployeeId = null;
    }

    public static InventoryArchive ReadSource()
    {
        if (Enabled)
            return Store!.Export();
        T Read<T>(string name, T empty) => File.Exists(Path.Combine(InventoryStore.DirectoryPath, name)) ? JsonSerializer.Deserialize<T>(File.ReadAllText(Path.Combine(InventoryStore.DirectoryPath, name))) ?? empty : empty;
        return new(Read("inventory.json", new InventoryData()), Read("products.json", new List<Product>()), Read("accounts.json", new List<UserAccount>()));
    }
}

internal static class SingleBranchMode
{
    internal static bool Preview { get; set; }
    public static bool Enabled => InventoryStore.TestDirectory is null || Preview;

    public static void Initialize()
    {
        if (SqlMode.Enabled)
            return;
        string marker = Path.Combine(InventoryStore.DirectoryPath, "single-elazig.initialized");
        if (File.Exists(marker))
            return;
        Operations.CreateBackup();
        var source = new InventoryArchive(InventoryStore.Load(), ProductStore.Load(), Accounts.Load());
        var prepared = SingleBranchImport.Prepare(source);
        InventoryStore.Save(prepared.Inventory);
        File.WriteAllText(marker, DateTime.Now.ToString("O"));
    }
}
