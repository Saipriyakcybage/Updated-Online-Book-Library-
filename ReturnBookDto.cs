using System.ComponentModel.DataAnnotations;

namespace Library.ModelDtos
{
    public class ReturnBookDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.(com|org|net|edu)$",
       ErrorMessage = "Email must end with .com, .org, .net, or .edu.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }
    }
}
