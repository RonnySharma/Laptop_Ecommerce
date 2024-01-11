using Laptop_Ecommerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Laptop_Ecommerce.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet <LaptopCompany>LaptopCompanies { get; set; }
        public DbSet <Processor>Processors { get; set; }
        public DbSet <GraphicsCard>GraphicsCards { get; set; }
        public DbSet <Laptop>Laptops { get; set; }
        public DbSet <Company>Companies { get; set; }
        public DbSet <ApplicationUser>ApplicationUsers { get; set; }
        public DbSet <ShoppingCart>ShoppingCarts { get; set; }
        public DbSet <OrderHeader>OrderHeaders { get; set; }
        public DbSet <OrderDetails>OrderDetails { get; set; }

    }
}