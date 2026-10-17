# CafeStock AI — Elazığ

C# / .NET 8 Windows Forms dönem projesi. Güncel kapsam yalnız **Elazığ şubesi**, bütün mevcut personeller ve mevcut ürün kataloğudur. Personel görevleri korunur; tüm personelin şube ataması Elazığ yapılır. Çoklu şube ve şubeler arası transfer sonraya bırakılmıştır.

## Çalıştırma ve SQL'e geçiş

1. Visual Studio 2026'da CafeStockAI.Desktop.sln dosyasını açın; başlangıç projesi CafeStockAI.Desktop olsun.
2. Paketleri geri yükleyip uygulamayı çalıştırın.
3. İlk açılışta mevcut JSON kayıtları yedeklenir ve tek Elazığ düzenine alınır. Diğer şubelerin kayıtları yedekte korunur. Personeller/roller ve ürünler korunur.
4. Giriş ekranındaki **Elazığ • SQL bağlantısı** düğmesini açın. SSMS'de kullandığınız sunucu ve veritabanı adını girip **Bağlan ve Elazığ'a aktar** seçin. Windows oturumu kullanılır.
5. Başarılı aktarımın ardından kullanıcı adınızla giriş yapın. Mevcut şifreler korunur; yeni hesapların ilk şifresi 123456'dır.

Uygulama önce bağlantıyı doğrular; EF migration ile yalnız **cafestock** şemasını oluşturur. Mevcut **dbo** tabloları değiştirilmez ve dbo içindeki kayıtlar otomatik birleştirilmez. Aktarım kaynağı çalışan masaüstü uygulamasının JSON kayıtlarıdır. Başarılı aktarım ve karşılaştırma sonrasında SQL etkinleştirilir. Dolu SQL şeması yeniden bağlanırken kaynak dosyalarla ezilmez.

Bağlantı ayarı Windows kullanıcısının CafeStockAI veri klasöründe connection.local.json olarak tutulur; Git'e eklenmez. Parola istenmez/saklanmaz. Yerel SQL Express'in kendinden imzalı sertifikası için yalnız yerel sunucuda sertifikaya güvenilir; uzak sunucularda sertifika doğrulaması kapatılmaz.

SQL bir kez etkinleştirildikten sonra bağlantı kesilirse uygulama sessizce JSON'a dönerek ayrı kayıtlar üretmez; giriş engellenir, bağlantı ekranından onarım yapılır.

## Katmanlar

- **Domain:** ilişkisel varlıklar; ürün doğrulaması ve sayım durum geçişleri.
- **Application:** taşıma modelleri, IInventoryPersistence ve sorgu arayüzleri, Elazığ aktarım dönüşümü, yerel tüketim tahmin modeli.
- **Infrastructure:** EF Core SQL Server 8.0.32, 17 ilişkisel tablo, migration/snapshot, SQL okuma/yazma, transaction, sürüm kontrolü, aktarım ve geri yükleme doğrulaması.
- **Desktop:** Türkçe formlar, giriş/yetki, ürün/personel, Bar/Depo sayımı, yönetici onayı/ret, stok hareketleri, raporlar, SQL bağlantı ekranı ve yedekleme.
- **DatabaseTool:** şema üretimi, bağlantı kontrolü ve ayrı ilişkisel test sağlayıcısıyla kalıcılık kontrolleri. Masaüstü başlangıç projesi değildir.

## İşlevler

Ürün görselleri ve kategori filtresi; personel görev/telefon/işe giriş tarihi; kullanıcı hesapları; Bar ve Depo miktarları; onay bekleyen sayımlar; stok giriş/çıkışı; CSV raporları; tahmin ve anormal tüketim önerileri; yedek/geri yükleme. Miktarlar 12 ve 7.5 gibi gösterilir; nokta ve virgülle kesirli giriş kabul edilir.

Yerel ridge regresyon modeli tüketim eğilimi ve hafta günü etkilerini öğrenir. Son 7 gün ayrı test için kullanılır; MAE gösterilir. Harici GPT/API kullanılmaz. [ÖRNEK] hareketler gerçek işletme doğruluğu ölçümü değildir.

## Kontroller ve doğrulama sınırı

- Desktop: --check-system <klasör>, --check-features <klasör>. Ayrı dosya tabanlı test kayıtları ve ekran görüntüleri.
- DatabaseTool: --schema <sql-dosyası>; SQL Server modeli ve ilişkileri sunucuya bağlanmadan doğrular.
- DatabaseTool: --check-persistence <klasör>; ayrı SQLite ilişkisel sağlayıcısında aktarım, CRUD, bakiye, hesap hash/salt, FK, rollback, eski sürüm ve geri yükleme kontrolleri. Bu, SQL Server canlı testinin yerine geçmez.
- DatabaseTool: --probe; bağlantıyı CAFESTOCK_SQL_CONNECTION ortam değişkeninden okur. Değeri repoya yazılmaz.

Kod derlendi, model ve ilişkisel test sağlayıcısı kontrolleri geçti. Ajan ortamından yerel SQL Express'e erişim engeli sürdüğü için kullanıcının SQL Server'ında migration/aktarım ve canlı CRUD henüz doğrulanmış sayılmaz. Bağlantı ekranı bu son adımı kullanıcının Windows oturumunda yapar.

## Ders şartları

[Şartname](docs/PROJE-SARTNAMESI.md), [veritabanı tasarımı](docs/VERITABANI-TASARIMI.md), [güncel SQL durumu](docs/SQL-GECISI-DURUM.md).

DevExpress dönüşümü henüz yapılmadı. Lisans/deneme süresinin teslimi kapsaması gerekir. Teslim raporu, ER/sınıf şemaları ve canlı demo ayrıca hazırlanacak. AI ile kod yardımından yararlanıldığı raporda belirtilmelidir. Kaynak kodu ve gerçek geliştirme geçmişi GitHub'da tutulur; gerçek veritabanı, kullanıcı dosyaları, yedekler ve gizli bilgiler depoya eklenmez.

## Çizelge görünümü
Sayım ekranı ve güncel stok raporu, Excel kaynağındaki A/D/G/J sütun grupları ve satır sırasıyla kategori bloklarını yan yana gösterir. Birim, Depo, Bar ve toplam sütunları vardır. Kategori filtresi ve Liste/Çizelge geçişi desteklenir; sayım girişleri aynı taslak üzerinde korunur. Dar ekranlarda yatay, uzun çizelgede dikey kaydırma kullanılabilir.

## 17 Ekim 2026 DevExpress ve teslim hazırlığı
- Gerçek `DevExpress.Win.Grid` 26.1.5 paketi eklendi. Formlar `XtraForm`; ürün/sayım/güncel stok kategori tabloları `GridControl` + `GridView`; giriş ve SQL kontrol düğmeleri `SimpleButton` kullanır. Diğer listelerde standart WinForms kontrolleri halen bulunur.
- Kurulu DevExpress lisansı değerlendirme sürümüdür. Deneme ekranında 9 Kasım 2026 bitişi görülmüştür. Lisans uyarısı kaldırılmaz veya gizlenmez; teslim tarihi için geçerli lisans kullanıcı hesabından doğrulanmalıdır. Değerlendirme DLL'leri GitHub'a veya yeni dağıtım paketine eklenmez.
- Visual Studio'da DevExpress yerel paket kaynağı (kurulumun Components/System/Components/packages klasörü) ya da yetkili DevExpress NuGet kaynağı etkin olmalıdır. Besleme anahtarını kaynak koda eklemeyin.
- Girişte SQL kurulum düğmesi kaldırıldı. Genel yetkili kullanıcı Ayarlar → Veritabanı bağlantısı ile kurulum yapabilir. Ayarlar → SQL bağlantısını kontrol et gerçek SQL snapshot'ından kayıt adetlerini okur; veri değiştirmez.
- Canlı SQL Server bağlantısı agent ortamından doğrulanamadı. Kaydedilmiş bağlantı dosyası tek başına veri aktarımının doğrulaması değildir. SQL kontrolü ve ekle/güncelle → yeniden aç senaryosu normal uygulamada çalıştırılmalıdır.
- Tamamlanan DevExpress özellik testinde kullanıcı girişleri, yetkiler, sayım onayı, çizelgede ürün sayısı, hücre girişleri, toplamlar ve filtrede taslak korunumu geçti. Son hücre yüksekliği, boş görsel başlığı ve SQL kontrol düğmesi için yeni etkileşimli test başlatılmadı; derleme kontrolü uygulanır.
- SQLite yalnız ayrı ilişkisel kalıcılık testlerinde kullanılır; üretim veritabanı SQL Server Express'tir.

## Mobil personel ve yönetim arayüzü
Çözümde CafeStockAI.Mobile projesi vardır. Visual Studio'da bu projeyi başlangıç projesi seçip Mobil Demo ile ayrı örnek verilerde veya Mobil SQL ile mevcut SQL kayıtlarında çalıştırabilirsiniz. Mobil SQL aktarım veya migration yapmaz.

Personel sayım gönderir; müdürler onay/ret ve analiz yapar. İşletme sahibi/genel müdür ürün, personel, hesap ve işletme ayarlarına da erişir. Mobil web ASP.NET Core kullanır; DevExpress masaüstünde korunur. Şu an yalnız localhost; yayın ve mobil veri erişimi kapalıdır.

Ayrıntılar: [Mobil kullanım](docs/MOBIL-KULLANIM.md).
