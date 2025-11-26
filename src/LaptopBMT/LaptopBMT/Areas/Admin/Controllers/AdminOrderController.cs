using LaptopBMT.Data;
using LaptopBMT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LaptopBMT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminOrderController : Controller
    {
        private readonly AppDbContext _context;

        public AdminOrderController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Hiển thị danh sách đơn hàng
        public IActionResult Index()
        {
            // Lấy tất cả đơn hàng từ database
            var orders = _context.Orders
            .Include(o => o.User) // <-- Lấy luôn User
            .ToList();
            return View(orders);
        }

        // ✅ Xem chi tiết đơn hàng
        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
                return NotFound();

            // Nếu muốn hiển thị cả chi tiết sản phẩm trong đơn hàng:
             var orderDetails = _context.OrderDetails
                 .Where(d => d.OrderId == id)
                 .ToList();
             ViewBag.OrderDetails = orderDetails;

            return View(order);
        }
        public IActionResult OrderDetail(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderId == id);

            return View(order);
        }

        // ✅ Xóa đơn hàng
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id); // Tự động tìm theo OrderId vì EF hiểu khóa chính
            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
