using System;
using System.Globalization;
using System.Threading.Tasks;
using InventoryWebApplication.Models;
using InventoryWebApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryWebApplication.Controllers
{
    [Route("sales")]
    public class SalesController : Controller
    {
        private readonly ProductsService _productsService;
        private readonly SalesService _salesService;
        public SalesController(ProductsService productsService, SalesService salesService)
        {
            _productsService = productsService;
            _salesService = salesService;
        }

        [HttpGet]
        [Authorize(Roles = Role.HrManager + "," + Role.StockManager + "," + Role.Seller)]
        public IActionResult SalesMenu()
        {
            return View();
        }

        [HttpPost]
        [Route("sell")]
        [Authorize(Roles = Role.HrManager + "," + Role.StockManager + "," + Role.Seller)]
        public async Task<IActionResult> SellProducts([FromBody] SaleInfo info)
        {
            info.SellTime = DateTime.Now;

            double total = 0;
            foreach (ProductSale productSale in info.Products)
            {
                Product product = await _productsService.GetById(productSale.Id);
                if (product is null) return NotFound();
                if (product.AvailableQuantity < productSale.Quantity) return BadRequest();
                total += product.SellPrice * productSale.Quantity;
            }

            double discount;
            if (info.Discount.Contains('%'))
            {
                _ = double.TryParse(info.Discount.Replace("%", "").Replace(",", "."), NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out double percentage);

                discount = total * percentage / 100f;
            }
            else
            {
                _ = double.TryParse(info.Discount.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture,
                    out discount);
            }

            if (total - discount < 0) discount = total;
            
            info.Discount = discount.ToString(CultureInfo.InvariantCulture);
            info.TotalPrice = total - discount;

            foreach (ProductSale productSale in info.Products)
            {
                await _productsService.ShiftProductQuantity(productSale.Id, -productSale.Quantity);
            }

            // Console.WriteLine(JsonConvert.SerializeObject(info));
            await _salesService.AddSale(info);

            return Ok();
        }
    }
}