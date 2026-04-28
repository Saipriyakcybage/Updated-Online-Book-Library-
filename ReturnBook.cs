using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class ReturnBook
    {
        public int Id { get; set; }
        [Required]
        public string Userid { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public DateTime ReturnedDate { get; set; }
        public ApplicationUser User { get; set; }
        public Product Product { get; set; }

    }
}
