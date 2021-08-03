using System.Threading.Tasks;
using InventoryWebApplication.Models;
using InventoryWebApplication.Models.DatabaseContexts;

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