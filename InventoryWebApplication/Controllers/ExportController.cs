using InventoryWebApplication.Services.Exporter;
using Microsoft.AspNetCore.Mvc;

namespace InventoryWebApplication.Controllers
{
    [Route("export")]
    public class ExportController : Controller
    {
        private readonly ExporterFactory _exporterFactory;
        public ExportController(ExporterFactory exporterFactory)
        {
            _exporterFactory = exporterFactory;
        }

        [HttpGet]
        [Route("menu")]
        public IActionResult ExportMenu()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ExportTable([FromQuery] string name, [FromQuery] string mode)
        {
            ExporterService exporter = _exporterFactory.GetInstance(name, mode);
            return File(exporter.Export(), exporter.ContentType, exporter.FileName);
        }
    }
}