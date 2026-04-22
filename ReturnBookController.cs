using Library.ModelDtos;
using Library.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Models
{
    [Authorize(Roles ="admin")]
    public class ReturnBookController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ReturnBookController(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            var returnedBooks = _context.ReturnBooks
                .Include(r => r.Product)
                .Include(r => r.User)
                .ToList();

            return View(returnedBooks);
        }

        public IActionResult AddReturnItem()
        {
            return View(new ReturnBookDto());
        }

        [HttpPost]
        public async Task<IActionResult> AddReturnItem(ReturnBookDto book)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(book.Username);
                if (user == null)
                {
                    ModelState.AddModelError("", $"User with username '{book.Username}' not found.");
                    return View(book);
                }

                var updateborrow = _context.Borrows.FirstOrDefault(b => b.UserId == user.Id && b.ProductId == book.ProductId && b.returnedDate==null);
                if (updateborrow == null)
                {
                    ModelState.AddModelError("", $"User '{book.Username}' has not borrowed the book with Product ID '{book.ProductId}', or it has already been returned.");
                    return View(book);
                }
                if (updateborrow != null)
                {
                    updateborrow.returnedDate = DateTime.Now;

                    if (updateborrow.dateTobereturn < DateTime.Now)
                    {
                        updateborrow.fine += (DateTime.Now - updateborrow.dateTobereturn).Days;
                    }

                    _context.Update(updateborrow);

                    var returnEntry = new ReturnBook
                    {
                        Userid = user.Id,
                        ProductId = book.ProductId,
                        ReturnedDate = DateTime.Now
                    };

                    _context.ReturnBooks.Add(returnEntry);
                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(book);
        }
        



    }
}
