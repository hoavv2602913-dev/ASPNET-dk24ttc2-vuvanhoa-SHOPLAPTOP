using System.ComponentModel.DataAnnotations;

namespace LaptopBMT.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string? Name { get; set; }

        [Display(Name = "Hãng")]
        public string? Brand { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        [Display(Name = "Ảnh")]
        public string? ImageUrl { get; set; }
        
        // Navigation : 1 Product có nhiều OrderDetail
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        // Navigation : 1 Product có nhiều CartItem
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
