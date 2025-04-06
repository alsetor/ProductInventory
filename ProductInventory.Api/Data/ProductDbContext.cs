using Microsoft.EntityFrameworkCore;
using ProductInventory.Api.Models;

namespace ProductInventory.Api.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}