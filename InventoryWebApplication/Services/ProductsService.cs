using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using InventoryWebApplication.DatabaseContexts;
using InventoryWebApplication.Models;
using Microsoft.Extensions.Logging;

namespace InventoryWebApplication.Services
{
    public class ProductsService : NameUniqueDatabaseService<Product>
    {
        private readonly DatabaseContext _databaseContext;

        public ProductsService(DatabaseContext databaseContext, ILogger<DatabaseService<Product>> logger) : base(
            databaseContext.Products, databaseContext, logger)
        {
            _databaseContext = databaseContext;
        }

        public async Task ShiftProductQuantityById(int id, int shift)
        {
            Product product = await GetById(id);
            if (product is null) throw new NullReferenceException();
            if (product.AvailableQuantity + shift < 0) throw new ArgumentException("Negative quantity");
            product.AvailableQuantity += shift;
            await _databaseContext.SaveChangesAsync();
        }

        public async Task ShiftProductQuantity([NotNull] Product product, int shift)
        {
            if (product.AvailableQuantity + shift < 0) throw new ArgumentException("Negative quantity");
            product.AvailableQuantity += shift;
            await _databaseContext.SaveChangesAsync();
        }

        protected override void SetValues(Product target, Product values)
        {
            target.Name = values.Name;
            target.Description = values.Description;
            target.AvailableQuantity = values.AvailableQuantity;
            target.Cost = values.Cost;
            target.SellPrice = values.SellPrice;
        }
    }
}