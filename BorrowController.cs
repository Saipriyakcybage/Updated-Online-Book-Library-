using Library.Migrations;
using Library.ModelDtos;
using Library.Models;
using Library.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Library.Controllers
{
    [Authorize(Roles = "client")]
    public class BorrowController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly int pageSize = 3;
        public BorrowController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int PageIndex = 1)
        {
            if (PageIndex < 1)
            {
                PageIndex = 1;
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch all borrow records for the logged-in user
            var borrows = _context.Borrows
                .Where(a => a.UserId == userId)
                .ToList();

            int totalCount = borrows.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var query = borrows.Skip((PageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            // Get borrow IDs that already have a notification in the Notifications table
            List<int> notificationBorrowIds = _context.Notifications
                .Where(n => borrows.Select(b => b.Id).Contains(n.BorrowId))
                .Select(n => n.BorrowId)
                .ToList();

            ViewData["PageIndex"] = PageIndex;
            ViewData["TotalPages"] = totalPages;
            ViewData["Notifications"] = notificationBorrowIds; // Store borrow IDs that have notifications

            return View(query);
        }



        [HttpPost]
        public IActionResult AddToBorrow()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItemsOfUser = _context.AddToCarts
                .Where(a => a.UserId == userId)
                .ToList();

            if (!cartItemsOfUser.Any())
            {
                TempData["Message"] = "Your cart is empty. Please add items to borrow.";
                return RedirectToAction("Index");
            }
   


            foreach (var item in cartItemsOfUser)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);

                if (product == null || product.Quantity < item.Quantity)
                {
                    TempData["Message"] = $"Not enough stock for product {product?.Name ?? "unknown"}.";
                    return RedirectToAction("Index");
                }

            

                var borrowItem = new Borrow()
                {
                    UserId = userId,

                    ProductId = item.ProductId,
                    borrowedDate = DateTime.Now,
                    dateTobereturn = DateTime.Now.AddDays(7),
                };



                _context.Add(borrowItem);

                _context.Remove(item);
                product.Quantity -= item.Quantity;
                _context.Update(product);
            }

            _context.SaveChanges();

            TempData["Message"] = "Items have been successfully borrowed.";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult AddToNotification(int id)
        {
            var data= _context.Borrows.FirstOrDefault(b => b.Id == id);

            if(data == null)
            {
                TempData["Message"] = "Something went wrong";
            }

            Notification notification = new Notification()
            {
                BorrowId = data.Id,
                RequestDate = DateTime.Now

            };

            _context.Add(notification);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult DeleteRequest(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var borrow = _context.Borrows.FirstOrDefault(e => e.Id == id && e.UserId==userId)!;
            var notification = _context.Notifications.FirstOrDefault(e => e.BorrowId == borrow.Id)!;
            _context.Remove(notification);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
