using InventoryWebApplication.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace InventoryWebApplication.DatabaseContexts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { }
        
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SaleInfo> Sales { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
    }
}