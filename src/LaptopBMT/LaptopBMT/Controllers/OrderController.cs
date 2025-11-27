using LaptopBMT.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class OrderController : Controller
{
    private readonly AppDbContext _context;

    public OrderController(AppDbContext context)
    {
        _context = context;
    }
    // ✅ Danh sách đơn hàng theo User
    public IActionResult List()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var orders = _context.Orders
            .Where(o => o.UserId == userId.Value)
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        return View(orders);
    }
    public IActionResult Details(int id)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var order = _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .FirstOrDefault(o => o.OrderId == id && o.UserId == userId); // Chỉ xem đơn của mình

        if (order == null)
            return NotFound();

        return View(order);
    }
}
