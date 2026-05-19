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
        public static DataTable BasvuruListesiDetayliGetir(string kategoriFiltre = null, string durumFiltre = null, string aramaMetni = null)
        {
            var dt = new DataTable();
            try
            {
                string sql = @"
SELECT b.Id, 
       COALESCE(CONCAT(k.ad,' ',k.soyad), b.VatandasTC, 'Anonim') AS VatandasAdi,
       b.VatandasTC,
       b.Kategori, b.Konu, b.Aciklama, b.Durum, b.KayitTarihi
FROM basvurular b
LEFT JOIN kullanicilar k ON k.tc = b.VatandasTC
WHERE 1=1";
                if (!string.IsNullOrWhiteSpace(kategoriFiltre) && kategoriFiltre != "Tümü")
                    sql += " AND b.Kategori = @kat";
                if (!string.IsNullOrWhiteSpace(durumFiltre) && durumFiltre != "Tümü")
                    sql += " AND b.Durum = @dur";
                if (!string.IsNullOrWhiteSpace(aramaMetni))
                    sql += " AND (b.Konu LIKE @ara OR b.VatandasTC LIKE @ara OR k.ad LIKE @ara OR k.soyad LIKE @ara)";
                sql += " ORDER BY b.KayitTarihi DESC";

                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var da = new MySqlDataAdapter(sql, con))
                {
                    if (!string.IsNullOrWhiteSpace(kategoriFiltre) && kategoriFiltre != "Tümü")
                        da.SelectCommand.Parameters.AddWithValue("@kat", kategoriFiltre);
                    if (!string.IsNullOrWhiteSpace(durumFiltre) && durumFiltre != "Tümü")
                        da.SelectCommand.Parameters.AddWithValue("@dur", durumFiltre);
                    if (!string.IsNullOrWhiteSpace(aramaMetni))
                        da.SelectCommand.Parameters.AddWithValue("@ara", "%" + aramaMetni.Trim() + "%");
                    con.Open();
                    da.Fill(dt);
                }
            }
            catch { }
            return dt;
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
       k.e_Mail AS VatandasEmail
FROM basvurular b
LEFT JOIN kullanicilar k ON k.tc = b.VatandasTC
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
WHERE AliciKadi IS NULL OR AliciKadi = @k
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
    }
}
