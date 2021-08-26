using System;
using InventoryWebApplication.Models;
using InventoryWebApplication.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InventoryWebApplication.Controllers
{
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