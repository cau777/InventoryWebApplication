using InventoryWebApplication.Models.DatabaseContexts;

namespace InventoryWebApplication.Services
{
    public class ProductsService
    {
        private readonly DatabaseContext _databaseContext;
        
        public ProductsService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
    }
}