using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryWebApplication.DatabaseContexts;
using InventoryWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryWebApplication.Services
{
    public class SalesService : DatabaseService<SaleInfo>
    {
        public SalesService(DatabaseContext databaseContext, ILogger<DatabaseService<SaleInfo>> logger)
            : base(databaseContext.Sales, databaseContext, logger) { }

        public async Task<SaleInfo[]> GetSales(DateTime start, DateTime end,
            List<string> usernames = null)
        {
            IQueryable<SaleInfo> all = ItemSet.Include(o => o.Method).Include(o => o.Seller);
            IQueryable<SaleInfo> filteredByDate = all;

            if (start != DateTime.MinValue) filteredByDate = filteredByDate.Where(o => o.SellTime >= start);
            if (end != DateTime.MaxValue) filteredByDate = filteredByDate.Where(o => o.SellTime <= end);

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

        public IDictionary<string, int> GetSalesCountPerProp(IEnumerable<SaleInfo> sales, Func<SaleInfo, string> prop)
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

        public IDictionary<string, double> GetSalesPerProp(IEnumerable<SaleInfo> sales, Func<SaleInfo, string> prop)
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

        public IDictionary<string, double> GetSalesProfitPerProp(IEnumerable<SaleInfo> sales,
            Func<SaleInfo, string> prop)
        {
            Dictionary<string, double> dict = new();

            foreach (SaleInfo sale in sales)
            {
                string value = prop(sale) ?? "Unknown";

                double prev = dict.GetValueOrDefault(value, 0);
                dict[value] = prev + sale.Profit;
            }

            return dict;
        }

        public IDictionary<string, int> GetSalesCountPerSeller(IEnumerable<SaleInfo> sales)
        {
            Dictionary<string, int> dict = new();
            foreach (SaleInfo sale in sales)
            {
                string user = sale.Seller.Name ?? "Unknown";

                int prev = dict.GetValueOrDefault(user, 0);
                dict[user] = prev + 1;
            }

            return dict;
        }

        public IDictionary<DateTime, (double, double)> GetDailySalesAndProfit(DateTime start, DateTime end,
            IEnumerable<SaleInfo> sales)
        {
            Dictionary<DateTime, (double, double)> dict = new();

            for (DateTime i = start.Date; i <= end.Date; i = i.AddDays(1))
            {
                dict.Add(i.Date, (0, 0));
            }

            foreach (SaleInfo info in sales)
            {
                (double totalPrice, double profit) = dict[info.SellTime.Date];
                dict[info.SellTime.Date] = (totalPrice + info.TotalPrice, profit + info.Profit);
            }

            return dict;
        }

        public IDictionary<DateTime, (double, double)> GetMonthlySalesAndProfit(DateTime start, DateTime end,
            IEnumerable<SaleInfo> sales)
        {
            Dictionary<DateTime, (double, double)> dict = new();
            DateTime startMonth = new(start.Year, start.Month, 1);
            DateTime endMonth = new(end.Year, end.Month, DateTime.DaysInMonth(end.Year, end.Month));

            for (DateTime i = startMonth; i <= endMonth; i = i.AddMonths(1))
            {
                dict.Add(i.Date, (0, 0));
            }

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