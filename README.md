# StajKariyerWeb 🚀

Yapay zeka destekli staj ve kariyer eşleştirme platformu.

## 📌 Proje Hakkında

StajKariyerWeb, öğrencilerin teknik yetkinlikleri, ilgi alanları ve proje deneyimlerine göre uygun kariyer alanlarını tahmin eden ve firmalar ile eşleşmesini sağlayan web tabanlı bir kariyer platformudur.

Sistem içerisinde:

* Öğrenci ve firma rol ayrımı
* Yapay zeka destekli kariyer tahmini
* Firma eşleşme sistemi
* Başvuru sistemi
* Firma yönetim paneli
* Geçmiş tahminler
* Profil yönetimi
* Rol bazlı yetkilendirme

özellikleri bulunmaktadır.

---

# 🛠 Kullanılan Teknolojiler

## Backend

* ASP.NET Core MVC
* Entity Framework Core
* ASP.NET Identity
* SQL Server

## Yapay Zeka & API

* Python
* FastAPI
* Scikit-learn

## Frontend

* Razor Views
* Bootstrap
* Glassmorphism UI Design
* Custom CSS

---

# 👥 Kullanıcı Rolleri

## 🎓 Öğrenci

* Kariyer tahmini oluşturabilir
* Firmaları görüntüleyebilir
* Firmalara başvuru yapabilir
* Yol haritası görebilir
* Geçmiş tahminlerini inceleyebilir
* Profil düzenleyebilir

## 🏢 Firma

* Firma paneline erişebilir
* Başvuruları görüntüleyebilir
* Önerilen adayları inceleyebilir
* Firma profilini düzenleyebilir
* Aday filtreleme işlemleri yapabilir

---

# 🤖 Yapay Zeka Sistemi

Kullanıcıdan alınan bilgiler:

* GNO
* Kariyer alanı
* Proje deneyimi
* Veritabanı bilgisi
* Python
* Java
* C#
* C++

değerleri üzerinden FastAPI servisinde çalışan makine öğrenmesi modeli ile analiz edilir.

Sonuç olarak:

* Kariyer tahmini
* Uygun alan
* Önerilen firmalar
  üretilir.

---

# 📂 Proje Özellikleri

✅ Rol bazlı giriş sistemi
✅ Öğrenci/Firma ayrımı
✅ Kariyer eşleştirme sistemi
✅ Firma başvuru sistemi
✅ Geçmiş tahmin kayıtları
✅ Modern dashboard yapısı
✅ CV yükleme sistemi
✅ Profil yönetimi
✅ Firma paneli
✅ SQL Server veritabanı
✅ FastAPI entegrasyonu

---

# ⚙️ Kurulum

## 1. Repository Clone

```bash
git clone https://github.com/rabiicolak/StajKariyerWeb.git
```

---

## 2. Veritabanı Ayarları

`appsettings.json` içerisindeki SQL Server bağlantısını düzenleyin.

---

## 3. Migration İşlemleri

```bash
Add-Migration InitialCreate
Update-Database
```

---

## 4. FastAPI Servisini Başlat

```bash
uvicorn main:app --reload
```

---

## 5. ASP.NET Projesini Çalıştır

Visual Studio üzerinden:

```bash
Ctrl + F5
```

---

# 🔐 Güvenlik

* ASP.NET Identity Authentication
* Role Based Authorization
* Student / Company ayrımı
* Yetkisiz sayfa erişim engellemesi

---

# 📈 Gelecek Geliştirmeler

* Gerçek zamanlı bildirim sistemi
* AI tabanlı CV analizi
* Mesajlaşma sistemi
* İlan oluşturma sistemi
* Gelişmiş aday filtreleme

---

# 👩‍💻 Geliştirici

Rabia Çolak
Bilgisayar Mühendisliği

GitHub:
https://github.com/rabiicolak
