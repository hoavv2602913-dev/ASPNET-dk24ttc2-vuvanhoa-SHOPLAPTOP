using LaptopBMT.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaptopBMT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Danh sách đơn hàng
        public IActionResult Index()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }

        // ✅ Xem chi tiết đơn hàng
        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null) return NotFound();

            return View(order);
        }
    }
}
