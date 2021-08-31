using System.Globalization;
using System.Threading.Tasks;
using InventoryWebApplication.Models;
using InventoryWebApplication.Models.Database;
using InventoryWebApplication.Operations;
using InventoryWebApplication.Services.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = Role.StockManagerAndAbove)]
        public IActionResult ListProducts()
        {
            return View();
        }

        [HttpGet]
        [Route("add")]
        [Authorize(Roles = Role.StockManagerAndAbove)]
        public IActionResult AddProductForm()
        {
            return View(MessageOperation.Empty);
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Roles = Role.SellerAndAbove)]
        public async Task<IActionResult> ProductById([FromRoute] int id)
        {
            Product product = await _productsService.GetById(id);
            if (product is null) return NotFound();
            return Json(product);
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = Role.StockManagerAndAbove)]
        public async Task<IActionResult> AddProduct([FromForm] string name, [FromForm] string description,
            [FromForm] int availableQuantity, [FromForm] string cost, [FromForm] string sell)
        {
            if (!float.TryParse(cost, NumberStyles.Any, CultureInfo.InvariantCulture, out float costPrice))
                return View("AddProductForm", new MessageOperation($"Invalid cost: {cost}"));

            if (!float.TryParse(sell, NumberStyles.Any, CultureInfo.InvariantCulture, out float sellPrice))
                return View("AddProductForm", new MessageOperation($"Invalid sell price: {sell}"));

            if (string.IsNullOrWhiteSpace(name))
                return View("AddProductForm", new MessageOperation($"Invalid name: {name}"));

            if (await _productsService.Add(new Product(name: name, description: description,
                availableQuantity: availableQuantity, cost: costPrice, sellPrice: sellPrice)))
                return View("AddProductForm", new MessageOperation($"Successfully added {name}", MessageSeverity.info));

            return View("AddProductForm", new MessageOperation($"Failed to add {name}"));
        }

        [HttpDelete]
        [Route("delete/{id:int}")]
        [Authorize(Roles = Role.StockManagerAndAbove)]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            bool result = await _productsService.DeleteById(id);
            return result ? Ok() : NotFound();
        }

        [HttpGet]
        [Route("edit/{id:int}")]
        [Authorize(Roles = Role.StockManagerAndAbove)]
        public IActionResult EditProductForm([FromRoute] int id)
        {
            return View(new MessageIdOperation(id));
        }

        [HttpPost]
        [Route("edit/{id:int}")]
        [Authorize(Roles = Role.StockManagerAndAbove)]
        public async Task<IActionResult> EditProduct([FromRoute] int id, [FromForm] string name,
            [FromForm] string description, [FromForm] int availableQuantity, [FromForm] string cost,
            [FromForm] string sell)
        {
            if (!float.TryParse(cost, NumberStyles.Any, CultureInfo.InvariantCulture, out float costPrice))
                return View("EditProductForm", new MessageIdOperation(id, $"Invalid cost: {cost}"));

            if (!float.TryParse(sell, NumberStyles.Any, CultureInfo.InvariantCulture, out float sellPrice))
                return View("EditProductForm", new MessageIdOperation(id, $"Invalid sell price: {sell}"));

            if (string.IsNullOrWhiteSpace(name))
                return View("EditProductForm", new MessageIdOperation(id, $"Invalid name: {name}"));

            if (await _productsService.UpdateById(id,
                new Product(id, name, description, availableQuantity, costPrice, sellPrice)))
                return View("EditProductForm", new MessageIdOperation(id, "Changes saved", MessageSeverity.info));

            return View("EditProductForm", new MessageIdOperation(id, "Failed to update product"));
        }
    }
}