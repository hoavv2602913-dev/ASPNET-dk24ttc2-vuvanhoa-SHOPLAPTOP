using LaptopBMT.Data;
using Microsoft.AspNetCore.Mvc;

public class ProductDetailController : Controller
{
    private readonly AppDbContext _context;

    public ProductDetailController(AppDbContext context)
    {
        _context = context;
    }
    public IActionResult Detail(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
        if (product == null)
            return NotFound(); 
        return View(product);
    }

}
