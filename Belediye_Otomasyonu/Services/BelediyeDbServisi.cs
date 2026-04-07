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
                ozet.PersonelSayisi = GuvenliTekSay(con, "SELECT COUNT(*) FROM personeller WHERE Aktif=1");
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

        public static bool PersonelYoneticiMi(string kullaniciAdi)
        {
            if (string.IsNullOrWhiteSpace(kullaniciAdi))
                return false;
            using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            using (var cmd = new MySqlCommand(
                "SELECT Yonetici FROM personeller WHERE KullaniciAdi=@k AND Aktif=1 LIMIT 1", con))
            {
                cmd.Parameters.AddWithValue("@k", kullaniciAdi.Trim());
                con.Open();
                var o = cmd.ExecuteScalar();
                if (o == null || o == DBNull.Value)
                    return false;
                return Convert.ToInt32(o) != 0;
            }
        }

        public static DataTable PersonelListesiGetir()
        {
            var dt = new DataTable();
            using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            using (var da = new MySqlDataAdapter(
                "SELECT Id, KullaniciAdi, Ad, Soyad, Aktif, Yonetici FROM personeller ORDER BY KullaniciAdi", con))
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

        public static string PersonelEkle(string kullaniciAdi, string sifre, string ad, string soyad, bool yonetici)
        {
            if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre))
                return "Kullanıcı adı ve şifre zorunlu.";

            try
            {
                using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
                using (var cmd = new MySqlCommand(@"
INSERT INTO personeller (KullaniciAdi, Sifre, Ad, Soyad, Aktif, Yonetici)
VALUES (@k, @s, @a, @soy, 1, @yn)", con))
                {
                    cmd.Parameters.AddWithValue("@k", kullaniciAdi.Trim());
                    cmd.Parameters.AddWithValue("@s", sifre);
                    cmd.Parameters.AddWithValue("@a", (object)ad ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@soy", (object)soyad ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@yn", yonetici ? 1 : 0);
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
            string kadi = null;
            int yonetici = 0;
            using (var con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            {
                con.Open();
                using (var q = new MySqlCommand(
                    "SELECT KullaniciAdi, Yonetici FROM personeller WHERE Id=@id LIMIT 1", con))
                {
                    q.Parameters.AddWithValue("@id", id);
                    using (var r = q.ExecuteReader())
                    {
                        if (!r.Read())
                            return "Kayıt yok.";
                        kadi = r.GetString(0);
                        yonetici = Convert.ToInt32(r.GetValue(1));
                    }
                }

                if (!aktif && string.Equals(kadi, oturumKullaniciAdi, StringComparison.OrdinalIgnoreCase))
                    return "Kendi hesabınızı pasifleştiremezsiniz.";

                if (!aktif && yonetici != 0)
                {
                    int diger;
                    using (var q = new MySqlCommand(
                        "SELECT COUNT(*) FROM personeller WHERE Yonetici=1 AND Aktif=1 AND Id<>@id", con))
                    {
                        q.Parameters.AddWithValue("@id", id);
                        diger = Convert.ToInt32(q.ExecuteScalar());
                    }
                    if (diger <= 0)
                        return "Son yönetici hesabı pasifleştirilemez.";
                }

                using (var u = new MySqlCommand("UPDATE personeller SET Aktif=@a WHERE Id=@id", con))
                {
                    u.Parameters.AddWithValue("@a", aktif ? 1 : 0);
                    u.Parameters.AddWithValue("@id", id);
                    u.ExecuteNonQuery();
                }
            }
            return null;
        }
    }
}
