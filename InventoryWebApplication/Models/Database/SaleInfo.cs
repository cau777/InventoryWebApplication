using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using InventoryWebApplication.Models.Interfaces;
using Newtonsoft.Json;

namespace InventoryWebApplication.Models.Database
{
    public class SaleInfo : IIdBasedModel, IToTableRow, ITableHeaders
    {
        // Products are stored in Json format
        [NotMapped]
        public List<ProductSale> Products
        {
            get => JsonConvert.DeserializeObject<List<ProductSale>>(ProductsJson);
            set => ProductsJson = JsonConvert.SerializeObject(value);
        }

        public string ProductsJson { get; set; }
        public PaymentMethod Method { get; set; }
        public User Seller { get; set; }
        public DateTime SellTime { get; set; }
        public string Discount { get; set; }
        public double TotalPrice { get; set; }
        public double Profit { get; set; }
        public int Id { get; set; }

        public string[] TableHeaders => new[]
        {
            "Id", "Products", "Method Id", "Method Name", "Seller Id", "Seller Name", "Sell Time", "Discount",
            "Total Price", "Profit"
        };

        public string[] ToTableRow()
        {
            return new[]
            {
                Id.ToString(CultureInfo.InvariantCulture),
                ProductsJson,
                Method.Id.ToString(CultureInfo.InvariantCulture),
                Method.Name,
                Seller.Id.ToString(CultureInfo.InvariantCulture),
                Seller.Name,
                SellTime.ToString("G"),
                Discount,
                TotalPrice.ToString(CultureInfo.InvariantCulture),
                Profit.ToString(CultureInfo.InvariantCulture)
            };
        }
    }
}