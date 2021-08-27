using InventoryWebApplication.Models;
using InventoryWebApplication.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryWebApplication.Controllers
{
    [Route("reports")]
    public class ReportsController : Controller
    {
        [HttpGet]
        [Authorize(Roles = Role.StockManagerAndAbove)]
        public IActionResult GeneralReports([FromQuery] FilterOperation startEnd)
        {
            return View(startEnd);
        }
    }
}