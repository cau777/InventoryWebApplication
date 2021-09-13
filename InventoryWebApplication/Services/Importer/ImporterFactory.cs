using System;
using System.Collections.Generic;
using InventoryWebApplication.Models.Database;
using InventoryWebApplication.Models.Interfaces;
using InventoryWebApplication.Services.Database;
using Microsoft.Extensions.DependencyInjection;
using InventoryWebApplication.Services.Exporter;

namespace InventoryWebApplication.Services.Importer
{
    public class ImporterFactory
    {
        // For now, this service can only import products
        public static readonly IReadOnlyDictionary<string, string[]> AvailableTablesAndHeaders =
            new Dictionary<string, string[]> { { "products", Product.StaticTableHeaders } };

        public static readonly IEnumerable<string> AvailableModes = new[] { "json", "csv" };

        private readonly IServiceProvider _serviceProvider;

        public ImporterFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        ///     Factory Method: creates an instance of the base class ImporterService
        /// </summary>
        /// <param name="name">The table to import to</param>
        /// <param name="mode">The type of importer to create</param>
        /// <returns>An ImporterService of the specified mode that imports data to the specified table</returns>
        /// <exception cref="InvalidOperationException">The name is not a valid table name</exception>
        public ImporterService GetInstance(string name, string mode)
        {
            return name.ToLower() switch
            {
                "products" => GetInstance<Product>(mode),
                _ => throw new InvalidOperationException()
            };
        }

        private ImporterService GetInstance<T>(string mode) where T : class, IIdBasedModel, IFromTableRow, new()
        {
            return mode.ToLower() switch
            {
                "json" => new JsonImporter<T>(GetService()),
                "csv" => new CsvImporter<T>(GetService()),
                _ => throw new InvalidOperationException()
            };

            DatabaseService<T> GetService()
            {
                return _serviceProvider.GetService<DatabaseService<T>>();
            }
        }
    }
}