using LaptopBMT.Data;
using LaptopBMT.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaptopBMT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Admin/AdminProduct
        public IActionResult Index()
        {
            return View(_context.Products.ToList());
        }

        // GET: Admin/AdminProduct/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminProduct/Create
        [HttpPost]
        public IActionResult Create(Product product, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Nếu có upload ảnh
                if (ImageFile != null)
                {
                    string uploadFolder = Path.Combine(_env.WebRootPath, "images/product_img");
                    if (!Directory.Exists(uploadFolder))
                        Directory.CreateDirectory(uploadFolder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    product.ImageUrl = $"/images/product_img/{fileName}";
                }

                _context.Products.Add(product);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(product);
        }
        // GET: Admin/AdminProduct/Edit
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Admin/AdminProduct/Edit
        [HttpPost]
        public IActionResult Edit(Product product, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = _context.Products.Find(product.ProductId);
                if (existingProduct == null)
                    return NotFound();

                // Cập nhật thông tin cơ bản
                existingProduct.Name = product.Name;
                existingProduct.Brand = product.Brand;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;

                // Nếu có upload ảnh mới, thì xử lý thay ảnh
                if (ImageFile != null)
                {
                    string uploadFolder = Path.Combine(_env.WebRootPath, "images/product_img");

                    if (!Directory.Exists(uploadFolder))
                        Directory.CreateDirectory(uploadFolder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    // Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(existingProduct.ImageUrl))
                    {
                        string oldFile = Path.Combine(_env.WebRootPath, existingProduct.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                    }

                    // Lưu ảnh mới
                    existingProduct.ImageUrl = $"/images/product_img/{fileName}";
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }
        // GET: Admin/AdminProduct/Delete
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Admin/AdminProduct/Delete
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
                return NotFound();

            // Nếu sản phẩm có ảnh -> xóa ảnh trong thư mục
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                string oldFile = Path.Combine(_env.WebRootPath, product.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}