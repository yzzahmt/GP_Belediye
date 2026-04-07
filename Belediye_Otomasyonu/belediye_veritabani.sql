-- Belediye otomasyonu: tek MySQL veritabanı (phpMyAdmin / MySQL Workbench).
-- Önce şifrenizi App.config -> MySqlBelediye içinde güncelleyin.
-- Bu dosyayı genelde bir kez çalıştırın; basvurular örnek kayıtlar ekler.

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

CREATE DATABASE IF NOT EXISTS belediye
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE belediye;

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

CREATE TABLE IF NOT EXISTS personeller (
  Id int(11) NOT NULL AUTO_INCREMENT,
  KullaniciAdi varchar(50) NOT NULL,
  Sifre varchar(100) NOT NULL,
  Ad varchar(100) DEFAULT NULL,
  Soyad varchar(100) DEFAULT NULL,
  Aktif tinyint(1) NOT NULL DEFAULT 1,
  Yonetici tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (Id),
  UNIQUE KEY uk_personel_kadi (KullaniciAdi)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS basvurular (
  Id int(11) NOT NULL AUTO_INCREMENT,
  Konu varchar(200) NOT NULL,
  Durum varchar(32) NOT NULL DEFAULT 'Beklemede',
  KayitTarihi datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

INSERT IGNORE INTO personeller (KullaniciAdi, Sifre, Ad, Soyad, Aktif, Yonetici) VALUES
('admin', 'admin123', 'Sistem', 'Yöneticisi', 1, 1);

INSERT INTO basvurular (Konu, Durum) VALUES
('Sokak aydinlatma arızası', 'Beklemede'),
('Park bakım talebi', 'Islemde'),
('Vergi borcu sorgusu', 'Tamamlandi'),
('Imar durumu bilgisi', 'Beklemede'),
('Cöp toplama saati', 'Islemde'),
('Evlilik cüzdanı başvurusu', 'Tamamlandi'),
('Sosyal yardım başvurusu', 'Beklemede'),
('Ruhsat yenileme', 'Islemde');

INSERT IGNORE INTO kullanicilar (ad, soyad, tc, e_Mail, KullaniciAdi, sifre) VALUES
('batuhan', 'kaya', '12345678912', 'batu1234@gmail.com', 'batu1234', '1234567');

SET FOREIGN_KEY_CHECKS = 1;

-- Eski kurulumda e-posta benzersizliği için bir kez (aynı e_Mail iki satırda varsa önce düzeltin):
-- ALTER TABLE kullanicilar ADD UNIQUE KEY uk_email (e_Mail);

-- Eski kurulumda personeller tablosu zaten varken bir kez çalıştırın (sütun varsa hata verebilir, yok sayın):
-- ALTER TABLE personeller ADD COLUMN Aktif tinyint(1) NOT NULL DEFAULT 1;
-- ALTER TABLE personeller ADD COLUMN Yonetici tinyint(1) NOT NULL DEFAULT 0;
-- UPDATE personeller SET Yonetici=1, Aktif=1 WHERE KullaniciAdi='admin';
