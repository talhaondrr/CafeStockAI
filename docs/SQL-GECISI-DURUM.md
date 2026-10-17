# SQL geçişi — Elazığ tek şube

17 Ekim 2026. Kullanıcının son isteği: yalnız Elazığ; bütün personeller ve görevleri, telefon/işe giriş tarihleri ve mevcut ürünler korunsun. Personel şube atamaları Elazığ olsun; kullanıcı görevleri sonradan düzenleyecek. Çoklu şube/şubeler arası transfer ertelendi.

## Kodda tamamlananlar

- Modeller Application katmanına ayrıldı; yerel tahmin modeli de Application içinde.
- SQL Server + EF Core 8.0.32; 17 ilişkisel tablo, FK/benzersiz indeks/check kısıtları; cafestock şeması; InitialElazig migration ve model snapshot.
- Ürünler, personeller, görevler, şube/alanlar, kullanıcılar, sayımlar/satırlar, hareketler ve işletme ayarları ilişkisel SQL kayıtlarına eşleniyor. Tek JSON kolonda veri saklanmıyor.
- IInventoryPersistence üzerinden InventoryStore, ProductStore ve Accounts SQL sağlayıcısına bağlanıyor. SQL etkinse stok ekranları, raporlar ve AI tüketim geçmişini SQL'den alıyor.
- Girişte SQL bağlantısı/aktarım formu. Windows kimlik doğrulaması, yerel özel ayar dosyası. Başarısız bağlantı/aktarımda SQL etkinleştirilmez. Bir kez etkinleştikten sonra bağlantı hatasında JSON'a sessiz dönüş yapılmaz.
- İlk tek şube hazırlığı önce dosyaları yedekler; personeller ve görevleri değişmez, bütün atamalar Elazığ olur. Diğer şubelerin stok/geçmişi Elazığ'a eklenmez, yedekte korunur.
- Eski Warehouse kimliklerinin kanonik Bar/Depo alanlarına eşlemesi tutulur. Personel/ürün/hareket/sayım kimlikleri korunur.
- SQL aktarımı tek transaction içindedir; kayıt adetleri/kimlikler, personel/ürün alanları, şifre hash/salt, sayım satırları ve ürün/alan bakiyeleri karşılaştırılır.
- Sürüm kontrolü eski snapshot'ın yeni kayıtları ezmesini engeller. Kaydedilmiş hareketler yerinde değiştirilmez. Stok farkları ve sayım onayı aynı transaction içinde kaydedilir.
- SQL yedeği ilişkisel kayıtlardan üretilir; geri yükleme yalnız cafestock şemasında tek transaction içindedir. Uyumsuz yedek hata verirse işlemler rollback olur. Mevcut dbo tabloları hedeflenmez.

## Mevcut dbo tablolarının durumu

Kullanıcının CSV'sinde Alerts, Areas, Balances, Branches, CountLines, Counts, Employees, Movements, Operations ve Products tabloları vardı; kimlikler int. Bunlar mevcut hâliyle korunur. Yeni cafestock şeması bunlarla isim/kimlik çakışmasına girmez. Bu tablolardaki veriler otomatik birleştirilmiş veya aktarılmış sayılmaz. Yeni aktarımın kaynağı mevcut masaüstü uygulamasının JSON kayıtlarıdır.

## Doğrulama

- Çözüm derlemesi: geçti.
- SQL Server modelinden şema/migration üretimi: geçti.
- Ayrı SQLite ilişkisel test sağlayıcısında: tüm personelleri koruma, görev/telefon/tarih karşılaştırması, eski alanları birleştirme, farklı şube stoğunu dışarıda bırakma, kaynakların değişmemesi, idempotent import, hash/salt korunması, ürün güncelleme, sayım ve fark hareketi, FK hatasında rollback, eski sürüm koruması, pasif personel geçmişi, SQL arşivi geri yükleme ve uyumsuz yedekte rollback test edildi.
- Desktop kontrolleri: giriş/yetki, sayım onayı, stok, rapor, AI, yedek, miktar biçimi ve Elazığ ekranları kontrol edildi.
- **Gerçek SQL Server üzerinde migration/aktarım ve canlı CRUD henüz doğrulanmadı.** Ajan ortamı yerel SQL Express'e erişemiyor; SSMS bağlantısı kullanıcı oturumunda başarılı. Son doğrulama uygulamanın bağlantı ekranından yapılacak.

SQLite yalnız test sağlayıcısıdır. Üretim veritabanı SQL Server'dır; SQLite testinin geçmesi SQL Server davranışlarının tamamının doğrulandığı anlamına gelmez. SQL Server migration scripti cafestock şemasında çalışır; gerçek kullanıcı veritabanında elle çalıştırmak yerine uygulamadaki bağlantı akışı kullanılmalıdır.

## Kullanıcı adımları

Uygulamayı kapatıp güncel çözümü derleyin; CafeStockAI.Desktop başlangıç projesi olsun. Giriş ekranındaki Elazığ • SQL bağlantısı düğmesinde SSMS'de bağlandığınız sunucu adı ve CafeStockAI veritabanını seçin. Bağlan ve Elazığ'a aktar düğmesi, mevcut masaüstü kayıtlarını yedekleyip SQL'e aktarır. Başarılı mesajından sonra giriş yapın. SSMS'de Tablolar'ı yenileyince cafestock önekli tablolar görünür; dbo tabloları kalır.

DevExpress dönüşümü bu SQL aşamasına dahil edilmedi; lisans/deneme zamanlaması bekliyor.
