using System;
using System.Linq;
using System.Threading.Tasks;
using InventoryWebApplication.DatabaseContexts;
using InventoryWebApplication.Models;
using Microsoft.Extensions.Logging;

namespace InventoryWebApplication.Services
{
    public class ProductsService : DatabaseService<Product>
    {
        private readonly ILogger<DatabaseService<Product>> _logger;
        private readonly DatabaseContext _databaseContext;

        public ProductsService(DatabaseContext databaseContext, ILogger<DatabaseService<Product>> logger) : base(
            databaseContext.Products, databaseContext, logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task ShiftProductQuantity(int id, int shift)
        {
            Product product = await GetById(id);
            if (product is null) throw new NullReferenceException();
            if (product.AvailableQuantity + shift < 0) throw new ArgumentException("Negative quantity");
            product.AvailableQuantity += shift;
            await _databaseContext.SaveChangesAsync();
        }

        public override bool IsPresent(Product element)
        {
            string lowerName = element.Name.ToLower();
            return ItemSet.Any(o => lowerName == o.Name.ToLower());
        }

        protected override bool CanBeAdded(Product element)
        {
            return !IsPresent(element);
        }

        protected override bool CanBeEdited(Product target, Product values)
        {
            string lowerName = target.Name.ToLower();
            return GetAll().All(o => o.Id == target.Id || o.Name.ToLower() != lowerName);
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