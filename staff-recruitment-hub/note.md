# HƯỚNG DẪN SETUP & CHẠY PROJECT

> Tài liệu này dùng để **hướng dẫn Setup môi trường và chạy Project lần đầu**

---

## [ 1 ] YÊU CẦU HỆ THỐNG

#### > YÊU CẦU .NET SDK

- **Yêu cầu Cài đặt**: `.NET 10 SDK`

Nếu máy chưa có, bắt buộc cài `.NET 10`
-> Tải tại: https://dotnet.microsoft.com/download

- **Hệ thống Target**: chạy trên `.NET 10`
- **Database**: SQL Server (Local hoặc Docker)
- **Công cụ EF Core**: Chạy lệnh sau nếu chưa cài:

```bash
dotnet tool install --global dotnet-ef
```

Kiểm tra sau khi cài:

```bash
dotnet --version
```

---

## [ 2 ] CÀI ĐẶT THƯ VIỆN DỰ ÁN

Chạy file sau trước khi chạy project:

```bash
SetupProject.bat
```

Script này sẽ thực hiện các Qui trình sau:

- Restore packages
- Setup các thư viện cần thiết cho Project

---

## [ 3 ] REDIS & DATABASE

### 🛠 HƯỚNG DẪN CÀI ĐẶT & CẤU HÌNH REDIS

Trường hợp máy **chưa có môi trường Redis**, hãy thực hiện theo các cách sau:

**Cách 1:** (Khuyên dùng)

- Cài **Memurai Redis**
- (Khuyến nghị) Cài thêm **Redis Insight** để quản lý Redis qua UI

**Cách 2:**

- Nếu đã có Docker, chạy lệnh sau để khởi tạo nhanh

```bash
docker run --name redis-local -p 6379:6379 -d redis
```

Thông tin cấu hình

- **Host:** `localhost`, **Port:**: `6379` (Mặc định)
- Nếu Redis dùng Host hay Port khác ➜ chỉnh lại trong:
   - `appsettings.json`
   - hoặc `appsettings.Development.json`

Ví dụ:

```json
"RedisConnection": "yourHostRedis:yourPortRedis,abortConnect=false,connectTimeout=5000"
```

### 🛠 HƯỚNG DẪN CÀI ĐẶT & CẤU HÌNH DATABASE

> ! Chỉ làm bước này nếu chưa có Data cho Database

**🔧 Nhớ cấu hình ConnectionDatabase trong:**

- `appsettings.json`
- hoặc `appsettings.Development.json`

**Bước 1:** Tạo Database & Migration ban đầu

> Yêu cầu: đã cài SQL Server trên máy tính

Chạy lệnh:

```bash
dotnet ef migrations add InitialCreate \
  --project ./src/SRB_ViewModel \
  --startup-project ./src/SRB_WebPortal
```

**Bước 2:** Update Migration

> Có thể bỏ qua nếu Bước 1 báo Done

```bash
dotnet ef migrations add FixDecimalPrecision \
  --project ./src/SRB_ViewModel \
  --startup-project ./src/SRB_WebPortal
```

**Bước 3:** Init Database Data

```bash
dotnet ef database update \
  --project ./src/SRB_ViewModel \
  --startup-project ./src/SRB_WebPortal
```

### BONUS: Khi Bước 1 bị lỗi

```bash
dotnet ef migrations remove \
  --project ./src/SRB_ViewModel \
  --startup-project ./src/SRB_WebPortal
```

---

## [ 4 ] RUN PROJECT

**Cách 1:** Chạy bằng File `.bat` (Khuyên dùng)

Auto build giao diện khi sửa Code:

```bash
RunWatchTailwind.bat
```

Chạy Web Server:

```bash
RunWebPortal.bat
```

**Cách 2:** Chạy bằng Terminal

Watch & Build Tailwind CSS:

```bash
npx @tailwindcss/cli \
  -i ./wwwroot/css/input.css \
  -o ./wwwroot/css/output.css \
  --watch
```

Chạy Web Server:

```bash
dotnet watch run
```

## [ 5 ] FINAL

Chúc bạn cài đặt và Chạy dự án thành công!
