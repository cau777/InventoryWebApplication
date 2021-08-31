using System.Globalization;
using InventoryWebApplication.Models.Interfaces;

namespace InventoryWebApplication.Models.Database
{
    /// <summary>
    ///     Stores information about a product available for sale
    /// </summary>
    public class Product : IIdBasedModel, INameBasedModel, ITableRow
    {
        public Product() { }

        public Product(int id = default, string name = null, string description = null, int availableQuantity = default,
            float cost = default, float sellPrice = default)
        {
            Id = id;
            Name = name;
            Description = description;
            AvailableQuantity = availableQuantity;
            Cost = cost;
            SellPrice = sellPrice;
        }

        public string Description { get; set; }
        public int AvailableQuantity { get; set; }
        public float Cost { get; set; }
        public float SellPrice { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public string[] TableRowHeaders => new[] { "Id", "Name", "Description", "Quantity", "Cost", "SellPrice" };

        public string[] ToTableRow()
        {
            return new[]
            {
                Id.ToString(CultureInfo.InvariantCulture),
                Name,
                Description,
                AvailableQuantity.ToString(CultureInfo.InvariantCulture),
                Cost.ToString(CultureInfo.InvariantCulture),
                SellPrice.ToString(CultureInfo.InvariantCulture)
            };
        }
    }
}