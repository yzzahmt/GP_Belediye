using System;

namespace Belediye_Otomasyonu.Models
{
    public class Kullanici
    {
        // Veritabanındaki tablonun birebir C# kopyası
        public int Id { get; set; } 
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Tc { get; set; }
        public string Email { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
    }
}
