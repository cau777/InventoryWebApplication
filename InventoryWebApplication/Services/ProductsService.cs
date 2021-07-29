using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryWebApplication.Models;
using InventoryWebApplication.Models.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryWebApplication.Services
{
    public class ProductsService
    {
        private readonly ILogger _logger;
        private readonly DatabaseContext _databaseContext;
        
        public ProductsService(DatabaseContext databaseContext, ILogger<ProductsService> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task<bool> ProductExists(string name)
        {
            string lowerName = name.ToLower();
            return await _databaseContext.Products.AnyAsync(o => o.Name.ToLower() == lowerName);
        }
        
        public Task<Product> FindProduct(int id)
        {
            return _databaseContext.Products.FirstOrDefaultAsync(o => o.Id == id);
        }

        public IEnumerable<Product> GetProducts()
        {
            return _databaseContext.Products;
        }

        public async Task<bool> AddProduct(string name, string description, float costPrice, float sellPrice)
        {
            bool alreadyExists = await ProductExists(name);
            if (alreadyExists)
            {
                _logger.LogWarning($"Failed to add product {name};{description};{costPrice};{sellPrice}. Product name already exists.");
                return false;
            }

            Product product = new()
            {
                Name = name,
                Description = description,
                Cost = costPrice,
                SellPrice = sellPrice
            };

            _databaseContext.Products.Add(product);
            await _databaseContext.SaveChangesAsync();
            
            _logger.LogInformation($"Successfully added product {name};{description};{costPrice};{sellPrice}");
            return true;
        }
    }
}