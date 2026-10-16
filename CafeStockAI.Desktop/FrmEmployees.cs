namespace CafeStockAI.Desktop;
public partial class FrmEmployees : Form, IInventoryPage
{
    private Guid? selected;
    private List<Employee> rows = new();
    public FrmEmployees()
    {
        InitializeComponent(); QuantityText.Setup(this);
        grid.SelectionChanged += (_, _) => SelectEmployee();
        searchInput.TextChanged += (_, _) => Filter();
        phoneInput.Leave += (_, _) => FormatPhone();
        Clear();
    }
    public void RefreshData() => PageUi.Run(() => { var data = Access.View(); rows = data.Employees; saveButton.Enabled=Access.Global; branchInput.Items.Clear(); branchInput.Items.AddRange(BranchStock.AllNames(data).Cast<object>().ToArray()); Filter(); });
    private void Filter()
    {
        string query = searchInput.Text.Trim();
        string digits = new string(query.Where(char.IsDigit).ToArray());
        PageUi.Bind(grid, rows.Where(r => (r.Name + " " + r.Role + " " + r.BranchesDisplay + " " + r.Phone).Contains(query, StringComparison.CurrentCultureIgnoreCase)
            || (digits.Length > 0 && digits.Length == query.Count(c => !char.IsWhiteSpace(c)) && new string(r.Phone.Where(char.IsDigit).ToArray()).Contains(digits))).OrderBy(r => r.Name).ToList());
        Clear();
        statusLabel.Text = $"{grid.Rows.Count} personel listeleniyor";
        totalValue.Text = rows.Count.ToString();
        branchValue.Text = rows.SelectMany(r => r.AssignedBranches).Where(b => !string.IsNullOrWhiteSpace(b)).Distinct().Count().ToString();
    }
    private void SelectEmployee()
    {
        if (grid.SelectedRows.Count == 0 || grid.SelectedRows[0].DataBoundItem is not Employee row) return;
        errors.Clear(); selected = row.Id; for (int i = 0; i < branchInput.Items.Count; i++) branchInput.SetItemChecked(i, row.AssignedBranches.Contains(branchInput.Items[i].ToString()!)); nameInput.Text = row.Name;
        hireDateInput.Checked = row.HireDate.HasValue; hireDateInput.Value = row.HireDate ?? DateTime.Today; roleInput.SelectedItem = row.Role; phoneInput.Text = row.Phone;
        FormatPhone(); editorTitle.Text = "Personeli düzenle"; editorHint.Text = "Bilgileri güncelleyip değişiklikleri kaydedin.";
        saveButton.Text = "Değişiklikleri kaydet"; deleteButton.Enabled = Access.Global;
    }
    private bool FormatPhone()
    {
        try { phoneInput.Text = PhoneNumber.Format(phoneInput.Text); errors.SetError(phoneInput, ""); return true; }
        catch (InvalidOperationException ex) { errors.SetError(phoneInput, ex.Message); return false; }
    }
    private void Clear()
    {
        selected = null; errors.Clear(); hireDateInput.Checked = false; for (int i = 0; i < branchInput.Items.Count; i++) branchInput.SetItemChecked(i,false); nameInput.Clear(); phoneInput.Clear(); roleInput.SelectedIndex = -1;
        editorTitle.Text = "Yeni personel ekle"; editorHint.Text = "Ekip üyenizin bilgilerini aşağıya yazın.";
        saveButton.Text = "Personeli kaydet"; deleteButton.Enabled = false; grid.ClearSelection();
    }
    private void New_Click(object? sender, EventArgs e) { Clear(); nameInput.Focus(); }
    private void Save_Click(object? sender, EventArgs e) => PageUi.Run(() =>
    {
        Access.RequireGlobal();
        errors.Clear();
        bool valid = true;
        if (string.IsNullOrWhiteSpace(nameInput.Text)) { errors.SetError(nameInput,"Ad soyad giriniz."); valid = false; }
        if (roleInput.SelectedItem is null) { errors.SetError(roleInput,"Görev seçiniz."); valid = false; }
        if (branchInput.CheckedItems.Count == 0) { errors.SetError(branchInput,"En az bir şube seçiniz."); valid = false; }
        if (!FormatPhone()) valid = false;
        if (!valid) { statusLabel.Text = "İşaretli alanları kontrol edin."; return; }
        var data = InventoryStore.Load();
        if (branchInput.CheckedItems.Cast<string>().Any(n => !BranchStock.AllNames(data).Contains(n))) throw new InvalidOperationException("Şube listesi değişmiş. Ekranı yeniden açın.");
        var row = new Employee { Id = selected ?? Guid.NewGuid(), Name = nameInput.Text.Trim(), Role = roleInput.Text, Phone = phoneInput.Text, HireDate = hireDateInput.Checked ? hireDateInput.Value.Date : null, Branches = branchInput.CheckedItems.Cast<string>().ToList(), BranchName = branchInput.CheckedItems.Cast<string>().First() };
        bool updated = selected.HasValue;
        data.Employees.RemoveAll(r => r.Id == row.Id); data.Employees.Add(row); Accounts.EnsureManager(data); InventoryStore.Save(data); Accounts.Sync(); RefreshData();
        statusLabel.Text = updated ? "Personel bilgileri güncellendi." : "Yeni personel kaydedildi.";
    });
    private void Delete_Click(object? sender, EventArgs e) => PageUi.Run(() =>
    {
        Access.RequireGlobal();
        if (selected is not Guid id || MessageBox.Show("Seçili personel silinsin mi?", Text, MessageBoxButtons.YesNo) != DialogResult.Yes) return;
        var data = InventoryStore.Load(); data.Employees.RemoveAll(r => r.Id == id); Accounts.EnsureManager(data); InventoryStore.Save(data); Accounts.Sync(); RefreshData(); statusLabel.Text = "Personel silindi.";
    });
}
