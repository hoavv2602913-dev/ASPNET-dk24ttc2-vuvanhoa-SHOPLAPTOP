using LaptopBMT.Data;
using LaptopBMT.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaptopBMT.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        // ✅ 1. Cần inject IWebHostEnvironment
        private readonly IWebHostEnvironment _environment;

        // ✅ 2. Cập nhật constructor
        public AccountController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // ✅ 3.[HttpGet] Profile để luôn lấy data mới nhất từ DB
        [HttpGet]
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users.Find(userId.Value);
            if (user == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }

        // ✅ 4.[HttpPost] Profile để nhận và xử lý file
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(User model, IFormFile? AvatarFile, string? removeAvatar)
        {
            var user = _context.Users.Find(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin cơ bản
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;

            // === LOGIC XỬ LÝ AVATAR ===

            // Nếu người dùng tải lên file mới
            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                // Xóa ảnh cũ (nếu có)
                if (!string.IsNullOrEmpty(user.AvatarUrl))
                {
                    var oldPath = Path.Combine(_environment.WebRootPath, "uploads", "avatars", user.AvatarUrl);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                // Upload ảnh mới
                // Chỉ lưu tên file (để khớp với logic view)
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(AvatarFile.FileName)}";
                var uploadDir = Path.Combine(_environment.WebRootPath, "uploads", "avatars");

                Directory.CreateDirectory(uploadDir); // Tạo thư mục nếu chưa có

                var filePath = Path.Combine(uploadDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await AvatarFile.CopyToAsync(stream); // Dùng async
                }

                // Cập nhật tên file mới vào DB
                user.AvatarUrl = fileName;
            }
            // (Nếu không chọn file mới, sẽ tự động giữ nguyên avatar cũ)

            // Lưu tất cả thay đổi vào DB
            await _context.SaveChangesAsync();

            // Cập nhật lại Session FullName (để thanh header hiển thị đúng)
            HttpContext.Session.SetString("FullName", user.FullName ?? user.UserName);

            ViewBag.Message = "Cập nhật thông tin thành công!";
            return View(user); // Trả về view với dữ liệu đã được cập nhật
        }

        #region Login, Register, Logout 

        // ✅ Trang đăng nhập
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // ✅ Xử lý đăng nhập
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username && u.PasswordHash == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("FullName", user.FullName ?? user.UserName);
                HttpContext.Session.SetString("Role", user.Role);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu!";
            return View();
        }

        // ✅ Trang đăng ký
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // ✅ Xử lý đăng ký
        [HttpPost]
        public IActionResult Register(string username, string password, string fullname, string email)
        {
            if (_context.Users.Any(u => u.UserName == username))
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại!";
                return View();
            }
            var user = new User
            {
                UserName = username,
                PasswordHash = password, // mã hóa mật khẩu
                FullName = fullname,
                Email = email,
                Role = "User",
                CreatedAt = DateTime.Now
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("FullName", user.FullName ?? user.UserName);
            HttpContext.Session.SetString("Role", user.Role);
            return RedirectToAction("Index", "Home");
        }

        // ✅ Đăng xuất
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
