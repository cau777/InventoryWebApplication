namespace InventoryWebApplication.Models.Interfaces
{
    public interface ITableRow
    {
        /// <summary>
        /// The table headers as a string array
        /// </summary>
        string[] TableRowHeaders { get; }
        
        /// <summary>
        /// Selects the most important fields to export
        /// </summary>
        /// <returns>
        /// An array of the string representation of the fields matching the headers
        /// </returns>
        string[] ToTableRow();
    }
}