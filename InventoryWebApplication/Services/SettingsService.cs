using System.Collections.Generic;
using System.Linq;
using InventoryWebApplication.Models;
using InventoryWebApplication.Models.DatabaseContexts;

namespace InventoryWebApplication.Services
{
    public class SettingsService
    {
        private readonly DatabaseContext _databaseContext;
        
        public SettingsService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IEnumerable<string> GetAvailablePaymentMethods()
        {
            return _databaseContext.PaymentMethods.Select(method => method.Method);
        }
    }
}