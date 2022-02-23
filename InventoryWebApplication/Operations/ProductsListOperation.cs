using InventoryWebApplication.Models.Database;
using JetBrains.Annotations;

namespace InventoryWebApplication.Operations
{
    public class ProductsListOperation
    {
        [CanBeNull]
        public string Query { get; set; }
        public Product[] Products { get; set; }
    }
}