using System.Diagnostics;
using Library.Models;
using Library.Services;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Library.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext context;
        private readonly int pageSize = 4;

        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index(int pageIndex)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            IQueryable<Product> query = context.Products.OrderByDescending(p => p.Id);

 
            decimal count = query.Count();
            int totalPages = (int)Math.Ceiling(count / pageSize);

            var products = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            ViewData["PageIndex"] = pageIndex;
            ViewData["TotalPages"] = totalPages;
            ViewBag.Products = products;
           
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
