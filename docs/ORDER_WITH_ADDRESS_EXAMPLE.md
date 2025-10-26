# Adres Bilgisi ile Sipariş Oluşturma

## 📋 Genel Bakış

Sipariş oluştururken **teslimat adresi (shipping address)** zorunludur. **Fatura adresi (billing address)** opsiyoneldir; eğer sağlanmazsa teslimat adresi fatura adresi olarak kullanılır.

## 📍 Address Value Object

```csharp
public class Address
{
    public string FullName { get; set; }           // Ad Soyad
    public string PhoneNumber { get; set; }        // Telefon
    public string AddressLine1 { get; set; }       // Adres satırı 1 (zorunlu)
    public string? AddressLine2 { get; set; }      // Adres satırı 2 (opsiyonel)
    public string City { get; set; }               // Şehir
    public string? District { get; set; }          // İlçe
    public string? State { get; set; }             // Eyalet/Bölge
    public string PostalCode { get; set; }         // Posta kodu
    public string Country { get; set; }            // Ülke
    public string? TaxId { get; set; }             // TC Kimlik No / Vergi No
    public string? TaxOffice { get; set; }         // Vergi Dairesi
}
```

## 🚀 Tam Örnek İstek

### Teslimat ve Fatura Adresi ile Sipariş

```json
POST /api/Orders/create
Authorization: Bearer {your-jwt-token}
Content-Type: application/json

{
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "currency": "TRY",
  "items": [
    {
      "productId": "d290f1ee-6c54-4b01-90e6-d701748f0851",
      "quantity": 1
    },
    {
      "productId": "a1b2c3d4-5e6f-7a8b-9c0d-1e2f3a4b5c6d",
      "quantity": 2
    }
  ],
  "shippingAddress": {
    "fullName": "Ayşe Demir",
    "phoneNumber": "+905551234567",
    "addressLine1": "Cumhuriyet Caddesi No: 45/7",
    "addressLine2": "Güneş Apartmanı Kat:3",
    "city": "İstanbul",
    "district": "Beşiktaş",
    "postalCode": "34340",
    "country": "Türkiye",
    "taxId": "12345678901",
    "taxOffice": "Beşiktaş Vergi Dairesi"
  },
  "billingAddress": {
    "fullName": "Demir Mücevherat Ltd. Şti.",
    "phoneNumber": "+902121234567",
    "addressLine1": "İstiklal Caddesi No: 100",
    "city": "İstanbul",
    "district": "Beyoğlu",
    "postalCode": "34433",
    "country": "Türkiye",
    "taxId": "1234567890",
    "taxOffice": "Beyoğlu Kurumlar Vergi Dairesi"
  },
  "customerNote": "Hediye paketi yapılsın lütfen. Özel gün için."
}
```

### Sadece Teslimat Adresi ile (Fatura = Teslimat)

```json
POST /api/Orders/create
Authorization: Bearer {your-jwt-token}
Content-Type: application/json

{
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "currency": "TRY",
  "items": [
    {
      "productId": "d290f1ee-6c54-4b01-90e6-d701748f0851",
      "quantity": 1
    }
  ],
  "shippingAddress": {
    "fullName": "Mehmet Kaya",
    "phoneNumber": "+905559876543",
    "addressLine1": "Atatürk Bulvarı No: 234 Daire: 12",
    "city": "Ankara",
    "district": "Çankaya",
    "postalCode": "06680",
    "country": "Türkiye"
  },
  "customerNote": "Kapıya bırakabilirsiniz"
}
```

## 📦 Sipariş Yanıtı

```json
{
  "orderId": "7d8f9a0b-1c2d-3e4f-5a6b-7c8d9e0f1a2b",
  "message": "Sipariş başarıyla oluşturuldu"
}
```

## 🔍 Sipariş Detayı Sorgulama

```bash
GET /api/Orders/7d8f9a0b-1c2d-3e4f-5a6b-7c8d9e0f1a2b
Authorization: Bearer {your-jwt-token}
```

### Yanıt

```json
{
  "id": "7d8f9a0b-1c2d-3e4f-5a6b-7c8d9e0f1a2b",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "totalAmount": {
    "amount": 15999.99,
    "currency": "TRY"
  },
  "currency": "TRY",
  "status": 0,
  "shippingAddress": {
    "fullName": "Ayşe Demir",
    "phoneNumber": "+905551234567",
    "addressLine1": "Cumhuriyet Caddesi No: 45/7",
    "addressLine2": "Güneş Apartmanı Kat:3",
    "city": "İstanbul",
    "district": "Beşiktaş",
    "postalCode": "34340",
    "country": "Türkiye",
    "taxId": "12345678901",
    "taxOffice": "Beşiktaş Vergi Dairesi"
  },
  "billingAddress": {
    "fullName": "Demir Mücevherat Ltd. Şti.",
    "phoneNumber": "+902121234567",
    "addressLine1": "İstiklal Caddesi No: 100",
    "city": "İstanbul",
    "district": "Beyoğlu",
    "postalCode": "34433",
    "country": "Türkiye",
    "taxId": "1234567890",
    "taxOffice": "Beyoğlu Kurumlar Vergi Dairesi"
  },
  "customerNote": "Hediye paketi yapılsın lütfen. Özel gün için.",
  "trackingNumber": null,
  "shippedAt": null,
  "deliveredAt": null,
  "items": [
    {
      "id": "item-guid",
      "productId": "d290f1ee-6c54-4b01-90e6-d701748f0851",
      "productName": "Altın Pırlanta Kolye",
      "unitPrice": {
        "amount": 12999.99,
        "currency": "TRY"
      },
      "quantity": 1,
      "totalPrice": {
        "amount": 12999.99,
        "currency": "TRY"
      }
    }
  ],
  "created": {
    "by": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "at": "2024-10-26T10:30:00Z"
  },
  "updated": {
    "by": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "at": "2024-10-26T10:30:00Z"
  }
}
```

## 🚚 Kargo Bilgilerini Güncelleme

Yeni endpoint'ler eklenebilir:

```csharp
// OrdersController'a eklenebilecek methodlar:

[HttpPut("{orderId}/tracking")]
public async Task<ActionResult> UpdateTrackingNumber(
    Guid orderId, 
    [FromBody] UpdateTrackingRequest request)
{
    // Implementation...
}

[HttpPut("{orderId}/shipping-address")]
public async Task<ActionResult> UpdateShippingAddress(
    Guid orderId, 
    [FromBody] AddressDto address)
{
    // Implementation...
}
```

## ✅ Validasyon Kuralları

### Zorunlu Alanlar (Shipping Address)
- ✅ `fullName` - En az 2 karakter
- ✅ `phoneNumber` - Geçerli telefon formatı
- ✅ `addressLine1` - En az 5 karakter
- ✅ `city` - En az 2 karakter
- ✅ `postalCode` - Geçerli posta kodu formatı
- ✅ `country` - En az 2 karakter

### Opsiyonel Alanlar
- ⚪ `addressLine2`
- ⚪ `district`
- ⚪ `state`
- ⚪ `taxId`
- ⚪ `taxOffice`

### Billing Address
- ⚪ Tamamen opsiyonel
- ⚪ Sağlanmazsa `shippingAddress` kullanılır

## 🌍 Farklı Ülkeler İçin Örnekler

### Türkiye
```json
{
  "fullName": "Ali Veli",
  "phoneNumber": "+905551234567",
  "addressLine1": "Sokak No: 45 Daire: 3",
  "city": "İstanbul",
  "district": "Kadıköy",
  "postalCode": "34710",
  "country": "Türkiye",
  "taxId": "12345678901"
}
```

### ABD
```json
{
  "fullName": "John Doe",
  "phoneNumber": "+1-555-123-4567",
  "addressLine1": "123 Main Street",
  "addressLine2": "Apt 4B",
  "city": "New York",
  "state": "NY",
  "postalCode": "10001",
  "country": "United States"
}
```

### Almanya
```json
{
  "fullName": "Hans Schmidt",
  "phoneNumber": "+49-30-12345678",
  "addressLine1": "Hauptstraße 123",
  "city": "Berlin",
  "postalCode": "10115",
  "country": "Deutschland"
}
```

## 🔒 Güvenlik

- Kullanıcılar sadece kendi adreslerini görebilir
- Admin rolü tüm adresleri görebilir
- Adres bilgileri şifrelenmeli (HTTPS)
- PII (Personally Identifiable Information) olarak değerlendirilir

## 📊 Veritabanı Yapısı

Address bilgileri Order tablosunda `owned entity` olarak saklanır:

```
Orders
  - ShippingFullName
  - ShippingPhoneNumber
  - ShippingAddressLine1
  - ShippingAddressLine2
  - ShippingCity
  - ShippingDistrict
  - ShippingState
  - ShippingPostalCode
  - ShippingCountry
  - ShippingTaxId
  - ShippingTaxOffice
  - BillingFullName
  - BillingPhoneNumber
  - ... (benzer şekilde tüm billing alanları)
```

## 🎯 İpuçları

1. **Teslimat adresi zorunludur** - Her siparişte mutlaka bulunmalıdır
2. **Fatura adresi opsiyoneldir** - Yoksa teslimat adresi kullanılır
3. **Telefon formatı** - Uluslararası format önerilir: `+90 555 123 45 67`
4. **Vergi bilgileri** - Kurumsal siparişler için fatura adresinde gereklidir
5. **Müşteri notu** - Max 1000 karakter
6. **Adres validation** - Frontend'de Google Maps API entegrasyonu önerilir

---

**Not:** Yeni bir database migration oluşturmanız gerekecek:
```bash
dotnet ef migrations add AddAddressToOrders --project Infrastructure --startup-project Jewelry.API
dotnet ef database update --project Infrastructure --startup-project Jewelry.API
```

