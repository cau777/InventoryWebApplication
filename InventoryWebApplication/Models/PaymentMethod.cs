namespace InventoryWebApplication.Models
{
    public class PaymentMethod : IIdBasedModel, INameBasedModel
    {
        public static readonly PaymentMethod Unknown = new("Unknown");
        
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProfitMarginPercentage { get; set; }

        public PaymentMethod() { }
        
        public PaymentMethod(string name = null, int profitMarginPercentage = default)
        {
            Name = name;
            ProfitMarginPercentage = profitMarginPercentage;
        }

        public double CalcMargin(double price)
        {
            return ProfitMarginPercentage / 100d * price;
        }
    }
}