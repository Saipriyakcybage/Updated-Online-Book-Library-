using Library.ModelDtos;
using Library.Models;
using Library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.Controllers
{
    [Authorize(Roles = "client")]
    public class AddToCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AddToCartController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var cartItemsOfUser = _context.AddToCarts.Where(u => u.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(cartItemsOfUser);
        }


        [HttpPost]
        public IActionResult AddItems(AddToCartDto dto, int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("You must be logged in to add items to the cart.");
            }

            // ✅ Ensure we're checking for an existing book in the cart properly
            var existingCartItem = _context.Borrows.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
            if (existingCartItem != null) // If the book already exists in the cart
            {
                TempData["Message"] = "You can only add one copy of each book.";
                return RedirectToAction("Index");
            }

            // ✅ Check if the product exists
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return NotFound($"Product with ID {productId} not found.");
            }

            // ✅ Set quantity to 1 to enforce the rule
            dto.Quantity = 1;

            if (dto.Quantity <= 0 || dto.Quantity > product.Quantity)
            {
                return BadRequest($"Invalid quantity. Available stock: {product.Quantity}");
            }

            // ✅ Add the book to the cart
            var cartItem = new AddToCart
            {
                UserId = userId,
                ProductId = productId, // Use the correct productId
                Quantity = dto.Quantity
            };

            _context.Add(cartItem);
            _context.SaveChanges();

            TempData["Message"] = "Book added to the cart successfully.";
            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult UpdateIteminCart(int quantityNeed, int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                TempData["Message"] = "Product not found.";
                return RedirectToAction("Index");
            }

            var cartItem = _context.AddToCarts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
            if (cartItem == null)
            {
                TempData["Message"] = "Cart item not found.";
                return RedirectToAction("Index");
            }

            if (quantityNeed > product.Quantity || quantityNeed != 1)
            {
                TempData["Message"] = "You can only have one book in your cart.";
                return RedirectToAction("Index");
            }

            cartItem.Quantity = quantityNeed;
            _context.SaveChanges();

            TempData["Message"] = "Cart item updated successfully.";
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult RemoveItemFromCart(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("You must be logged in to remove items from the cart.");
            }

            var cartItem = _context.AddToCarts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
            if (cartItem == null)
            {
                TempData["Message"] = "Cart item not found.";
                return RedirectToAction("Index");
            }

            _context.AddToCarts.Remove(cartItem);
            _context.SaveChanges();

            TempData["Message"] = "Item removed from the cart.";
            return RedirectToAction("Index");
        }


    }
}