using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaptopBMT.Data;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }
    public IActionResult Contact()
    {
        return View();
    }
    // Phân trang sản phẩm trên trang chủ
    public IActionResult Index(int page = 1)
    {
        int pageSize = 8; // ✅ số sản phẩm mỗi trang

        var products = _context.Products
            .OrderByDescending(p => p.ProductId);

        var pagedData = products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // ✅ Gửi tổng số trang qua ViewBag
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(products.Count() / (double)pageSize);

        return View(pagedData);
    }
}
