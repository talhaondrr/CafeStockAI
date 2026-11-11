using System.Data;
using System.Globalization;
using CafeStockAI.Application;
using Microsoft.EntityFrameworkCore;
using D = CafeStockAI.Domain;
using M = CafeStockAI.Application.Models;

namespace CafeStockAI.Infrastructure;
public sealed class SqlInventoryPersistence(Func<CafeStockDbContext> factory) : IInventoryPersistence, IWorkspacePersistence
{
    private static string Key(string value) => value.Trim().Normalize().ToUpper(new CultureInfo("tr-TR"));
    private static Guid Stable(string value) => new(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes("cafestock:" + value))[..16]);
    private static void Precision(decimal value)
    {
        if (decimal.Round(value, 6) != value || Math.Abs(value) >= 1000000000000m)
            throw new InvalidDataException("Miktar SQL hassasiyetini aşıyor; veri yuvarlanmadan aktarım durduruldu.");
    }

    private static D.BusinessSettings Settings(CafeStockDbContext db) => db.Set<D.BusinessSettings>().SingleOrDefault() ?? throw new InvalidDataException("SQL aktarımı tamamlanmamış.");
    private static void Revision(CafeStockDbContext db, D.BusinessSettings settings, byte[]? expected = null)
    {
        if (expected is not null && !settings.Version.SequenceEqual(expected))
            throw new InvalidOperationException("Kayıtlar başka bir işlemde değişti. Sayfayı yenileyip tekrar deneyin.");
        db.Entry(settings).Property(e => e.BusinessName).IsModified = true;
    }

    public M.InventoryData LoadInventory()
    {
        using var db = factory();
        using var tx = db.Database.BeginTransaction(IsolationLevel.Serializable);
        var data = ReadInventory(db);
        tx.Commit();
        return data;
    }

    private static M.InventoryData ReadInventory(CafeStockDbContext db, bool includeInactive = false)
    {
        var settings = Settings(db);
        var branches = db.Set<D.Branch>().Where(b => b.IsActive).AsNoTracking().ToList();
        var names = branches.ToDictionary(b => b.Id, b => b.Name);
        var roles = db.Set<D.Role>().AsNoTracking().ToDictionary(r => r.Id, r => r.Name);
        var assigned = db.Set<D.EmployeeBranch>().AsNoTracking().ToList();
        var data = new M.InventoryData
        {
            BranchRegistryInitialized = true,
            SqlVersion = settings.Version.ToArray(),
            Settings = new()
            {
                BusinessName = settings.BusinessName,
                OperatorName = settings.OperatorName
            },
            Branches = branches.Select(b => new M.Branch { Id = b.Id, Code = b.Code, Name = b.Name, Address = b.Address }).ToList()
        };
        data.Employees = db.Set<D.Employee>().Where(e => includeInactive || e.IsActive).AsNoTracking().ToList().Select(e => new M.Employee { Id = e.Id, IsActive = e.IsActive, Name = e.Name, Role = roles[e.RoleId], Phone = e.Phone, HireDate = e.HireDate, Branches = assigned.Where(a => a.EmployeeId == e.Id).Select(a => names[a.BranchId]).DefaultIfEmpty("Elazığ").ToList() }).ToList();
        data.Warehouses = db.StockAreas.AsNoTracking().ToList().Select(a => new M.Warehouse { Id = a.Id, BranchName = names[a.BranchId], StockArea = a.Area == D.StockAreaKind.Bar ? "Bar" : "Depo", Name = a.Name, Code = a.Code, Location = a.Location }).ToList();
        data.Movements = db.StockMovements.AsNoTracking().ToList().Select(m => new M.StockMovement { Id = m.Id, ProductId = m.ProductId, WarehouseId = m.AreaId, Date = m.OccurredAt, Type = m.Type, Quantity = m.Quantity, Note = m.Note, ActorId = m.ActorId, CountId = m.CountId }).ToList();
        var lines = db.Set<D.StockCountLine>().AsNoTracking().ToList();
        data.Counts = db.Set<D.StockCount>().AsNoTracking().ToList().Select(c => new M.StockCount { Id = c.Id, WarehouseId = c.SourceWarehouseId, BranchName = names[c.BranchId], Date = c.SubmittedAt, Operator = c.SubmittedName, Status = c.Status == D.CountStatus.Pending ? "Onay bekliyor" : c.Status == D.CountStatus.Approved ? "Onaylandı" : "Reddedildi", Reviewer = c.ReviewedName, ReviewedAt = c.ReviewedAt, ReviewNote = c.ReviewNote, Lines = lines.Where(l => l.CountId == c.Id).Select(l => new M.CountLine { ProductId = l.ProductId, Expected = l.Expected, Counted = l.Counted, BarExpected = l.ExpectedBar, BarCounted = l.CountedBar, DepotExpected = l.ExpectedDepot, DepotCounted = l.CountedDepot }).ToList() }).ToList();
        return data;
    }

    public List<M.Product> LoadProducts()
    {
        using var db = factory();
        return ReadProducts(db);
    }

    private static List<M.Product> ReadProducts(CafeStockDbContext db)
    {
        var categories = db.Set<D.Category>().AsNoTracking().ToDictionary(c => c.Id, c => c.Name);
        var units = db.Set<D.Unit>().AsNoTracking().ToDictionary(u => u.Id, u => u.Name);
        return db.Products.Where(p => p.IsActive).AsNoTracking().ToList().Select(p => new M.Product { Id = p.Id, Code = p.Code, Name = p.Name, Category = categories[p.CategoryId], Unit = units[p.UnitId], MinimumStock = p.MinimumStock, SourceCell = p.SourceCell }).ToList();
    }

    public List<M.UserAccount> LoadAccounts()
    {
        using var db = factory();
        return ReadAccounts(db);
    }

    private static List<M.UserAccount> ReadAccounts(CafeStockDbContext db) => db.Set<D.UserAccount>().AsNoTracking().ToList().Select(a => new M.UserAccount { EmployeeId = a.EmployeeId, Username = a.Username, Hash = a.PasswordHash, Salt = a.PasswordSalt, Enabled = a.IsEnabled }).ToList();
    public InventoryArchive Export()
    {
        using var db = factory();
        using var tx = db.Database.BeginTransaction(IsolationLevel.Serializable);
        var archive = ReadArchive(db);
        tx.Commit();
        return archive;
    }

    private static InventoryArchive ReadArchive(CafeStockDbContext db) => new(ReadInventory(db, true), ReadProducts(db), ReadAccounts(db), db.Set<D.ImportedLocation>().AsNoTracking().ToList().GroupBy(l => l.SourceWarehouseId).ToDictionary(g => g.Key, g => g.Last().AreaId));
    private static void Upsert<T>(CafeStockDbContext db, T row, Dictionary<Guid, T> existing, Guid id)
        where T : class
    {
        if (existing.TryGetValue(id, out var current))
            db.Entry(current).CurrentValues.SetValues(row);
        else
        {
            db.Add(row);
            existing[id] = row;
        }
    }

    private static void WriteInventory(CafeStockDbContext db, M.InventoryData data)
    {
        if (data.Branches.Count != 1 || data.Branches[0].Name != "Elazığ" || data.Warehouses.Any(w => w.BranchName != "Elazığ") || data.Employees.Any(e => e.AssignedBranches.Count != 1 || e.AssignedBranches[0] != "Elazığ"))
            throw new InvalidOperationException("Bu sürüm yalnız Elazığ şubesini kullanır.");
        var branch = data.Branches[0];
        var storedBranches = db.Set<D.Branch>().ToDictionary(b => b.Id);
        Upsert(db, new D.Branch { Id = branch.Id, Name = branch.Name, Code = branch.Code, Address = branch.Address }, storedBranches, branch.Id);
        var roles = db.Set<D.Role>().ToDictionary(r => r.Name, StringComparer.Ordinal);
        foreach (var name in data.Employees.Select(e => e.Role).Distinct())
            if (!roles.ContainsKey(name))
            {
                var r = new D.Role
                {
                    Id = Stable("role:" + name),
                    Name = name
                };
                roles[name] = r;
                db.Add(r);
            }

        var staff = db.Set<D.Employee>().ToDictionary(e => e.Id);
        var present = data.Employees.Select(e => e.Id).ToHashSet();
        foreach (var old in staff.Values.Where(e => !present.Contains(e.Id)))
            old.IsActive = false;
        foreach (var e in data.Employees)
            Upsert(db, new D.Employee { Id = e.Id, IsActive = e.IsActive, Name = e.Name, Phone = e.Phone, HireDate = e.HireDate, RoleId = roles[e.Role].Id }, staff, e.Id);
        var assignments = db.Set<D.EmployeeBranch>().ToList();
        foreach (var old in assignments.Where(a => !present.Contains(a.EmployeeId) || a.BranchId != branch.Id))
            db.Remove(old);
        foreach (var e in data.Employees)
            if (!assignments.Any(a => a.EmployeeId == e.Id && a.BranchId == branch.Id))
                db.Add(new D.EmployeeBranch { EmployeeId = e.Id, BranchId = branch.Id });
        var areas = db.StockAreas.ToDictionary(a => a.Id);
        foreach (var w in data.Warehouses)
            Upsert(db, new D.StockArea { Id = w.Id, BranchId = branch.Id, Area = w.StockArea == "Bar" ? D.StockAreaKind.Bar : D.StockAreaKind.Depot, Name = w.Name, Code = w.Code, Location = w.Location }, areas, w.Id);
        var movements = db.StockMovements.ToDictionary(m => m.Id);
        foreach (var m in data.Movements)
        {
            Precision(m.Quantity);
            var row = new D.StockMovement
            {
                Id = m.Id,
                AreaId = m.WarehouseId,
                ProductId = m.ProductId,
                Quantity = m.Quantity,
                OccurredAt = m.Date,
                Type = m.Type,
                Note = m.Note,
                ActorId = m.ActorId,
                CountId = m.CountId,
                IsDemo = m.Note.StartsWith("[ÖRNEK]")
            };
            if (movements.TryGetValue(m.Id, out var previous))
            {
                if (previous.ProductId != row.ProductId || previous.AreaId != row.AreaId || previous.Quantity != row.Quantity || previous.OccurredAt != row.OccurredAt || previous.Type != row.Type || previous.Note != row.Note || previous.ActorId != row.ActorId || previous.CountId != row.CountId)
                    throw new InvalidOperationException("Kaydedilmiş stok hareketi değiştirilemez.");
            }
            else
                db.Add(row);
        }

        if (movements.Keys.Except(data.Movements.Select(m => m.Id)).Any())
            throw new InvalidOperationException("Stok geçmişi eksiltilerek kaydedilemez.");
        Guid? PersonId(string name)
        {
            var matches = staff.Values.Where(e => e.Name == name).ToList();
            return matches.Count == 1 ? matches[0].Id : null;
        }

        var counts = db.Set<D.StockCount>().ToDictionary(c => c.Id);
        var oldLines = db.Set<D.StockCountLine>().ToList();
        foreach (var c in data.Counts)
        {
            var state = c.Status switch
            {
                "Onay bekliyor" => D.CountStatus.Pending,
                "Onaylandı" => D.CountStatus.Approved,
                "Reddedildi" => D.CountStatus.Rejected,
                _ => throw new InvalidDataException("Sayım durumu geçersiz.")};
            var row = D.StockCount.FromHistory(c.Id, branch.Id, c.WarehouseId, c.Operator, c.Date, state, c.Reviewer, c.ReviewedAt, c.ReviewNote, PersonId(c.Operator), PersonId(c.Reviewer));
            row.IsDemo = c.Operator.StartsWith("[ÖRNEK]");
            Upsert(db, row, counts, c.Id);
            foreach (var l in c.Lines)
            {
                foreach (var v in new decimal? []
                {
                    l.Expected,
                    l.Counted,
                    l.BarExpected,
                    l.BarCounted,
                    l.DepotExpected,
                    l.DepotCounted
                }

                )
                    if (v.HasValue)
                        Precision(v.Value);
                var line = new D.StockCountLine
                {
                    CountId = c.Id,
                    ProductId = l.ProductId,
                    Expected = l.Expected,
                    Counted = l.Counted,
                    ExpectedBar = l.BarExpected,
                    CountedBar = l.BarCounted,
                    ExpectedDepot = l.DepotExpected,
                    CountedDepot = l.DepotCounted
                };
                var old = oldLines.FirstOrDefault(x => x.CountId == c.Id && x.ProductId == l.ProductId);
                if (old is null)
                    db.Add(line);
                else
                    db.Entry(old).CurrentValues.SetValues(line);
            }
        }
    }

    private static void WriteProducts(CafeStockDbContext db, List<M.Product> products)
    {
        var categories = db.Set<D.Category>().ToDictionary(c => c.Name, StringComparer.Ordinal);
        var units = db.Set<D.Unit>().ToDictionary(u => u.Name, StringComparer.Ordinal);
        var stored = db.Products.ToDictionary(p => p.Id);
        foreach (var old in stored.Values.Where(p => !products.Any(x => x.Id == p.Id)))
            old.IsActive = false;
        foreach (var p in products)
        {
            Precision(p.MinimumStock);
            if (string.IsNullOrWhiteSpace(p.Unit))
                throw new InvalidOperationException("Eksik ürün birimlerini tamamlayınız.");
            if (!categories.ContainsKey(p.Category))
            {
                var c = new D.Category
                {
                    Id = Stable("category:" + p.Category),
                    Name = p.Category
                };
                categories[c.Name] = c;
                db.Add(c);
            }

            if (!units.ContainsKey(p.Unit))
            {
                var u = new D.Unit
                {
                    Id = Stable("unit:" + p.Unit),
                    Name = p.Unit
                };
                units[u.Name] = u;
                db.Add(u);
            }

            Upsert(db, new D.Product(p.Name, categories[p.Category].Id, units[p.Unit].Id, p.MinimumStock) { Id = p.Id, Code = p.Code, SourceCell = p.SourceCell }, stored, p.Id);
        }
    }

    private static void WriteAccounts(CafeStockDbContext db, List<M.UserAccount> accounts)
    {
        var stored = db.Set<D.UserAccount>().ToDictionary(a => a.EmployeeId);
        foreach (var old in stored.Values.Where(a => !accounts.Any(x => x.EmployeeId == a.EmployeeId)))
            old.IsEnabled = false;
        foreach (var a in accounts)
        {
            if (Convert.FromBase64String(a.Salt).Length != 16 || Convert.FromBase64String(a.Hash).Length != 32)
                throw new InvalidDataException("Hesap şifre biçimi geçersiz.");
            Upsert(db, new D.UserAccount { EmployeeId = a.EmployeeId, Username = a.Username, UsernameKey = Key(a.Username), PasswordHash = a.Hash, PasswordSalt = a.Salt, IsEnabled = a.Enabled }, stored, a.EmployeeId);
        }
    }

    public void SaveInventory(M.InventoryData data)
    {
        using var db = factory();
        using var tx = db.Database.BeginTransaction(IsolationLevel.Serializable);
        var settings = Settings(db);
        Revision(db, settings, data.SqlVersion);
        WriteInventory(db, data);
        settings.BusinessName = data.Settings.BusinessName;
        settings.OperatorName = data.Settings.OperatorName;
        db.SaveChanges();
        tx.Commit();
        data.SqlVersion = settings.Version.ToArray();
    }

    public void SaveProducts(List<M.Product> products)
    {
        using var db = factory();
        using var tx = db.Database.BeginTransaction(IsolationLevel.Serializable);
        Revision(db, Settings(db));
        WriteProducts(db, products);
        db.SaveChanges();
        tx.Commit();
    }

    public void SaveAccounts(List<M.UserAccount> accounts)
    {
        using var db = factory();
        using var tx = db.Database.BeginTransaction(IsolationLevel.Serializable);
        Revision(db, Settings(db));
        WriteAccounts(db, accounts);
        db.SaveChanges();
        tx.Commit();
    }

    public void SaveStaffAndAccounts(M.InventoryData data, List<M.UserAccount> accounts)
    {
        using var db = factory();
        using var tx = db.Database.BeginTransaction(IsolationLevel.Serializable);
        Revision(db, Settings(db), data.SqlVersion);
        WriteInventory(db, data);
        WriteAccounts(db, accounts);
        db.SaveChanges();
        tx.Commit();
    }

    public void SaveProductsChecked(List<M.Product> products, byte[] version)
    {
        using var db = factory();
        using var tx = db.Database.BeginTransaction(IsolationLevel.Serializable);
        Revision(db, Settings(db), version);
        WriteProducts(db, products);
        db.SaveChanges();
        tx.Commit();
    }

    public void SaveAccountsChecked(List<M.UserAccount> accounts, byte[] version)
    {
        using var db = factory();
        using var tx = db.Database.BeginTransaction(IsolationLevel.Serializable);
        Revision(db, Settings(db), version);
        WriteAccounts(db, accounts);
        db.SaveChanges();
        tx.Commit();
    }

    public bool IsInitialized()
    {
        using var db = factory();
        return db.Set<D.BusinessSettings>().Any();
    }

    public void Import(InventoryArchive archive, string fingerprint)
    {
        using var db = factory();
        using var tx = db.Database.BeginTransaction(IsolationLevel.Serializable);
        if (db.Set<D.ImportHistory>().Any(h => h.SourceFingerprint == fingerprint))
        {
            tx.Commit();
            return;
        }

        if (db.Set<D.BusinessSettings>().Any())
            throw new InvalidOperationException("SQL kayıtları zaten var. Kaynak veriler tekrar aktarılamaz.");
        var settings = new D.BusinessSettings
        {
            Id = Stable("settings"),
            BusinessName = archive.Inventory.Settings.BusinessName,
            OperatorName = archive.Inventory.Settings.OperatorName
        };
        db.Add(settings);
        WriteProducts(db, archive.Products);
        WriteInventory(db, archive.Inventory);
        WriteAccounts(db, archive.Accounts);
        var history = new D.ImportHistory
        {
            SourceFingerprint = fingerprint,
            AppliedAt = DateTime.Now,
            ProductCount = archive.Products.Count,
            EmployeeCount = archive.Inventory.Employees.Count,
            MovementCount = archive.Inventory.Movements.Count,
            CountCount = archive.Inventory.Counts.Count
        };
        db.Add(history);
        if (archive.LocationAliases is not null)
            foreach (var alias in archive.LocationAliases)
                db.Add(new D.ImportedLocation { ImportId = history.Id, SourceWarehouseId = alias.Key, AreaId = alias.Value });
        db.SaveChanges();
        Verify(archive, ReadArchive(db));
        tx.Commit();
    }

    public static void Verify(InventoryArchive expected, InventoryArchive actual)
    {
        if (!expected.Products.Select(p => p.Id).Order().SequenceEqual(actual.Products.Select(p => p.Id).Order()) || !expected.Inventory.Employees.Select(e => e.Id).Order().SequenceEqual(actual.Inventory.Employees.Select(e => e.Id).Order()) || expected.Inventory.Movements.Count != actual.Inventory.Movements.Count || expected.Inventory.Counts.Count != actual.Inventory.Counts.Count || expected.Accounts.Count != actual.Accounts.Count)
            throw new InvalidDataException("Aktarılan kayıt adetleri/kimlikleri uyuşmuyor.");
        if (expected.LocationAliases is not null && (actual.LocationAliases is null || expected.LocationAliases.Count != actual.LocationAliases.Count || expected.LocationAliases.Any(a => actual.LocationAliases.GetValueOrDefault(a.Key) != a.Value)))
            throw new InvalidDataException("Eski alan kimliği eşlemesi uyuşmuyor.");
        foreach (var person in expected.Inventory.Employees)
        {
            var saved = actual.Inventory.Employees.Single(e => e.Id == person.Id);
            if (person.Name != saved.Name || person.Role != saved.Role || person.Phone != saved.Phone || person.HireDate != saved.HireDate || person.IsActive != saved.IsActive || !person.AssignedBranches.Order().SequenceEqual(saved.AssignedBranches.Order()))
                throw new InvalidDataException("Personel bilgileri aktarımda uyuşmuyor.");
        }

        foreach (var product in expected.Products)
        {
            var saved = actual.Products.Single(p => p.Id == product.Id);
            if (product.Name != saved.Name || product.Category != saved.Category || product.Unit != saved.Unit || product.MinimumStock != saved.MinimumStock || product.Code != saved.Code || product.SourceCell != saved.SourceCell)
                throw new InvalidDataException("Ürün bilgileri aktarımda uyuşmuyor.");
        }

        foreach (var p in expected.Products)
            foreach (var area in expected.Inventory.Warehouses)
                if (expected.Inventory.Balance(p.Id, area.Id) != actual.Inventory.Balance(p.Id, area.Id))
                    throw new InvalidDataException("Aktarımda stok bakiyesi uyuşmuyor.");
        foreach (var count in expected.Inventory.Counts)
        {
            var saved = actual.Inventory.Counts.Single(c => c.Id == count.Id);
            if (count.Status != saved.Status || count.BranchName != saved.BranchName || count.Operator != saved.Operator || count.Reviewer != saved.Reviewer || count.ReviewNote != saved.ReviewNote || count.Date != saved.Date || count.ReviewedAt != saved.ReviewedAt || count.Lines.Count != saved.Lines.Count)
                throw new InvalidDataException("Sayım geçmişi aktarımda uyuşmuyor.");
            foreach (var line in count.Lines)
            {
                var l = saved.Lines.Single(l => l.ProductId == line.ProductId);
                if (line.Expected != l.Expected || line.Counted != l.Counted || line.BarExpected != l.BarExpected || line.BarCounted != l.BarCounted || line.DepotExpected != l.DepotExpected || line.DepotCounted != l.DepotCounted)
                    throw new InvalidDataException("Sayım satırları aktarımda uyuşmuyor.");
            }
        }

        foreach (var a in expected.Accounts)
        {
            var saved = actual.Accounts.Single(x => x.EmployeeId == a.EmployeeId);
            if (a.Hash != saved.Hash || a.Salt != saved.Salt || a.Username != saved.Username)
                throw new InvalidDataException("Aktarımda hesap bilgisi uyuşmuyor.");
        }
    }

    public void Restore(InventoryArchive archive)
    {
        using var db = factory();
        using var tx = db.Database.BeginTransaction(IsolationLevel.Serializable);
        // Only the application's cafestock schema is affected; dbo is never targeted.
        db.Set<D.ImportedLocation>().ExecuteDelete();
        db.Set<D.StockCountLine>().ExecuteDelete();
        db.Set<D.TransferLine>().ExecuteDelete();
        db.StockMovements.ExecuteDelete();
        db.Set<D.StockCount>().ExecuteDelete();
        db.Set<D.Transfer>().ExecuteDelete();
        db.Set<D.UserAccount>().ExecuteDelete();
        db.Set<D.EmployeeBranch>().ExecuteDelete();
        db.Set<D.Employee>().ExecuteDelete();
        db.StockAreas.ExecuteDelete();
        db.Set<D.Branch>().ExecuteDelete();
        db.Products.ExecuteDelete();
        db.Set<D.Category>().ExecuteDelete();
        db.Set<D.Unit>().ExecuteDelete();
        db.Set<D.Role>().ExecuteDelete();
        db.Set<D.BusinessSettings>().ExecuteDelete();
        db.Add(new D.BusinessSettings { Id = Stable("settings"), BusinessName = archive.Inventory.Settings.BusinessName, OperatorName = archive.Inventory.Settings.OperatorName });
        WriteProducts(db, archive.Products);
        WriteInventory(db, archive.Inventory);
        WriteAccounts(db, archive.Accounts);
        if (archive.LocationAliases is not null)
        {
            var history = db.Set<D.ImportHistory>().FirstOrDefault();
            if (history is null)
            {
                history = new D.ImportHistory
                {
                    SourceFingerprint = Guid.NewGuid().ToString("N"),
                    AppliedAt = DateTime.Now
                };
                db.Add(history);
            }

            foreach (var alias in archive.LocationAliases)
                db.Add(new D.ImportedLocation { ImportId = history.Id, SourceWarehouseId = alias.Key, AreaId = alias.Value });
        }

        db.SaveChanges();
        Verify(archive, ReadArchive(db));
        tx.Commit();
    }
}
