using System.Globalization;
using InventoryWebApplication.Models.Interfaces;

namespace InventoryWebApplication.Models.Database
{
    public class PaymentMethod : IIdBasedModel, INameBasedModel, IToTableRow, ITableHeaders
    {
        public PaymentMethod() { }

        public PaymentMethod(string name = null, int profitMarginPercentage = default)
        {
            Name = name;
            ProfitMarginPercentage = profitMarginPercentage;
        }

        public int ProfitMarginPercentage { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public string[] TableHeaders => new[] { "Id", "Name", "Profit Margin (%)" };

        public string[] ToTableRow()
        {
            return new[]
            {
                Id.ToString(CultureInfo.InvariantCulture),
                Name,
                ProfitMarginPercentage.ToString(CultureInfo.InvariantCulture)
            };
        }
    }
}