using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Brand { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }

        public string Description { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ImageFileName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }


    }
}
