using System;
using System.Linq;
using System.Threading.Tasks;
using InventoryWebApplication.DatabaseContexts;
using InventoryWebApplication.Models.Database;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryWebApplication.Services.Database
{
    public class ProductsService : NameUniqueDatabaseService<Product>
    {
        private readonly DatabaseContext _databaseContext;

        public ProductsService(DatabaseContext databaseContext, ILogger<DatabaseService<Product>> logger) : base(
            databaseContext.Products, databaseContext, logger)
        {
            _databaseContext = databaseContext;
        }

        /// <summary>
        ///     Adds a number to the available quantity of a product
        /// </summary>
        /// <param name="product">Product to be updated</param>
        /// <param name="shift">Number to add</param>
        /// <exception cref="ArgumentException">The result of the operation would be negative</exception>
        public async Task ShiftProductQuantity([System.Diagnostics.CodeAnalysis.NotNull] Product product, int shift)
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

        public async Task<Product[]> SearchProducts([CanBeNull] string query)
        {
            IQueryable<Product> source = ItemSet;
            if (query is not null)
            {
                query = query.ToLower();
                source = ItemSet.Where(o => o.Name.ToLower().Contains(query));
            }
            return await source.ToArrayAsync();
        }
    } 
}