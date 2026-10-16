# CafeStock AI

.NET 8 Windows Forms ile kafe şubelerinin Bar/Depo stoklarını, personelini ve sayımlarını yöneten masaüstü uygulaması.

## Çalıştırma

Windows ve .NET 8 Windows Desktop çalışma ortamı gerekir. `CafeStockAI.Desktop.sln` dosyasını Visual Studio'da açıp projeyi çalıştırın.

İlk açılışta ürün kataloğu, personel hesapları ve [ÖRNEK] etiketli deneme stokları hazırlanır. Kullanıcı adı personelin tam adı, ilk şifre `123456` olur. Şifre yönetim merkezinden değiştirilebilir; şifreler PBKDF2 ile saklanır.

## İşlevler

- Ürün görselleri, kategori filtresi ve kritik stok eşikleri
- Şube yönetimi, birden fazla şubeye personel atama ve göreve göre erişim
- Şube bazında ayrı Bar/Depo miktarları
- Sayım gönderimi, yönetici onayı/ret ve stok değişikliği kontrolü
- Stok giriş/çıkışı, alanlar ve şubeler arasında transfer
- CSV raporları, günlük yedek ve yedekten geri yükleme
- Geçmiş tüketimden eğilim ve hafta günü etkisini öğrenen yerel ridge regresyon modeli
- Tam sayılarda gereksiz ondalık sıfırlar olmadan miktar gösterimi; nokta veya virgülle kesirli giriş

## Kontroller

Derlenen uygulamayı `--check-system <çıktı-klasörü>` ve `--check-features <çıktı-klasörü>` parametreleriyle çalıştırın. Kontroller ayrı test verileri kullanır, sonuçları ve ekran görüntülerini belirtilen klasöre yazar.

## Veri kapsamı

Kayıtlar bu Windows kullanıcısının `%LOCALAPPDATA%\CafeStockAI` klasöründeki yerel JSON dosyalarında tutulur. Gerçek stok/personel verileri, hesap dosyaları ve yedekler Git deposuna eklenmez. Kaynaktaki demo kayıtları ve örnek hareketler deneme içindir.

Bu sürüm bilgisayarlar arasında internet üzerinden eşitleme yapmaz. Yerel tahmin modeli harici GPT/sohbet servisine bağlı değildir. Gerçek kullanım öncesinde deneme verileri yerine gerçek açılış sayımı ve işletmeye uygun eşikler hazırlanmalıdır.
