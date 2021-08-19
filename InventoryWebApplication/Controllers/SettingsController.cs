using InventoryWebApplication.Models;
using InventoryWebApplication.Operations;
using InventoryWebApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryWebApplication.Controllers
{
    [Route("settings")]
    public class SettingsController : Controller
    {
        private readonly SettingsService _settingsService;
        public SettingsController(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        [Authorize(Roles = Role.HrManager)]
        public IActionResult Settings()
        {
            return View(MessageOperation.Empty);
        }

        [HttpPost]
        [Authorize(Roles = Role.HrManager)]
        public IActionResult EditSettings([FromForm] string paymentMethods)
        {
            return RedirectToAction("Settings", "Settings");
        }
    }
}