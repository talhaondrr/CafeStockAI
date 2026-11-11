using CafeStockAI.Application.Models;

namespace CafeStockAI.Application.Mobile;
public sealed partial class MobileWorkspace
{
    public void SaveStaff(Guid id, StaffEdit edit)
    {
        var a = Snapshot(id);
        RequireGlobal(Person(a, id));
        string name = Text(edit.Name);
        if (name.Length == 0 || !Roles.Contains(edit.Role))
            throw new InvalidOperationException("Personel adı ve geçerli görev girin.");
        if (a.Inventory.Employees.Any(e => e.Id != edit.Id && AccountCredentials.Key(e.Name) == AccountCredentials.Key(name)))
            throw new InvalidOperationException("Bu isimde personel var.");
        string digits = new string (Text(edit.Phone).Where(char.IsDigit).ToArray());
        if (digits.Length == 12 && digits.StartsWith("90"))
            digits = "0" + digits[2..];
        if (digits.Length == 10)
            digits = "0" + digits;
        if (digits.Length != 0 && (digits.Length != 11 || digits[0] != '0'))
            throw new InvalidOperationException("11 haneli telefon girin.");
        var old = a.Inventory.Employees.SingleOrDefault(e => e.Id == edit.Id);
        if (edit.Id.HasValue && old is null)
            throw new InvalidOperationException("Personel bulunamadı.");
        var row = old ?? new Employee();
        row.Name = name;
        row.Role = edit.Role;
        row.HireDate = edit.HireDate;
        row.Phone = digits.Length == 11 ? $"{digits[..4]} {digits[4..7]} {digits[7..9]} {digits[9..]}" : "";
        row.Branches = ["Elazığ"];
        row.BranchName = "Elazığ";
        if (old is null)
            a.Inventory.Employees.Add(row);
        if (!a.Inventory.Employees.Any(e => e.IsActive && Global(e) && a.Accounts.Any(u => u.EmployeeId == e.Id && u.Enabled)))
            throw new InvalidOperationException("Son aktif yönetici görevi değiştirilemez.");
        // Save staff and account changes together via an archive transaction.
        var account = a.Accounts.SingleOrDefault(u => u.EmployeeId == row.Id);
        if (account is null)
        {
            account = new UserAccount
            {
                EmployeeId = row.Id
            };
            AccountCredentials.Set(account, "123456");
            a.Accounts.Add(account);
        }

        if (string.IsNullOrWhiteSpace(account.Username))
            account.Username = name.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].ToLower(System.Globalization.CultureInfo.GetCultureInfo("tr-TR"));
        Checked.SaveStaffAndAccounts(a.Inventory, a.Accounts);
    }

    public void RemoveStaff(Guid id, Guid employeeId)
    {
        var a = Snapshot(id);
        RequireGlobal(Person(a, id));
        if (id == employeeId)
            throw new InvalidOperationException("Kendi personel kaydınızı kaldıramazsınız.");
        var target = a.Inventory.Employees.SingleOrDefault(e => e.Id == employeeId) ?? throw new InvalidOperationException("Personel bulunamadı.");
        target.IsActive = false;
        var account = a.Accounts.FirstOrDefault(u => u.EmployeeId == employeeId);
        if (account is not null)
            account.Enabled = false;
        if (!a.Inventory.Employees.Any(e => e.IsActive && Global(e) && a.Accounts.Any(u => u.EmployeeId == e.Id && u.Enabled)))
            throw new InvalidOperationException("Son yönetici kaldırılamaz.");
        Checked.SaveStaffAndAccounts(a.Inventory, a.Accounts);
    }

    public void ChangeAccount(Guid id, AccountEdit edit)
    {
        var a = Snapshot(id);
        RequireGlobal(Person(a, id));
        var target = a.Accounts.SingleOrDefault(u => u.EmployeeId == edit.EmployeeId) ?? throw new InvalidOperationException("Hesap bulunamadı.");
        if (edit.EmployeeId == id && !edit.Enabled)
            throw new InvalidOperationException("Kendi hesabınızı kapatamazsınız.");
        target.Enabled = edit.Enabled;
        if (!string.IsNullOrEmpty(edit.Password))
            AccountCredentials.Set(target, edit.Password);
        if (!a.Inventory.Employees.Any(e => e.IsActive && Global(e) && a.Accounts.Any(u => u.EmployeeId == e.Id && u.Enabled)))
            throw new InvalidOperationException("Son yönetici hesabı kapatılamaz.");
        Checked.SaveAccountsChecked(a.Accounts, a.Inventory.SqlVersion);
    }
}
