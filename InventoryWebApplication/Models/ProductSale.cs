using Newtonsoft.Json;

namespace InventoryWebApplication.Models
{
    public class ProductSale
    {
        [JsonProperty("i")]
        public int Id { get; set; }
        
        [JsonProperty("q")]
        public int Quantity { get; set; }
    }
}