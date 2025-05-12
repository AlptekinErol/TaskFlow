# 🧩 TaskFlow – RabbitMQ & Redis Temelli Mikro Proje

**TaskFlow**, RabbitMQ ve Redis kullanılarak geliştirilmiş öğretici bir mikroservis demosudur. Projenin amacı, bu iki güçlü teknolojiyi .NET ekosisteminde kullanmayı öğrenmek ve orta düzey senaryolarda uygulayabilmektir.

---

##  Hedefler

-  RabbitMQ ile mesaj kuyruğu oluşturmak ve mesaj yayını gerçekleştirmek
-  Redis ile veri cache'lemek ve süreli (TTL) veri yönetimi sağlamak
-  Worker Service ile arka planda mesaj tüketmek
-  API ile görevleri oluşturmak ve Redis üzerinden sorgulamak

---

##  Proje Yapısı

```
TaskFlow/
├── TaskFlow.Api             → Görev oluşturma ve sorgulama API'si
├── TaskFlow.Worker          → RabbitMQ’den mesajları dinleyip Redis’e kaydeder
├── TaskFlow.Shared          → Ortak DTO ve mesaj modelleri
├── TaskFlow.Infrastructure → RabbitMQ ve Redis servisleri
└── docker-compose.yml       → RabbitMQ ve Redis container tanımları
```

---

##  Kullanılan Teknolojiler

- ASP.NET Core 8 Web API
- .NET Worker Service
- RabbitMQ.Client
- StackExchange.Redis
- Docker & Docker Compose
- Swagger (Swashbuckle)

---

##  Kurulum

### 1. Bağımlılıkları başlat (Docker ile)
```bash
docker-compose up -d
```

> Bu işlem sonrası aşağıdaki servisler aktif olur:
> - RabbitMQ → http://localhost:15672 (guest/guest)
> - Redis → localhost:6379

### 2. Projeyi derle ve çalıştır
```bash
dotnet run --project TaskFlow.Api
dotnet run --project TaskFlow.Worker
```

---

##  API Kullanımı

### POST /api/tasks
Yeni bir görev oluşturur ve RabbitMQ kuyruğuna mesaj gönderir.

```json
{
  "title": "Redis öğren",
  "description": "Set/Get işlemleri test edilecek"
}
```

### 🔍 GET /api/tasks/{id}
Redis'ten ilgili görev verisini getirir. TTL süresi dolmuşsa 404 döner.

---

##  Özellikler

- ✅ RabbitMQ üzerinden asenkron mesajlaşma
- ✅ Worker servis ile mesaj tüketimi
- ✅ Redis TTL (expire) ile süreli veri saklama
- ✅ API ile görev oluşturma ve sorgulama
- ✅ Docker ile hızlı ortam kurulumu

---

##  Öğrenilenler

- Raw RabbitMQ bağlantısı ile kuyruğa mesaj gönderme ve tüketme
- Redis’te veri saklama, TTL uygulama ve CLI üzerinden sorgulama
- .NET Worker Service ve DI kullanımı
- Gerçek zamanlı senaryo üzerinden mesajlaşma mantığını kavrama
