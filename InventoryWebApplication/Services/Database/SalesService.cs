using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryWebApplication.DatabaseContexts;
using InventoryWebApplication.Models.Database;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryWebApplication.Services.Database
{
    public class SalesService : DatabaseService<SaleInfo>
    {
        public SalesService(DatabaseContext databaseContext, ILogger<DatabaseService<SaleInfo>> logger)
            : base(databaseContext.Sales, databaseContext, logger) { }

        public override IEnumerable<SaleInfo> GetAll()
        {
            return ItemSet.Include(o => o.Method).Include(o => o.Seller);
        }

        /// <summary>
        ///     Returns sales that happened between two dates optionally filtered by users
        /// </summary>
        /// <param name="start">The start date (inclusive)</param>
        /// <param name="end">The final date (inclusive)</param>
        /// <param name="usernames">If specified, only allow sales from those users</param>
        /// <returns>An array of the matching sales</returns>
        public async Task<SaleInfo[]> GetSales(DateTime start, DateTime end, [CanBeNull] List<string> usernames = null)
        {
            IQueryable<SaleInfo> all = ItemSet.Include(o => o.Method).Include(o => o.Seller);
            IQueryable<SaleInfo> filteredByDate = all;

            if (start != DateTime.MinValue) filteredByDate = filteredByDate.Where(o => o.SellTime.Date >= start);
            if (end != DateTime.MaxValue) filteredByDate = filteredByDate.Where(o => o.SellTime.Date <= end);

            IQueryable<SaleInfo> filteredByUsers;

            if (usernames != null && usernames.Count != 0)
            {
                string first = usernames[0];
                filteredByUsers = all.Where(o => o.Seller.Name == first);

                for (int index = 1; index < usernames.Count; index++)
                {
                    string username = usernames[index];
                    filteredByUsers = filteredByUsers.Union(all.Where(o => o.Seller.Name == username));
                }
            }
            else
            {
                filteredByUsers = filteredByDate;
            }

            return await filteredByUsers.ToArrayAsync();
        }

        /// <summary>
        ///     Counts the number of sales per value of a property
        /// </summary>
        /// <param name="sales">Sales to search</param>
        /// <param name="prop">Function to get the property value</param>
        /// <returns>
        ///     A dictionary with keys that represent property values and values that represent the number of sales with that
        ///     value
        /// </returns>
        public IDictionary<string, int> CountSalesPerProperty(IEnumerable<SaleInfo> sales, Func<SaleInfo, string> prop)
        {
            Dictionary<string, int> dict = new();
            foreach (SaleInfo sale in sales)
            {
                string value = prop(sale) ?? "Unknown";

                int prev = dict.GetValueOrDefault(value, 0);
                dict[value] = prev + 1;
            }

            return dict;
        }

        /// <summary>
        ///     Sums the total price of sales per value of a property
        /// </summary>
        /// <param name="sales">Sales to search</param>
        /// <param name="prop">Function to get the property value</param>
        /// <returns>
        ///     A dictionary with keys that represent property values and values that represent the total price of sales with
        ///     that value
        /// </returns>
        public IDictionary<string, double> SumSalesPerProperty(IEnumerable<SaleInfo> sales, Func<SaleInfo, string> prop)
        {
            Dictionary<string, double> dict = new();

            foreach (SaleInfo sale in sales)
            {
                string value = prop(sale) ?? "Unknown";

                double prev = dict.GetValueOrDefault(value, 0);
                dict[value] = prev + sale.TotalPrice;
            }

            return dict;
        }

        /// <summary>
        ///     Sums the total profit of sales per value of a property
        /// </summary>
        /// <param name="sales">Sales to search</param>
        /// <param name="prop">Function to get the property value</param>
        /// <returns>
        ///     A dictionary with keys that represent property values and values that represent the total profit of sales with
        ///     that value
        /// </returns>
        public IDictionary<string, double> SumSalesProfitPerProperty(IEnumerable<SaleInfo> sales,
            Func<SaleInfo, string> prop)
        {
            Dictionary<string, double> dict = new();

            foreach (SaleInfo sale in sales)
            {
                string value = prop(sale) ?? "Unknown";

                double prev = dict.GetValueOrDefault(value, 0);
                dict[value] = prev + sale.Profit;
            }

            // Avoid negative numbers
            foreach ((string key, double value) in dict)
            {
                dict[key] = Math.Max(0, value);
            }

            return dict;
        }

        /// <summary>
        ///     Gets the daily sales and profit in the specified time period
        /// </summary>
        /// <param name="start">The start day (inclusive)</param>
        /// <param name="end">The end day (inclusive)</param>
        /// <param name="sales">Sales to search</param>
        /// <returns>
        ///     A dictionary with keys representing dates and values representing a tuple of the total price and the total
        ///     profit of sales in the day
        /// </returns>
        public IDictionary<DateTime, (double, double)> GetDailySalesAndProfit(DateTime start, DateTime end,
            IEnumerable<SaleInfo> sales)
        {
            Dictionary<DateTime, (double, double)> dict = new();

            // All dates between start and end are added
            for (DateTime i = start.Date; i <= end.Date; i = i.AddDays(1)) 
                dict.Add(i.Date, (0, 0));

            foreach (SaleInfo info in sales)
            {
                (double totalPrice, double profit) = dict[info.SellTime.Date];
                dict[info.SellTime.Date] = (totalPrice + info.TotalPrice, profit + info.Profit);
            }

            return dict;
        }

        /// <summary>
        ///     Gets the monthly sales and profit in the specified time period
        /// </summary>
        /// <param name="start">The start month (inclusive)</param>
        /// <param name="end">The end month (inclusive)</param>
        /// <param name="sales">Sales to search</param>
        /// <returns>
        ///     A dictionary with keys representing months and values representing a tuple of the total price and the total
        ///     profit of sales in the month
        /// </returns>
        public IDictionary<DateTime, (double, double)> GetMonthlySalesAndProfit(DateTime start, DateTime end,
            IEnumerable<SaleInfo> sales)
        {
            Dictionary<DateTime, (double, double)> dict = new();
            DateTime startMonth = new(start.Year, start.Month, 1);
            DateTime endMonth = new(end.Year, end.Month, DateTime.DaysInMonth(end.Year, end.Month));

            for (DateTime i = startMonth; i <= endMonth; i = i.AddMonths(1)) dict.Add(i.Date, (0, 0));

            foreach (SaleInfo info in sales)
            {
                DateTime onlyMonth = new(info.SellTime.Year, info.SellTime.Month, 1);
                (double totalPrice, double profit) = dict[onlyMonth];
                dict[onlyMonth] = (totalPrice + info.TotalPrice, profit + info.Profit);
            }

            return dict;
        }

        protected override bool CanBeAdded(SaleInfo element)
        {
            return true;
        }

        protected override bool CanBeEdited(SaleInfo target, SaleInfo values)
        {
            return true;
        }

        protected override void SetValues(SaleInfo target, SaleInfo values)
        {
            target.Discount = values.Discount;
            target.Method = values.Method;
            target.ProductsJson = values.ProductsJson;
            target.SellTime = values.SellTime;
            target.TotalPrice = values.TotalPrice;
        }
    }
}