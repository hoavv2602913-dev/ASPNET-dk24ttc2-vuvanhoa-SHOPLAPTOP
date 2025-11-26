using LaptopBMT.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LaptopBMT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy tổng đơn hàng theo tháng (1-12)
            var revenueData = Enumerable.Range(1, 12).Select(month =>
                _context.Orders
                    .Where(o => o.OrderDate.Month == month && o.Status == "Hoàn thành")
                    .Sum(o => (decimal?)o.TotalAmount) ?? 0
            ).ToList();

            ViewBag.RevenueData = revenueData;

            return View();
        }
    }
}
