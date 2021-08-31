using InventoryWebApplication.DatabaseContexts;
using InventoryWebApplication.Models.Database;
using Microsoft.Extensions.Logging;

namespace InventoryWebApplication.Services.Database
{
    public class PaymentMethodsService : NameUniqueDatabaseService<PaymentMethod>
    {
        public PaymentMethodsService(DatabaseContext databaseContext, ILogger<DatabaseService<PaymentMethod>> logger) :
            base(databaseContext.PaymentMethods, databaseContext, logger) { }

        protected override void SetValues(PaymentMethod target, PaymentMethod values)
        {
            target.Name = values.Name;
            target.ProfitMarginPercentage = values.ProfitMarginPercentage;
        }
    }
}