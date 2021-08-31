using System.Threading.Tasks;
using InventoryWebApplication.Models;
using InventoryWebApplication.Models.Database;
using InventoryWebApplication.Operations;
using InventoryWebApplication.Services.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryWebApplication.Controllers
{
    [Route("paymentMethods")]
    public class PaymentMethodsController : Controller
    {
        private readonly PaymentMethodsService _paymentMethodsService;

        public PaymentMethodsController(PaymentMethodsService paymentMethodsService)
        {
            _paymentMethodsService = paymentMethodsService;
        }

        [HttpGet]
        [Authorize(Roles = Role.HrManager)]
        public IActionResult ListPaymentMethods()
        {
            return View();
        }

        [HttpGet]
        [Route("add")]
        [Authorize(Roles = Role.HrManager)]
        public IActionResult AddPaymentMethodForm()
        {
            return View(MessageOperation.Empty);
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = Role.HrManager)]
        public async Task<IActionResult> AddPaymentMethod([FromForm] string name, [FromForm] int profitMargin)
        {
            if (await _paymentMethodsService.Add(new PaymentMethod(name, profitMargin)))
                return View("AddPaymentMethodForm",
                    new MessageOperation($"Successfully added {name}", MessageSeverity.info));

            return View("AddPaymentMethodForm", new MessageOperation($"Failed to add {name}"));
        }

        [HttpGet]
        [Route("edit/{id:int}")]
        [Authorize(Roles = Role.HrManager)]
        public IActionResult EditPaymentMethodForm(int id)
        {
            return View(new MessageIdOperation(id));
        }

        [HttpPost]
        [Route("edit/{id:int}")]
        [Authorize(Roles = Role.HrManager)]
        public async Task<IActionResult> EditPaymentMethod(int id, [FromForm] string name, [FromForm] int profitMargin)
        {
            if (await _paymentMethodsService.UpdateById(id, new PaymentMethod(name, profitMargin)))
                return View("EditPaymentMethodForm",
                    new MessageIdOperation(id, "Changes saved", MessageSeverity.info));

            return View("EditPaymentMethodForm", new MessageIdOperation(id, $"Failed to update {name}"));
        }
    }
}