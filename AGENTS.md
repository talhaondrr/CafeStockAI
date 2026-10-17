# Proje geliştirme kuralları

Kullanıcının 16 Ekim 2026 tarihinde paylaştığı dört sayfalık Nesne Tabanlı Programlama dönem projesi yönergesi esas alınır. Ayrıntılı eşleme: docs/PROJE-SARTNAMESI.md.

- Hedef: bireysel C# Windows Forms projesi; teslim ortamı Visual Studio 2026. Çözüm dosyasındaki sürüm satırını değiştirerek ortam geçişi yapılmış gibi gösterme. 2026 ile açma, tasarım ve derleme doğrulanmalı.
- DevExpress zorunludur. Standart WinForms kontrolleri tek başına yeterli değildir. Gerçek lisanslı/deneme paketleri kullanılmalı; aynı adlı sahte kontroller veya eksik bağımlılıkları gizleyen yedek arayüz oluşturma.
- Veritabanı Access olamaz. Kararlaştırılan hedef SQL Server Express + Entity Framework Core. Kullanıcı, personel, görev, şube, ürün, sayım, hareket ve transferler ilişkisel tablolarda tutulmalı. SQL içinde tek JSON metni saklamak geçişin tamamlandığı anlamına gelmez.
- Mevcut JSON uygulaması geçiş süresince çalışır kalmalı. Gerçek veriyi önce yedekle; aktarımı kimlikler, ilişkiler ve bakiyeler bakımından doğrula. Testleri ayrı veritabanında yap.
- Arayüz, iş mantığı ve veri erişimini ayır. Domain, Application, Infrastructure ve Desktop katmanları hedeflenir. Sınıf, kapsülleme, arayüz, uygun kalıtım/çok biçimlilik, koleksiyon/generic ve istisna yönetimini gerçek örneklerle göster.
- Yapay zekâyı zorunlu kabul et: yönergenin ana teknik şartları ve rubriği bunu söylüyor. Yerel tüketim tahmin modeli mevcut; bulut API'si veya ML.NET tek başına zorunlu değil. Eğitim, test ayrımı, hata metriği ve kullanıcıya faydası doğrulanmalı. Kullanılmayan teknolojileri raporda kullanılmış gibi yazma.
- GitHub'da gerçek geliştirme boyunca anlamlı commitler ve doğru README gerekli. Geçmişi sonradan uydurma. Kullanıcı onayı olmadan başka kişilere mesaj gönderme.
- Parola, API anahtarı, bağlantı dizisi, gerçek hesap dosyası/veritabanı/yedek repoya konmaz. Windows kimlik doğrulaması tercih edilir; yapılandırma ortam değişkeni veya Git dışında yerel dosyadan okunur.
- Türkçe arayüz, mevcut kahve teması, kategori filtresi ve görseller korunur. Ham kimlik/kod sütunları kullanıcıya gösterilmez. Miktarlar 12 ve 7.5 biçiminde; kesirler korunur, gereksiz sıfırlar eklenmez.
- Kişi birden fazla şubeye atanabilir. Genel müdür/işletme sahibi tüm şubeler; diğer görevler atandıkları şubeler kapsamındadır. Yetki kontrolü servis katmanında da yapılır.
- Sayım onaylanana kadar stok değişmez. Stok değişmişse eski sayım onaylanmaz. Transfer atomiktir; kaynak/hedef toplamı korunur. Bu davranışları SQL geçişinde koru.
- Teslimler: kaynak kodu/GitHub, ER şeması ve OOP açıklamalarını içeren yazılı rapor, ekran görüntüleri ve çalışan canlı demo. AI ile kod yardımından yararlanıldığı raporda açıkça belirtilmeli. Öğrencinin açıklayabileceği bir yapı ve demo hazırlanmalı.
- DevExpress lisansı yok; kullanıcı hesabı olabilir. Olası teslim 9 Kasım 2026 veya finaller. Kesin teslim tarihi bilinmiyor. 30 günlük denemeyi finali kapsadığı varsayımıyla başlatma; lisans/hesap erişimi kullanıcı tarafından sağlanır.

## Güncel kullanıcı kapsamı — 17 Ekim 2026
Kullanıcı uygulamayı yalnız Elazığ şubesi için sadeleştirdi. Bütün personeller ve ürünler korunacak; görevler değiştirilmeden tüm personelin şube ataması Elazığ olacak, kullanıcı sonradan düzenleyecek. Çoklu şube ve şubeler arası transfer ertelendi. Eski SQL dbo tabloları korunur; mevcut masaüstü kayıtları cafestock şemasına aktarılır. Eski dbo kayıtları otomatik olarak birleştirilmiş sayılmaz.

Analiz tahminleri ve tedarik önerilerinde Adet/Şişe/Koli/Paket/Fişek birimleri yukarı yuvarlanarak tam sayı gösterilir. Kg/Litre gibi kesirli birimler korunur. Öğrenen modelin ham çıktısı veya kaydedilmiş gerçek stoklar sırf gösterim için değiştirilmez.

Stok/sayım görünümü kullanıcının MACK BEAR Excel çizelgesi gibi kategorileri kaynak hücre sırasına göre yan yana bloklarda gösterir. Ürün görseli/adı, birim, Depo, Bar ve toplam görünür. Kategori filtresi ve görünüm değişimi sayım taslağını kaybettirmemeli; liste görünümüne geçiş ve CSV raporları korunmalı.

Kategori kutuları kaynak kategori sırasını koruyarak o anda en kısa sütuna yerleşir; kullanıcı sağdaki boş alan yerine solda uzayan sabit Excel sütunlarını istemiyor. Ürünler, sayım ve stok raporunda dengeli dağılım uygulanır.

## DevExpress geçişi
Gerçek DevExpress.Win.Grid 26.1.5 kurulu yerel paketlerle projeye eklendi. StockSheet artık gerçek GridControl/GridView kullanır; form tabanı XtraForm. Deneme bildirimi ekranında bitiş 9 Kasım 2026 görüldü. Lisans uyarısını/splash ekranını bastırma veya evaluation DLL dağıtma. Etkileşimli testleri peş peşe başlatma; her süreç deneme penceresi açıp kullanıcıyı engelleyebilir. Başarılı test koşusundan sonra yalnız yeni değişikliklerin gerektirdiği testleri yap.

## Mobil arayüz
Kullanıcı tüm görevlerin telefon arayüzünü, işletme sahibinin yönetim kontrolleri dahil istedi. Yayın yetkisi yok: sadece localhost geliştirme ve ayrı demo. Tünel, LAN/WAN, dış sunucu veya güvenlik duvarı değişikliği yapma. Mobil SQL mevcut cafestock şemasını kullanır; migration/import otomatik çalıştırmaz. Kimlik istek bazındadır; Desktop.Access statik oturumu webde paylaşılmaz.
Mobil personeli aktif listeden kaldırma komutu otomatik onay incelemesinde reddedildi. Kullanıcıdan bu işlem özelinde onay alınmadan ekleme. Diğer izinli ekranlar ayrı ilerletildi.
\nKullanıcı daha sonra açıkça mobil personel kaldırmayı ve hesabının kapanmasını onayladı: geçmiş korunacak. RemoveStaff bu yetkiyle eklendi; kalıcı silme yapılmaz, personel IsActive=false ve hesap Enabled=false aynı transaction içinde kaydedilir. Kendi kaydı/son yönetici kaldırılmaz.\n