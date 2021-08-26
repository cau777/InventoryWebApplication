using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using InventoryWebApplication.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InventoryWebApplication.Models
{
    public class SaleInfo : IIdBasedModel
    {
        public int Id { get; set; }

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
    }
}