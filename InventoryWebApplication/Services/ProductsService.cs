using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryWebApplication.Models;
using InventoryWebApplication.Models.DatabaseContexts;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryWebApplication.Services
{
    public class ProductsService
    {
        public int ProductsCount => _databaseContext.Products.Count();
        
        private readonly ILogger<ProductsService> _logger;
        private readonly DatabaseContext _databaseContext;
        
        public ProductsService(DatabaseContext databaseContext, ILogger<ProductsService> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task<Product> FindProduct(int id)
        {
            return await _databaseContext.Products.FirstOrDefaultAsync(o => o.Id == id);
        }

        public IEnumerable<Product> GetProducts()
        {
            return _databaseContext.Products;
        }

        public async Task<bool> AddProduct(string name, string description, int quantity, float costPrice,
            float sellPrice)
        {
            Product product = new()
            {
                Name = name,
                Description = description,
                AvailableQuantity = quantity,
                Cost = costPrice,
                SellPrice = sellPrice
            };

            _databaseContext.Products.Add(product);
            await _databaseContext.SaveChangesAsync();
            
            _logger.LogInformation($"Successfully added product {name};{description};{costPrice};{sellPrice}");
            return true;
        }

        public async Task<bool> EditProduct(int id, string name, string description, int quantity, float costPrice,
            float sellPrice)
        {
            Product product = await GetProduct(id);

            if (product is null)
            {
                _logger.LogWarning($"Failed to update product {id}. Product does not exist.");
                return false;
            }

            product.Name = name;
            product.Description = description;
            product.AvailableQuantity = quantity;
            product.Cost = costPrice;
            product.SellPrice = sellPrice;
            
            await _databaseContext.SaveChangesAsync();
            
            _logger.LogInformation($"Successfully updated product {id}");
            return true;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            Product product = await GetProduct(id);
            if (product is null)
            {
                _logger.LogWarning($"Failed to product user {id}. Id not found.");
                return false;
            }

            _databaseContext.Products.Remove(product);
            await _databaseContext.SaveChangesAsync();
            
            _logger.LogInformation($"Successfully removed product {id}");
            return true;
        }

        public async Task ShiftProductQuantity(int id, int shift)
        {
            Product product = await GetProduct(id);
            if (product is null) throw new NullReferenceException();
            if (product.AvailableQuantity + shift < 0) throw new ArgumentException("Negative quantity");
            product.AvailableQuantity += shift;
            await _databaseContext.SaveChangesAsync();
        }
        
        [ItemCanBeNull]
        private async Task<Product> GetProduct(int id)
        {
            return await _databaseContext.Products.FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}