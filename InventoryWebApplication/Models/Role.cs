using InventoryWebApplication.CustomExceptions;

namespace InventoryWebApplication.Models
{
    public static class Role
    {
        /*
         * Roles:
         * hr-manager -> can add/remove/edit users, create new products and update quantities
         * stock-manager -> can create new products and update quantities
         * seller -> can only update the quantity of products
         */

        public static string[] AvailableRoles => new[] {"hr-manager", "stock-manager", "seller"};

        public const string HrManager = "hr-manager";
        public const string StockManager = "stock-manager";
        public const string Seller = "seller";

        public static string GetFormattedName(string role)
        {
            return role switch
            {
                HrManager => "Hr Manager",
                StockManager => "Stock Manager",
                Seller => "Seller",
                _ => throw new InvalidRoleException(role)
            };
        }
    }
}