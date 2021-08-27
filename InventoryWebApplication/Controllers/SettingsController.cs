using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryWebApplication.Models;
using InventoryWebApplication.Models.Database;
using InventoryWebApplication.Operations;
using InventoryWebApplication.Services.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryWebApplication.Controllers
{
    [Route("settings")]
    public class SettingsController : Controller
    {
        private readonly PaymentMethodsService _paymentMethodsService;

        public SettingsController(PaymentMethodsService paymentMethodsService)
        {
            _paymentMethodsService = paymentMethodsService;
        }

        [HttpGet]
        [Authorize(Roles = Role.HrManager)]
        public IActionResult Settings()
        {
            return View(MessageOperation.Empty);
        }

        [HttpPost]
        [Authorize(Roles = Role.HrManager)]
        public async Task<IActionResult> EditSettings([FromForm] string paymentNames)
        {
            List<PaymentMethod> paymentMethods = new();
            
            foreach (string info in paymentNames.Split("\n").Select(o => o.Trim()))
            {
                if (info.Length == 0)
                    continue;

                string[] parts = info.Split(",", 2);
                int margin = 100;
                
                if (parts.Length == 2)
                {
                    if (int.TryParse(parts[1].Replace("%", "").Trim(), out int converted)) margin = converted;
                }

                paymentMethods.Add(new PaymentMethod(parts[0].Trim(), margin));
            }
            await _paymentMethodsService.Set(paymentMethods);
            
            return RedirectToAction("Settings", "Settings");
        }
    }
}