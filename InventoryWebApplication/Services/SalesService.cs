using System.Threading.Tasks;
using InventoryWebApplication.DatabaseContexts;
using InventoryWebApplication.Models;

namespace InventoryWebApplication.Services
{
    public class SalesService
    {
        private readonly DatabaseContext _databaseContext;
        public SalesService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task AddSale(SaleInfo saleInfo)
        {
            _databaseContext.Add(saleInfo);
            await _databaseContext.SaveChangesAsync();
        }
    }
}