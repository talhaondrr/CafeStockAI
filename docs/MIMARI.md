# CafeStockAI kod yapısı

Bu düzenleme mevcut ekranları ve veri biçimini koruyarak kodun okunmasını kolaylaştırır. Veritabanı tabloları/migration değişmedi; gerçek personel, şifre ve stok kayıtlarına bu düzenleme sırasında yazılmadı.

## Katmanlar ve sorumluluklar

- **Domain:** EF tablolarında kullanılan iş nesneleri ve temel doğrulamalar.
- **Application:** Ortak modeller, stok/sayım kuralları, rol yetkileri, parola doğrulaması ve tüketim tahmini. Windows Forms veya ASP.NET bağımlılığı yoktur.
- **Infrastructure:** EF Core, SQL Server bağlantı ayarı, kalıcı kayıtlar ve transaction işlemleri. Application arayüzlerini uygular.
- **Desktop:** DevExpress/Windows Forms ekranları; masaüstü oturumu ve eski JSON verilerinden geçiş için mevcut uyumluluk kodu.
- **Mobile:** ASP.NET başlangıcı, HTTP istekleri ve tarayıcı arayüzü. Mobil kimlik her istekte kontrol edilir; masaüstünün statik oturumu kullanılmaz.
- **DatabaseTool / tests:** Gerçek veriyi kullanmadan ilişkisel kayıt, yetki ve sayım kontrolleri.

Bağımlılık yönü: Desktop/Mobile → Infrastructure → Application → Domain. Application, Infrastructure veya ekran projelerine referans vermez.

## Bir işlemi nerede bulurum?

| İş | Dosya / klasör |
|---|---|
| Mobil uygulamayı başlatma | `CafeStockAI.Mobile/Program.cs` |
| Servis ve demo/SQL seçimi | `CafeStockAI.Mobile/Hosting/MobileServices.cs` |
| Güvenlik, hata yanıtı ve oturum kontrolü | `CafeStockAI.Mobile/Hosting/MobilePipeline.cs` |
| Giriş ve çıkış adresleri | `CafeStockAI.Mobile/Endpoints/AuthenticationEndpoints.cs` |
| Sayım adresleri | `CafeStockAI.Mobile/Endpoints/CountsEndpoints.cs` |
| Ürün/personel/hesap yönetim adresleri | `CafeStockAI.Mobile/Endpoints/ManagementEndpoints.cs` |
| Ekrana gönderilen veri | `CafeStockAI.Mobile/Endpoints/WorkspaceEndpoints.cs` |
| Mobil sayım/personel/ürün iş kuralları | `CafeStockAI.Application/Mobile/MobileWorkspace.*.cs` |
| Rol yetkileri | `CafeStockAI.Application/EmployeePermissions.cs` |
| Parola hash üretimi ve doğrulaması | `CafeStockAI.Application/AccountCredentials.cs` |
| SQL bağlantı bilgisini okuma ve oluşturma | `CafeStockAI.Infrastructure/SqlConnectionSettings.cs` |
| Ortak ürün görseli seçimi | `CafeStockAI.Application/ProductImageKey.cs` |
| Masaüstü hesap, oturum ve dosya kilidi | `CafeStockAI.Desktop/Security/` |
| Tek ürünle sayım ekranı | `CafeStockAI.Mobile/wwwroot/count-workflow.js` |
| Telefon kasası önizlemesi | `CafeStockAI.Mobile/wwwroot/phone-preview.js` ve `.css` |

`MobileWorkspace` tek servis olarak kalır; ilgili bölümleri özellik bazında partial dosyalara ayrılır. Bu birden fazla ayrı servis veya yeni bir tasarım deseni değildir. Aynı transaction ve yetki sınırlarını koruyup gezinmeyi kolaylaştırır.

## Korunan kurallar

Sayım gönderimi stok değiştirmez; stok onayda güncellenir. Bar ve Depo bağımsızdır; sayılmayan alan null kalır. Tekrarlanan gönderim ikinci kayıt oluşturmaz. Ürün/personel kaldırmada geçmiş kayıtlar korunur. Kapalı hesabın açık oturumu da geçersizleşir. SQL'deki sürüm kontrolü eski ekranın yeni verileri ezmesini engeller. Uygulama bu aşamada yalnız localhost'ta çalışır.

## Doğrulama

- Çözümün derlenmesi: Desktop, Mobile ve DatabaseTool dahil.
- `CafeStockAI.DatabaseTool --check-persistence <ayrı-test-klasörü>`: SQLite üzerinde ilişkisel kayıt ve transaction testleri; canlı SQL Server bağlantı testi yerine geçmez.
- `tests/mobile_http_checks.py`: CAFESTOCK_MOBILE_URL ile yalnız ayrı Mobil Demo ortamında HTTP güvenlik/iş akışı testleri.
- `node tests/count_workflow_checks.cjs`: Enter, kategori geçişi, sıfır miktarı, geri dönüş ve taslak kaydı.

Kod biçimi `.editorconfig` ile tanımlıdır. Designer ve migration dosyaları üretilen kaynaklar olarak ayrı tutulur. DevExpress'in deneme lisansı uyarısı lisans durumuna bağlıdır; bastırılmamıştır.

## Bilinen sınır

Masaüstündeki bazı eski ekran işlemleri hâlâ statik uyumluluk servislerini kullanır. Bu çalışma katman ayrımını güçlendirir; projenin tamamının saf Clean Architecture'a dönüştürüldüğü iddia edilmez. Çalışan özellikleri korumak için gereksiz yeni repository/factory katmanları eklenmemiştir.
