using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace InventoryWebApplication.Models
{
    public class SaleInfo
    {
        public int Id { get; set; }

        [NotMapped]
        public List<ProductSale> Products
        {
            get => JsonConvert.DeserializeObject<List<ProductSale>>(ProductsJson);
            set => ProductsJson = JsonConvert.SerializeObject(value);
        }

        public string ProductsJson { get; set; }
        public DateTime SellTime { get; set; }
        public string Discount { get; set; }
        public double TotalPrice { get; set; }
    }
}