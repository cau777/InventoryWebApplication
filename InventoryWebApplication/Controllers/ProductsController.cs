using InventoryWebApplication.Models;
using InventoryWebApplication.Services;
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
        [Authorize(Roles = Role.HrManager + "," + Role.StockManager)]
        public IActionResult ListProducts()
        {
            return View();
        }
    }
}