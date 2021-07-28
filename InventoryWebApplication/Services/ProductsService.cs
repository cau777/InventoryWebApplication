using System.Threading.Tasks;
using InventoryWebApplication.Models;
using InventoryWebApplication.Models.DatabaseContexts;
using Microsoft.EntityFrameworkCore;

namespace InventoryWebApplication.Services
{
    public class ProductsService
    {
        private readonly DatabaseContext _databaseContext;
        
        public ProductsService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<Product> FindProduct(int id)
        {
            return _databaseContext.Products.FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}