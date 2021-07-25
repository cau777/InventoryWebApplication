namespace InventoryWebApplication.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float BuyPrice { get; set; }
        public float SellPrice { get; set; }
        
        public float Profit => SellPrice - BuyPrice;
    }
}