using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaptopBMT.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }
        public User? User { get; set; }
        // Navigation: Thuộc tính điều hướng bên Nhiều
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
