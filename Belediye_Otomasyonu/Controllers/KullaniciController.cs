using System;
using MySql.Data.MySqlClient;
using Belediye_Otomasyonu.Models;

namespace Belediye_Otomasyonu.Controllers
{
    public class KullaniciController
    {
        public bool KullaniciEkle(Kullanici yeniKullanici)
        {
            bool islemBasarili = false;

            using (MySqlConnection con = new MySqlConnection(DatabaseConfig.MySqlBelediye))
            {
                try
                {
                    con.Open();

                    using (var kontrol = new MySqlCommand(
                        "SELECT 1 FROM kullanicilar WHERE LOWER(TRIM(e_Mail)) = LOWER(TRIM(@e)) LIMIT 1", con))
                    {
                        kontrol.Parameters.AddWithValue("@e", yeniKullanici.Email ?? "");
                        if (kontrol.ExecuteScalar() != null)
                            return false;
                    }

                    string sorgu = "INSERT INTO kullanicilar (ad, soyad, tc, e_Mail, KullaniciAdi, sifre) VALUES (@ad, @soyad, @tc, @email, @kadi, @sifre)";

                    using (MySqlCommand cmd = new MySqlCommand(sorgu, con))
                    {
                        cmd.Parameters.AddWithValue("@ad", yeniKullanici.Ad);
                        cmd.Parameters.AddWithValue("@soyad", yeniKullanici.Soyad);
                        cmd.Parameters.AddWithValue("@tc", yeniKullanici.Tc);
                        cmd.Parameters.AddWithValue("@email", yeniKullanici.Email);
                        cmd.Parameters.AddWithValue("@kadi", yeniKullanici.KullaniciAdi);
                        cmd.Parameters.AddWithValue("@sifre", yeniKullanici.Sifre);

                        cmd.ExecuteNonQuery();
                        islemBasarili = true;
                    }
                }
                catch (MySqlException ex) when (ex.Number == 1062)
                {
                    islemBasarili = false;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Veritabanı Diyor Ki: " + ex.Message);
                    islemBasarili = false;
                }
            }

            return islemBasarili;
        }
    }
}