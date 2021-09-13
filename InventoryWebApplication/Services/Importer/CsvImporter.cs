using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using InventoryWebApplication.Models.Interfaces;
using InventoryWebApplication.Services.Database;

namespace InventoryWebApplication.Services.Importer
{
    public class CsvImporter<T> : ImporterService where T : class, IIdBasedModel, IFromTableRow, new()
    {
        private readonly DatabaseService<T> _databaseService;

        public CsvImporter(DatabaseService<T> databaseService)
        {
            _databaseService = databaseService;
        }

        public override async Task Import(Stream stream)
        {
            StreamReader reader = new(stream);

            string firstLine = await reader.ReadLineAsync();
            Console.WriteLine(firstLine);
            if (firstLine is null) return;
            string[] headers = firstLine.Split(",");

            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                string[] values = line.Split(",");
                Console.WriteLine(values);
                Dictionary<string, string> row = new();

                for (int i = 0; i < Math.Min(headers.Length, values.Length); i++) 
                    row[headers[i]] = values[i];

                T entry = new();
                entry.FromTableRow(row);
                await _databaseService.Add(entry);
            }
        }
    }
}