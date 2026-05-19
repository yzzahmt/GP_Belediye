# Belediye Otomasyonu

Vatandaş ve personel girişleri, kayıt, başvuru özetleri ve personel paneli için hazırlanmış bir Windows masaüstü uygulamasıdır. .NET Framework 4.7.2 ve MySQL kullanır.

## Hazır sürüm

Çalıştırılabilir paketi **Releases** (sürümler) bölümünden indirin. Arşivi açtıktan sonra uygulamayı çalıştırmadan önce aşağıdaki MySQL adımlarını tamamlamanız gerekir; aksi halde giriş ve kayıt ekranları veritabanına bağlanamaz.

## Veritabanını kurmak

1. Bilgisayarınızda MySQL kurulu ve çalışır durumda olmalı (yerel veya ağ üzerinden erişebildiğiniz bir sunucu).
2. Depodaki **`Belediye_Otomasyonu/belediye_veritabani.sql`** dosyasını MySQL’e uygulayın. Bunu phpMyAdmin’den içe aktararak, MySQL Workbench ile script olarak çalıştırarak veya komut satırından `mysql` ile yapabilirsiniz. Script `belediye` veritabanını, tabloları ve örnek verileri oluşturur.
3. Daha önce aynı veritabanını kurduysanız ve tablolar güncellendiyse, SQL dosyasının sonundaki yorum satırlarında eski kurulumlar için `ALTER` örnekleri vardır; ihtiyaç halinde tek tek çalıştırın.

## MySQL bağlantı ayarı

Uygulama bağlantı bilgisini **`Belediye_Otomasyonu/App.config`** dosyasındaki `connectionStrings` bölümünden okur. `MySqlBelediye` adlı satırı kendi ortamınıza göre düzenleyin:

- **Server** — MySQL sunucu adresi (genelde `127.0.0.1` veya `localhost`)
- **Port** — Varsayılan `3306`
- **Database** — `belediye` (script ile oluşturulan isimle aynı olmalı)
- **Uid** / **Pwd** — MySQL kullanıcı adı ve şifresi

Release paketinde dağıtıyorsanız, kullanıcıların yanında `App.config` yanında derlenmiş yapılandırma dosyası (`Belediye_Otomasyonu.exe.config` benzeri) oluşur; bağlantıyı orada güncellemek gerekir.

## Kaynak koddan derleme

Visual Studio ile `Belediye_Otomasyonu/Belediye_Otomasyonu.csproj` dosyasını açıp Debug veya Release derlemesi alabilirsiniz. NuGet paketleri çözümlenmiş olmalıdır.

## Kısa notlar

- Varsayılan personel hesabı SQL dosyasında tanımlıdır; üretimde şifreyi mutlaka değiştirin.
- E-posta adresi kayıtta tekrar kullanılamaz; veritabanında `e_Mail` için benzersizlik vardır.

Sorun yaşarsanız önce MySQL servisinin ayakta olduğundan, script’in hatasız çalıştığından ve `App.config` içindeki kullanıcı/şifre ile veritabanına erişilebildiğinden emin olun.
