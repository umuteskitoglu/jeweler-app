# Kuyumcu Yönetim Sistemi (Jeweler App)

## 📋 Proje Hakkında

Bu proje, modern ve kapsamlı bir kuyumculuk e-ticaret platformu için geliştirilmiş, Clean Architecture prensipleri ile tasarlanmış bir .NET 9.0 uygulamasıdır. Yüzük, kolye, bileklik, küpe ve diğer tüm mücevher türlerini yönetmek için özel olarak tasarlanmıştır.

## 🏗️ Mimari

Proje Clean Architecture prensiplerine göre 4 katmanda yapılandırılmıştır:

```
jeweler-app/
├── Domain/              # İş kuralları ve entity'ler
├── Application/         # Use case'ler ve business logic
├── Infrastructure/      # Veritabanı, dış servisler
└── Jewelry.API/         # REST API endpoints
```

### Teknolojiler

- **.NET 9.0** - Framework
- **Entity Framework Core** - ORM
- **PostgreSQL** - Veritabanı
- **MediatR** - CQRS pattern
- **JWT** - Authentication
- **FluentValidation** - Validasyon
- **Swagger** - API dokümantasyonu

## ✨ Özellikler

### 🔐 Kullanıcı Yönetimi
- Kullanıcı kaydı ve giriş
- JWT tabanlı authentication
- Refresh token desteği
- Role-based authorization

### 💍 Ürün Yönetimi
Tüm mücevher türleri için özelleştirilmiş destek:

#### Genel Özellikler
- **Ürün Bilgileri**: İsim, açıklama, SKU, fiyat, stok
- **Mücevher Tipi**: Yüzük, Kolye, Bileklik, Küpe, Kolye Ucu, Halhal, Broş, Zincir, Charm, Set
- **Hedef Kitle**: Unisex, Erkek, Kadın, Çocuk
- **Koleksiyon**: Ürünleri koleksiyonlara göre gruplama
- **Özelleştirme**: Kişiselleştirilebilir ürün işaretleme
- **Sertifika**: Sertifika numarası takibi
- **Çoklu Görsel**: Birden fazla ürün görseli

#### Materyal Bilgileri
- Materyal tipi (Altın, Gümüş, Platin, vb.)
- Ayar bilgisi (14K, 18K, 22K, 24K, 925 Ayar, vb.)
- Ağırlık (gram, ons)

#### Taş Bilgileri
- Taş tipi (Pırlanta, Yakut, Zümrüt, Safir, vb.)
- Karat
- Kesim (Yuvarlak, Prenses, Oval, vb.)
- Renk (D, E, F skalası veya renk açıklaması)
- Saflık (IF, VVS1, VVS2, VS1, vb.)
- Sertifika numarası ve kurumu (GIA, IGI, HRD, vb.)

#### Ölçüler
- Uzunluk, genişlik, yükseklik
- Yüzük numarası
- Ölçü birimi

#### Kolye Özellikleri
- Zincir uzunluğu
- Zincir tipi (Kablo, İp, Kutu, Yılan, Figaro, vb.)
- Kilit tipi (Istakoz, Yaylı Halka, Toggle, Mıknatıs, vb.)
- Kolye ucu bilgisi
- Ayarlanabilir uzunluk (min-max)

#### Yüzük Özellikleri
- Yüzük numarası
- Yeniden boyutlandırılabilir mi
- Minimum-maksimum numara
- Stil (Solitaire, Eternity, Cocktail, vb.)
- Taş tutma şekli (Pıranta, Bezel, Pave, Kanal, vb.)

#### Küpe Özellikleri
- Arka tutturma tipi (Post, Kanca, Klips, Kaldıraç, vb.)
- Çift mi tek mi
- Sarkma uzunluğu
- Hipoalerjenik mi

### 📦 Sipariş Yönetimi
- Sipariş oluşturma (teslimat ve fatura adresleri ile)
- Sipariş durumu takibi
- Müşteri siparişlerini görüntüleme
- Stok kontrolü
- Kargo takip numarası
- Müşteri notları
- Otomatik tarih damgalama (kargo, teslimat)

### ❤️ Favoriler / Wishlist
- Ürünleri favorilere ekleme
- Favorilerden çıkarma
- Favori listesini görüntüleme
- Favori durumu kontrolü
- Favori sayısı
- Toggle (ekle/çıkar) özelliği

### 💳 Ödeme Entegrasyonu
- Iyzico ödeme entegrasyonu
- Güvenli ödeme işlemleri

## 🚀 Kurulum

### Gereksinimler
- .NET 9.0 SDK
- PostgreSQL 14+
- Docker (opsiyonel)

### Adımlar

1. **Repoyu klonlayın**
```bash
git clone <repo-url>
cd jeweler-app
```

2. **Veritabanı bağlantı ayarları**

`Jewelry.API/appsettings.json` dosyasını düzenleyin:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=jewelerdb;Username=postgres;Password=yourpassword"
  }
}
```

3. **Veritabanı migration'larını çalıştırın**
```bash
dotnet ef database update --project Infrastructure --startup-project Jewelry.API
```

4. **Uygulamayı başlatın**
```bash
dotnet run --project Jewelry.API
```

Uygulama `https://localhost:5001` adresinde çalışacaktır.

5. **Swagger UI**
API dokümantasyonu için: `https://localhost:5001/swagger`

## 📚 API Endpoint'leri

### Authentication (`/api/Auth`)

#### Kullanıcı Kaydı
```http
POST /api/Auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePass123!",
  "firstName": "Ali",
  "lastName": "Yılmaz",
  "role": "Customer"
}
```

#### Giriş
```http
POST /api/Auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePass123!"
}
```

#### Token Yenileme
```http
POST /api/Auth/refresh-token
Content-Type: application/json

{
  "email": "user@example.com",
  "refreshToken": "your-refresh-token"
}
```

#### Çıkış
```http
POST /api/Auth/logout
Content-Type: application/json

{
  "email": "user@example.com",
  "refreshToken": "your-refresh-token"
}
```

### Ürünler (`/api/Products`)

#### Tüm Ürünleri Listele
```http
GET /api/Products/all
```

#### Ürün Detayı
```http
GET /api/Products?id=product-guid
```

#### Kolye Oluşturma
```http
POST /api/Products/create-product
Content-Type: application/json

{
  "name": {
    "value": "Altın Pırlanta Kolye"
  },
  "description": "14 ayar altın üzerine 0.25 karat pırlanta kolye",
  "price": {
    "amount": 12999.99,
    "currency": "TRY"
  },
  "stock": 5,
  "categoryId": "category-guid",
  "jewelryType": 2,
  "targetGender": 2,
  "material": {
    "type": "Altın",
    "purity": "14K",
    "weight": 3.5,
    "weightUnit": "gram"
  },
  "necklaceSpec": {
    "chainLength": 45,
    "chainType": 1,
    "claspType": 1,
    "hasPendant": true,
    "pendantDescription": "Pıranta tutturma tekniği ile 0.25 karat",
    "isAdjustable": true,
    "minLength": 40,
    "maxLength": 45
  },
  "gemstones": [
    {
      "type": "Pırlanta",
      "carat": 0.25,
      "cut": "Yuvarlak",
      "color": "G",
      "clarity": "VS1",
      "certificateNumber": "GIA123456",
      "certificateAuthority": "GIA"
    }
  ],
  "imageUrls": [
    "https://example.com/kolye1.jpg",
    "https://example.com/kolye2.jpg"
  ],
  "isCustomizable": false,
  "collectionName": "Yıldız Koleksiyonu"
}
```

#### Yüzük Oluşturma
```http
POST /api/Products/create-product
Content-Type: application/json

{
  "name": {
    "value": "Tek Taş Pırlanta Yüzük"
  },
  "description": "18 ayar beyaz altın tek taş yüzük",
  "price": {
    "amount": 24999.99,
    "currency": "TRY"
  },
  "stock": 3,
  "categoryId": "category-guid",
  "jewelryType": 1,
  "targetGender": 2,
  "material": {
    "type": "Beyaz Altın",
    "purity": "18K",
    "weight": 2.8,
    "weightUnit": "gram"
  },
  "ringSpec": {
    "size": 15,
    "isResizable": true,
    "minSize": 13,
    "maxSize": 17,
    "style": "Solitaire",
    "setting": "Pıranta Tutturma"
  },
  "gemstones": [
    {
      "type": "Pırlanta",
      "carat": 0.50,
      "cut": "Yuvarlak",
      "color": "E",
      "clarity": "VVS1",
      "certificateNumber": "GIA789012",
      "certificateAuthority": "GIA"
    }
  ],
  "dimensions": {
    "ringSize": 15
  },
  "imageUrls": [
    "https://example.com/yuzuk1.jpg"
  ],
  "certificateNumber": "CERT-2024-001",
  "isCustomizable": true
}
```

#### Küpe Oluşturma
```http
POST /api/Products/create-product
Content-Type: application/json

{
  "name": {
    "value": "Zümrüt Küpe"
  },
  "description": "925 ayar gümüş zümrüt taşlı küpe",
  "price": {
    "amount": 1499.99,
    "currency": "TRY"
  },
  "stock": 10,
  "categoryId": "category-guid",
  "jewelryType": 4,
  "targetGender": 2,
  "material": {
    "type": "Gümüş",
    "purity": "925 Ayar",
    "weight": 1.2,
    "weightUnit": "gram"
  },
  "earringSpec": {
    "backingType": "Post",
    "isPair": true,
    "dropLength": 2.5,
    "isHypoallergenic": true
  },
  "gemstones": [
    {
      "type": "Zümrüt",
      "carat": 0.30,
      "cut": "Oval",
      "color": "Yeşil"
    }
  ],
  "imageUrls": [
    "https://example.com/kupe1.jpg"
  ]
}
```

#### Ürün Güncelleme
```http
PUT /api/Products/update-product
Content-Type: application/json

{
  "id": "product-guid",
  "name": {
    "value": "Güncellenmiş Ürün Adı"
  },
  "price": {
    "amount": 15999.99,
    "currency": "TRY"
  },
  "stock": 8,
  "categoryId": "category-guid",
  "updatedBy": "user-guid",
  ...
}
```

### Siparişler (`/api/Orders`)

#### Sipariş Oluşturma
```http
POST /api/Orders/create
Content-Type: application/json
Authorization: Bearer {token}

{
  "customerId": "customer-guid",
  "currency": "TRY",
  "items": [
    {
      "productId": "product-guid",
      "quantity": 2
    }
  ],
  "shippingAddress": {
    "fullName": "Ahmet Yılmaz",
    "phoneNumber": "+905551234567",
    "addressLine1": "Atatürk Cad. No:123 Daire:4",
    "addressLine2": "Çamlık Apt.",
    "city": "İstanbul",
    "district": "Kadıköy",
    "postalCode": "34710",
    "country": "Türkiye",
    "taxId": "12345678901",
    "taxOffice": "Kadıköy Vergi Dairesi"
  },
  "billingAddress": {
    "fullName": "Ahmet Yılmaz",
    "phoneNumber": "+905551234567",
    "addressLine1": "Atatürk Cad. No:123 Daire:4",
    "city": "İstanbul",
    "district": "Kadıköy",
    "postalCode": "34710",
    "country": "Türkiye",
    "taxId": "12345678901",
    "taxOffice": "Kadıköy Vergi Dairesi"
  },
  "customerNote": "Lütfen kapıya bırakın"
}
```

**Not:** Eğer `billingAddress` sağlanmazsa, `shippingAddress` fatura adresi olarak kullanılır.

#### Sipariş Detayı
```http
GET /api/Orders/{orderId}
Authorization: Bearer {token}
```

#### Müşteri Siparişleri
```http
GET /api/Orders/customer/{customerId}
Authorization: Bearer {token}
```

#### Sipariş Durumu Güncelleme
```http
PUT /api/Orders/{orderId}/status
Content-Type: application/json
Authorization: Bearer {token}

{
  "status": 2,
  "updatedBy": "user-guid"
}
```

**Sipariş Durumları:**
- 0: Beklemede (Pending)
- 1: Onaylandı (Confirmed)
- 2: Ödendi (Paid)
- 3: Kargoya Verildi (Shipped)
- 4: Tamamlandı (Completed)
- 5: İptal Edildi (Cancelled)

## 🗂️ Mücevher Tipleri

| Kod | Tip | Açıklama |
|-----|-----|----------|
| 1 | Ring | Yüzük |
| 2 | Necklace | Kolye |
| 3 | Bracelet | Bileklik |
| 4 | Earring | Küpe |
| 5 | Pendant | Kolye Ucu |
| 6 | Anklet | Halhal |
| 7 | Brooch | Broş |
| 8 | Chain | Zincir |
| 9 | Charm | Charm |
| 10 | Set | Set |
| 99 | Other | Diğer |

## 🔗 Zincir Tipleri

- 1: Cable (Kablo)
- 2: Rope (İp)
- 3: Box (Kutu)
- 4: Snake (Yılan)
- 5: Figaro
- 6: Curb
- 7: Wheat (Buğday)
- 8: Herringbone (Balık kılçığı)
- 9: Singapore
- 10: Bead (Boncuk)
- 11: Omega

## 🔒 Kilit Tipleri

- 1: Lobster Claw (Istakoz)
- 2: Spring Ring (Yaylı Halka)
- 3: Toggle
- 4: Magnetic (Mıknatıslı)
- 5: Box Safety (Kutu Emniyet)
- 6: Barrel Screw (Vida)
- 7: Hook (Kanca)
- 8: Slide Pin

## 👥 Hedef Kitle

- 0: Unisex
- 1: Male (Erkek)
- 2: Female (Kadın)
- 3: Children (Çocuk)

## 🛠️ Geliştirme

### Yeni Migration Oluşturma
```bash
dotnet ef migrations add MigrationName --project Infrastructure --startup-project Jewelry.API
```

### Migration Uygulama
```bash
dotnet ef database update --project Infrastructure --startup-project Jewelry.API
```

### Test Çalıştırma
```bash
dotnet test
```

### Build
```bash
dotnet build
```

## 📝 Veritabanı Şeması

### Products Tablosu
Ana ürün bilgileri ve tüm mücevher özellikleri bu tabloda saklanır:
- Temel bilgiler (Ad, SKU, Fiyat, Stok)
- Materyal bilgileri (JSON)
- Taş bilgileri (JSON koleksiyon)
- Ölçüler (JSON)
- Tip özel özellikler (Kolye, Yüzük, Küpe)
- Görseller (JSON array)

### Users Tablosu
Kullanıcı hesapları ve authentication bilgileri

### Orders Tablosu
Sipariş bilgileri ve sipariş kalemleri

### Categories Tablosu
Ürün kategorileri (hiyerarşik yapı destekler)

## 🔐 Güvenlik

- JWT token tabanlı authentication
- Refresh token desteği
- Password hashing (BCrypt)
- HTTPS zorunlu (production)
- CORS yapılandırması
- SQL injection koruması (EF Core parametreli sorgular)

## 🌍 Lokalizasyon (Çoklu Dil Desteği)

Uygulama **Türkçe (tr-TR)** ve **İngilizce (en-US)** dillerini desteklemektedir.

### Dil Seçimi

Dil seçimi için 3 yöntem desteklenir:

1. **HTTP Header (Önerilen)**
```bash
curl -H "Accept-Language: tr-TR" https://localhost:5001/api/products/all
curl -H "Accept-Language: en-US" https://localhost:5001/api/products/all
```

2. **Query String**
```bash
https://localhost:5001/api/products/all?lang=tr-TR
https://localhost:5001/api/products/all?lang=en-US
```

3. **Cookie** (Otomatik olarak kaydedilir)

### Lokalizasyon API'leri

```bash
# Mevcut dili öğren
GET /api/Localization/current

# Desteklenen dilleri listele
GET /api/Localization/supported

# Çeviri test et
GET /api/Localization/test/{key}
```

**Detaylı dokümantasyon için:** [LOCALIZATION.md](LOCALIZATION.md)

## 🌟 Best Practices

- **Clean Architecture**: Katmanlar arası bağımlılıklar minimum
- **CQRS Pattern**: Okuma ve yazma işlemleri ayrı
- **Value Objects**: Domain modeli zenginleştirilmiş
- **Repository Pattern**: Veri erişimi soyutlanmış
- **Dependency Injection**: Loose coupling
- **Async/Await**: Performans optimizasyonu
- **Localization**: Çoklu dil desteği

## 📞 İletişim

Sorularınız için:
- Email: [email]
- GitHub Issues: [repo-issues-url]

## 📄 Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

---

**Not**: Production ortamına geçmeden önce:
1. Güvenlik ayarlarını gözden geçirin
2. HTTPS'i etkinleştirin
3. Veritabanı yedekleme stratejisi oluşturun
4. Log mekanizmasını yapılandırın
5. Rate limiting ekleyin
6. API key yönetimi ekleyin
