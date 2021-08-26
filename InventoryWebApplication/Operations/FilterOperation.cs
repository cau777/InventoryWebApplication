using System;
using System.Collections.Generic;

namespace InventoryWebApplication.Operations
{
    public class FilterOperation
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<string> Usernames { get; set; }

        public FilterOperation()
        {
            Start = DateTime.MinValue;
            End = DateTime.MaxValue;
            Usernames = new List<string>();
        }
    }
}