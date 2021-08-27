using System;
using System.Collections.Generic;
using InventoryWebApplication.Models.Database;
using InventoryWebApplication.Models.Interfaces;
using InventoryWebApplication.Services.Database;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryWebApplication.Services.Exporter
{
    public class ExporterFactory
    {
        public static readonly IEnumerable<string> AvailableTables = new[]
            { "products", "users", "sales", "payment methods" };

        public static readonly IEnumerable<string> Modes = new[] { "json", "csv" };

        private readonly IServiceProvider _serviceProvider;

        public ExporterFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ExporterService GetInstance(string name, string mode)
        {
            return name.ToLower() switch
            {
                "products" => GetInstance<Product>(mode),
                "users" => GetInstance<User>(mode),
                "sales" => GetInstance<SaleInfo>(mode),
                "payment methods" => GetInstance<PaymentMethod>(mode),
                _ => throw new InvalidOperationException()
            };
        }

        private ExporterService GetInstance<T>(string mode) where T : class, IIdBasedModel, ITableRow
        {
            return mode.ToLower() switch
            {
                "json" => new JsonExporter<T>(GetService()),
                "csv" => new CsvExporter<T>(GetService()),
                _ => throw new InvalidOperationException()
            };

            DatabaseService<T> GetService()
            {
                return _serviceProvider.GetService<DatabaseService<T>>();
            }
        }
    }
}