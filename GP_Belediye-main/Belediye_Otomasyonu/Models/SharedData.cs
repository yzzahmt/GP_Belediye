using System;
using System.Collections.Generic;

namespace Belediye_Otomasyonu.Models
{
    /// <summary>
    /// Vatandaşın yaptığı başvuruyu temsil eden veri modeli.
    /// </summary>
    public class Basvuru
    {
        public string BasvuruID { get; set; }
        public string AdSoyad { get; set; }
        public string TCNo { get; set; }
        public string Telefon { get; set; }
        public string Kategori { get; set; }
        public string Aciklama { get; set; }
        public string Durum { get; set; } // Bekliyor, Atandı, Tamamlandı, Reddedildi
        public string AtananPersonel { get; set; }
        public string AtanmaNotari { get; set; }
        public DateTime BasvuruTarihi { get; set; }
        public DateTime AtanmaTarihi { get; set; }
        public string Oncelik { get; set; }
    }

    /// <summary>
    /// Belediyede çalışan personeli temsil eden veri modeli.
    /// </summary>
    public class Personel
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Departman { get; set; }
        public string Unvan { get; set; }
        public int AtananIsCount { get; set; }

        public string AdSoyad => $"{Ad} {Soyad}";
    }

    /// <summary>
    /// Uygulamadaki tüm formların ortak eriştiği statik bellek veri deposu.
    /// </summary>
    public static class SharedData
    {
        public static List<Basvuru> Basvurular = new List<Basvuru>();
        public static List<Personel> Personeller = new List<Personel>();

        static SharedData()
        {
            // Başlangıç personellerini ekle
            Personeller.Add(new Personel { Ad = "Ahmet", Soyad = "Yılmaz", Departman = "Altyapı", Unvan = "Mühendis", AtananIsCount = 1 });
            Personeller.Add(new Personel { Ad = "Mehmet", Soyad = "Kaya", Departman = "Park-Bahçe", Unvan = "Teknisyen", AtananIsCount = 0 });
            Personeller.Add(new Personel { Ad = "Ayşe", Soyad = "Demir", Departman = "Temizlik", Unvan = "Şef", AtananIsCount = 0 });
            Personeller.Add(new Personel { Ad = "Fatma", Soyad = "Çelik", Departman = "Su", Unvan = "Tekniker", AtananIsCount = 0 });
            Personeller.Add(new Personel { Ad = "Ali", Soyad = "Öztürk", Departman = "Elektrik", Unvan = "Mühendis", AtananIsCount = 0 });

            // Başlangıç başvurularını ekle
            Basvurular.Add(new Basvuru
            {
                BasvuruID = "BEL2026-00001",
                AdSoyad = "Veli Can",
                TCNo = "12345678901",
                Telefon = "05551234567",
                Kategori = "Altyapı",
                Aciklama = "Gazi caddesindeki logar kapağı çökmüş durumda, kaza riski var.",
                Durum = "Atandı",
                AtananPersonel = "Ahmet Yılmaz",
                AtanmaNotari = "Ekip sahaya yönlendirildi.",
                BasvuruTarihi = DateTime.Now.AddDays(-3),
                AtanmaTarihi = DateTime.Now.AddDays(-2),
                Oncelik = "Acil"
            });

            Basvurular.Add(new Basvuru
            {
                BasvuruID = "BEL2026-00002",
                AdSoyad = "Ayşe Koç",
                TCNo = "98765432109",
                Telefon = "05329876543",
                Kategori = "Temizlik",
                Aciklama = "Parkın yanındaki çöpler 2 gündür alınmadı.",
                Durum = "Bekliyor",
                BasvuruTarihi = DateTime.Now.AddDays(-1)
            });

            Basvurular.Add(new Basvuru
            {
                BasvuruID = "BEL2026-00003",
                AdSoyad = "Mustafa Can",
                TCNo = "54321098765",
                Telefon = "05441238990",
                Kategori = "Su",
                Aciklama = "Sokakta şebeke suyu borusu patladı, su boşa akıyor.",
                Durum = "Bekliyor",
                BasvuruTarihi = DateTime.Now
            });
        }
    }
}
