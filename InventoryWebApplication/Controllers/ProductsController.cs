using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using InventoryWebApplication.Models;
using InventoryWebApplication.Models.Operations;
using InventoryWebApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace InventoryWebApplication.Controllers
{
    [Route("products")]
    public class ProductsController : Controller
    {
        private readonly ProductsService _productsService;

        public ProductsController(ProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        [Authorize(Roles = Role.HrManager + "," + Role.StockManager)]
        public IActionResult ListProducts()
        {
            return View();
        }

        [HttpGet]
        [Route("add")]
        [Authorize(Roles = Role.HrManager + "," + Role.StockManager)]
        public IActionResult AddProductForm()
        {
            return View(MessageOperation.Empty);
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = Role.HrManager + "," + Role.StockManager)]
        public async Task<IActionResult> AddProduct([FromForm] string name, [FromForm] string description,
            [FromForm] string cost, [FromForm] string sell)
        {
            if (!float.TryParse(cost, NumberStyles.Any, CultureInfo.InvariantCulture, out float costPrice))
                return View("AddProductForm", new MessageOperation($"Invalid cost: {cost}"));

            if (!float.TryParse(sell, NumberStyles.Any, CultureInfo.InvariantCulture, out float sellPrice))
                return View("AddProductForm", new MessageOperation($"Invalid sell price: {sell}"));

            if (string.IsNullOrWhiteSpace(name))
                return View("AddProductForm", new MessageOperation($"Invalid name: {name}"));
            
            if (await _productsService.AddProduct(name, description, costPrice, sellPrice))
                return View("AddProductForm", new MessageOperation($"Successfully added {name}", MessageSeverity.info));

            return View("AddProductForm", new MessageOperation($"Failed to add {name}"));
        }
    }
}