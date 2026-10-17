# Şartnameye uyum — CafeStock AI

Kaynak: Kullanıcının paylaştığı 4 sayfalık “Nesne Tabanlı Programlama — Dönem Projesi Yönergesi”. Durum tarihi: 16 Ekim 2026. Bu belge hedefleri ve mevcut durumu ayırır; tamamlanmamış işleri tamamlanmış saymaz.

## Zorunlu şartlar ve mevcut durum

| Şart | Mevcut uygulama | Gerekli uyarlama |
|---|---|---|
| Bireysel C# Windows Forms | C# / .NET 8 WinForms | Korunacak; öğrenci mimariyi açıklayabilmeli |
| Visual Studio 2026 | Çözüm 2022 / 17.14; kurulum envanterinde 2026 doğrulanmadı | 2026 kurulumu, çözüm ve tasarımcı kontrolü |
| DevExpress | Standart kontroller ve özel çizimler; paket referansı yok | XtraForm, GridControl/GridView, düzen ve giriş bileşenlerine gerçek geçiş |
| Access dışı veritabanı | Yerel JSON dosyaları | SQL Server Express + EF Core; ilişkisel veri modeli ve aktarım |
| CRUD ve ilişkili tablolar | İşlevler var, ilişkisel DB yok | İşlemler veritabanı üzerinden çalışmalı; PK/FK ve doğrulamalar |
| OOP kavramları | Sınıflar, IInventoryPage, List/Dictionary, form kalıtımı, QuantityInput override, try/catch var | İş mantığı/veri erişimi ayrılmalı; domain doğrulaması ve servis arayüzleri geliştirilmeli |
| Kullanıcı doğrulaması ve hata mesajları | Mevcut | DB/DevExpress geçişinde korunacak |
| Yapay zekâ | Yerel ridge regresyon; trend/hafta günü, 7 gün tahmin, ayrılan test günleri ve MAE | Veri SQL'den alınmalı; model ve ölçümler rapor/demo ile açıklanmalı |
| GitHub ve süreç geçmişi | Yerel commitler var; son gönderim kimlik erişimi yüzünden tamamlanmadı | Push doğrulaması, sonraki işler için düzenli gerçek commitler |
| README | Çalışan yerel sürüm anlatılıyor | Yeni teknolojiler gerçekten devreye girdikçe güncellenecek |
| Yazılı rapor | Teslim raporu henüz yok | Aşağıdaki rapor kapsamı hazırlanacak |
| Canlı sunum/demo | Ekranlar ve tanılama kontrolleri mevcut | Senaryo, teknik açıklama ve teslim ortamında prova |

Yapay zekâ için 5. bölümdeki “opsiyonel/bonus” ifadesi, ana tablo, 2.5 ve rubrikteki “zorunlu” ifadeleriyle çelişiyor. Daha sıkı olan zorunlu şart esas alınacak; öğretmene gerektiğinde bu çelişki sorulabilir. ML.NET, bulut API veya yerel LLM örnek seçeneklerdir; hepsini kullanmak zorunlu değildir.

## Uygulama sırası

1. Şartnameyi ve mimari/ER tasarımını kaydet (bu belge).
2. SQL Express bağlantısını doğrula; ayrı geliştirme/test veritabanlarını kur. Kurulum dizininin görülmesi, sunucunun erişilebilir olduğu anlamına gelmez.
3. Domain, Application ve Infrastructure projelerini ekle. EF Core SQL Server sağlayıcısı, DbContext, eşlemeler ve migrations geliştir.
4. Mevcut JSON dosyalarını yedekleyip bir defalık aktar. Ürün/personel/şube kimliklerini koru; kişi-şube ilişkisinin çoktan çoğa olduğunu uygula. Hesap hash ve salt değerlerini yeniden şifre üretmeden taşı.
5. Hesaplar, katalog, personel, şube, hareket, sayım ve transferleri veritabanından çalıştır. İlişkileri ve silme kısıtlarını doğrula; onay/transfer için transaction kullan.
6. Gerçek DevExpress kurulumu/lisans erişimi ile ekranları dönüştür. Designer görünümü de çalışmalı. Tema, Türkçe başlıklar ve miktar biçimi korunmalı.
7. Yerel tahmin servisini SQL sorgularıyla besle; ayrılmış test kümesindeki hata metriğini ve örnek veri sınırlarını göster.
8. VS 2026'da derleme, Designer ve canlı senaryoları doğrula; güncel GitHub gönderimini kontrol et.
9. Rapor, ER görseli, ekran görüntüleri ve sunum/demo hazırla.

## Hedef mimari

- **Domain:** Product, Branch, Employee, UserAccount, StockCount, StockMovement, Transfer ve değer/doğrulama kuralları.
- **Application:** authentication, authorization, catalogue, employee, count approval, transfer ve forecast servisleri; repository/transaction arayüzleri.
- **Infrastructure:** EF Core, SQL Server eşlemeleri/migrations, repository uygulamaları, JSON aktarımı ve yedekleme.
- **Desktop:** DevExpress formları ve ekran modelleri. SQL sorgusu, parola doğrulaması veya transfer muhasebesi form olaylarına gömülmez.

## Veri modeli

ER taslağı: VERITABANI-TASARIMI.md. Bu tasarım şu an çalışır SQL altyapısı değildir; EF Core eşlemeleri ve migration ile uygulanıp test edilecek.

## Teslim raporu

1. Kapak: ad-soyad, öğrenci numarası, proje adı.
2. Özet ve amaç.
3. Gerçekte kullanılan teknolojiler: Visual Studio, DevExpress, veritabanı, AI.
4. Tablolar, ilişkiler, ER şeması.
5. Sınıf ve katman yapısı; OOP kavramlarının kod içindeki somut örnekleri.
6. Ekran görüntüleriyle uygulama tanıtımı.
7. Karşılaşılan zorluklar ve çözümler.
8. Sonuç, GitHub adresi, kaynaklar; AI yardımı kullanım açıklaması.

Demo: yetkili/yetkisiz giriş; ürün/personel/şube CRUD; çoklu şube atama; sayım gönder/incele/onayla/reddet; yetersiz stok ve transfer; SQL'deki kayıt/ilişkileri gösterme; AI tahmini/hata metriği; yedekleme/geri yükleme.

## Puanlama

İşlevsellik 25; OOP/kod kalitesi 20; veritabanı 15; DevExpress arayüz 10; GitHub 10; yazılı rapor 10; sunum/demo 10. Ana toplam 100. Yönerge ayrıca zorunlu AI entegrasyonunu +10 olarak gösteriyor; değerlendirme biçimi öğretmenden netleştirilebilir.

## Teslim/kurulum durumu

DevExpress lisansı yok, kullanıcı hesabı olabilir. Vize başlangıcı 9 Kasım 2026; proje için kesin teslim tarihi bildirilmedi, finaller de olası. 16 Ekim'den 9 Kasım'a 24 gün vardır; 30 günlük deneme bu aralığa yeter, belirsiz final tarihine yeteceği varsayılamaz. Deneme koşulları/uyumlu sürüm kurulurken resmi kaynaklardan kontrol edilir.

## 17 Ekim güncellemesi
Kullanıcı kapsamı yalnız Elazığ olarak sadeleştirdi. Bütün personeller ve roller/ürünler korunuyor; çoklu şube ertelendi. SQL geçiş kodları ve migration eklendi; canlı SQL Server doğrulaması bekliyor. Güncel durum: SQL-GECISI-DURUM.md.
