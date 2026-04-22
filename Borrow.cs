using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Borrow
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public DateTime borrowedDate { get; set; } = DateTime.Now;

        public DateTime dateTobereturn { get; set; } = DateTime.Now.AddDays(7);

        public DateTime? returnedDate { get; set; } = null;


        public int fine { get; set; } = 0;

        public ApplicationUser User { get; set; }
        public Product Product { get; set; }
    }
}

