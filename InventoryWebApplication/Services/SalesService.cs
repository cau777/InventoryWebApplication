using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryWebApplication.DatabaseContexts;
using InventoryWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryWebApplication.Services
{
    public class SalesService : DatabaseService<SaleInfo>
    {
        public SalesService(DatabaseContext databaseContext, ILogger<DatabaseService<SaleInfo>> logger) : base(
            databaseContext.Sales, databaseContext, logger) { }

        public async Task<SaleInfo[]> GetSalesOverPeriod(DateTime start, DateTime end)
        {
            return await ItemSet.Include(o => o.Method).Where(o => o.SellTime >= start && o.SellTime <= end)
                .ToArrayAsync();
        }

        public IDictionary<PaymentMethod, int> GetSalesCountPerPaymentMethod(IEnumerable<SaleInfo> sales)
        {
            Dictionary<PaymentMethod, int> dict = new();
            foreach (SaleInfo sale in sales)
            {
                PaymentMethod paymentMethod = sale.Method ?? PaymentMethod.Unknown;

                int prev = dict.GetValueOrDefault(paymentMethod, 0);
                dict[paymentMethod] = prev + 1;
            }

            return dict;
        }

        public IDictionary<PaymentMethod, double> GetSalesPerPaymentMethod(IEnumerable<SaleInfo> sales)
        {
            Dictionary<PaymentMethod, double> dict = new();

            foreach (SaleInfo sale in sales)
            {
                PaymentMethod paymentMethod = sale.Method ?? PaymentMethod.Unknown;

                double prev = dict.GetValueOrDefault(paymentMethod, 0);
                dict[paymentMethod] = prev + sale.TotalPrice;
            }

            return dict;
        }

        public IDictionary<PaymentMethod, double> GetSalesProfitPerPaymentMethod(IEnumerable<SaleInfo> sales)
        {
            Dictionary<PaymentMethod, double> dict = new();

            foreach (SaleInfo sale in sales)
            {
                PaymentMethod paymentMethod = sale.Method ?? PaymentMethod.Unknown;

                double prev = dict.GetValueOrDefault(paymentMethod, 0);
                dict[paymentMethod] = prev + sale.Profit;
            }

            return dict;
        }

        public IDictionary<int, (double, double)> GetDailySalesAndProfit(IEnumerable<SaleInfo> sales)
        {
            Dictionary<int, (double, double)> dict = new();
            DateTime now = DateTime.Now;

            int today = now.Day;
            DateTime start = new(now.Year, now.Month, 1);

            for (int i = 1; i <= today; i++)
            {
                dict.Add(i, (0, 0));
            }

            foreach (SaleInfo info in sales.Where(o => o.SellTime >= start))
            {
                (double totalPrice, double profit) = dict[info.SellTime.Day];
                dict[info.SellTime.Day] = (totalPrice + info.TotalPrice, profit + info.Profit);
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