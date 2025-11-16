using LaptopBMT.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace LaptopBMT.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public string? AvatarUrl { get; set; }
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
