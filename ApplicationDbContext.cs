using Library.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<AddToCart> AddToCarts { get; set; }
        public DbSet<Borrow> Borrows { get; set; }
        public DbSet<ReturnBook> ReturnBooks {  get; set; }

        public DbSet<Notification> Notifications { get; set; }
    }
}
