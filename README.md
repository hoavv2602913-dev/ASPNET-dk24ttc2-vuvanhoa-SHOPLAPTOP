# 📘 Thông tin Đề tài ASP.NET

## 🏫 Trường Đại học Trà Vinh
**Chuyên đề:** ASP.NET

---

## 💡 Chủ đề
**Xây dựng website bán Laptop**

---

## 👨‍🎓 Thông tin Sinh viên

| Họ và tên     | Lớp       | Cơ sở ngành                                             | Giảng viên hướng dẫn |
|----------------|------------|-------------------------------------------------------|-----------------------|
| **Vũ Văn Hòa** | DK24TTC2   | ASPNET-dk24ttc2-vuvanhoav-SHOPLAPTOP                  | TS. Đoàn Phước Miền  |

---

## 📧 Liên hệ

- **Email:** [hoavv2602913@tvu-onschool.edu.vn](mailto:hoavv2602913@tvu-onschool.edu.vn)  
- **SĐT:** 0367 531 194  

---

## 🕹️ Ghi chú

> Dự án được phát triển trong khuôn khổ môn học **ASP.NET** tại **Trường Đại học Trà Vinh**.  
> Sinh viên chịu trách nhiệm toàn bộ quá trình **thiết kế** và **xây dựng website bán laptop**.

---

## 🚀 Hướng dẫn cài đặt và chạy

###  Yêu cầu hệ thống
- **.NET 5.0 SDK**
- **SQL Server** (LocalDB hoặc SQL Server 2019+)
- **Visual Studio 2019/2022** hoặc **VS Code**

### Tài khoản Demo
| Vai trò |Tài khoản | Mật khẩu |
|---------|-------------------|----------|
| **Admin** | `admin` | `123` |
| **User** | `hoavv` | `123` |
### Cấu hình Database
1. Mở **SQL Server Management Studio (SSMS)**.
2. Restore file Database backup `DBShopLaptopBMT.bak` trong thư mục gốc.
###  Cấu hình kết nối
Mở `appsettings.json` và cập nhật `ConnectionStrings` nếu cần:
```json
 "DefaultConnection": "Server=(localdb)\\SQLEXPRESS;Database=LaptopBMT;Trusted_Connection=True;TrustServerCertificate=True;"
```

---
**Báo cáo cập nhật

> Ngày cập nhật: ** 18/10/2025 : Tìm hiểu cơ sở lý thuyết, các khái niệm, công cụ để thực hiện đề tài **.  
> Ngày cập nhật: ** 19/10/2025 : Upload báo cáo về Cơ sở lý thuyết phục vụ đề tài: Chương 1, Chương 2 **.  
> Ngày cập nhật: ** 20/10/2025 : Thực hành sử dụng Bootstrap để tạo giao diện trang Web cơ bản **.  
> Ngày cập nhật: ** 21/10/2025 : Thực hành sử dụng SQL Server tạo các bảng dữ liệu Sản phẩm, Người dùng, cập nhật báo cáo chuyên đề.  
> Ngày cập nhật: ** 28/10/2025 : Thực hành tạo các chức năng giao diện, thêm, sửa sản phẩm lưu vào CSDL **.  
> Ngày cập nhật: ** 08/11/2025 : Thực hành tạo các chức năng giao diện đặt hàng lưu vào CSDL **.  
> Ngày cập nhật: ** 12/11/2025 : Upload code lên /src **.  
> Ngày cập nhật: ** 21/11/2025 : Upload BC CĐ ASPNET VuVanHoa_170124242_DK24TTC2 lên /progress-report **.  
---
