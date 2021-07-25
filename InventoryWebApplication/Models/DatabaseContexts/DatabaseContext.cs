using Microsoft.EntityFrameworkCore;

namespace InventoryWebApplication.Models.DatabaseContexts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}