using System.Collections.Generic;

namespace InventoryWebApplication.Models.Interfaces
{
    public interface IFromTableRow
    {
        void FromTableRow(Dictionary<string, string> row);
    }
}