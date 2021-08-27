using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using InventoryWebApplication.Models;
using InventoryWebApplication.Models.Database;
using InventoryWebApplication.Services.Database;
using InventoryWebApplication.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryWebApplication.Controllers
{
    [Route("sales")]
    public class SalesController : Controller
    {
        private readonly ProductsService _productsService;
        private readonly SalesService _salesService;
        private readonly PaymentMethodsService _paymentMethodsService;
        private readonly UsersService _usersService;

        public SalesController(ProductsService productsService, SalesService salesService,
            PaymentMethodsService paymentMethodsService, UsersService usersService)
        {
            _productsService = productsService;
            _salesService = salesService;
            _paymentMethodsService = paymentMethodsService;
            _usersService = usersService;
        }

        [HttpGet]
        [Authorize(Roles = Role.SellerAndAbove)]
        public IActionResult SalesMenu()
        {
            return View();
        }

        [HttpPost]
        [Route("sell")]
        [Authorize(Roles = Role.SellerAndAbove)]
        public async Task<IActionResult> SellProducts([FromBody] SaleInfo info)
        {
            info.SellTime = DateTime.Now;
            List<Product> products = new(info.Products.Count);

            double total = 0;
            double cost = 0;
            foreach (ProductSale productSale in info.Products)
            {
                Product product = await _productsService.GetById(productSale.Id);
                if (product is null) return NotFound();
                if (product.AvailableQuantity < productSale.Quantity) return BadRequest();
                
                total += product.SellPrice * productSale.Quantity;
                cost += product.Cost * productSale.Quantity;
                products.Add(product);
            }
            
            double discount;
            if (info.Discount.Contains('%'))
            {
                _ = double.TryParse(info.Discount.Replace("%", "").Replace(",", "."), NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out double percentage);

                discount = total * percentage / 100d;
            }
            else
            {
                _ = double.TryParse(info.Discount.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture,
                    out discount);
            }
            
            if (total - discount < 0) discount = total;

            info.Discount = discount.ToString(CultureInfo.InvariantCulture);
            info.TotalPrice = total;
            info.Method = await _paymentMethodsService.GetById(info.Method.Id);

            if (info.Method is null) return BadRequest();
            
            info.Profit = (info.TotalPrice - discount) * info.Method.ProfitMarginPercentage / 100d - cost;
            info.Seller = await _usersService.GetByName(User.Claims.GetName());

            int index = 0;
            foreach (ProductSale productSale in info.Products)
            {
                await _productsService.ShiftProductQuantity(products[index++], -productSale.Quantity);
            }
            
            await _salesService.Add(info);

            return Ok();
        }
    }
}