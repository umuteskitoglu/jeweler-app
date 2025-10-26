# ❤️ Favoriler / Wishlist Özelliği

## 📋 Genel Bakış

Kullanıcılar beğendikleri ürünleri favorilerine ekleyerek daha sonra tekrar görüntüleyebilirler. Bu özellik wishlist (istek listesi) olarak da bilinir.

## 🎯 Özellikler

- ✅ Ürünleri favorilere ekleme
- ✅ Favorilerden çıkarma
- ✅ Tüm favorileri listeleme
- ✅ Favori durumu kontrolü (bir ürün favoride mi?)
- ✅ Favori sayısı
- ✅ Toggle özelliği (varsa çıkar, yoksa ekle)
- ✅ JWT authentication ile korumalı
- ✅ Kullanıcıya özel favori listesi
- ✅ Veritabanında unique constraint (bir kullanıcı aynı ürünü birden fazla favorilere ekleyemez)

## 🔐 Authentication

Tüm favoriler endpoint'leri **JWT token** gerektirir. Token'dan otomatik olarak kullanıcı ID'si alınır.

```http
Authorization: Bearer {your-jwt-token}
```

## 📚 API Endpoint'leri

### 1. Favorilere Ekleme

Bir ürünü favorilere ekler.

```http
POST /api/Favorites/add
Authorization: Bearer {token}
Content-Type: application/json

{
  "productId": "d290f1ee-6c54-4b01-90e6-d701748f0851"
}
```

**Yanıt (200 OK):**
```json
{
  "favoriteId": "7d8f9a0b-1c2d-3e4f-5a6b-7c8d9e0f1a2b",
  "message": "Product added to favorites successfully"
}
```

**Hata (400 Bad Request):**
```json
{
  "error": "Product is already in favorites"
}
```

### 2. Favorilerden Çıkarma

Bir ürünü favorilerden çıkarır.

```http
DELETE /api/Favorites/{productId}
Authorization: Bearer {token}
```

**Yanıt (200 OK):**
```json
{
  "message": "Product removed from favorites successfully"
}
```

**Hata (404 Not Found):**
```json
{
  "error": "Favorite not found"
}
```

### 3. Favori Toggle

Ürün favorilerdeyse çıkarır, değilse ekler. En kullanışlı endpoint!

```http
POST /api/Favorites/toggle
Authorization: Bearer {token}
Content-Type: application/json

{
  "productId": "d290f1ee-6c54-4b01-90e6-d701748f0851"
}
```

**Yanıt - Eklendi (200 OK):**
```json
{
  "isFavorite": true,
  "favoriteId": "7d8f9a0b-1c2d-3e4f-5a6b-7c8d9e0f1a2b",
  "message": "Product added to favorites"
}
```

**Yanıt - Çıkarıldı (200 OK):**
```json
{
  "isFavorite": false,
  "message": "Product removed from favorites"
}
```

### 4. Favorileri Listele

Kullanıcının tüm favori ürünlerini getirir.

```http
GET /api/Favorites
Authorization: Bearer {token}
```

**Yanıt (200 OK):**
```json
[
  {
    "favoriteId": "7d8f9a0b-1c2d-3e4f-5a6b-7c8d9e0f1a2b",
    "productId": "d290f1ee-6c54-4b01-90e6-d701748f0851",
    "productName": "Altın Pırlanta Kolye",
    "productSlug": "altin-pirlanta-kolye",
    "price": {
      "amount": 12999.99,
      "currency": "TRY"
    },
    "imageUrl": "https://example.com/images/product1.jpg",
    "jewelryType": 2,
    "isInStock": true,
    "addedAt": "2024-10-26T10:30:00Z"
  },
  {
    "favoriteId": "8e9f0a1b-2c3d-4e5f-6a7b-8c9d0e1f2a3b",
    "productId": "a1b2c3d4-5e6f-7a8b-9c0d-1e2f3a4b5c6d",
    "productName": "Gümüş Küpe",
    "productSlug": "gumus-kupe",
    "price": {
      "amount": 599.99,
      "currency": "TRY"
    },
    "imageUrl": "https://example.com/images/product2.jpg",
    "jewelryType": 4,
    "isInStock": true,
    "addedAt": "2024-10-25T15:20:00Z"
  }
]
```

### 5. Favori Durumu Kontrolü

Bir ürünün favorilerde olup olmadığını kontrol eder.

```http
GET /api/Favorites/check/{productId}
Authorization: Bearer {token}
```

**Yanıt (200 OK):**
```json
{
  "isFavorite": true
}
```

### 6. Favori Sayısı

Kullanıcının toplam favori sayısını döner.

```http
GET /api/Favorites/count
Authorization: Bearer {token}
```

**Yanıt (200 OK):**
```json
{
  "count": 5
}
```

## 💻 Frontend Entegrasyon Örnekleri

### React/TypeScript

```typescript
import axios from 'axios';

const api = axios.create({
  baseURL: 'https://api.example.com/api',
  headers: {
    'Authorization': `Bearer ${localStorage.getItem('token')}`
  }
});

// Favorilere ekle
const addToFavorites = async (productId: string) => {
  try {
    const response = await api.post('/Favorites/add', { productId });
    console.log(response.data.message);
    return response.data;
  } catch (error) {
    console.error('Error adding to favorites:', error);
  }
};

// Toggle favorileri
const toggleFavorite = async (productId: string) => {
  try {
    const response = await api.post('/Favorites/toggle', { productId });
    return response.data.isFavorite;
  } catch (error) {
    console.error('Error toggling favorite:', error);
  }
};

// Favorileri listele
const getFavorites = async () => {
  try {
    const response = await api.get('/Favorites');
    return response.data;
  } catch (error) {
    console.error('Error fetching favorites:', error);
  }
};

// Favori durumu kontrol et
const checkIsFavorite = async (productId: string) => {
  try {
    const response = await api.get(`/Favorites/check/${productId}`);
    return response.data.isFavorite;
  } catch (error) {
    console.error('Error checking favorite:', error);
    return false;
  }
};

// React Component Örneği
const FavoriteButton = ({ productId }: { productId: string }) => {
  const [isFavorite, setIsFavorite] = useState(false);
  
  useEffect(() => {
    checkIsFavorite(productId).then(setIsFavorite);
  }, [productId]);
  
  const handleClick = async () => {
    const newStatus = await toggleFavorite(productId);
    setIsFavorite(newStatus);
  };
  
  return (
    <button onClick={handleClick}>
      {isFavorite ? '❤️ Favorilerde' : '🤍 Favorilere Ekle'}
    </button>
  );
};
```

### Vue.js

```vue
<template>
  <div>
    <button @click="toggleFavorite" :class="{ active: isFavorite }">
      <span v-if="isFavorite">❤️</span>
      <span v-else>🤍</span>
    </button>
  </div>
</template>

<script>
export default {
  props: ['productId'],
  data() {
    return {
      isFavorite: false
    }
  },
  mounted() {
    this.checkFavoriteStatus();
  },
  methods: {
    async checkFavoriteStatus() {
      const response = await this.$axios.get(`/Favorites/check/${this.productId}`);
      this.isFavorite = response.data.isFavorite;
    },
    async toggleFavorite() {
      const response = await this.$axios.post('/Favorites/toggle', {
        productId: this.productId
      });
      this.isFavorite = response.data.isFavorite;
    }
  }
}
</script>
```

## 🗄️ Veritabanı Yapısı

### Favorites Tablosu

```sql
CREATE TABLE Favorites (
    Id UUID PRIMARY KEY,
    UserId UUID NOT NULL,
    ProductId UUID NOT NULL,
    CreatedBy UUID NOT NULL,
    CreatedAt TIMESTAMP NOT NULL,
    CONSTRAINT FK_Favorites_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Favorites_Products FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Favorites_User_Product UNIQUE (UserId, ProductId)
);

CREATE INDEX IX_Favorites_UserId ON Favorites(UserId);
CREATE INDEX IX_Favorites_ProductId ON Favorites(ProductId);
```

### Önemli Noktalar

- **Unique Constraint**: Bir kullanıcı aynı ürünü birden fazla kez favorilere ekleyemez
- **Cascade Delete**: Kullanıcı veya ürün silinirse, ilgili favori kayıtları da silinir
- **Indexler**: Hızlı sorgulama için UserId ve ProductId'de index
- **No Soft Delete**: Favoriler için soft delete kullanılmıyor, direkt silinir

## 🎨 UI/UX Önerileri

### 1. Favori Butonu

```html
<!-- Ürün kartında -->
<div class="product-card">
  <button class="favorite-btn">
    <span class="icon">{{ isFavorite ? '❤️' : '🤍' }}</span>
  </button>
  <!-- Ürün bilgileri -->
</div>
```

### 2. Favoriler Sayfası

- Favori ürünleri grid layout'ta göster
- Stokta olmayanları işaretle
- Fiyat değişikliklerini vurgula
- "Tümünü temizle" butonu ekle
- Boş liste için güzel bir mesaj göster

### 3. Badge/Counter

```html
<!-- Header'da favori sayısı -->
<a href="/favorites">
  ❤️ Favoriler <span class="badge">5</span>
</a>
```

### 4. Animasyonlar

- Favoriye eklendiğinde kalp animasyonu
- Favorilerden çıkarılırken fade out
- Optimistic UI updates (hemen görsel değişiklik, sonra API çağrısı)

## 🔒 Güvenlik Notları

1. **Authentication**: Tüm endpoint'ler JWT ile korumalı
2. **Authorization**: Kullanıcılar sadece kendi favorilerini görebilir/değiştirebilir
3. **Rate Limiting**: Favorilere hızlı ekleme/çıkarma için rate limit uygulayın
4. **Validation**: Product ID ve User ID validation yapılır
5. **SQL Injection**: EF Core parametreli sorgular kullanır (güvenli)

## 🚀 Migration

Veritabanına Favorites tablosunu eklemek için:

```bash
dotnet ef migrations add AddFavorites --project Infrastructure --startup-project Jewelry.API
dotnet ef database update --project Infrastructure --startup-project Jewelry.API
```

## 📊 İstatistikler ve Analytics

Favoriler üzerinden değerli insights elde edebilirsiniz:

- En çok favorilenen ürünler
- Favori conversion rate (favoriden satın almaya dönüşüm)
- Kategorilere göre favori dağılımı
- Zamana göre favori trends
- Favoriye ekleme sonrası satın alma süresi

## 🎯 İpuçları

1. **Toggle kullanın**: `/toggle` endpoint'i en kullanışlısı, client tarafı daha basit
2. **Optimistic UI**: Butona tıklandığında hemen değişiklik göster, API yanıtını beklemeden
3. **Error handling**: Ağ hataları için retry mekanizması ekleyin
4. **Cache**: Favori durumlarını client tarafta cache'leyin
5. **Bildirimler**: Favori ürünlerde indirim olduğunda kullanıcıya bildirim gönderin
6. **Sosyal özellikler**: "X kişi bu ürünü favoriledi" gibi bilgiler gösterin

## 🔄 Gelecek Geliştirmeler

- 📧 Email bildirimleri (favorideki ürünlerde indirim varsa)
- 🔔 Push notifications
- 📤 Favori listesini paylaşma
- 🎁 Favori listesi hediye önerileri
- 📈 Fiyat takibi (favorideki ürünlerin fiyat geçmişi)
- 🏷️ Favori koleksiyonları (örn: "Evlilik Hediyeleri", "Kendime Alacaklarım")

---

**Not:** Favoriler özelliği müşteri engagement ve conversion rate için çok önemlidir. Analytics takibini mutlaka yapın!

