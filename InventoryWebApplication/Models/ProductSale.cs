using Newtonsoft.Json;

namespace InventoryWebApplication.Models
{
    /// <summary>
    /// Stores information about a single sold product and its quantity
    /// </summary>
    public class ProductSale
    {
        [JsonProperty("i")]
        public int Id { get; set; }
        
        [JsonProperty("q")]
        public int Quantity { get; set; }
    }
}