using LaptopBMT.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopBMT.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        // Khóa ngoại
        public int CartId { get; set; }

        // Khóa ngoại
        public int ProductId { get; set; }

        // Navigation: Thuộc tính điều hướng bên Một (có thể rỗng)
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? ImageUrl { get; set; }

        // Navigation: Thuộc tính điều hướng bên Một (có thể rỗng)
        public Cart? Cart { get; set; }
    }
}
