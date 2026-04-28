using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class AddToCart
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }

        public ApplicationUser User { get; set; } 
        public Product Product { get; set; }
    }
}
