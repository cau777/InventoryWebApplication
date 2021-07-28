using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryWebApplication.Models;
using InventoryWebApplication.Models.DatabaseContexts;
using Microsoft.EntityFrameworkCore;

namespace InventoryWebApplication.Services
{
    public class ProductsService
    {
        private readonly DatabaseContext _databaseContext;
        
        public ProductsService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
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
            if (alreadyExists) return false;

            Product product = new()
            {
                Name = name,
                Description = description,
                Cost = costPrice,
                SellPrice = sellPrice
            };

            _databaseContext.Products.Add(product);
            await _databaseContext.SaveChangesAsync();
            return true;
        }
    }
}