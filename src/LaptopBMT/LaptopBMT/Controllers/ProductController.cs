using LaptopBMT.Data;
using LaptopBMT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaptopBMT.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        // Tìm kiếm sản phẩm theo từ khóa
        public IActionResult Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return View(new List<Product>());
            }

            var results = _context.Products
                .Where(p => p.Name.Contains(keyword))
                .ToList();

            ViewBag.Keyword = keyword;
            return View(results);
        }
    }
}
