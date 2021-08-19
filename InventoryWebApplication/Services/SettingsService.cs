using System.Collections.Generic;
using System.Linq;
using InventoryWebApplication.DatabaseContexts;
using InventoryWebApplication.Models;

namespace InventoryWebApplication.Services
{
    public class SettingsService
    {
        private readonly DatabaseContext _databaseContext;
        
        public SettingsService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IEnumerable<string> GetPaymentMethods()
        {
            return _databaseContext.PaymentMethods.Select(method => method.Name);
        }

        public void SetPaymentMethods(IEnumerable<PaymentMethod> methods)
        {
            foreach (PaymentMethod method in methods)
            {
                if (!_databaseContext.PaymentMethods.Any(o => o.Name == method.Name))
                {
                    _databaseContext.PaymentMethods.Add(new PaymentMethod(method.Name));
                }
            }
        }
    }
}