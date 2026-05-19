using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Belediye_Otomasyonu.Services
{
    public sealed class DashboardOzet
    {
        public int KayitliVatandasSayisi { get; set; }
        public int PersonelSayisi { get; set; }
        public int ToplamBasvuru { get; set; }
        public int Beklemede { get; set; }
        public int Islemde { get; set; }
        public int Tamamlandi { get; set; }
    }

    internal static class BelediyeDbServisi
    {
        static BelediyeDbServisi()
        {
            VeritabaniGuncelle();
        }

        public static void VeritabaniGuncelle()
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                {
                    con.Open();
                    bool hasDept = false;
                    bool hasPers = false;
                    using (var cmd = new MySqlCommand("SHOW COLUMNS FROM basvurular", con))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string col = reader.GetString(0);
                            if (string.Equals(col, "AtananDepartman", StringComparison.OrdinalIgnoreCase))
                                hasDept = true;
                            if (string.Equals(col, "AtananPersonelKadi", StringComparison.OrdinalIgnoreCase))
                                hasPers = true;
                        }
                    }

                    if (!hasDept)
                    {
                        using (var cmd = new MySqlCommand("ALTER TABLE basvurular ADD COLUMN AtananDepartman varchar(100) DEFAULT NULL", con))
                            cmd.ExecuteNonQuery();
                    }
                    if (!hasPers)
                    {
                        using (var cmd = new MySqlCommand("ALTER TABLE basvurular ADD COLUMN AtananPersonelKadi varchar(50) DEFAULT NULL", con))
                            cmd.ExecuteNonQuery();
                    }

                    // Borçlar tablosu oluştur
                    using (var cmd = new MySqlCommand(@"
CREATE TABLE IF NOT EXISTS borclar (
    Id int NOT NULL AUTO_INCREMENT,
    VatandasTC varchar(11) NOT NULL,
    Aciklama varchar(200) NOT NULL,
    Miktar decimal(10,2) NOT NULL,
    SonOdemeTarihi date NOT NULL,
    OdenmeTarihi datetime DEFAULT NULL,
    Durum varchar(32) NOT NULL DEFAULT 'Odenmedi',
    PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;", con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Örnek borçları tohumla (seed)
                    using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM borclar", con))
                    {
                        long count = Convert.ToInt64(cmd.ExecuteScalar());
                        if (count == 0)
                        {
                            using (var cmdSeed = new MySqlCommand(@"
INSERT INTO borclar (VatandasTC, Aciklama, Miktar, SonOdemeTarihi, Durum) VALUES
('12345678912', 'Emlak Vergisi 2026 1. Taksit', 450.00, '2026-05-31', 'Odenmedi'),
('12345678912', 'Çevre Temizlik Vergisi 2026', 180.50, '2026-06-30', 'Odenmedi'),
('12345678912', 'Su Borcu Faturası - Nisan 2026', 220.00, '2026-05-25', 'Odenmedi'),
('12345678912', 'İdari Para Cezası (Park İhlali)', 500.00, '2026-04-15', 'Odenmedi')", con))
                            {
                                cmdSeed.ExecuteNonQuery();
                            }
                        }
                    }
                    // Personel Mesajları Tablosu
                    using (var cmd = new MySqlCommand(@"
CREATE TABLE IF NOT EXISTS personel_mesajlari (
    Id int NOT NULL AUTO_INCREMENT,
    GonderenKadi varchar(50) NOT NULL,
    AliciKadi varchar(50) NOT NULL,
    Mesaj TEXT NOT NULL,
    Tarih datetime DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;", con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Personel Görevleri Tablosu
                    using (var cmd = new MySqlCommand(@"
CREATE TABLE IF NOT EXISTS personel_gorevleri (
    Id int NOT NULL AUTO_INCREMENT,
    Baslik varchar(200) NOT NULL,
    Aciklama TEXT NULL,
    AtananPersonelKadi varchar(50) NOT NULL,
    VerenKadi varchar(50) NOT NULL,
    Oncelik varchar(32) NOT NULL DEFAULT 'Orta',
    Durum varchar(32) NOT NULL DEFAULT 'Baslanmadi',
    OlusturmaTarihi datetime DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;", con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Örnek görevler tohumla
                    using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM personel_gorevleri", con))
                    {
                        long count = Convert.ToInt64(cmd.ExecuteScalar());
                        if (count == 0)
                        {
                            using (var cmdSeed = new MySqlCommand(@"
INSERT INTO personel_gorevleri (Baslik, Aciklama, AtananPersonelKadi, VerenKadi, Oncelik, Durum) VALUES
('Sokak Lambası Bakımı', 'Atatürk Mahallesi 5. Sokak üzerindeki arızalı sokak lambalarının değiştirilmesi.', 'personel1', 'admin', 'Yüksek', 'Baslanmadi'),
('Park Düzenleme Çalışması', 'Belediye parkındaki çim biçme ve bank onarım işlemlerinin tamamlanması.', 'personel1', 'admin', 'Orta', 'DevamEdiyor'),
('Evrak Arşivleme', '2025 yılına ait vatandaş başvuru dilekçelerinin taranarak dijital arşive aktarılması.', 'personel1', 'admin', 'Düşük', 'Tamamlandi')", con))
                            {
                                cmdSeed.ExecuteNonQuery();
                            }
                        }
                    }

                    // Örnek mesajlar tohumla
                    using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM personel_mesajlari", con))
                    {
                        long count = Convert.ToInt64(cmd.ExecuteScalar());
                        if (count == 0)
                        {
                            using (var cmdSeed = new MySqlCommand(@"
INSERT INTO personel_mesajlari (GonderenKadi, AliciKadi, Mesaj) VALUES
('admin', 'personel1', 'Merhaba Ahmet Bey, bugün atanan sokak lambası arıza görevini inceleyebilir misiniz?'),
('personel1', 'admin', 'Merhaba sayın başkanım, ekip ile birlikte sokağa geçiyoruz. Öğleden sonra tamamlamış oluruz.'),
('admin', 'GENEL', 'Arkadaşlar genel kurul toplantısı saat 14:00 da konferans salonunda yapılacaktır. Tüm personelin katılımı rica olunur.')", con))
                            {
                                cmdSeed.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("VeritabaniGuncelle Hatası: " + ex.Message);
            }
        }

        /// <summary>Sadece personeller tablosunu kontrol eder (yönetici girişi için değil).</summary>
        public static bool PersonelGirisDogrula(string kullaniciAdi, string sifre)
        {
            if (string.IsNullOrWhiteSpace(kullaniciAdi) || sifre == null)
                return false;

            using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            using (var cmd = new MySqlCommand(
                "SELECT 1 FROM personeller WHERE KullaniciAdi=@k AND Sifre=@s AND Aktif=1 LIMIT 1", con))
            {
                cmd.Parameters.AddWithValue("@k", kullaniciAdi.Trim());
                cmd.Parameters.AddWithValue("@s", sifre);
                con.Open();
                return cmd.ExecuteScalar() != null;
            }
        }

        /// <summary>Sadece yoneticiler tablosunu kontrol eder.</summary>
        public static bool YoneticiGirisDogrula(string kullaniciAdi, string sifre)
        {
            if (string.IsNullOrWhiteSpace(kullaniciAdi) || sifre == null)
                return false;

            using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            using (var cmd = new MySqlCommand(
                "SELECT 1 FROM yoneticiler WHERE KullaniciAdi=@k AND Sifre=@s AND Aktif=1 LIMIT 1", con))
            {
                cmd.Parameters.AddWithValue("@k", kullaniciAdi.Trim());
                cmd.Parameters.AddWithValue("@s", sifre);
                con.Open();
                return cmd.ExecuteScalar() != null;
            }
        }

        public static bool VatandasGirisDogrula(string tc, string sifre)
        {
            if (string.IsNullOrWhiteSpace(tc) || tc.Length != 11 || sifre == null)
                return false;

            using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            using (var cmd = new MySqlCommand(
                "SELECT 1 FROM kullanicilar WHERE tc=@tc AND sifre=@s LIMIT 1", con))
            {
                cmd.Parameters.AddWithValue("@tc", tc.Trim());
                cmd.Parameters.AddWithValue("@s", sifre);
                con.Open();
                return cmd.ExecuteScalar() != null;
            }
        }

        /// <summary>Aynı e-posta (büyük/küçük harf ve baş/son boşluk yok sayılarak) zaten kayıtlı mı.</summary>
        public static bool KullaniciEmailZatenKayitli(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            using (var cmd = new MySqlCommand(
                "SELECT 1 FROM kullanicilar WHERE LOWER(TRIM(e_Mail)) = LOWER(TRIM(@e)) LIMIT 1", con))
            {
                cmd.Parameters.AddWithValue("@e", email);
                con.Open();
                return cmd.ExecuteScalar() != null;
            }
        }

        public static bool TryGetKullaniciDisplayName(string tc, out string displayName)
        {
            displayName = null;
            if (string.IsNullOrWhiteSpace(tc))
                return false;

            using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            using (var cmd = new MySqlCommand(
                "SELECT ad, soyad FROM kullanicilar WHERE tc=@tc LIMIT 1", con))
            {
                cmd.Parameters.AddWithValue("@tc", tc.Trim());
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        return false;
                    var ad = r.IsDBNull(0) ? "" : r.GetString(0);
                    var soyad = r.IsDBNull(1) ? "" : r.GetString(1);
                    displayName = (ad + " " + soyad).Trim();
                    return !string.IsNullOrEmpty(displayName);
                }
            }
        }

        private static int GuvenliTekSay(MySqlConnection con, string sql)
        {
            try
            {
                using (var c = new MySqlCommand(sql, con))
                {
                    var s = c.ExecuteScalar();
                    return s == null || s == DBNull.Value ? 0 : Convert.ToInt32(s);
                }
            }
            catch (MySqlException)
            {
                return 0;
            }
        }

        public static DashboardOzet YukleDashboardOzet()
        {
            var ozet = new DashboardOzet();
            using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            {
                con.Open();
                ozet.KayitliVatandasSayisi = GuvenliTekSay(con, "SELECT COUNT(*) FROM kullanicilar");
                ozet.PersonelSayisi = GuvenliTekSay(con, "SELECT COUNT(*) FROM personeller WHERE Aktif=1") +
                                      GuvenliTekSay(con, "SELECT COUNT(*) FROM yoneticiler WHERE Aktif=1");
                ozet.ToplamBasvuru = GuvenliTekSay(con, "SELECT COUNT(*) FROM basvurular");
                try
                {
                    using (var c = new MySqlCommand(
                        "SELECT Durum, COUNT(*) FROM basvurular GROUP BY Durum", con))
                    using (var r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var d = r.GetString(0);
                            var n = Convert.ToInt32(r.GetValue(1));
                            if (string.Equals(d, "Beklemede", StringComparison.OrdinalIgnoreCase))
                                ozet.Beklemede = n;
                            else if (string.Equals(d, "Islemde", StringComparison.OrdinalIgnoreCase))
                                ozet.Islemde = n;
                            else if (string.Equals(d, "Tamamlandi", StringComparison.OrdinalIgnoreCase))
                                ozet.Tamamlandi = n;
                        }
                    }
                }
                catch (MySqlException)
                {
                }
            }
            return ozet;
        }

        internal static void YukleGenelSayilar(out int toplamBasvuru, out int kayitliVatandas)
        {
            toplamBasvuru = 0;
            kayitliVatandas = 0;
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                {
                    con.Open();
                    kayitliVatandas = GuvenliTekSay(con, "SELECT COUNT(*) FROM kullanicilar");
                    toplamBasvuru = GuvenliTekSay(con, "SELECT COUNT(*) FROM basvurular");
                }
            }
            catch (MySqlException)
            {
            }
        }

        public static bool SifremiUnuttumGuncelle(string tc, string email, string yeniSifre)
        {
            if (string.IsNullOrWhiteSpace(tc) || tc.Trim().Length != 11 || string.IsNullOrWhiteSpace(yeniSifre))
                return false;

            var em = email?.Trim() ?? "";
            var t = tc.Trim();

            using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            {
                con.Open();
                using (var tx = con.BeginTransaction())
                {
                    int n;
                    using (var cmd = new MySqlCommand(
                        "UPDATE kullanicilar SET sifre=@s WHERE tc=@t AND e_Mail=@e LIMIT 1", con, tx))
                    {
                        cmd.Parameters.AddWithValue("@s", yeniSifre);
                        cmd.Parameters.AddWithValue("@t", t);
                        cmd.Parameters.AddWithValue("@e", em);
                        n = cmd.ExecuteNonQuery();
                    }
                    if (n == 0)
                    {
                        tx.Rollback();
                        return false;
                    }
                    using (var cmd2 = new MySqlCommand(@"
UPDATE personeller p
INNER JOIN kullanicilar k ON p.KullaniciAdi = k.KullaniciAdi
SET p.Sifre = @s
WHERE k.tc = @t AND k.e_Mail = @e", con, tx))
                    {
                        cmd2.Parameters.AddWithValue("@s", yeniSifre);
                        cmd2.Parameters.AddWithValue("@t", t);
                        cmd2.Parameters.AddWithValue("@e", em);
                        cmd2.ExecuteNonQuery();
                    }
                    tx.Commit();
                    return true;
                }
            }
        }

        /// <summary>Yönetici olup olmadığını yoneticiler tablosunda arar.</summary>
        public static bool YoneticiMi(string kullaniciAdi)
        {
            if (string.IsNullOrWhiteSpace(kullaniciAdi))
                return false;
            using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            using (var cmd = new MySqlCommand(
                "SELECT 1 FROM yoneticiler WHERE KullaniciAdi=@k AND Aktif=1 LIMIT 1", con))
            {
                cmd.Parameters.AddWithValue("@k", kullaniciAdi.Trim());
                con.Open();
                return cmd.ExecuteScalar() != null;
            }
        }

        public static DataTable PersonelListesiGetir()
        {
            var dt = new DataTable();
            using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            using (var da = new MySqlDataAdapter(
                "SELECT Id, KullaniciAdi, Ad, Soyad, Departman, Aktif FROM personeller ORDER BY KullaniciAdi", con))
            {
                con.Open();
                da.Fill(dt);
            }
            return dt;
        }

        public static DataTable YoneticiListesiGetir()
        {
            var dt = new DataTable();
            using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            using (var da = new MySqlDataAdapter(
                "SELECT Id, KullaniciAdi, Ad, Soyad, Unvan, Aktif FROM yoneticiler ORDER BY KullaniciAdi", con))
            {
                con.Open();
                da.Fill(dt);
            }
            return dt;
        }

        public static DataTable KullanicilarPersoneleUygunGetir()
        {
            var dt = new DataTable();
            using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            using (var da = new MySqlDataAdapter(@"
SELECT k.KullaniciAdi, k.ad, k.soyad, k.tc
FROM kullanicilar k
LEFT JOIN personeller p ON p.KullaniciAdi = k.KullaniciAdi AND p.Aktif = 1
WHERE p.Id IS NULL
ORDER BY k.KullaniciAdi", con))
            {
                con.Open();
                da.Fill(dt);
            }
            return dt;
        }

        public static string PersonelEkle(string kullaniciAdi, string sifre, string ad, string soyad, string departman = null)
        {
            if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre))
                return "Kullanıcı adı ve şifre zorunlu.";

            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand(@"
INSERT INTO personeller (KullaniciAdi, Sifre, Ad, Soyad, Departman, Aktif)
VALUES (@k, @s, @a, @soy, @dep, 1)", con))
                {
                    cmd.Parameters.AddWithValue("@k", kullaniciAdi.Trim());
                    cmd.Parameters.AddWithValue("@s", sifre);
                    cmd.Parameters.AddWithValue("@a", (object)ad ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@soy", (object)soyad ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@dep", string.IsNullOrWhiteSpace(departman) ? (object)DBNull.Value : departman.Trim());
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                return "Bu kullanıcı adı zaten kayıtlı.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string YoneticiEkle(string kullaniciAdi, string sifre, string ad, string soyad, string unvan = null)
        {
            if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre))
                return "Kullanıcı adı ve şifre zorunlu.";
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand(@"
INSERT INTO yoneticiler (KullaniciAdi, Sifre, Ad, Soyad, Unvan, Aktif)
VALUES (@k, @s, @a, @soy, @unv, 1)", con))
                {
                    cmd.Parameters.AddWithValue("@k", kullaniciAdi.Trim());
                    cmd.Parameters.AddWithValue("@s", sifre);
                    cmd.Parameters.AddWithValue("@a", (object)ad ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@soy", (object)soyad ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@unv", string.IsNullOrWhiteSpace(unvan) ? (object)DBNull.Value : unvan.Trim());
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (MySqlException ex) when (ex.Number == 1062) { return "Bu kullanıcı adı zaten kayıtlı."; }
            catch (Exception ex) { return ex.Message; }
        }

        public static string KullaniciyiPersoneleAktar(string kullaniciAdi)
        {
            if (string.IsNullOrWhiteSpace(kullaniciAdi))
                return "Kullanıcı seçilmedi.";

            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand(@"
INSERT INTO personeller (KullaniciAdi, Sifre, Ad, Soyad, Aktif, Yonetici)
SELECT k.KullaniciAdi, k.sifre, k.ad, k.soyad, 1, 0
FROM kullanicilar k WHERE k.KullaniciAdi=@k
ON DUPLICATE KEY UPDATE
  Sifre = VALUES(Sifre),
  Ad = VALUES(Ad),
  Soyad = VALUES(Soyad),
  Aktif = 1,
  Yonetici = personeller.Yonetici", con))
                {
                    cmd.Parameters.AddWithValue("@k", kullaniciAdi.Trim());
                    con.Open();
                    var n = cmd.ExecuteNonQuery();
                    if (n <= 0)
                        return "Vatandaş kaydı bulunamadı.";
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string PersonelAktifDegistir(int id, bool aktif, string oturumKullaniciAdi)
        {
            using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            {
                con.Open();
                string kadi = null;
                using (var q = new MySqlCommand("SELECT KullaniciAdi FROM personeller WHERE Id=@id LIMIT 1", con))
                {
                    q.Parameters.AddWithValue("@id", id);
                    using (var r = q.ExecuteReader())
                    {
                        if (!r.Read()) return "Kayıt yok.";
                        kadi = r.GetString(0);
                    }
                }
                if (!aktif && string.Equals(kadi, oturumKullaniciAdi, StringComparison.OrdinalIgnoreCase))
                    return "Kendi hesabınızı pasifleştiremezsiniz.";

                using (var u = new MySqlCommand("UPDATE personeller SET Aktif=@a WHERE Id=@id", con))
                {
                    u.Parameters.AddWithValue("@a", aktif ? 1 : 0);
                    u.Parameters.AddWithValue("@id", id);
                    u.ExecuteNonQuery();
                }
            }
            return null;
        }

        public static string YoneticiAktifDegistir(int id, bool aktif, string oturumKullaniciAdi)
        {
            using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            {
                con.Open();
                string kadi = null;
                using (var q = new MySqlCommand("SELECT KullaniciAdi FROM yoneticiler WHERE Id=@id LIMIT 1", con))
                {
                    q.Parameters.AddWithValue("@id", id);
                    using (var r = q.ExecuteReader())
                    {
                        if (!r.Read()) return "Kayıt yok.";
                        kadi = r.GetString(0);
                    }
                }
                if (!aktif && string.Equals(kadi, oturumKullaniciAdi, StringComparison.OrdinalIgnoreCase))
                    return "Kendi yönetici hesabınızı pasifleştiremezsiniz.";
                // Son yönetici kontrolü
                if (!aktif)
                {
                    int diger;
                    using (var q = new MySqlCommand("SELECT COUNT(*) FROM yoneticiler WHERE Aktif=1 AND Id<>@id", con))
                    {
                        q.Parameters.AddWithValue("@id", id);
                        diger = Convert.ToInt32(q.ExecuteScalar());
                    }
                    if (diger <= 0) return "Sistemde en az bir aktif yönetici kalmalıdır.";
                }
                using (var u = new MySqlCommand("UPDATE yoneticiler SET Aktif=@a WHERE Id=@id", con))
                {
                    u.Parameters.AddWithValue("@a", aktif ? 1 : 0);
                    u.Parameters.AddWithValue("@id", id);
                    u.ExecuteNonQuery();
                }
            }
            return null;
        }

        public static DataTable VatandasListesiGetir()
        {
            var dt = new DataTable();
            using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            using (var da = new MySqlDataAdapter("SELECT Id, ad, soyad, tc, e_Mail, KullaniciAdi FROM kullanicilar ORDER BY Id DESC", con))
            {
                con.Open();
                da.Fill(dt);
            }
            return dt;
        }

        public static string VatandasGuncelle(int id, string ad, string soyad, string tc, string email, string kadi, string sifre)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                {
                    con.Open();
                    string sql = string.IsNullOrWhiteSpace(sifre) 
                        ? "UPDATE kullanicilar SET ad=@a, soyad=@s, tc=@t, e_Mail=@e, KullaniciAdi=@k WHERE Id=@id"
                        : "UPDATE kullanicilar SET ad=@a, soyad=@s, tc=@t, e_Mail=@e, KullaniciAdi=@k, sifre=@p WHERE Id=@id";

                    using (var cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@a", ad);
                        cmd.Parameters.AddWithValue("@s", soyad);
                        cmd.Parameters.AddWithValue("@t", tc);
                        cmd.Parameters.AddWithValue("@e", email);
                        cmd.Parameters.AddWithValue("@k", kadi);
                        if (!string.IsNullOrWhiteSpace(sifre))
                            cmd.Parameters.AddWithValue("@p", sifre);

                        cmd.ExecuteNonQuery();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string VatandasSil(int id)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand("DELETE FROM kullanicilar WHERE Id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static DataTable BasvuruListesiGetir()
        {
            var dt = new DataTable();
            using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            using (var da = new MySqlDataAdapter("SELECT Id, Konu, Kategori, Durum, VatandasTC, KayitTarihi FROM basvurular ORDER BY KayitTarihi DESC", con))
            {
                con.Open();
                da.Fill(dt);
            }
            return dt;
        }

        /// <summary>Tüm başvuruları vatandaş ad/soyadı ile birlikte getirir (personel ekranı için).</summary>
        public static DataTable BasvuruListesiDetayliGetir(string kategoriFiltre = null, string durumFiltre = null, string aramaMetni = null, string personelKadi = null)
        {
            var dt = new DataTable();
            try
            {
                // "Tümü", "Tumu", "Tüm", "Tum", boş → filtre yok
                bool katAktif = !string.IsNullOrWhiteSpace(kategoriFiltre) && !FiltreSifirla(kategoriFiltre);
                bool durAktif = !string.IsNullOrWhiteSpace(durumFiltre)   && !FiltreSifirla(durumFiltre);

                string sql = @"
SELECT b.Id, 
       COALESCE(CONCAT(k.ad,' ',k.soyad), b.VatandasTC, 'Anonim') AS VatandasAdi,
       b.VatandasTC,
       b.Kategori, b.Konu, b.Aciklama, b.Durum, b.KayitTarihi,
       COALESCE(b.AtananDepartman, '') AS AtananDepartman,
       COALESCE(b.AtananPersonelKadi, '') AS AtananPersonelKadi,
       COALESCE(CONCAT(p.Ad, ' ', p.Soyad), b.AtananPersonelKadi, '') AS AtananPersonelAdi
FROM basvurular b
LEFT JOIN kullanicilar k ON k.tc = b.VatandasTC
LEFT JOIN personeller p ON p.KullaniciAdi = b.AtananPersonelKadi
WHERE 1=1";
                if (katAktif)
                    sql += " AND b.Kategori = @kat";
                if (durAktif)
                    sql += " AND b.Durum = @dur";
                if (!string.IsNullOrWhiteSpace(aramaMetni))
                    sql += " AND (b.Konu LIKE @ara OR b.VatandasTC LIKE @ara OR k.ad LIKE @ara OR k.soyad LIKE @ara)";
                if (!string.IsNullOrWhiteSpace(personelKadi))
                    sql += " AND b.AtananPersonelKadi = @persKadi";
                sql += " ORDER BY b.KayitTarihi DESC";

                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter(sql, con))
                {
                    if (katAktif)
                        da.SelectCommand.Parameters.AddWithValue("@kat", kategoriFiltre.Trim());
                    if (durAktif)
                        da.SelectCommand.Parameters.AddWithValue("@dur", durumFiltre.Trim());
                    if (!string.IsNullOrWhiteSpace(aramaMetni))
                        da.SelectCommand.Parameters.AddWithValue("@ara", "%" + aramaMetni.Trim() + "%");
                    if (!string.IsNullOrWhiteSpace(personelKadi))
                        da.SelectCommand.Parameters.AddWithValue("@persKadi", personelKadi.Trim());
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        /// <summary>Filtre değerinin "tümü" anlamına gelip gelmediğini kontrol eder.</summary>
        private static bool FiltreSifirla(string deger)
        {
            if (string.IsNullOrWhiteSpace(deger)) return true;
            string d = deger.Trim().ToLowerInvariant();
            return d == "tümü" || d == "tumu" || d == "tüm" || d == "tum" || d == "hepsi" || d == "all";
        }

        /// <summary>Tek bir başvurunun detay bilgilerini getirir.</summary>
        public static DataTable BasvuruDetayiGetir(int id)
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter(@"
SELECT b.Id, b.Konu, b.Kategori, b.Aciklama, b.Durum, b.VatandasTC, b.KayitTarihi,
       COALESCE(CONCAT(k.ad,' ',k.soyad), b.VatandasTC, 'Anonim') AS VatandasAdi,
       k.e_Mail AS VatandasEmail,
       b.AtananDepartman, b.AtananPersonelKadi,
       COALESCE(CONCAT(p.Ad, ' ', p.Soyad), b.AtananPersonelKadi, 'Atanmadı') AS AtananPersonelAdi
FROM basvurular b
LEFT JOIN kullanicilar k ON k.tc = b.VatandasTC
LEFT JOIN personeller p ON p.KullaniciAdi = b.AtananPersonelKadi
WHERE b.Id = @id LIMIT 1", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@id", id);
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        /// <summary>Başvuruya personel notu ekler.</summary>
        public static string BasvuruNotEkle(int basvuruId, string personelAdi, string not)
        {
            if (string.IsNullOrWhiteSpace(not)) return "Not boş bırakılamaz.";
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand(
                    "INSERT INTO basvuru_notlari (BasvuruId, PersonelAdi, Not) VALUES (@bid, @p, @n)", con))
                {
                    cmd.Parameters.AddWithValue("@bid", basvuruId);
                    cmd.Parameters.AddWithValue("@p", personelAdi.Trim());
                    cmd.Parameters.AddWithValue("@n", not.Trim());
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        /// <summary>Bir başvurunun tüm notlarını getirir.</summary>
        public static DataTable BasvuruNotlariniGetir(int basvuruId)
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter(
                    "SELECT PersonelAdi, Not, EklenmeTarihi FROM basvuru_notlari WHERE BasvuruId=@id ORDER BY EklenmeTarihi DESC", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@id", basvuruId);
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        public static string BasvuruDurumGuncelle(int id, string durum)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand("UPDATE basvurular SET Durum=@d WHERE Id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@d", durum);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // ── Bildirim Metodları ───────────────────────────────────────────────

        public static string BildirimGonder(string gonderen, string aliciKadi, string baslik, string icerik)
        {
            if (string.IsNullOrWhiteSpace(baslik) || string.IsNullOrWhiteSpace(icerik))
                return "Başlık ve içerik zorunludur.";
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand(@"
INSERT INTO bildirimler (Baslik, Icerik, GonderenKadi, AliciKadi)
VALUES (@b, @i, @g, @a)", con))
                {
                    cmd.Parameters.AddWithValue("@b", baslik.Trim());
                    cmd.Parameters.AddWithValue("@i", icerik.Trim());
                    cmd.Parameters.AddWithValue("@g", gonderen.Trim());
                    cmd.Parameters.AddWithValue("@a", string.IsNullOrWhiteSpace(aliciKadi) ? (object)DBNull.Value : aliciKadi.Trim());
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        public static DataTable BildirimleriGetir(string kullaniciAdi)
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter(@"
SELECT Id, Baslik, Icerik, GonderenKadi, GonderimTarihi, Okundu
FROM bildirimler
WHERE AliciKadi IS NULL 
   OR AliciKadi = @k 
   OR (AliciKadi = 'TUM_VATANDAS' AND EXISTS(SELECT 1 FROM kullanicilar WHERE tc=@k))
   OR (AliciKadi = 'TUM_PERSONEL' AND NOT EXISTS(SELECT 1 FROM kullanicilar WHERE tc=@k))
ORDER BY GonderimTarihi DESC", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@k", kullaniciAdi.Trim());
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        public static int OkunmamisBildirimSayisi(string kullaniciAdi)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand(@"
SELECT COUNT(*) FROM bildirimler
WHERE (AliciKadi IS NULL OR AliciKadi = @k) AND Okundu = 0", con))
                {
                    cmd.Parameters.AddWithValue("@k", kullaniciAdi.Trim());
                    con.Open();
                    var r = cmd.ExecuteScalar();
                    return r == null ? 0 : Convert.ToInt32(r);
                }
            }
            catch { return 0; }
        }

        public static void BildirimleriOkunduIsaretle(string kullaniciAdi)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand(@"
UPDATE bildirimler SET Okundu=1
WHERE (AliciKadi IS NULL OR AliciKadi = @k) AND Okundu=0", con))
                {
                    cmd.Parameters.AddWithValue("@k", kullaniciAdi.Trim());
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
        }

        public static DataTable PersonelListesiAdiyla()
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter(
                    "SELECT KullaniciAdi, CONCAT(Ad,' ',Soyad) AS AdSoyad FROM personeller WHERE Aktif=1 ORDER BY Ad", con))
                {
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        // ── Duyuru Metodları ─────────────────────────────────────────────────

        public static DataTable DuyurulariGetir(bool sadecAktif = true)
        {
            var dt = new DataTable();
            try
            {
                string sql = sadecAktif
                    ? "SELECT Id, Baslik, Icerik, YayinTarihi FROM duyurular WHERE Aktif=1 ORDER BY YayinTarihi DESC"
                    : "SELECT Id, Baslik, Icerik, YayinTarihi, Aktif FROM duyurular ORDER BY YayinTarihi DESC";
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter(sql, con))
                {
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        public static string DuyuruEkle(string baslik, string icerik)
        {
            if (string.IsNullOrWhiteSpace(baslik) || string.IsNullOrWhiteSpace(icerik))
                return "Başlık ve içerik zorunludur.";
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand(
                    "INSERT INTO duyurular (Baslik, Icerik) VALUES (@b, @i)", con))
                {
                    cmd.Parameters.AddWithValue("@b", baslik.Trim());
                    cmd.Parameters.AddWithValue("@i", icerik.Trim());
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        public static string DuyuruGuncelle(int id, string baslik, string icerik)
        {
            if (string.IsNullOrWhiteSpace(baslik) || string.IsNullOrWhiteSpace(icerik))
                return "Başlık ve içerik zorunludur.";
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand(
                    "UPDATE duyurular SET Baslik=@b, Icerik=@i WHERE Id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@b", baslik.Trim());
                    cmd.Parameters.AddWithValue("@i", icerik.Trim());
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        public static string DuyuruAktifDegistir(int id, bool aktif)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand("UPDATE duyurular SET Aktif=@a WHERE Id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@a", aktif ? 1 : 0);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        public static string DuyuruSil(int id)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand("DELETE FROM duyurular WHERE Id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        // ── Sistem Logu ────────────────────────────────────────────────────────

        public static void SistemLoguEkle(string kullanici, string rol, string islem)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand(
                    "INSERT INTO sistem_logu (Kullanici, Rol, Islem) VALUES (@k, @r, @i)", con))
                {
                    cmd.Parameters.AddWithValue("@k", kullanici ?? "");
                    cmd.Parameters.AddWithValue("@r", rol ?? "");
                    cmd.Parameters.AddWithValue("@i", islem ?? "");
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
        }

        public static DataTable SistemLoguGetir(int limit = 100)
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter(
                    $"SELECT Id, Kullanici, Rol, Islem, Tarih FROM sistem_logu ORDER BY Tarih DESC LIMIT {limit}", con))
                {
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
        }
        public static DataTable SistemLoglariGetir(string arama)
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                {
                    string sql = @"
SELECT Id, Kullanici AS KullaniciAdi, Rol, Islem, Tarih 
FROM sistem_logu";
                    if (!string.IsNullOrWhiteSpace(arama))
                    {
                        sql += " WHERE Kullanici LIKE @a OR Rol LIKE @a OR Islem LIKE @a";
                    }
                    sql += " ORDER BY Tarih DESC LIMIT 250";

                    using (var da = new MySqlDataAdapter(sql, con))
                    {
                        if (!string.IsNullOrWhiteSpace(arama))
                            da.SelectCommand.Parameters.AddWithValue("@a", "%" + arama.Trim() + "%");
                        con.Open();
                        da.Fill(dt);
                    }
                }
            }
            catch { }
            return dt;
        }
        // ── Rapor Metodları ─────────────────────────────────────────────────────────

        /// <summary>Kategori bazında başvuru sayılarını getirir.</summary>
        public static DataTable KategoriBazliRaporGetir()
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter(
                    "SELECT Kategori, COUNT(*) AS Adet FROM basvurular GROUP BY Kategori ORDER BY Adet DESC", con))
                {
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        /// <summary>Durum bazında başvuru sayılarını getirir.</summary>
        public static DataTable DurumBazliRaporGetir()
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter(
                    "SELECT Durum, COUNT(*) AS Adet FROM basvurular GROUP BY Durum ORDER BY Adet DESC", con))
                {
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        /// <summary>Son 30 günün günlük başvuru sayılarını getirir.</summary>
        public static DataTable GunlukBasvuruRaporGetir()
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter(@"
SELECT DATE(KayitTarihi) AS Gun, COUNT(*) AS Adet 
FROM basvurular 
WHERE KayitTarihi >= DATE_SUB(NOW(), INTERVAL 30 DAY)
GROUP BY DATE(KayitTarihi) 
ORDER BY Gun", con))
                {
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        // ── Vatandaş Başvuru Metodları ───────────────────────────────────────

        public static DataTable VatandasBasvurulariniGetir(string tc)
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter(@"
SELECT Id, Konu, Kategori, Durum, KayitTarihi
FROM basvurular WHERE VatandasTC = @tc
ORDER BY KayitTarihi DESC", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@tc", tc.Trim());
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        public static string VatandasBasvuruEkle(string tc, string konu, string kategori, string aciklama)
        {
            if (string.IsNullOrWhiteSpace(konu))
                return "Konu zorunludur.";
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand(@"
INSERT INTO basvurular (Konu, Kategori, Aciklama, VatandasTC)
VALUES (@k, @kat, @a, @tc)", con))
                {
                    cmd.Parameters.AddWithValue("@k", konu.Trim());
                    cmd.Parameters.AddWithValue("@kat", string.IsNullOrWhiteSpace(kategori) ? "Genel" : kategori.Trim());
                    cmd.Parameters.AddWithValue("@a", string.IsNullOrWhiteSpace(aciklama) ? (object)DBNull.Value : aciklama.Trim());
                    cmd.Parameters.AddWithValue("@tc", tc.Trim());
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        // ── Personel → Vatandaş Profil Güncelleme ───────────────────────────

        public static string PersonelVatandasBilgiGuncelle(int id, string ad, string soyad, string email)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand(
                    "UPDATE kullanicilar SET ad=@a, soyad=@s, e_Mail=@e WHERE Id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@a", (ad ?? "").Trim());
                    cmd.Parameters.AddWithValue("@s", (soyad ?? "").Trim());
                    cmd.Parameters.AddWithValue("@e", (email ?? "").Trim());
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        public static DataTable DepartmanlariGetir()
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter(
                    "SELECT DISTINCT Departman FROM personeller WHERE Departman IS NOT NULL AND Departman <> '' AND Aktif = 1 ORDER BY Departman", con))
                {
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        public static DataTable DepartmanPersonelleriniGetir(string departman)
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                {
                    string sql = string.IsNullOrWhiteSpace(departman) || departman == "Tümü" || departman == "(Tümü)"
                        ? "SELECT KullaniciAdi, CONCAT(Ad, ' ', Soyad) AS AdSoyad FROM personeller WHERE Aktif = 1 ORDER BY Ad"
                        : "SELECT KullaniciAdi, CONCAT(Ad, ' ', Soyad) AS AdSoyad FROM personeller WHERE Departman = @dep AND Aktif = 1 ORDER BY Ad";
                    using (var da = new MySqlDataAdapter(sql, con))
                    {
                        if (!string.IsNullOrWhiteSpace(departman) && departman != "Tümü" && departman != "(Tümü)")
                            da.SelectCommand.Parameters.AddWithValue("@dep", departman.Trim());
                        con.Open();
                        da.Fill(dt);
                    }
                }
            }
            catch { }
            return dt;
        }

        public static string BasvuruPersonelAta(int basvuruId, string departman, string personelKadi, string gonderenYonetici)
        {
            if (string.IsNullOrWhiteSpace(personelKadi))
                return "Personel seçimi zorunludur.";
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                {
                    con.Open();
                    string konu = "Başvuru";
                    using (var cmdK = new MySqlCommand("SELECT Konu FROM basvurular WHERE Id=@id LIMIT 1", con))
                    {
                        cmdK.Parameters.AddWithValue("@id", basvuruId);
                        var obj = cmdK.ExecuteScalar();
                        if (obj != null && obj != DBNull.Value)
                            konu = obj.ToString();
                    }

                    using (var cmd = new MySqlCommand(
                        "UPDATE basvurular SET AtananDepartman=@d, AtananPersonelKadi=@p WHERE Id=@id", con))
                    {
                        cmd.Parameters.AddWithValue("@id", basvuruId);
                        cmd.Parameters.AddWithValue("@d", string.IsNullOrWhiteSpace(departman) ? (object)DBNull.Value : departman.Trim());
                        cmd.Parameters.AddWithValue("@p", personelKadi.Trim());
                        cmd.ExecuteNonQuery();
                    }

                    string baslik = "Yeni Başvuru Atandı (No: " + basvuruId + ")";
                    string icerik = "Size yeni bir başvuru atandı. Konu: " + konu;
                    using (var cmdN = new MySqlCommand(@"
                        INSERT INTO bildirimler (Baslik, Icerik, GonderenKadi, AliciKadi)
                        VALUES (@b, @i, @g, @a)", con))
                    {
                        cmdN.Parameters.AddWithValue("@b", baslik);
                        cmdN.Parameters.AddWithValue("@i", icerik);
                        cmdN.Parameters.AddWithValue("@g", gonderenYonetici ?? "Sistem");
                        cmdN.Parameters.AddWithValue("@a", personelKadi.Trim());
                        cmdN.ExecuteNonQuery();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // ── Gelismiş Bildirim Yonetim Metodları ───────────────────────────────

        public static string BildirimSil(int bildirimId)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand("DELETE FROM bildirimler WHERE Id = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", bildirimId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        public static string TumBildirimleriTemizle(string kullaniciAdi)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand("DELETE FROM bildirimler WHERE AliciKadi = @k OR AliciKadi IS NULL", con))
                {
                    cmd.Parameters.AddWithValue("@k", kullaniciAdi.Trim());
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        // ── Detaylı Vatandas Profil Yonetimi ────────────────────────────────

        public static string VatandasTamProfilGuncelle(string tc, string ad, string soyad, string email, string kadi, string telefon, string adres, string yeniSifre = null)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                {
                    con.Open();

                    // Check if columns exist first (to make it robust)
                    bool hasTel = false;
                    bool hasAdres = false;
                    using (var cmdCol = new MySqlCommand("SHOW COLUMNS FROM kullanicilar", con))
                    using (var reader = cmdCol.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string col = reader.GetString(0);
                            if (string.Equals(col, "telefon", StringComparison.OrdinalIgnoreCase)) hasTel = true;
                            if (string.Equals(col, "adres", StringComparison.OrdinalIgnoreCase)) hasAdres = true;
                        }
                    }

                    if (!hasTel)
                    {
                        using (var cmd = new MySqlCommand("ALTER TABLE kullanicilar ADD COLUMN telefon varchar(20) DEFAULT NULL", con))
                            cmd.ExecuteNonQuery();
                    }
                    if (!hasAdres)
                    {
                        using (var cmd = new MySqlCommand("ALTER TABLE kullanicilar ADD COLUMN adres text DEFAULT NULL", con))
                            cmd.ExecuteNonQuery();
                    }

                    string sql = @"
UPDATE kullanicilar 
SET ad=@a, soyad=@s, e_Mail=@e, KullaniciAdi=@k, telefon=@t, adres=@adr";
                    if (!string.IsNullOrWhiteSpace(yeniSifre))
                    {
                        sql += ", sifre=@p";
                    }
                    sql += " WHERE tc=@tc";

                    using (var cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@tc", tc.Trim());
                        cmd.Parameters.AddWithValue("@a", ad.Trim());
                        cmd.Parameters.AddWithValue("@s", soyad.Trim());
                        cmd.Parameters.AddWithValue("@e", email.Trim());
                        cmd.Parameters.AddWithValue("@k", kadi.Trim());
                        cmd.Parameters.AddWithValue("@t", string.IsNullOrWhiteSpace(telefon) ? (object)DBNull.Value : telefon.Trim());
                        cmd.Parameters.AddWithValue("@adr", string.IsNullOrWhiteSpace(adres) ? (object)DBNull.Value : adres.Trim());
                        if (!string.IsNullOrWhiteSpace(yeniSifre))
                        {
                            cmd.Parameters.AddWithValue("@p", yeniSifre);
                        }
                        cmd.ExecuteNonQuery();
                    }
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        public static DataTable VatandasTamProfilGetir(string tc)
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                {
                    con.Open();
                    // Column robustness check
                    bool hasTel = false;
                    bool hasAdres = false;
                    using (var cmdCol = new MySqlCommand("SHOW COLUMNS FROM kullanicilar", con))
                    using (var reader = cmdCol.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string col = reader.GetString(0);
                            if (string.Equals(col, "telefon", StringComparison.OrdinalIgnoreCase)) hasTel = true;
                            if (string.Equals(col, "adres", StringComparison.OrdinalIgnoreCase)) hasAdres = true;
                        }
                    }
                    if (!hasTel)
                    {
                        using (var cmd = new MySqlCommand("ALTER TABLE kullanicilar ADD COLUMN telefon varchar(20) DEFAULT NULL", con))
                            cmd.ExecuteNonQuery();
                    }
                    if (!hasAdres)
                    {
                        using (var cmd = new MySqlCommand("ALTER TABLE kullanicilar ADD COLUMN adres text DEFAULT NULL", con))
                            cmd.ExecuteNonQuery();
                    }

                    using (var da = new MySqlDataAdapter("SELECT ad, soyad, tc, e_Mail, KullaniciAdi, telefon, adres FROM kullanicilar WHERE tc=@tc LIMIT 1", con))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@tc", tc.Trim());
                        da.Fill(dt);
                    }
                }
            }
            catch { }
            return dt;
        }

        // ── Gelişmiş Raporlama & Analiz Metodları ────────────────────────────

        public static DataTable DepartmanBazliPerformansRaporu()
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter(@"
SELECT 
    COALESCE(b.AtananDepartman, 'Atanmamış') AS Departman,
    COUNT(*) AS ToplamTalep,
    SUM(CASE WHEN b.Durum = 'Tamamlandi' THEN 1 ELSE 0 END) AS Tamamlanan,
    SUM(CASE WHEN b.Durum = 'Islemde' THEN 1 ELSE 0 END) AS Islemdeki,
    SUM(CASE WHEN b.Durum = 'Beklemede' THEN 1 ELSE 0 END) AS Bekleyen,
    ROUND(SUM(CASE WHEN b.Durum = 'Tamamlandi' THEN 1 ELSE 0 END) * 100.0 / COUNT(*), 1) AS BasariOrani
FROM basvurular b
GROUP BY b.AtananDepartman
ORDER BY ToplamTalep DESC", con))
                {
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        public static DataTable PersonelBazliPerformansRaporu()
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter(@"
SELECT 
    COALESCE(CONCAT(p.Ad, ' ', p.Soyad), b.AtananPersonelKadi, 'Atanmamış') AS Personel,
    COALESCE(p.Departman, '-') AS Departman,
    COUNT(*) AS ToplamTalep,
    SUM(CASE WHEN b.Durum = 'Tamamlandi' THEN 1 ELSE 0 END) AS Tamamlanan,
    ROUND(SUM(CASE WHEN b.Durum = 'Tamamlandi' THEN 1 ELSE 0 END) * 100.0 / COUNT(*), 1) AS BitirmeOrani
FROM basvurular b
LEFT JOIN personeller p ON p.KullaniciAdi = b.AtananPersonelKadi
GROUP BY b.AtananPersonelKadi, p.Ad, p.Soyad, p.Departman
ORDER BY ToplamTalep DESC", con))
                {
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        public static string SistemLoglariniTemizle()
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand("TRUNCATE TABLE sistem_logu", con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        // ── Finans ve Borç Tahsilat Metodları ──────────────────────────────

        public static DataTable VatandasBorclariniGetir(string tc)
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter("SELECT Id, Aciklama, Miktar, SonOdemeTarihi, OdenmeTarihi, Durum FROM borclar WHERE VatandasTC=@tc ORDER BY Durum ASC, SonOdemeTarihi DESC", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@tc", tc.Trim());
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        public static string BorcOde(int id)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand("UPDATE borclar SET Durum='Odendi', OdenmeTarihi=NOW() WHERE Id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        public static DataTable TumBorclariGetirDetayli()
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter(@"
SELECT b.Id, b.VatandasTC, COALESCE(CONCAT(k.ad, ' ', k.soyad), 'Kayıtsız Vatandaş') AS VatandasAdi, b.Aciklama, b.Miktar, b.SonOdemeTarihi, b.OdenmeTarihi, b.Durum 
FROM borclar b 
LEFT JOIN kullanicilar k ON b.VatandasTC = k.tc 
ORDER BY b.Id DESC", con))
                {
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        public static string BorcEkle(string tc, string aciklama, decimal miktar, DateTime sonOdemeTarihi)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand("INSERT INTO borclar (VatandasTC, Aciklama, Miktar, SonOdemeTarihi, Durum) VALUES (@tc, @a, @m, @s, 'Odenmedi')", con))
                {
                    cmd.Parameters.AddWithValue("@tc", tc.Trim());
                    cmd.Parameters.AddWithValue("@a", aciklama.Trim());
                    cmd.Parameters.AddWithValue("@m", miktar);
                    cmd.Parameters.AddWithValue("@s", sonOdemeTarihi);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        public static void FinansOzetiGetir(out decimal toplamTahsilat, out decimal bekleyenAlacak)
        {
            toplamTahsilat = 0;
            bekleyenAlacak = 0;
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                {
                    con.Open();
                    using (var cmd1 = new MySqlCommand("SELECT SUM(Miktar) FROM borclar WHERE Durum='Odendi'", con))
                    {
                        var res = cmd1.ExecuteScalar();
                        if (res != DBNull.Value && res != null) toplamTahsilat = Convert.ToDecimal(res);
                    }
                    using (var cmd2 = new MySqlCommand("SELECT SUM(Miktar) FROM borclar WHERE Durum='Odenmedi'", con))
                    {
                        var res = cmd2.ExecuteScalar();
                        if (res != DBNull.Value && res != null) bekleyenAlacak = Convert.ToDecimal(res);
                    }
                }
            }
            catch { }
        }

        public static DataTable BorcDetayGetir(int id)
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter(@"
SELECT b.Id, b.VatandasTC, COALESCE(CONCAT(k.ad, ' ', k.soyad), 'Kayıtsız Vatandaş') AS VatandasAdi, b.Aciklama, b.Miktar, b.SonOdemeTarihi, b.OdenmeTarihi, b.Durum 
FROM borclar b 
LEFT JOIN kullanicilar k ON b.VatandasTC = k.tc 
WHERE b.Id = @id LIMIT 1", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@id", id);
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        // ── Personel Görev Takibi Metodları ─────────────────────────────────────────

        public static DataTable PersonelGorevleriGetir(string kadi, bool isYonetici)
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                {
                    string sql = isYonetici
                        ? "SELECT Id, Baslik, Aciklama, AtananPersonelKadi, VerenKadi, Oncelik, Durum, OlusturmaTarihi FROM personel_gorevleri ORDER BY Id DESC"
                        : "SELECT Id, Baslik, Aciklama, AtananPersonelKadi, VerenKadi, Oncelik, Durum, OlusturmaTarihi FROM personel_gorevleri WHERE AtananPersonelKadi=@k OR VerenKadi=@k ORDER BY Id DESC";

                    using (var da = new MySqlDataAdapter(sql, con))
                    {
                        if (!isYonetici)
                            da.SelectCommand.Parameters.AddWithValue("@k", kadi);
                        con.Open();
                        da.Fill(dt);
                    }
                }
            }
            catch { }
            return dt;
        }

        public static string PersonelGoreviEkle(string baslik, string aciklama, string atananKadi, string verenKadi, string oncelik)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand(
                    "INSERT INTO personel_gorevleri (Baslik, Aciklama, AtananPersonelKadi, VerenKadi, Oncelik, Durum) VALUES (@b, @a, @at, @v, @o, 'Baslanmadi')", con))
                {
                    cmd.Parameters.AddWithValue("@b", baslik.Trim());
                    cmd.Parameters.AddWithValue("@a", aciklama?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@at", atananKadi.Trim());
                    cmd.Parameters.AddWithValue("@v", verenKadi.Trim());
                    cmd.Parameters.AddWithValue("@o", oncelik.Trim());
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        public static string PersonelGorevDurumuGuncelle(int id, string durum)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand("UPDATE personel_gorevleri SET Durum=@d WHERE Id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@d", durum);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        public static string PersonelGorevSil(int id)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand("DELETE FROM personel_gorevleri WHERE Id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        // ── Personel İçi Mesajlaşma (Chat) Metodları ─────────────────────────────────

        public static string MesajGonder(string gonderen, string alici, string mesaj)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand(
                    "INSERT INTO personel_mesajlari (GonderenKadi, AliciKadi, Mesaj) VALUES (@g, @a, @m)", con))
                {
                    cmd.Parameters.AddWithValue("@g", gonderen.Trim());
                    cmd.Parameters.AddWithValue("@a", alici.Trim());
                    cmd.Parameters.AddWithValue("@m", mesaj.Trim());
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        public static DataTable MesajlariGetir(string kadi1, string kadi2)
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                {
                    string sql;
                    if (kadi2 == "GENEL")
                    {
                        sql = "SELECT Id, GonderenKadi, AliciKadi, Mesaj, Tarih FROM personel_mesajlari WHERE AliciKadi='GENEL' ORDER BY Tarih ASC LIMIT 100";
                    }
                    else
                    {
                        sql = "SELECT Id, GonderenKadi, AliciKadi, Mesaj, Tarih FROM personel_mesajlari WHERE (GonderenKadi=@k1 AND AliciKadi=@k2) OR (GonderenKadi=@k2 AND AliciKadi=@k1) ORDER BY Tarih ASC LIMIT 100";
                    }

                    using (var da = new MySqlDataAdapter(sql, con))
                    {
                        if (kadi2 != "GENEL")
                        {
                            da.SelectCommand.Parameters.AddWithValue("@k1", kadi1);
                            da.SelectCommand.Parameters.AddWithValue("@k2", kadi2);
                        }
                        con.Open();
                        da.Fill(dt);
                    }
                }
            }
            catch { }
            return dt;
        }

        public static DataTable TumKullanicilarListesiGetir(string oturumKadi)
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter(@"
                    SELECT KullaniciAdi, CONCAT(Ad, ' ', Soyad) AS AdSoyad, COALESCE(Departman, 'Personel') AS Detay, 'Personel' AS Rol FROM personeller WHERE Aktif=1 AND KullaniciAdi != @ok
                    UNION
                    SELECT KullaniciAdi, CONCAT(Ad, ' ', Soyad) AS AdSoyad, COALESCE(Unvan, 'Yönetici') AS Detay, 'Yonetici' AS Rol FROM yoneticiler WHERE Aktif=1 AND KullaniciAdi != @ok", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@ok", oturumKadi);
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        // ── Personel Profil Metodları ─────────────────────────────────────────

        /// <summary>Personel kendi profil bilgilerini getirir (personeller tablosu).</summary>
        public static DataTable PersonelTamProfilGetir(string kullaniciAdi)
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                {
                    con.Open();
                    // Önce personeller tablosunda telefon/adres kolonlarını güvence altına al
                    bool hasTel = false, hasAdres = false, hasEmail = false;
                    using (var cmdCol = new MySqlCommand("SHOW COLUMNS FROM personeller", con))
                    using (var reader = cmdCol.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string col = reader.GetString(0);
                            if (string.Equals(col, "Telefon", StringComparison.OrdinalIgnoreCase)) hasTel = true;
                            if (string.Equals(col, "Adres", StringComparison.OrdinalIgnoreCase)) hasAdres = true;
                            if (string.Equals(col, "Email", StringComparison.OrdinalIgnoreCase)) hasEmail = true;
                        }
                    }
                    if (!hasTel)
                    {
                        using (var cmd = new MySqlCommand("ALTER TABLE personeller ADD COLUMN Telefon varchar(20) DEFAULT NULL", con))
                            cmd.ExecuteNonQuery();
                    }
                    if (!hasAdres)
                    {
                        using (var cmd = new MySqlCommand("ALTER TABLE personeller ADD COLUMN Adres text DEFAULT NULL", con))
                            cmd.ExecuteNonQuery();
                    }
                    if (!hasEmail)
                    {
                        using (var cmd = new MySqlCommand("ALTER TABLE personeller ADD COLUMN Email varchar(100) DEFAULT NULL", con))
                            cmd.ExecuteNonQuery();
                    }

                    using (var da = new MySqlDataAdapter(
                        "SELECT KullaniciAdi, Ad, Soyad, COALESCE(Email,'') AS Email, COALESCE(Telefon,'') AS Telefon, COALESCE(Adres,'') AS Adres, COALESCE(Departman,'') AS Departman FROM personeller WHERE KullaniciAdi=@k LIMIT 1", con))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@k", kullaniciAdi.Trim());
                        da.Fill(dt);
                    }
                }
            }
            catch { }
            return dt;
        }

        /// <summary>Personel kendi bilgilerini günceller (isteğe bağlı şifre dahil).</summary>
        public static string PersonelTamProfilGuncelle(string kullaniciAdi, string ad, string soyad, string email, string telefon, string adres, string yeniSifre = null)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                {
                    con.Open();
                    string sql = @"UPDATE personeller SET Ad=@a, Soyad=@s, Email=@e, Telefon=@t, Adres=@adr";
                    if (!string.IsNullOrWhiteSpace(yeniSifre))
                        sql += ", Sifre=@p";
                    sql += " WHERE KullaniciAdi=@k";

                    using (var cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@k", kullaniciAdi.Trim());
                        cmd.Parameters.AddWithValue("@a", (ad ?? "").Trim());
                        cmd.Parameters.AddWithValue("@s", (soyad ?? "").Trim());
                        cmd.Parameters.AddWithValue("@e", (email ?? "").Trim());
                        cmd.Parameters.AddWithValue("@t", string.IsNullOrWhiteSpace(telefon) ? (object)DBNull.Value : telefon.Trim());
                        cmd.Parameters.AddWithValue("@adr", string.IsNullOrWhiteSpace(adres) ? (object)DBNull.Value : adres.Trim());
                        if (!string.IsNullOrWhiteSpace(yeniSifre))
                            cmd.Parameters.AddWithValue("@p", yeniSifre);
                        cmd.ExecuteNonQuery();
                    }
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }

        /// <summary>Yönetici kendi profil bilgilerini getirir (yoneticiler tablosu).</summary>
        public static DataTable YoneticiTamProfilGetir(string kullaniciAdi)
        {
            var dt = new DataTable();
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                {
                    con.Open();
                    bool hasTel = false, hasAdres = false, hasEmail = false;
                    using (var cmdCol = new MySqlCommand("SHOW COLUMNS FROM yoneticiler", con))
                    using (var reader = cmdCol.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string col = reader.GetString(0);
                            if (string.Equals(col, "Telefon", StringComparison.OrdinalIgnoreCase)) hasTel = true;
                            if (string.Equals(col, "Adres", StringComparison.OrdinalIgnoreCase)) hasAdres = true;
                            if (string.Equals(col, "Email", StringComparison.OrdinalIgnoreCase)) hasEmail = true;
                        }
                    }
                    if (!hasTel)
                    {
                        using (var cmd = new MySqlCommand("ALTER TABLE yoneticiler ADD COLUMN Telefon varchar(20) DEFAULT NULL", con))
                            cmd.ExecuteNonQuery();
                    }
                    if (!hasAdres)
                    {
                        using (var cmd = new MySqlCommand("ALTER TABLE yoneticiler ADD COLUMN Adres text DEFAULT NULL", con))
                            cmd.ExecuteNonQuery();
                    }
                    if (!hasEmail)
                    {
                        using (var cmd = new MySqlCommand("ALTER TABLE yoneticiler ADD COLUMN Email varchar(100) DEFAULT NULL", con))
                            cmd.ExecuteNonQuery();
                    }

                    using (var da = new MySqlDataAdapter(
                        "SELECT KullaniciAdi, Ad, Soyad, COALESCE(Email,'') AS Email, COALESCE(Telefon,'') AS Telefon, COALESCE(Adres,'') AS Adres, COALESCE(Unvan,'') AS Unvan FROM yoneticiler WHERE KullaniciAdi=@k LIMIT 1", con))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@k", kullaniciAdi.Trim());
                        da.Fill(dt);
                    }
                }
            }
            catch { }
            return dt;
        }

        /// <summary>Yönetici kendi bilgilerini günceller.</summary>
        public static string YoneticiTamProfilGuncelle(string kullaniciAdi, string ad, string soyad, string email, string telefon, string adres, string yeniSifre = null)
        {
            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                {
                    con.Open();
                    string sql = @"UPDATE yoneticiler SET Ad=@a, Soyad=@s, Email=@e, Telefon=@t, Adres=@adr";
                    if (!string.IsNullOrWhiteSpace(yeniSifre))
                        sql += ", Sifre=@p";
                    sql += " WHERE KullaniciAdi=@k";

                    using (var cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@k", kullaniciAdi.Trim());
                        cmd.Parameters.AddWithValue("@a", (ad ?? "").Trim());
                        cmd.Parameters.AddWithValue("@s", (soyad ?? "").Trim());
                        cmd.Parameters.AddWithValue("@e", (email ?? "").Trim());
                        cmd.Parameters.AddWithValue("@t", string.IsNullOrWhiteSpace(telefon) ? (object)DBNull.Value : telefon.Trim());
                        cmd.Parameters.AddWithValue("@adr", string.IsNullOrWhiteSpace(adres) ? (object)DBNull.Value : adres.Trim());
                        if (!string.IsNullOrWhiteSpace(yeniSifre))
                            cmd.Parameters.AddWithValue("@p", yeniSifre);
                        cmd.ExecuteNonQuery();
                    }
                }
                return null;
            }
            catch (Exception ex) { return ex.Message; }
        }
    }
}
