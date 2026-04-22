using System.ComponentModel.DataAnnotations;

namespace Library.ModelDtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.(com|org|net|edu)$",
       ErrorMessage = "Email must end with .com, .org, .net, or .edu.")]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        public bool RememberMe { get; set; }

    }
}
