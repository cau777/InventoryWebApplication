using System.Globalization;
using InventoryWebApplication.Models.Interfaces;

namespace InventoryWebApplication.Models.Database
{
    public class PaymentMethod : IIdBasedModel, INameBasedModel, ITableRow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProfitMarginPercentage { get; set; }

        public PaymentMethod() { }
        
        public PaymentMethod(string name = null, int profitMarginPercentage = default)
        {
            Name = name;
            ProfitMarginPercentage = profitMarginPercentage;
        }

        public string[] TableRowHeaders => new[] { "Id", "Name", "Profit Margin (%)" };
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