using System;
using System.Collections.Generic;
using InventoryWebApplication.Models.Interfaces;
using InventoryWebApplication.Services.Database;
using Newtonsoft.Json;

namespace InventoryWebApplication.Services.Exporter
{
    public class JsonExporter<T> : ExporterService where T : class, IIdBasedModel, IToTableRow, ITableHeaders
    {
        private readonly DatabaseService<T> _databaseService;

        public JsonExporter(DatabaseService<T> databaseService)
        {
            _databaseService = databaseService;
        }

        public override string ContentType => "application/json";
        public override string FileExtension => "json";

        public override byte[] Export()
        {
            List<Dictionary<string, string>> entries = new();

            foreach (T model in _databaseService.GetAll())
            {
                Dictionary<string, string> dict = new();

                string[] headers = model.TableHeaders;
                string[] values = model.ToTableRow();

                for (int i = 0; i < Math.Min(headers.Length, values.Length); i++)
                    dict[headers[i]] = values[i];

                entries.Add(dict);
            }

            string serializationResult = JsonConvert.SerializeObject(entries, Formatting.Indented);
            return EncodeUtf8(serializationResult);
        }
    }
}