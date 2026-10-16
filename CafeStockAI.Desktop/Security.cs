using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
namespace CafeStockAI.Desktop;
public sealed class UserAccount
{
 public Guid EmployeeId { get; set; }
 public string Username { get; set; } = "";
 public string Salt { get; set; } = "";
 public string Hash { get; set; } = "";
 public bool Enabled { get; set; } = true;
}
internal sealed class DataLock : IDisposable
{
 private readonly Mutex mutex;
 private DataLock() { mutex = new Mutex(false, "Local\\CafeStockAI.Data.v3"); try { if (!mutex.WaitOne(TimeSpan.FromSeconds(15))) throw new IOException("Başka bir işlem kayıt yapıyor. Yeniden deneyin."); } catch (AbandonedMutexException) { } }
 public static DataLock Enter() => new();
 public void Dispose() { mutex.ReleaseMutex(); mutex.Dispose(); }
}
internal static class Accounts
{
 private static string FilePath => Path.Combine(InventoryStore.DirectoryPath,"accounts.json");
 internal static string Key(string s) => s.Trim().Normalize(NormalizationForm.FormKC).ToUpper(new CultureInfo("tr-TR"));
 public static List<UserAccount> Load() => File.Exists(FilePath) ? JsonSerializer.Deserialize<List<UserAccount>>(File.ReadAllText(FilePath)) ?? throw new InvalidDataException("Kullanıcı dosyası geçersiz.") : new();
 private static void Save(List<UserAccount> users) { Directory.CreateDirectory(InventoryStore.DirectoryPath); File.WriteAllText(FilePath+".tmp", JsonSerializer.Serialize(users)); File.Move(FilePath+".tmp",FilePath,true); }
 private static void SetPassword(UserAccount user,string password) { user.Salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16)); user.Hash = Convert.ToBase64String(Rfc2898DeriveBytes.Pbkdf2(password,Convert.FromBase64String(user.Salt),210000,HashAlgorithmName.SHA256,32)); }
 public static void Sync()
 {
  using var transaction = DataLock.Enter(); var users = Load(); var staff = InventoryStore.Load().Employees;
  foreach (var person in staff)
  {
   var user = users.FirstOrDefault(u=>u.EmployeeId==person.Id);
   if (user is null) { user = new UserAccount {EmployeeId=person.Id}; SetPassword(user,"123456"); users.Add(user); }
   string name = person.Name.Trim(); if (staff.Count(e=>Key(e.Name)==Key(name))>1) name += " " + person.Id.ToString("N")[..6];
   user.Username = name;
  }
  foreach (var user in users.Where(u=>!staff.Any(e=>e.Id==u.EmployeeId))) user.Enabled=false;
  Save(users);
 }
 public static bool Login(string username,string password)
 {
  Access.EmployeeId=null; Sync(); var user=Load().FirstOrDefault(u=>u.Enabled && Key(u.Username)==Key(username)); if(user is null) return false;
  bool valid=CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(user.Hash), Rfc2898DeriveBytes.Pbkdf2(password,Convert.FromBase64String(user.Salt),210000,HashAlgorithmName.SHA256,32));
  if(valid) Access.EmployeeId=user.EmployeeId; return valid;
 }
 public static void EnsureManager(InventoryData data)
 {
  if(InventoryStore.TestDirectory is not null && Access.EmployeeId is null)return;var users=Load();if(!data.Employees.Any(e=>e.Role is "İşletme Sahibi" or "Genel Müdür" && (!users.Any(u=>u.EmployeeId==e.Id)||users.Any(u=>u.EmployeeId==e.Id&&u.Enabled))))throw new InvalidOperationException("En az bir aktif genel müdür veya işletme sahibi bulunmalı.");
 }
 public static void Enable(Guid id,bool enabled)
 {
  Access.RequireGlobal(); if(id==Access.EmployeeId&&!enabled)throw new InvalidOperationException("Kendi hesabınızı kapatamazsınız.");using var transaction=DataLock.Enter();var users=Load();if(!enabled&&!users.Any(u=>u.EmployeeId!=id&&u.Enabled&&InventoryStore.Load().Employees.Any(e=>e.Id==u.EmployeeId&&e.Role is "İşletme Sahibi" or "Genel Müdür")))throw new InvalidOperationException("Son yönetici hesabı kapatılamaz.");users.Single(u=>u.EmployeeId==id).Enabled=enabled;Save(users);
 }
 public static void Change(Guid id,string password,bool? enabled=null)
 {
  if(id!=Access.EmployeeId) Access.RequireGlobal();
  if(password.Length<6) throw new InvalidOperationException("Şifre en az 6 karakter olmalı.");
  using var transaction=DataLock.Enter(); var users=Load(); var user=users.Single(u=>u.EmployeeId==id); SetPassword(user,password); if(enabled.HasValue) { Access.RequireGlobal(); if(id==Access.EmployeeId && !enabled.Value) throw new InvalidOperationException("Kendi hesabınızı kapatamazsınız."); user.Enabled=enabled.Value; } Save(users);
 }
}
internal static class Access
{
 internal static Guid? EmployeeId;
 public static Employee? Current => EmployeeId is Guid id && Accounts.Load().Any(u=>u.EmployeeId==id&&u.Enabled) ? InventoryStore.Load().Employees.FirstOrDefault(e=>e.Id==id) : null;
 private static bool Test => InventoryStore.TestDirectory is not null && EmployeeId is null;
 public static bool Global => Test || Current?.Role is "İşletme Sahibi" or "Genel Müdür";
 public static bool Approver => Global || Current?.Role is "Şube Müdürü" or "Bölge Müdürü" or "Süpervizör";
 public static void RequireGlobal() { if(!Global) throw new UnauthorizedAccessException("Bu işlem için genel yönetim yetkisi gerekiyor."); }
 public static void RequireBranch(string branch) { if(!Global && Current?.AssignedBranches.Contains(branch)!=true) throw new UnauthorizedAccessException("Bu şubeye erişim yetkiniz yok."); }
 public static void RequireMovement(string branch) { RequireBranch(branch); if(!Approver && Current?.Role!="Depo Sorumlusu") throw new UnauthorizedAccessException("Stok hareketi düzenleme yetkiniz yok."); }
 public static InventoryData View()
 {
  var data=InventoryStore.Load(); if(Global) return data;
  var person=Current ?? throw new UnauthorizedAccessException("Önce sisteme giriş yapınız."); var allowed=person.AssignedBranches.ToHashSet();
  data.Branches.RemoveAll(b=>!allowed.Contains(b.Name)); data.Warehouses.RemoveAll(w=>!allowed.Contains(w.BranchName)); var areas=data.Warehouses.Select(w=>w.Id).ToHashSet(); data.Movements.RemoveAll(m=>!areas.Contains(m.WarehouseId)); data.Counts.RemoveAll(c=>!allowed.Contains(c.BranchName));
  data.Employees.RemoveAll(e=>!e.AssignedBranches.Any(allowed.Contains)); foreach(var e in data.Employees) { e.Branches=e.AssignedBranches.Where(allowed.Contains).ToList(); e.BranchName=e.Branches.FirstOrDefault()??""; }
  data.Settings.OperatorName=person.Name; return data;
 }
}
