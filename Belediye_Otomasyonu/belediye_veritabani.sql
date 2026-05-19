-- Belediye otomasyonu: tek MySQL veritabanı (phpMyAdmin / MySQL Workbench).
-- Önce şifrenizi App.config -> MySqlBelediye içinde güncelleyin.
-- Bu dosyayı genelde bir kez çalıştırın; basvurular örnek kayıtlar ekler.

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

CREATE DATABASE IF NOT EXISTS belediye
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE belediye;

-- ── Vatandaş (portal kullanıcıları) ─────────────────────────────────────────
CREATE TABLE IF NOT EXISTS kullanicilar (
  Id int(11) NOT NULL AUTO_INCREMENT,
  ad varchar(50) NOT NULL,
  soyad varchar(50) NOT NULL,
  tc varchar(11) NOT NULL,
  e_Mail varchar(100) NOT NULL,
  KullaniciAdi varchar(50) NOT NULL,
  sifre varchar(100) NOT NULL,
  PRIMARY KEY (Id),
  UNIQUE KEY uk_tc (tc),
  UNIQUE KEY uk_email (e_Mail)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ── Personel (sıradan çalışanlar) ────────────────────────────────────────────
--   Yonetici kolonu KALDIRILDI — adminler artık yoneticiler tablosunda
CREATE TABLE IF NOT EXISTS personeller (
  Id int(11) NOT NULL AUTO_INCREMENT,
  KullaniciAdi varchar(50) NOT NULL,
  Sifre varchar(100) NOT NULL,
  Ad varchar(100) DEFAULT NULL,
  Soyad varchar(100) DEFAULT NULL,
  Departman varchar(100) DEFAULT NULL,
  Aktif tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (Id),
  UNIQUE KEY uk_personel_kadi (KullaniciAdi)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ── Yöneticiler (admin hesapları — ayrı tablo) ───────────────────────────────
CREATE TABLE IF NOT EXISTS yoneticiler (
  Id int(11) NOT NULL AUTO_INCREMENT,
  KullaniciAdi varchar(50) NOT NULL,
  Sifre varchar(100) NOT NULL,
  Ad varchar(100) DEFAULT NULL,
  Soyad varchar(100) DEFAULT NULL,
  Unvan varchar(100) DEFAULT NULL,
  Aktif tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (Id),
  UNIQUE KEY uk_yonetici_kadi (KullaniciAdi)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ── Başvurular ───────────────────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS basvurular (
  Id int(11) NOT NULL AUTO_INCREMENT,
  Konu varchar(200) NOT NULL,
  Kategori varchar(100) NOT NULL DEFAULT 'Genel',
  Aciklama TEXT NULL,
  Durum varchar(32) NOT NULL DEFAULT 'Beklemede',
  VatandasTC varchar(11) NULL,
  KayitTarihi datetime DEFAULT CURRENT_TIMESTAMP,
  AtananDepartman varchar(100) DEFAULT NULL,
  AtananPersonelKadi varchar(50) DEFAULT NULL,
  PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ── Bildirimler (yöneticiden personele) ──────────────────────────────────────
CREATE TABLE IF NOT EXISTS bildirimler (
  Id int(11) NOT NULL AUTO_INCREMENT,
  Baslik varchar(200) NOT NULL,
  Icerik TEXT NOT NULL,
  GonderenKadi varchar(50) NOT NULL,
  AliciKadi varchar(50) NULL COMMENT 'NULL = tüm personeller',
  GonderimTarihi datetime DEFAULT CURRENT_TIMESTAMP,
  Okundu tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (Id),
  KEY idx_alici (AliciKadi),
  KEY idx_okundu (Okundu)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ── Duyurular (vatandaşlara) ─────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS duyurular (
  Id int(11) NOT NULL AUTO_INCREMENT,
  Baslik varchar(200) NOT NULL,
  Icerik TEXT NOT NULL,
  YayinTarihi datetime DEFAULT CURRENT_TIMESTAMP,
  Aktif tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ── Başvuru Notları (personel not ekleyebilir) ───────────────────────────────
CREATE TABLE IF NOT EXISTS basvuru_notlari (
  Id int(11) NOT NULL AUTO_INCREMENT,
  BasvuruId int(11) NOT NULL,
  PersonelAdi varchar(100) NOT NULL,
  Not TEXT NOT NULL,
  EklenmeTarihi datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (Id),
  KEY idx_basvuru (BasvuruId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ── Sistem Logu ───────────────────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS sistem_logu (
  Id int(11) NOT NULL AUTO_INCREMENT,
  Kullanici varchar(100),
  Rol varchar(50),
  Islem varchar(500),
  Tarih datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (Id),
  KEY idx_tarih (Tarih)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ── Örnek veriler ─────────────────────────────────────────────────────────────
INSERT IGNORE INTO yoneticiler (KullaniciAdi, Sifre, Ad, Soyad, Unvan, Aktif) VALUES
('admin', 'admin123', 'Sistem', 'Yöneticisi', 'Belediye Başkanı', 1);

INSERT IGNORE INTO personeller (KullaniciAdi, Sifre, Ad, Soyad, Departman, Aktif) VALUES
('personel1', 'personel123', 'Ahmet', 'Yılmaz', 'Teknik İşler', 1);

INSERT INTO basvurular (Konu, Kategori, Durum) VALUES
('Sokak aydınlatma arızası', 'Temizlik & Çevre', 'Beklemede'),
('Park bakım talebi', 'Temizlik & Çevre', 'Islemde'),
('Vergi borcu sorgusu', 'Vergi & Ruhsat', 'Tamamlandi'),
('İmar durumu bilgisi', 'İmar & Yapı', 'Beklemede'),
('Çöp toplama saati', 'Temizlik & Çevre', 'Islemde'),
('Evlilik cüzdanı başvurusu', 'Evlilik & Nüfus', 'Tamamlandi'),
('Sosyal yardım başvurusu', 'Sosyal Yardım', 'Beklemede'),
('Ruhsat yenileme', 'Vergi & Ruhsat', 'Islemde');

INSERT IGNORE INTO kullanicilar (ad, soyad, tc, e_Mail, KullaniciAdi, sifre) VALUES
('Batuhan', 'Kaya', '12345678912', 'batu1234@gmail.com', 'batu1234', '1234567');

SET FOREIGN_KEY_CHECKS = 1;

-- ── Mevcut kurulum güncelleme komutları (zaten tablo varsa çalıştırın) ─────────
-- Eski personeller tablosundan adminleri yoneticiler tablosuna taşımak için:
-- INSERT IGNORE INTO yoneticiler (KullaniciAdi, Sifre, Ad, Soyad, Aktif)
--   SELECT KullaniciAdi, Sifre, Ad, Soyad, Aktif FROM personeller WHERE Yonetici=1;
-- DELETE FROM personeller WHERE Yonetici=1;
-- ALTER TABLE personeller DROP COLUMN IF EXISTS Yonetici;
-- ALTER TABLE personeller ADD COLUMN IF NOT EXISTS Departman varchar(100) DEFAULT NULL;
-- ALTER TABLE yoneticiler ADD COLUMN IF NOT EXISTS Unvan varchar(100) DEFAULT NULL;

-- Yeni tablolar (zaten kurulu sistemlerde çalıştırın):
-- CREATE TABLE IF NOT EXISTS basvuru_notlari (Id int AUTO_INCREMENT, BasvuruId int NOT NULL, PersonelAdi varchar(100) NOT NULL, Not TEXT NOT NULL, EklenmeTarihi datetime DEFAULT CURRENT_TIMESTAMP, PRIMARY KEY(Id), KEY idx_basvuru(BasvuruId)) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
-- CREATE TABLE IF NOT EXISTS sistem_logu (Id int AUTO_INCREMENT, Kullanici varchar(100), Rol varchar(50), Islem varchar(500), Tarih datetime DEFAULT CURRENT_TIMESTAMP, PRIMARY KEY(Id)) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
