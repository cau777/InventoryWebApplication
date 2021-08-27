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
        [Authorize(Roles = Role.StockManagerAndAbove + "," + Role.Seller)]
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
            [FromForm] string availableQuantity, [FromForm] string cost, [FromForm] string sell)
        {
            if (!int.TryParse(availableQuantity, NumberStyles.Any, CultureInfo.InvariantCulture, out int quantity))
                return View("AddProductForm", new MessageOperation($"Invalid quantity: {availableQuantity}"));

            if (!float.TryParse(cost, NumberStyles.Any, CultureInfo.InvariantCulture, out float costPrice))
                return View("AddProductForm", new MessageOperation($"Invalid cost: {cost}"));

            if (!float.TryParse(sell, NumberStyles.Any, CultureInfo.InvariantCulture, out float sellPrice))
                return View("AddProductForm", new MessageOperation($"Invalid sell price: {sell}"));

            if (string.IsNullOrWhiteSpace(name))
                return View("AddProductForm", new MessageOperation($"Invalid name: {name}"));

            if (await _productsService.Add(new Product(name: name, description: description,
                availableQuantity: quantity, cost: costPrice, sellPrice: sellPrice)))
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
            [FromForm] string description, [FromForm] string availableQuantity, [FromForm] string cost,
            [FromForm] string sell)
        {
            if (!int.TryParse(availableQuantity, NumberStyles.Any, CultureInfo.InvariantCulture, out int quantity))
                return View("EditProductForm", new MessageIdOperation($"Invalid quantity: {availableQuantity}", id));

            if (!float.TryParse(cost, NumberStyles.Any, CultureInfo.InvariantCulture, out float costPrice))
                return View("EditProductForm", new MessageIdOperation($"Invalid cost: {cost}", id));

            if (!float.TryParse(sell, NumberStyles.Any, CultureInfo.InvariantCulture, out float sellPrice))
                return View("EditProductForm", new MessageIdOperation($"Invalid sell price: {sell}", id));

            if (string.IsNullOrWhiteSpace(name))
                return View("EditProductForm", new MessageIdOperation($"Invalid name: {name}", id));

            bool result = await _productsService.UpdateById(new Product(id, name, description, quantity, costPrice, sellPrice));
            return View("EditProductForm",
                result
                    ? new MessageIdOperation("Changes saved", MessageSeverity.info, id)
                    : new MessageIdOperation("Failed to update product", id));
        }
    }
}