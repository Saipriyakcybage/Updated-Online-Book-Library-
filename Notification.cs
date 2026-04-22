using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Notification
    {
        public int Id { get; set; }
        [Required]

        public DateTime RequestDate { get; set; }

        [Required]
        public int BorrowId { get; set; }
        

       

    }
}
