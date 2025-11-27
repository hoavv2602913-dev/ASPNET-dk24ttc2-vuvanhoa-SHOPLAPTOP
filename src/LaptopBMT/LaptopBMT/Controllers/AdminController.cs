using LaptopBMT.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LaptopBMT.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        // 📊 Trang Dashboard thống kê
        public IActionResult Dashboard()
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied");

            // --- Thống kê cơ bản ---
            var totalUsers = _context.Users.Count();
            var totalProducts = _context.Products.Count();
            var totalOrders = _context.Carts.Count(c => c.IsCheckedOut);
            var totalRevenue = _context.CartItems
                .Where(ci => ci.Cart != null && ci.Cart.IsCheckedOut)
                .Sum(ci => ci.Quantity * ci.UnitPrice);

            // --- Doanh thu theo tháng ---
            var monthlyRevenue = _context.Carts
            .Where(c => c.IsCheckedOut && c.CreatedAt != null)
            .GroupBy(c => c.CreatedAt!.Value.Month)
            .Select(g => new
            {
                Month = g.Key,
                Revenue = g.Sum(c => c.CartItems.Sum(ci => ci.Quantity * ci.UnitPrice))
            })
            .OrderBy(x => x.Month)
            .ToList();
            
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalProducts = totalProducts;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.MonthlyRevenue = monthlyRevenue;

            return View();
        }

        // 🚫 Không có quyền truy cập
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
