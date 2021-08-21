using System;

namespace InventoryWebApplication.Operations
{
    public class StartEndDateTimeOperation
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public StartEndDateTimeOperation()
        {
            Start = DateTime.MinValue;
            End = DateTime.MaxValue;
        }
    }
}