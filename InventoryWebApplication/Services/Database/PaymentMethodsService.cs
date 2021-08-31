using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryWebApplication.DatabaseContexts;
using InventoryWebApplication.Models.Database;
using Microsoft.Extensions.Logging;

namespace InventoryWebApplication.Services.Database
{
    public class PaymentMethodsService : NameUniqueDatabaseService<PaymentMethod>
    {
        protected override void SetValues(PaymentMethod target, PaymentMethod values)
        {
            target.Name = values.Name;
            target.ProfitMarginPercentage = values.ProfitMarginPercentage;
        }

        //TODO: Change
        public async Task Set(ICollection<PaymentMethod> newMethods)
        {
            foreach (PaymentMethod newMethod in newMethods)
            {
                if (!await Add(newMethod))
                    await UpdateByName(newMethod.Name, newMethod);
            }

            foreach (PaymentMethod method in ItemSet)
            {
                string lowerName = method.Name.ToLower();
                if (newMethods.All(o => o.Name.ToLower() != lowerName))
                {
                    await DeleteByName(method.Name);
                }
            }

            await DatabaseContext.SaveChangesAsync();
        }

        public PaymentMethodsService(DatabaseContext databaseContext, ILogger<DatabaseService<PaymentMethod>> logger) :
            base(databaseContext.PaymentMethods, databaseContext, logger) { }
    }
}