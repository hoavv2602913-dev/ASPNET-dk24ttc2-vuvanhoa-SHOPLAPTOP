using LaptopBMT.Data;
using LaptopBMT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaptopBMT.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public IActionResult AddToCart(int laptopId)
        {
            // 🔥 Lấy UserId từ Session
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account"); // nếu chưa đăng nhập, quay về trang login

            var laptop = _context.Products.FirstOrDefault(l => l.ProductId == laptopId);
            if (laptop == null) return NotFound();

            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId.Value);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId.Value,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            var cartItem = _context.CartItems
                .FirstOrDefault(ci => ci.CartId == cart.CartId && ci.ProductId == laptopId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = laptop.ProductId,
                    Quantity = 1,
                    UnitPrice = laptop.Price
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Cart");
        }

        // ✅ Xóa sản phẩm khỏi giỏ hàng
        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var item = _context.CartItems.FirstOrDefault(i => i.CartItemId == cartItemId);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // ✅ Cập nhật số lượng sản phẩm
        [HttpPost]
        public IActionResult UpdateQuantity(int cartItemId, int quantity)
        {
            var item = _context.CartItems.FirstOrDefault(i => i.CartItemId == cartItemId);
            if (item != null && quantity > 0)
            {
                item.Quantity = quantity;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // ✅ Thanh toán
        [HttpPost]
        public IActionResult Checkout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cart = _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == userId.Value);

            if (cart == null || !cart.CartItems.Any())
                return RedirectToAction("Index");

            var order = new Order
            {
                UserId = userId.Value,
                UserName = HttpContext.Session.GetString("FullName") ?? "Khách hàng",
                TotalAmount = cart.CartItems.Sum(i => i.Quantity * i.UnitPrice),
                Status = "Chờ xử lý",
                OrderDate = DateTime.Now
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in cart.CartItems)
            {
                _context.OrderDetails.Add(new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }

            // ✅ Xóa giỏ hàng sau khi checkout
            _context.CartItems.RemoveRange(cart.CartItems);
            _context.SaveChanges();
            // Sau khi tạo đơn hàng thành công
            return RedirectToAction("Success", "Cart", new { orderId = order.OrderId });
        }

        // ✅ Trang sau thanh toán thành công
        public IActionResult Success(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }

        // ✅ Hiển thị giỏ hàng
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cart = _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == userId.Value);

            return View(cart);
        }
    }
}
