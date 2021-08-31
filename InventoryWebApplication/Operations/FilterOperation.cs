using System;
using System.Collections.Generic;

namespace InventoryWebApplication.Operations
{
    /// <summary>
    ///     Contains parameters used to filter SaleInfos in a view
    /// </summary>
    public class FilterOperation
    {
        public FilterOperation()
        {
            Start = DateTime.MinValue;
            End = DateTime.MaxValue;
            Usernames = new List<string>();
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<string> Usernames { get; set; }
    }
}