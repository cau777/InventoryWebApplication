using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using InventoryWebApplication.Models;
using InventoryWebApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InventoryWebApplication.Controllers
{
    [Route("sales")]
    public class SalesController : Controller
    {
        private readonly ProductsService _productsService;

        public SalesController(ProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        [Authorize(Roles = Role.HrManager + "," + Role.StockManager + "," + Role.Seller)]
        public IActionResult SalesMenu()
        {
            return View(new
            {
                Currency = "$"
            });
        }

        [HttpPost]
        [Route("sell")]
        [Authorize(Roles = Role.HrManager + "," + Role.StockManager + "," + Role.Seller)]
        public async Task<IActionResult> SellProducts([FromBody] SaleInfo info)
        {
            info.SellTime = DateTime.Now;

            float total = 0;
            foreach (ProductSale productSale in info.Products)
            {
                Product product = await _productsService.FindProduct(productSale.Id);
                if (product is null) return NotFound();
                if (product.AvailableQuantity < productSale.Quantity) return BadRequest();
                total += product.SellPrice * productSale.Quantity;
            }

            float discount;
            if (info.Discount.Contains('%'))
            {
                _ = float.TryParse(info.Discount.Replace("%", "").Replace(",", "."), NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out float percentage);

                discount = total * percentage / 100f;
            }
            else
            {
                _ = float.TryParse(info.Discount.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture,
                    out discount);
            }

            info.Discount = discount.ToString(CultureInfo.InvariantCulture);
            info.TotalPrice = total - discount;

            foreach (ProductSale productSale in info.Products)
            {
                await _productsService.ShiftProductQuantity(productSale.Id, -productSale.Quantity);
            }

            return Ok();
        }
    }
}