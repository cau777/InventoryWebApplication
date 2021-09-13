using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using InventoryWebApplication.Models.Interfaces;
using InventoryWebApplication.Services.Database;
using Newtonsoft.Json;

namespace InventoryWebApplication.Services.Importer
{
    public class JsonImporter<T> : ImporterService where T : class, IIdBasedModel, IFromTableRow, new()
    {
        private readonly DatabaseService<T> _databaseService;

        public JsonImporter(DatabaseService<T> databaseService)
        {
            _databaseService = databaseService;
        }

        public override async Task Import(Stream stream)
        {
            StreamReader reader = new(stream);
            string data = await reader.ReadToEndAsync();
            List<Dictionary<string, string>> rows =
                JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(data);

            foreach (Dictionary<string,string> row in rows)
            {
                T entry = new();
                entry.FromTableRow(row);

                await _databaseService.Add(entry);
            }
        }
    }
}