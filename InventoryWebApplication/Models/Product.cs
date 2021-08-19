namespace InventoryWebApplication.Models
{
    public class Product : IIdBasedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AvailableQuantity { get; set; }
        public float Cost { get; set; }
        public float SellPrice { get; set; }

        public float Profit => SellPrice - Cost;

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
    }
}