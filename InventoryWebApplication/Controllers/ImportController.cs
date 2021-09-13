using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using InventoryWebApplication.Models;
using InventoryWebApplication.Operations;
using InventoryWebApplication.Services.Importer;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace InventoryWebApplication.Controllers
{
    [Route("import")]
    public class ImportController : Controller
    {
        private readonly ImporterFactory _importerFactory;

        public ImportController(ImporterFactory importerFactory)
        {
            _importerFactory = importerFactory;
        }

        [HttpGet]
        [Route("menu")]
        [Authorize(Roles = Role.StockManagerAndAbove)]
        public IActionResult ImportMenu()
        {
            return View(new List<MessageOperation>(0));
        }

        [HttpPost]
        [Authorize(Roles = Role.StockManagerAndAbove)]
        public async Task<IActionResult> ImportTable([FromForm] string name, [FromForm] string mode,
            [FromForm]
            List<IFormFile> files)
        {
            ImporterService importerService = _importerFactory.GetInstance(name, mode);
            List<MessageOperation> messages = new();

            foreach (IFormFile formFile in files)
            {
                Stream stream = formFile.OpenReadStream();
                BufferedStream buffer = new(stream);

                try
                {
                    await importerService.Import(buffer);
                    messages.Add(new MessageOperation($"Successfully added element from file {formFile.FileName}",
                        MessageSeverity.success));
                }
                catch (Exception)
                {
                    messages.Add(new MessageOperation($"Wrong format on file {formFile.FileName}",
                        MessageSeverity.danger));
                }
            }

            return View("ImportMenu", messages);
        }
    }
}