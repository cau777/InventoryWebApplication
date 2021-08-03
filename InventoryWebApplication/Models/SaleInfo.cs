using System;
using System.Collections.Generic;

namespace InventoryWebApplication.Models
{
    public class SaleInfo
    {
        public List<ProductSale> Products { get; set; }
        public DateTime SellTime { get; set; }
        public string Discount { get; set; }
        public float TotalPrice { get; set; }
    }
}