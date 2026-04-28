using System.ComponentModel.DataAnnotations;

namespace Library.ModelDtos
{
    public class AddToCartDto
    {

        public string UserId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

    }
}
