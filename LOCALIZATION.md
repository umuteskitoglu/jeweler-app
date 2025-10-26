# Lokalizasyon (Çoklu Dil Desteği)

## 📋 Genel Bakış

Uygulama **JSON tabanlı** lokalizasyon sistemi kullanmaktadır. Şu anda **Türkçe (tr-TR)** ve **İngilizce (en-US)** dillerini desteklemektedir. Varsayılan dil Türkçe'dir.

### Neden JSON?

- ✅ **Kolay düzenleme**: JSON dosyaları .resx dosyalarından çok daha okunaklı
- ✅ **Versiyon kontrolü**: Git diff'leri daha anlamlı
- ✅ **Hiyerarşik yapı**: Nested key'ler ile organize edilmiş (örn: `User.RegisteredSuccessfully`)
- ✅ **IDE desteği**: Herhangi bir text editörde düzenlenebilir
- ✅ **Performans**: Singleton olarak yüklenir, memory'de tutulur

## 🌍 Desteklenen Diller

| Dil Kodu | Dil | Varsayılan |
|----------|-----|------------|
| `tr-TR` | Türkçe | ✅ |
| `en-US` | English | ❌ |

## 🚀 Kullanım

### 1. HTTP Header ile Dil Seçimi

En önerilen yöntem, her istekte `Accept-Language` header'ını kullanmaktır:

```bash
# Türkçe için
curl -H "Accept-Language: tr-TR" https://localhost:5001/api/products/all

# İngilizce için
curl -H "Accept-Language: en-US" https://localhost:5001/api/products/all
```

### 2. Query String ile Dil Seçimi

URL'de `lang` parametresi kullanılabilir:

```bash
# Türkçe için
https://localhost:5001/api/products/all?lang=tr-TR

# İngilizce için
https://localhost:5001/api/products/all?lang=en-US
```

### 3. Cookie ile Dil Seçimi

Uygulama son seçilen dili cookie'de saklar:

```bash
curl -b "cookie=.AspNetCore.Culture=c%3Dtr-TR%7Cuic%3Dtr-TR" https://localhost:5001/api/products/all
```

## 📝 API Endpoint'leri

### Mevcut Dili Öğrenme

```http
GET /api/Localization/current
```

**Yanıt:**
```json
{
  "culture": "tr-TR",
  "uiCulture": "tr-TR"
}
```

### Desteklenen Dilleri Listele

```http
GET /api/Localization/supported
```

**Yanıt:**
```json
{
  "languages": [
    {
      "code": "tr-TR",
      "name": "Türkçe",
      "englishName": "Turkish"
    },
    {
      "code": "en-US",
      "name": "English",
      "englishName": "English"
    }
  ]
}
```

### Lokalizasyon Test Etme

```http
GET /api/Localization/test/{key}
```

**Örnek:**
```bash
# Türkçe
curl -H "Accept-Language: tr-TR" https://localhost:5001/api/Localization/test/Success
# Yanıt: {"key":"Success","value":"Başarılı","culture":"tr-TR"}

# İngilizce  
curl -H "Accept-Language: en-US" https://localhost:5001/api/Localization/test/Success
# Yanıt: {"key":"Success","value":"Success","culture":"en-US"}
```

## 💬 Hata Mesajları

### Kullanıcı İşlemleri

| Anahtar | Türkçe | English |
|---------|---------|---------|
| `UserRegisteredSuccessfully` | Kullanıcı başarıyla kaydedildi | User registered successfully |
| `UserAlreadyExists` | Bu e-posta adresi ile kayıtlı bir kullanıcı zaten mevcut | User with the given email already exists |
| `InvalidCredentials` | E-posta veya şifre hatalı | Invalid credentials |
| `LoggedInSuccessfully` | Giriş başarılı | Logged in successfully |
| `LoggedOutSuccessfully` | Çıkış başarılı | Logged out successfully |
| `InvalidToken` | Geçersiz token | Invalid token |
| `RefreshTokenRequired` | Refresh token gerekli | Refresh token is required |

### Ürün İşlemleri

| Anahtar | Türkçe | English |
|---------|---------|---------|
| `ProductCreatedSuccessfully` | Ürün başarıyla oluşturuldu | Product created successfully |
| `ProductUpdatedSuccessfully` | Ürün başarıyla güncellendi | Product updated successfully |
| `ProductNotFound` | Ürün bulunamadı | Product not found |
| `ProductPriceCannotBeNegative` | Ürün fiyatı negatif olamaz | Product price cannot be negative |
| `ProductStockCannotBeNegative` | Ürün stok miktarı negatif olamaz | Product stock cannot be negative |
| `InsufficientStock` | {0} ürünü için yeterli stok yok | Insufficient stock for product {0} |

### Sipariş İşlemleri

| Anahtar | Türkçe | English |
|---------|---------|---------|
| `OrderCreatedSuccessfully` | Sipariş başarıyla oluşturuldu | Order created successfully |
| `OrderNotFound` | Sipariş bulunamadı | Order not found |
| `OrderMustContainItems` | Sipariş en az bir ürün içermelidir | Order must contain at least one item |
| `OrderStatusUpdatedSuccessfully` | Sipariş durumu başarıyla güncellendi | Order status updated successfully |

### Mücevher Tipleri

| Anahtar | Türkçe | English |
|---------|---------|---------|
| `Ring` | Yüzük | Ring |
| `Necklace` | Kolye | Necklace |
| `Bracelet` | Bileklik | Bracelet |
| `Earring` | Küpe | Earring |
| `Pendant` | Kolye Ucu | Pendant |
| `Anklet` | Halhal | Anklet |
| `Brooch` | Broş | Brooch |
| `Chain` | Zincir | Chain |
| `Charm` | Charm | Charm |
| `Set` | Set | Set |

## 📁 JSON Dosya Yapısı

Lokalizasyon dosyaları `Infrastructure/Localization/Resources/` klasöründe bulunur:

```
Infrastructure/
  Localization/
    Resources/
      tr-TR.json    # Türkçe çeviriler
      en-US.json    # İngilizce çeviriler
```

### JSON Formatı

```json
{
  "Category": {
    "Key": "Value",
    "AnotherKey": "Another Value"
  },
  "AnotherCategory": {
    "Message": "Message with {0} parameter"
  }
}
```

### Kullanım

```csharp
// Basit kullanım
_localization.GetString("User.RegisteredSuccessfully")

// Parametreli kullanım
_localization.GetString("Product.InsufficientStock", productName)
```

## 🔧 Geliştirme

### Yeni Dil Ekleme

1. **JSON dosyası oluşturun:**
```bash
# Örnek: Almanca için
cat > Infrastructure/Localization/Resources/de-DE.json << 'EOF'
{
  "Common": {
    "Success": "Erfolgreich",
    "Error": "Fehler"
  },
  "User": {
    "RegisteredSuccessfully": "Benutzer erfolgreich registriert"
  }
}
EOF
```

2. **LocalizationService.cs'te dili ekleyin:**
```csharp
var supportedCultures = new[] { "tr-TR", "en-US", "de-DE" };
```

3. **Program.cs'e dili ekleyin:**
```csharp
var supportedCultures = new[]
{
    new CultureInfo("tr-TR"),
    new CultureInfo("en-US"),
    new CultureInfo("de-DE")  // Yeni dil
};
```

3. **LocalizationController'da desteklenen diller listesini güncelleyin:**
```csharp
languages = new[]
{
    new { code = "tr-TR", name = "Türkçe", englishName = "Turkish" },
    new { code = "en-US", name = "English", englishName = "English" },
    new { code = "de-DE", name = "Deutsch", englishName = "German" }
}
```

### Yeni Çeviri Anahtarı Ekleme

1. **JSON dosyalarına ekleyin:**

**tr-TR.json:**
```json
{
  "Product": {
    "Deleted": "Ürün başarıyla silindi",
    "DeleteError": "{0} ürünü silinemedi: {1}"
  }
}
```

**en-US.json:**
```json
{
  "Product": {
    "Deleted": "Product deleted successfully",
    "DeleteError": "Product {0} could not be deleted: {1}"
  }
}
```

2. **Kodda kullanın:**
```csharp
// Basit mesaj
var message = _localizationService.GetString("Product.Deleted");

// Parametreli mesaj
var message = _localizationService.GetString("Product.DeleteError", productName, errorReason);
```

### JSON Dosyası Düzenleme İpuçları

1. **Hiyerarşik yapı kullanın:** Mesajları kategorilere ayırın
   ```json
   {
     "User": { ... },
     "Product": { ... },
     "Order": { ... }
   }
   ```

2. **Anlamlı isimler:** Key isimleri açıklayıcı olsun
   ```json
   "RegisteredSuccessfully" ✅
   "RS" ❌
   ```

3. **Parametreler için format string:** `{0}`, `{1}`, vb. kullanın
   ```json
   "WelcomeMessage": "Merhaba {0}, hoş geldiniz!"
   ```

4. **JSON formatına uyun:** Virgüller, tırnaklar vs. dikkat edin
   - Online JSON validator kullanabilirsiniz: https://jsonlint.com

## 📱 Frontend Entegrasyonu

### JavaScript/TypeScript

```typescript
// Axios örneği
import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:5001/api',
  headers: {
    'Accept-Language': localStorage.getItem('language') || 'tr-TR'
  }
});

// Dil değiştirme
function changeLanguage(lang: 'tr-TR' | 'en-US') {
  localStorage.setItem('language', lang);
  api.defaults.headers['Accept-Language'] = lang;
  // Sayfayı yenile veya state'i güncelle
}
```

### React Örneği

```tsx
import { useState, useEffect } from 'react';

function LanguageSelector() {
  const [language, setLanguage] = useState(localStorage.getItem('language') || 'tr-TR');

  const changeLanguage = (newLang: string) => {
    setLanguage(newLang);
    localStorage.setItem('language', newLang);
    // API isteklerinde kullanılacak
  };

  return (
    <select value={language} onChange={(e) => changeLanguage(e.target.value)}>
      <option value="tr-TR">🇹🇷 Türkçe</option>
      <option value="en-US">🇺🇸 English</option>
    </select>
  );
}
```

### Vue.js Örneği

```vue
<template>
  <div>
    <select v-model="currentLanguage" @change="changeLanguage">
      <option value="tr-TR">🇹🇷 Türkçe</option>
      <option value="en-US">🇺🇸 English</option>
    </select>
  </div>
</template>

<script>
export default {
  data() {
    return {
      currentLanguage: localStorage.getItem('language') || 'tr-TR'
    }
  },
  methods: {
    changeLanguage() {
      localStorage.setItem('language', this.currentLanguage);
      // Axios instance'ı güncelle
      this.$axios.defaults.headers['Accept-Language'] = this.currentLanguage;
    }
  }
}
</script>
```

## 🧪 Test

### Postman ile Test

1. **Collection'a değişken ekleyin:**
```json
{
  "key": "language",
  "value": "tr-TR"
}
```

2. **Headers'a ekleyin:**
```
Accept-Language: {{language}}
```

3. **Environment değişkenini değiştirerek test edin:**
   - `tr-TR` → Türkçe yanıtlar
   - `en-US` → İngilizce yanıtlar

### cURL ile Test

```bash
# Türkçe test
curl -H "Accept-Language: tr-TR" \
     -X POST \
     -H "Content-Type: application/json" \
     -d '{"email":"test@test.com","password":"wrong"}' \
     https://localhost:5001/api/Auth/login

# Yanıt: {"error":"E-posta veya şifre hatalı"}

# İngilizce test
curl -H "Accept-Language: en-US" \
     -X POST \
     -H "Content-Type: application/json" \
     -d '{"email":"test@test.com","password":"wrong"}' \
     https://localhost:5001/api/Auth/login

# Yanıt: {"error":"Invalid credentials"}
```

## 🎯 Best Practices

1. **Header kullanın:** Query string yerine `Accept-Language` header'ı tercih edin
2. **Varsayılan dil:** Kullanıcının tercih ettiği dili localStorage'da saklayın
3. **Fallback:** Desteklenmeyen diller için varsayılan dile (tr-TR) geri dönün
4. **Tutarlılık:** Tüm isteklerde aynı dil başlığını kullanın
5. **Cache:** Çeviriler statik olduğu için önbelleğe alınabilir

## 🔍 Sorun Giderme

### Çeviriler Görünmüyor

1. Resource dosyalarının doğru konumda olduğundan emin olun
2. Build action'ın "Embedded Resource" olduğunu kontrol edin
3. Namespace'lerin doğru olduğunu kontrol edin

### Yanlış Dil Dönüyor

1. `Accept-Language` header'ının doğru gönderildiğini kontrol edin
2. Browser'ın otomatik language header'ını override ettiğinizden emin olun
3. Middleware sırasının doğru olduğunu kontrol edin (UseRequestLocalization)

### Özel Mesajlar Çalışmıyor

1. Anahtar isminin resource dosyasında mevcut olduğundan emin olun
2. Büyük-küçük harf duyarlılığına dikkat edin
3. ILocalizationService'in doğru inject edildiğini kontrol edin

## 📚 Kaynaklar

- [ASP.NET Core Localization](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization)
- [Resource Files (.resx)](https://learn.microsoft.com/en-us/dotnet/core/extensions/resources)
- [CultureInfo Class](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)

---

**Not:** Yeni dil eklemek veya çeviri güncellemek için lütfen geliştirme ekibiyle iletişime geçin.

