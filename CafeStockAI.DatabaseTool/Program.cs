using CafeStockAI.Domain;
using CafeStockAI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

if (args.Length == 2 && args[0] == "--check-persistence")
{
    PersistenceChecks.Run(args[1]);
    return 0;
}

if (args.Length != 2 || args[0] != "--schema")
{
    if (args.Length == 1 && args[0] == "--probe")
    {
        string? connection = Environment.GetEnvironmentVariable("CAFESTOCK_SQL_CONNECTION");
        if (string.IsNullOrWhiteSpace(connection))
        {
            Console.Error.WriteLine("CAFESTOCK_SQL_CONNECTION ortam değişkenini bu oturumda ayarlayın. Bağlantı bilgisi kaynak koda yazılmaz.");
            return 2;
        }

        try
        {
            await using var db = new CafeStockDbContext(new DbContextOptionsBuilder<CafeStockDbContext>().UseSqlServer(connection, o => o.CommandTimeout(5)).Options);
            bool connected = await new SqlDatabaseProbe(db).CanConnectAsync();
            Console.WriteLine(connected ? "SQL bağlantısı başarılı." : "SQL bağlantısı kurulamadı. Sunucu adı ve hizmeti kontrol edin.");
            return connected ? 0 : 1;
        }
        catch
        {
            Console.Error.WriteLine("SQL bağlantı kontrolü başarısız. Bağlantı bilgisini paylaşmadan sunucu adı ve erişimi kontrol edin.");
            return 1;
        }
    }

    Console.WriteLine("Kullanım: --schema <sql-dosyası> | --probe");
    return 2;
}

await using (var db = new CafeStockDbContext(new DbContextOptionsBuilder<CafeStockDbContext>().UseSqlServer().Options))
{
    var entities = db.Model.GetEntityTypes().ToList();
    if (entities.Count != 17)
        throw new InvalidOperationException($"Beklenmeyen tablo sayısı: {entities.Count}");
    if (entities.SelectMany(e => e.GetProperties()).Any(p => p.GetColumnType()?.Contains("json", StringComparison.OrdinalIgnoreCase) == true))
        throw new InvalidOperationException("JSON sütunu kullanılmamalı.");
    if (entities.SelectMany(e => e.GetForeignKeys()).Any(f => f.DeleteBehavior != DeleteBehavior.Restrict))
        throw new InvalidOperationException("Silme ilişkileri kısıtlı olmalı.");
    string script = db.Database.GenerateCreateScript();
    Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(args[1]))!);
    await File.WriteAllTextAsync(args[1], "-- Şema taslağı: EF Core modelinden üretilmiştir. Migration/aktarımı tamamlanmadan gerçek DB'de çalıştırmayın.\n" + script);
    try
    {
        new Product("", Guid.NewGuid(), Guid.NewGuid(), 0);
        throw new Exception("Ürün doğrulaması başarısız.");
    }
    catch (ArgumentException)
    {
    }

    var count = new StockCount();
    count.Review(Guid.NewGuid(), false, "Kontrol gerekli", DateTime.Now);
    try
    {
        count.Review(Guid.NewGuid(), true, "", DateTime.Now);
        throw new Exception("Çift onay engellenmedi.");
    }
    catch (InvalidOperationException)
    {
    }

    Console.WriteLine($"Model doğrulandı: {entities.Count} ilişkisel tablo, {entities.Sum(e => e.GetForeignKeys().Count())} dış anahtar. Ürün doğrulama ve çift inceleme engeli geçti. SQL şeması üretildi; sunucuda çalıştırılmadı.");
}

return 0;
