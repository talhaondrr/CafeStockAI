# Mobil kullanım ve geliştirme

## Visual Studio 2026 ile açma
1. CafeStockAI.Desktop.sln dosyasını açın.
2. CafeStockAI.Mobile projesine sağ tıklayın → Başlangıç Projesi Olarak Ayarla.
3. Üstteki çalıştırma listesinden Mobil Demo veya Mobil SQL profilini seçin.
4. F5 ile çalıştırın.

### Mobil Demo
Adres: http://localhost:5186

Hesaplar: Demo Personel, Demo Müdür, Demo Sahip. Şifre: 123456.

Yapay örnek veriler kullanılır; gerçek personel ve stok dosyaları okunmaz. Demo bellektedir; sunucu yeniden başladığında sıfırlanır. Eksik katalog birimleri yalnız demo içinde Adet olarak örneklenir. Gerçek katalog değiştirilmez.

### Mobil SQL
Adres: https://localhost:7291 veya geliştirmede http://localhost:5187

Masaüstündeki aynı personel kullanıcı adı ve parolaları kullanılır. Varsayılan olarak LocalApplicationData/CafeStockAI/connection.local.json dosyasındaki sunucu/veritabanı bilgisi ve Windows kimlik doğrulaması kullanılır.

Kaynak klasör farklıysa CAFESTOCK_DESKTOP_DIRECTORY; doğrudan bağlantı gerekiyorsa CAFESTOCK_SQL_CONNECTION ortam değişkeni kullanılabilir. Bağlantıyı repoya yazmayın. SQL bağlantısı yoksa demo moduna sessiz geçilmez. Mobil proje migration veya aktarım çalıştırmaz; önce masaüstündeki SQL kurulumu tamamlanmalıdır.

Canlı SQL Server erişimi agent ortamından doğrulanamadı. Gerçek bilgisayarda ayrıca kontrol edilmelidir. HTTPS geliştirme sertifikası geçerli olmalıdır; tarayıcı uyarısını atlamayın.

## Göreve göre ekranlar
| Görev | Mobil erişim |
|---|---|
| Personel | Ana sayfa, stok görüntüleme, sayım ve kendi sayım geçmişi |
| Depo Sorumlusu | Personel ekranları ve stok giriş/çıkışı |
| Şube Müdürü, Bölge Müdürü, Süpervizör | Stok hareketi, sayım onay/ret, AI analizi ve rapor |
| İşletme Sahibi, Genel Müdür | Yukarıdakiler; ürün, personel, hesap ve işletme ayarları |

İşletme sahibi veya genel müdür personeli aktif listeden kaldırabilir; hesap aynı transaction içinde kapanır, geçmiş sayım ve hareketler korunur. Kendi kaydını ve son aktif yöneticiyi kaldırma engellenir. Yedek geri yükleme ve SQL sunucusu ayarı masaüstündedir. Çoklu şube ve transfer ertelenmiştir.

## Sayım
- Bar ve Depo miktarı ayrı girilir. Nokta/virgül kabul edilir; gereksiz sıfırlar gösterilmez.
- Boş alan sıfır sayılmaz; bütün aktif ürünler doldurulmalıdır.
- Taslak aynı tarayıcıda kullanıcı ve demo/SQL ayrımıyla saklanır. Filtre ve sayfa yenilemesi miktarları kaybettirmez.
- Açık sayfada bağlantı kesilirse miktar girilebilir; göndermek için bağlantı gerekir. Tam çevrimdışı PWA değildir.
- Başlangıç stoğu ve birimler sunucuda korunan imzalı taslağa bağlanır. Taslak 24 saat geçerlidir ve kişiye özeldir.
- Aynı sayımın tekrar gönderimi ikinci kayıt oluşturmaz.
- Onay bekleyen sayım stok değiştirmez. Onayda fark hareketleri ve durum birlikte kaydedilir.
- Stok, ürün veya birim değişirse eski taslak reddedilir. SQL rowversion masaüstüyle eşzamanlı değişimleri denetler.

## Yönetim
Ürün adı, kategori, birim ve kritik stok düzenlenebilir. Ürün kaldırma, aktif listeden çıkarır; SQL geçmişini korur. Geçmişi olan ürünün birimi için ayrı ürün açılmalıdır.

Personel ekleme ve kullanıcı hesabı aynı transaction içinde kaydedilir. Yeni kullanıcı adı personelin adı, ilk şifre 123456. Telefon 0555 123 45 67 biçimindedir. Hesap ayarları genel yönetim yetkisi gerektirir; kendi hesabını ve son yöneticiyi kapatma kontrolü vardır.

Sayım detayları ve yönetici notları, stok hareketleri, AI tahmini, MAE ve CSV raporu gösterilir.

## Çalıştırma ve güvenlik sınırı
Bu sürüm yalnız localhost erişimine izin verir. Yayın, dış IP, LAN, tünel, alan adı ve barındırma oluşturulmadı. Mobil veriyle gerçek erişim ileride HTTPS ve izin verilen sunucu adresiyle ayrı aşamadır. Masaüstü halen doğrudan SQL kullanır; internetten masaüstü erişimi istenirse ortak API istemcisine taşınmalıdır.

Mobil web ASP.NET Core 8, HTML/CSS/JavaScript kullanır. Windows Forms ve DevExpress masaüstünde korunur. Mobil projede DevExpress DLL bulunmaz.

ASP.NET Core oturum çerezi HttpOnly ve SameSite Strict; üretimde Secure zorunludur. POST işlemlerinde antiforgery, girişte hız sınırı, CSP ve önbelleksiz yanıtlar vardır. Her istekte hesap ve görev yeniden denetlenir. Parola değişimi veya hesap kapatma mevcut oturumu da geçersizleştirir. Web kullanıcıları Desktop.Access statik oturumunu paylaşmaz.

Data Protection anahtarları varsayılan olarak LocalApplicationData/CafeStockAI.Mobile/keys altında; Git dışında tutulur. Testler ayrı klasör kullanır. İnternet yayını öncesi HTTPS, sunucu adresi, anahtar şifreleme, demo modunun kapalı olması ve güçlü parolalar ayrıca hazırlanmalıdır.

## Testler
- DatabaseTool --check-persistence: ayrı SQLite veritabanında kalıcılık, mobil onay, personel+hesap atomikliği, rollback ve eşzamanlılık. Canlı SQL Server testi değildir.
- Mobil Demo açıkken python tests/mobile_http_checks.py: giriş, CSRF, görevler, tekrar gönderim, onay, hesap kapatma, stok çakışması ve statik dosyalar. Sıfırdan açılmış demo ortamı gerektirir.
- 390 × 844 telefon görünümünde tarayıcı testi: personel girişi, 99 ürün, 7,5+12=19,5, filtre/yenilemede taslak, onaya gönderim, işletme sahibi onayı ve personel menüsü doğrulandı.
- Gerçek telefon ve mobil veri erişimi henüz test edilmedi.

ASP.NET Core antiforgery kaynağı: https://learn.microsoft.com/aspnet/core/security/anti-request-forgery

## Ayrı alan sayımı güncellemesi
Sayımın başında Bar veya Depo seçilir. Ürün başına yalnız seçilen alanın miktarı girilir. Bar ve Depo ayrı sayım kimlikleriyle kaydedilir, ayrı onaylanır. Alanlar için ayrı kişisel taslaklar saklanır; Alanı değiştir düğmesi miktarları silmeden diğer alana geçer. Sayılmayan alan null saklanır, sıfır kabul edilmez. Bir alanın değişmesi diğer alanın sayımını geçersiz kılmaz. Masaüstü Onay ekranı da bu sayımları alanına göre onaylar; önceki birleşik sayımlar korunur.

Güncel tarayıcı kontrolü: 99 ürün için tek Bar kutusu, Depo kutusu bulunmaması, Bar için 7,5 ve Depo için 12 girildiğinde taslakların ayrı kalması, yenilemede Depo 12 değerinin korunması, Bar sayımının ayrı onaya gönderilmesi. Önceki 19,5 toplamlı birleşik giriş bu güncellemeyle kaldırılmıştır.
