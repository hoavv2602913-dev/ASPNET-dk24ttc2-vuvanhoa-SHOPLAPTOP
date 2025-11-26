using LaptopBMT.Data;
using LaptopBMT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Cần thêm using này cho .FirstOrDefaultAsync()

namespace LaptopBMT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public UsersController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // 📋 Danh sách người dùng
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Index", "Home");
            var users = _context.Users.ToList();
            return View(users);
        }
        // ✅ Thêm người dùng
        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Index", "Home");

            return View();
        }
        // ✅ POST Thêm người dùng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            user.CreatedAt = DateTime.Now;
            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // ✅ Sửa người dùng
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // Thêm IFormFile? AvatarFile và xử lý logic
        [HttpPost]
        [ValidateAntiForgeryToken] // Thêm để bảo mật
        public async Task<IActionResult> Edit(int id, string newPassword, User user, IFormFile? AvatarFile)
        {
            // Dùng FirstOrDefaultAsync để tương thích với await
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (existingUser == null)
            {
                return NotFound();
            }

            // Cập nhật các trường khác
            existingUser.UserName = user.UserName;
            existingUser.FullName = user.FullName;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;

            // Nếu có nhập mật khẩu mới thì mới đổi
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                // PHẢI hash (mã hóa) mật khẩu này trước khi lưu vào DB.
                existingUser.PasswordHash = newPassword;
            }
            // LOGIC XỬ LÝ AVATAR (Lấy từ action Profile )
            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                // Xóa ảnh cũ nếu có
                if (!string.IsNullOrEmpty(existingUser.AvatarUrl))
                {
                    var oldPath = Path.Combine(_environment.WebRootPath, existingUser.AvatarUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                // Upload ảnh mới
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(AvatarFile.FileName)}";
                var uploadDir = Path.Combine(_environment.WebRootPath, "uploads", "avatars");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                var filePath = Path.Combine(uploadDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    // Dùng CopyToAsync vì đây là action async
                    await AvatarFile.CopyToAsync(stream);
                }

                // Cập nhật đường dẫn vào DB
                existingUser.AvatarUrl = fileName;
            }
            // ========================================================

            // Dùng SaveChangesAsync
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // ✅ Xóa người dùng
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            // (Nên) Xóa file avatar của user trước khi xóa user
            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                var oldPath = Path.Combine(_environment.WebRootPath, user.AvatarUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // ✅ Phân quyền
        [HttpGet]
        public IActionResult AssignRole(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AssignRole(int id, string role)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            user.Role = role;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Profile()
        {
            int userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
            var user = _context.Users.Find(userId);
            return View(user);
        }

        [HttpPost]
        public IActionResult Profile(User model, IFormFile? AvatarFile, string? removeAvatar)
        {
            var user = _context.Users.Find(model.UserId);
            if (user == null) return NotFound();

            // Cập nhật thông tin
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;

            // Nếu người dùng nhấn "Xóa ảnh đại diện"
            if (!string.IsNullOrEmpty(removeAvatar))
            {
                if (!string.IsNullOrEmpty(user.AvatarUrl))
                {
                    var oldPath = Path.Combine(_environment.WebRootPath, user.AvatarUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                user.AvatarUrl = null;
            }
            else if (AvatarFile != null && AvatarFile.Length > 0)
            {
                // Xóa ảnh cũ nếu có
                if (!string.IsNullOrEmpty(user.AvatarUrl))
                {
                    var oldPath = Path.Combine(_environment.WebRootPath, user.AvatarUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                // Upload ảnh mới
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(AvatarFile.FileName)}";
                var uploadDir = Path.Combine(_environment.WebRootPath, "uploads", "avatars");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                var filePath = Path.Combine(uploadDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    AvatarFile.CopyTo(stream);
                }

                user.AvatarUrl = fileName;
            }

            _context.SaveChanges();
            ViewBag.Message = "Cập nhật thông tin thành công!";
            return View(user);
        }
    }
}