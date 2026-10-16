namespace CafeStockAI.Desktop;
internal static class SystemCheck
{
    internal static void Run(string directory)
    {
        Directory.CreateDirectory(directory);
        InventoryStore.TestDirectory = Path.Combine(directory, "data-" + Guid.NewGuid().ToString("N"));
        PageUi.Strict = true;
        var checks = new List<string>();
        static void Assert(bool condition, string message) { if (!condition) throw new InvalidOperationException("Kontrol başarısız: " + message); }
        static void MustReject(Action action, string message)
        {
            bool rejected = false;
            try { action(); } catch (InvalidOperationException) { rejected = true; }
            Assert(rejected, message);
        }
        try
        {
            Assert(PhoneNumber.Format("05321234567") == "0 (532) 123 45 67", "telefon 0 öneki");
            Assert(PhoneNumber.Format("+90 532 123 45 67") == "0 (532) 123 45 67", "telefon +90");
            Assert(PhoneNumber.Format("00905321234567") == "0 (532) 123 45 67", "telefon 0090");
            Assert(PhoneNumber.Format("5321234567") == "0 (532) 123 45 67", "telefon 10 hane");
            Assert(PhoneNumber.Format("02121234567") == "0 (212) 123 45 67", "sabit telefon");
            Assert(PhoneNumber.Format("0 (532) 123 45 67") == "0 (532) 123 45 67", "telefon tekrar biçimleme");
            Assert(PhoneNumber.Format("") == "", "boş telefon");
            MustReject(() => PhoneNumber.Format("0532123"), "eksik telefon engeli");
            MustReject(() => PhoneNumber.Format("05321234567abc"), "telefon harf engeli");
            checks.Add("Telefon biçimleri (+90, 0090, 0, 10 hane), sabit hat ve geçersiz giriş kontrolleri geçti.");
            using (var login = new FrmLogin())
            {
                login.ShowInTaskbar = false; login.Opacity = 0; login.Show();
                ((Button)login.Controls.Find("btnLogin", true).Single()).PerformClick();
                Assert(((Label)login.Controls.Find("errorLabel",true).Single()).Text.Length > 0, "giriş zorunlu alan kontrolü");
                var password = (TextBox)login.Controls.Find("txtPassword",true).Single();
                var toggle = (CheckBox)login.Controls.Find("showPassword",true).Single(); toggle.Checked = true;
                Assert(!password.UseSystemPasswordChar, "şifre görünürlüğü"); toggle.Checked = false;
                ((Label)login.Controls.Find("errorLabel",true).Single()).Text = "";
                using var bitmap = new Bitmap(login.Width,login.Height); login.DrawToBitmap(bitmap,new Rectangle(Point.Empty,login.Size)); bitmap.Save(Path.Combine(directory,"FrmLogin.png")); login.Close();
            }
            checks.Add("Giriş ekranı, boş kullanıcı kontrolü ve şifre görünürlüğü geçti.");
            var catalog = ProductStore.ReadCatalog();
            Assert(catalog.Count == 99 && catalog.Select(p => p.Category).Distinct().Count() == 16, "çizelge ürün ve kategori sayısı");
            Assert(catalog.Select(p => p.Id).Distinct().Count() == catalog.Count && catalog.Select(p => p.Code).Distinct().Count() == catalog.Count, "katalog benzersiz kimlikler");
            Assert(catalog.Count(p => p.Name == "KARAMEL") == 2, "farklı kategorilerde aynı ürün adı");
            foreach (var item in catalog) Assert(ProductImages.Get(item).Width == 96, "ürün görseli " + item.Code);
            checks.Add("99 ürünün temsili görselleri yüklendi.");
            var merged = ProductStore.MergeCatalog(new List<Product>());
            Assert(ProductStore.MergeCatalog(merged).Count == merged.Count, "tekrar aktarımda çift kayıt engeli");
            var existingProduct = new Product { Code = "MBC-001", Name = "Önceden kayıtlı ürün", Unit = "Kg" };
            var preserved = ProductStore.MergeCatalog(new List<Product> { existingProduct });
            Assert(preserved.Contains(existingProduct) && preserved.Select(p => p.Code).Distinct().Count() == preserved.Count, "önceki kayıt ve kod çakışması korunması");
            ProductStore.Save(merged);
            Assert(ProductStore.Load().Count == 99, "katalog kayıtlarının yeniden okunması");
            using (var catalogPage = new FrmProducts())
            {
                catalogPage.ShowInTaskbar = false; catalogPage.Opacity = 0; catalogPage.Show(); catalogPage.RefreshData();
                using var bitmap = new Bitmap(catalogPage.Width,catalogPage.Height); catalogPage.DrawToBitmap(bitmap,new Rectangle(Point.Empty,catalogPage.Size)); bitmap.Save(Path.Combine(directory,"FrmProducts-catalog.png"));
                var categoryFilter = (ComboBox)catalogPage.Controls.Find("categoryFilter",true).Single(); categoryFilter.SelectedItem = "SÜT ÇEŞİTLERİ";
                var productGrid = (DataGridView)catalogPage.Controls.Find("grid",true).Single(); Assert(productGrid.Rows.Count == 3,"ürün yönetiminde kategori filtresi"); categoryFilter.SelectedItem = "Tüm kategoriler";
                catalogPage.Close();
            }
            File.Delete(Path.Combine(InventoryStore.DirectoryPath,"products.json"));
            checks.Add("99 ürün, 16 kategori, tekrarsız aktarım, mevcut kayıtların korunması ve ürün ekranı geçti.");
            // Fresh test data lives outside the user's actual product and inventory files.
            Assert(ProductStore.Load().Count == 0 && InventoryStore.Load().Warehouses.Count == 0, "boş başlangıç");
            using (var emptyDashboard = new FrmDashboard()) { emptyDashboard.CheckAllPages(); }
            checks.Add("Boş kayıtlarla bütün sayfalar açıldı.");
            var coffee = new Product { Code = "KHV-001", Name = "Kahve", Category = "ANA MALZEME", Unit = "Kg", MinimumStock = 5 };
            var milk = new Product { Code = "SUT-001", Name = "Süt", Category = "SÜT ÇEŞİTLERİ", Unit = "Litre", MinimumStock = 3 };
            ProductStore.Save(new List<Product> { coffee, milk });
            var warehouse = new Warehouse { Code = "D-001", Name = "Ana Depo", Location = "Zemin kat", BranchName = "İnegöl", StockArea = "Depo" };
            var second = new Warehouse { Code = "D-002", Name = "Servis Deposu", Location = "Servis alanı", BranchName = "Ankara", StockArea = "Depo" };
            var data = new InventoryData(); data.Warehouses.AddRange(new[] { warehouse, second });
            data.Employees.Add(new Employee { Name = "Örnek Personel", Role = "Depo Sorumlusu", Phone = "0 (532) 123 45 67", BranchName = "İnegöl" });
            data.Settings.BusinessName = "Örnek Kafe"; data.Settings.OperatorName = "Sayım görevlisi"; InventoryStore.Save(data);
            InventoryService.AddMovement(coffee.Id, warehouse.Id, 12.5M, "Giriş", "İlk teslimat");
            InventoryService.AddMovement(coffee.Id, warehouse.Id, 2M, "Çıkış", "Servise çıkış");
            InventoryService.AddMovement(milk.Id, warehouse.Id, 10M, "Giriş", "Günlük teslimat");
            Assert(InventoryStore.Load().Balance(coffee.Id, warehouse.Id) == 10.5M, "giriş çıkış bakiyesi");
            MustReject(() => InventoryService.AddMovement(coffee.Id, warehouse.Id, 11M, "Çıkış", ""), "negatif stok engeli");
            MustReject(() => InventoryService.AddMovement(coffee.Id, warehouse.Id, 0, "Giriş", ""), "sıfır hareket engeli");
            MustReject(() => InventoryService.AddMovement(coffee.Id, Guid.NewGuid(), 1, "Giriş", ""), "geçersiz depo engeli");
            checks.Add("Giriş/çıkış, ondalık miktar ve yetersiz stok kontrolleri geçti.");
            var lines = new List<CountLine> { new() { ProductId = coffee.Id, Expected = 10.5M, Counted = 9M }, new() { ProductId = milk.Id, Expected = 10M, Counted = 0M } };
            InventoryService.CompleteCount(warehouse.Id, lines, "Sayım görevlisi");
            var after = InventoryStore.Load();
            Assert(after.Balance(coffee.Id, warehouse.Id) == 9M && after.Balance(milk.Id, warehouse.Id) == 0M, "sayım düzeltmeleri");
            Assert(after.Counts.Count == 1 && after.Movements.Count(m => m.Type == "Sayım düzeltmesi") == 2, "sayım geçmişi");
            MustReject(() => InventoryService.CompleteCount(warehouse.Id, lines, "Sayım görevlisi"), "eski stokla sayım engeli");
            Assert(InventoryStore.Load().Counts.Count == 1, "reddedilen sayım kaydedilmedi");
            checks.Add("Sayım farkı, sıfır sayımı, kayıtların yeniden okunması ve eski stokla sayım engeli geçti.");
            Assert(new Employee { BranchName = "İnegöl" }.AssignedBranches.SequenceEqual(new[] { "İnegöl" }), "eski tek şube personel kaydı");
            var branchData = InventoryStore.Load();
            var legacyExtra = new Warehouse { Name = "Eski ikinci depo", Code = "D-003", BranchName = "İnegöl" }; branchData.Warehouses.Add(legacyExtra); InventoryStore.Save(branchData);
            InventoryService.AddMovement(coffee.Id,legacyExtra.Id,5,"Giriş","");
            InventoryService.AddMovement(coffee.Id,second.Id,4,"Giriş","");
            var branchLines = new List<BranchCountLine> { new() { ProductId = coffee.Id, ExpectedBar = 0, ExpectedDepot = 14, CountedBar = 2, CountedDepot = 3 }, new() { ProductId = milk.Id, ExpectedBar = 0, ExpectedDepot = 0, CountedBar = 1, CountedDepot = 3 } };
            BranchStock.Complete("İnegöl",branchLines,"Şube Müdürü");
            var branchResult = InventoryStore.Load();
            Assert(BranchStock.Balance(branchResult,coffee.Id,"İnegöl","Bar") == 2 && BranchStock.Balance(branchResult,coffee.Id,"İnegöl","Depo") == 3, "şube Bar/Depo ayrı bakiye");
            Assert(branchResult.Warehouses.Where(w => w.BranchName == "İnegöl").All(w => branchResult.Balance(coffee.Id,w.Id) >= 0), "eski birden fazla depoda negatif stok oluşturulmadı");
            Assert(BranchStock.Balance(branchResult,coffee.Id,"Ankara","Depo") == 4, "başka şubenin stoğu korunması");
            Assert(branchResult.Counts.Last().Lines.First().BarCounted == 2 && branchResult.Counts.Last().Lines.First().DepotCounted == 3, "sayımda alan miktarları kalıcı kayıt");
            MustReject(() => BranchStock.Complete("İnegöl",branchLines,"Şube Müdürü"), "değişmiş alan stokları sayım engeli");
            BranchStock.Complete("Elazığ",new List<BranchCountLine> { new() { ProductId = coffee.Id }, new() { ProductId = milk.Id } },"Sayım görevlisi");
            Assert(InventoryStore.Load().Warehouses.Count(w => w.BranchName == "Elazığ") == 2,"önceden depo tanımı olmadan şube sayımı");
            checks.Add("Şube bazında Bar/Depo sayımı, ayrı stoklar, eski depoların korunması ve ön tanımsız şube sayımı geçti.");
            BranchRegistry.SaveBranch(null,"SUB-TEST","Geçici Şube","");
            var temporaryBranch = InventoryStore.Load().Branches.Single(b => b.Name == "Geçici Şube"); BranchRegistry.Delete(temporaryBranch.Id);
            Assert(!BranchStock.AllNames(InventoryStore.Load()).Contains("Geçici Şube"),"silinen şube geri eklenmedi");
            var inegolBranch = InventoryStore.Load().Branches.Single(b => b.Name == "İnegöl");
            BranchRegistry.SaveBranch(inegolBranch.Id,inegolBranch.Code,"İnegöl Merkez","Yeni adres");
            Assert(BranchStock.Balance(InventoryStore.Load(),coffee.Id,"İnegöl Merkez","Bar") == 2,"şube adı değişince stok korundu");
            Assert(InventoryStore.Load().Employees.Any(p => p.AssignedBranches.Contains("İnegöl Merkez")),"şube adı değişince personel ilişkisi güncellendi");
            MustReject(() => BranchRegistry.Delete(inegolBranch.Id),"geçmişi olan şube silme engeli");
            MustReject(() => BranchRegistry.SaveBranch(null,"SUB-NEW","Ankara",""),"tekrar şube adı engeli");
            BranchRegistry.SaveBranch(inegolBranch.Id,inegolBranch.Code,"İnegöl","");
            BranchMovements.Add(coffee.Id,"İnegöl","Bar",1,"Giriş",""); BranchMovements.Add(coffee.Id,"İnegöl","Bar",1,"Çıkış","");
            Assert(BranchStock.Balance(InventoryStore.Load(),coffee.Id,"İnegöl","Bar") == 2,"şube alanı giriş çıkış");
            MustReject(() => BranchMovements.Add(coffee.Id,"İnegöl","Bar",100,"Çıkış",""),"şube alanında yetersiz stok engeli");
            checks.Add("Şube ekleme/silme, tekrar ad engeli, ad değişiminde geçmiş ve personel korunması, alan bazında hareket kontrolleri geçti.");
            foreach (var form in new Form[] { new FrmProducts(), new FrmWarehouses(), new FrmStockMovements(), new FrmStockCount(), new FrmEmployees(), new FrmReports(), new FrmAnalysis(), new FrmSettings() })
            {
                using (form)
                {
                    form.ShowInTaskbar = false; form.Opacity = 0; form.Show(); ((IInventoryPage)form).RefreshData();
                    if (form is FrmWarehouses)
                    {

                        ((TextBox)form.Controls.Find("nameInput",true).Single()).Text = "İzmir";

                        ((Button)form.Controls.Find("saveButton",true).Single()).PerformClick();
                        Assert(InventoryStore.Load().Branches.Any(b => b.Name == "İzmir"),"şube ekranından kayıt");
                        Assert(form.Controls.Find("branchInput",true).Length == 0 && form.Controls.Find("codeInput",true).Length == 0,"şube yönetiminde gereksiz kod/depo seçimi yok");
                    }
                    if (form is FrmStockMovements)
                    {
                        Assert(((ComboBox)form.Controls.Find("branchInput",true).Single()).Items.Contains("İzmir"),"yeni şube hareket listesinde");
                        Assert(((ComboBox)form.Controls.Find("areaInput",true).Single()).Items.Count == 2,"yalnızca Bar ve Depo alanları");
                    }
                    if (form is FrmAnalysis || form is FrmReports)
                        Assert(((ComboBox)form.Controls.Find("warehouseInput",true).Single()).Items.Contains("İzmir"),"yeni şube analiz/rapor listesinde");
                    if (form is FrmEmployees)
                    {
                        ((TextBox)form.Controls.Find("nameInput",true).Single()).Text = "Deneme Personeli";
                        ((ComboBox)form.Controls.Find("roleInput",true).Single()).SelectedItem = "Şube Müdürü";
                        var branches = (CheckedListBox)form.Controls.Find("branchInput",true).Single(); Assert(branches.Items.Contains("İzmir"),"yeni şube personel listesinde"); branches.SetItemChecked(branches.Items.IndexOf("Ankara"),true); branches.SetItemChecked(branches.Items.IndexOf("Bursa"),true);
                        ((TextBox)form.Controls.Find("phoneInput",true).Single()).Text = "+90 533 123 45 67";
                        var hireDate = (DateTimePicker)form.Controls.Find("hireDateInput",true).Single(); hireDate.Value = new DateTime(2024,4,15); hireDate.Checked = true;
                        ((Button)form.Controls.Find("saveButton",true).Single()).PerformClick();
                                                Assert(InventoryStore.Load().Employees.Single(p => p.Name == "Deneme Personeli").Phone == "0 (533) 123 45 67", "personel telefonu kalıcı kayıt");
                        Assert(InventoryStore.Load().Employees.Single(p => p.Name == "Deneme Personeli").AssignedBranches.SequenceEqual(new[] { "Ankara", "Bursa" }),"çoklu personel şubesi kalıcı kayıt");
                        var employeesGrid = (DataGridView)form.Controls.Find("grid",true).Single(); employeesGrid.Rows[0].Selected = true;
                        Assert(branches.CheckedItems.Count == 2,"personel düzenlemede şube tiklerinin yüklenmesi");
                        Assert(InventoryStore.Load().Employees.Single(p => p.Name == "Deneme Personeli").HireDate == new DateTime(2024,4,15) && hireDate.Checked,"işe giriş tarihi kayıt ve düzenleme");
                    }
                    if (form is FrmStockCount)
                    {
                        var combo = (ComboBox)form.Controls.Find("branchInput", true).Single(); Assert(combo.Items.Contains("İzmir"),"yeni şube sayım listesinde"); combo.SelectedItem = "İnegöl";
                        ((Button)form.Controls.Find("startButton", true).Single()).PerformClick();
                        var grid = (DataGridView)form.Controls.Find("grid", true).Single();
                        Assert(grid.Rows.Count == 2, "sayım satırları"); grid.Rows[0].Cells["colCounted"].Value = 8.25M; grid.Rows[0].Cells["colDepotCounted"].Value = 4M;
                        Assert(grid.Rows[0].Cells["colDifference"].Value is decimal, "sayım farkı sütunu");
                        var filter = (ComboBox)form.Controls.Find("categoryInput",true).Single(); filter.SelectedItem = "SÜT ÇEŞİTLERİ";
                        Assert(grid.Rows.Count == 1 && ((CountDraft)grid.Rows[0].DataBoundItem).Name == "Süt", "kategoriye göre sayım filtresi");
                        grid.Rows[0].Cells["colCounted"].Value = 1M; grid.Rows[0].Cells["colDepotCounted"].Value = 2M;
                        filter.SelectedItem = "Tüm kategoriler";
                        Assert(grid.Rows.Count == 2 && ((CountDraft)grid.Rows[0].DataBoundItem).BarCounted == 8.25M,"filtre değişiminde sayım değerleri korundu");
                        Assert(grid.Columns["imageColumn"].HeaderText == "" && grid.Columns["categoryColumn"].DataPropertyName == "Category","görsel başlığı boş ve ayrı kategori sütunu");
                    }
                    if (form is FrmReports)
                    {
                        var combo = (ComboBox)form.Controls.Find("reportInput", true).Single();
                        for (int i = 0; i < 3; i++)
                        {
                            combo.SelectedIndex = i; ((Button)form.Controls.Find("refreshButton", true).Single()).PerformClick();
                            var grid = (DataGridView)form.Controls.Find("grid", true).Single(); Assert(grid.Rows.Count > 0, "rapor satırları " + i);
                        }
                    }
                    using var bitmap = new Bitmap(form.Width, form.Height); form.DrawToBitmap(bitmap, new Rectangle(Point.Empty, form.Size));
                    bitmap.Save(Path.Combine(directory, form.GetType().Name + ".png")); form.Close();
                }
            }
            checks.Add("Sekiz sayfanın kayıtlarla açılması, üç rapor türü ve düzenlenebilir sayım tablosu geçti.");
            using (var branchPage = new FrmWarehouses())
            {
                branchPage.ShowInTaskbar = false; branchPage.Opacity = 0; branchPage.Show(); branchPage.RefreshData();
                var branchGrid = (DataGridView)branchPage.Controls.Find("grid",true).Single();
                foreach (DataGridViewRow row in branchGrid.Rows) if (Convert.ToString(row.Cells[0].Value) == "Ankara") row.Selected = true;
                var membersGrid = (DataGridView)branchPage.Controls.Find("membersGrid",true).Single();
                Assert(membersGrid.Rows.Count == 1 && ((Employee)membersGrid.Rows[0].DataBoundItem).Name == "Deneme Personeli","şubeye ait personel listesi");
                Assert(((Label)branchPage.Controls.Find("detailSummary",true).Single()).Text.Contains("Deneme Personeli"),"şube müdürü özeti");
                Assert(((Employee)membersGrid.Rows[0].DataBoundItem).HireDateDisplay == "15.04.2024","şube detayında işe giriş tarihi");
                using var bitmap = new Bitmap(branchPage.Width,branchPage.Height); branchPage.DrawToBitmap(bitmap,new Rectangle(Point.Empty,branchPage.Size)); bitmap.Save(Path.Combine(directory,"FrmWarehouses.png")); branchPage.Close();
            }
            checks.Add("Kategori filtresi sırasında sayım değerleri korundu; şube müdürü, personel ve işe giriş tarihi detayları geçti.");
            using (var dashboard = new FrmDashboard())
            {
                dashboard.Opacity = 0; dashboard.ShowInTaskbar = false; dashboard.Show(); dashboard.CheckAllPages(); dashboard.ShowProducts(); dashboard.ShowOverview();
                using var bitmap = new Bitmap(dashboard.Width, dashboard.Height); dashboard.DrawToBitmap(bitmap, new Rectangle(Point.Empty, dashboard.Size)); bitmap.Save(Path.Combine(directory, "FrmDashboard.png"));
                dashboard.Close();
            }
            checks.Add("Ana panelde bütün menü geçişleri ve genel bakışa dönüş geçti.");
            File.WriteAllLines(Path.Combine(directory, "result.txt"), checks.Prepend("BAŞARILI"));
        }
        finally { PageUi.Strict = false; InventoryStore.TestDirectory = null; }
    }
}
