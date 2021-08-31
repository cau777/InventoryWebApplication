using System;

namespace InventoryWebApplication.Models
{
    public static class Role
    {
        /// <summary>
        ///     Can manage users, products and payment methods, see reports, sell products, and export tables
        /// </summary>
        public const string HrManager = "hr-manager";

        /// <summary>
        ///     Can manage products, see reports, sell products, and export tables
        /// </summary>
        public const string StockManager = "stock-manager";

        /// <summary>
        ///     Can sell products
        /// </summary>
        public const string Seller = "seller";

        public const string StockManagerAndAbove = HrManager + "," + StockManager;
        public const string SellerAndAbove = HrManager + "," + StockManager + "," + Seller;

        /// <summary>
        ///     Array of available roles, ordered from most powerful to least powerful
        /// </summary>
        public static string[] AvailableRoles => new[] { HrManager, StockManager, Seller };

        /// <summary>
        ///     Formats the role name
        /// </summary>
        /// <param name="role">Database role name</param>
        /// <returns>A nicely formatted string with the role name</returns>
        /// <exception cref="ArgumentOutOfRangeException">The provided value isn't a valid role</exception>
        public static string GetFormattedName(string role)
        {
            return role switch
            {
                HrManager => "Hr Manager",
                StockManager => "Stock Manager",
                Seller => "Seller",
                _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
            };
        }
    }
}