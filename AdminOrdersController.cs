using Library.ModelDtos;
using Library.Models;
using Library.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Library.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("/AdminOrders/{action=Index}/{id?}")]

    public class AdminBorrowedBooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly int _pageSize = 5;

        public AdminBorrowedBooksController(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index(int pageIndex = 1)
        {
            IQueryable<Borrow> query = _context.Borrows.Include(b => b.User)
                                                       .Include(b => b.Product)
                                                       .OrderByDescending(b => b.borrowedDate);

            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            decimal count = query.Count();
            int totalPages = (int)Math.Ceiling(count / _pageSize);

            query = query.Skip((pageIndex - 1) * _pageSize).Take(_pageSize);
            var borrowedBooks = query.ToList();

            ViewBag.BorrowedBooks = borrowedBooks;
            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalPages = totalPages;

            return View();
        }

        public IActionResult Details(int id)
        {
            var borrow = _context.Borrows.Include(b => b.User)
                                         .Include(b => b.Product)
                                         .FirstOrDefault(b => b.Id == id);

            if (borrow == null)
            {
                return RedirectToAction("Index");
            }

            return View(borrow);
        }

        public IActionResult Notification()
        {
            var notification = _context.Notifications.ToList();
            return View(notification);
        }
        [HttpPost]
        public IActionResult AcceptRequest(int Id)
        {
           

            var notification = _context.Notifications.FirstOrDefault(b => b.Id == Id)!;
            var borrow = _context.Borrows.FirstOrDefault(b => b.Id == notification.BorrowId)!;
            borrow.returnedDate = notification.RequestDate;
            if (borrow.dateTobereturn < notification.RequestDate)
            {
                borrow.fine += (DateTime.Now - borrow.dateTobereturn).Days;
            }
            _context.Update(borrow);
            _context.Remove(notification);
            _context.SaveChanges();
            return RedirectToAction("Notification");
        }
    }
}
