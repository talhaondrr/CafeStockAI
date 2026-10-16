namespace CafeStockAI.Desktop;
public partial class FrmWarehouses : Form, IInventoryPage
{
    private sealed class BranchView
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Managers { get; set; } = "";
        public int People { get; set; }
    }
    private Guid? selected;
    private InventoryData data = new();
    public FrmWarehouses()
    {
        InitializeComponent(); QuantityText.Setup(this);
        grid.SelectionChanged += (_,_) => ShowBranch();
        searchInput.TextChanged += (_,_) => Filter(); Clear();
    }
    public void RefreshData() => PageUi.Run(() => { data = Access.View(); saveButton.Enabled=Access.Global; Filter(); });
    private List<Employee> Members(string name) => data.Employees.Where(p => p.AssignedBranches.Contains(name)).OrderBy(p => p.Role == "Şube Müdürü" ? 0 : 1).ThenBy(p => p.Name).ToList();
    private string Managers(string name)
    {
        var names = Members(name).Where(p => p.Role == "Şube Müdürü").Select(p => p.Name).ToList(); return names.Count == 0 ? "Atanmamış" : string.Join(", ",names);
    }
    private void Filter()
    {
        PageUi.Bind(grid,data.Branches.Where(b => b.Name.Contains(searchInput.Text.Trim(),StringComparison.CurrentCultureIgnoreCase)).Select(b => new BranchView { Id = b.Id, Name = b.Name, Managers = Managers(b.Name), People = Members(b.Name).Count }).ToList());
        Clear(); statusLabel.Text = $"{grid.Rows.Count} şube. Müdür ve personel detayları için bir şube seçin.";
    }
    private void ShowBranch()
    {
        if (grid.SelectedRows.Count == 0 || grid.SelectedRows[0].DataBoundItem is not BranchView row) return;
        selected = row.Id; nameInput.Text = row.Name; saveButton.Text = "Güncelle"; deleteButton.Enabled = Access.Global;
        detailTitle.Text = row.Name + " şubesi";
        var people = Members(row.Name);
        detailSummary.Text = $"Şube müdürü: {Managers(row.Name)}\nAtanan personel sayısı: {people.Count}";
        PageUi.Bind(membersGrid,people);
    }
    private void Clear()
    {
        selected = null; nameInput.Clear(); saveButton.Text = "Kaydet"; deleteButton.Enabled = false; grid.ClearSelection();
        detailTitle.Text = "Şube personelleri"; detailSummary.Text = "Detaylarını görmek için soldan bir şube seçin."; membersGrid.DataSource = null;
    }
    private void New_Click(object? sender,EventArgs e) { Clear(); nameInput.Focus(); }
    private void Save_Click(object? sender,EventArgs e) => PageUi.Run(() => { BranchRegistry.SaveNamedBranch(selected,nameInput.Text); RefreshData(); statusLabel.Text = "Şube kaydedildi."; });
    private void Delete_Click(object? sender,EventArgs e) => PageUi.Run(() =>
    {
        if (selected is not Guid id || MessageBox.Show("Seçili şube silinsin mi?",Text,MessageBoxButtons.YesNo) != DialogResult.Yes) return;
        BranchRegistry.Delete(id); RefreshData();
    });
}
