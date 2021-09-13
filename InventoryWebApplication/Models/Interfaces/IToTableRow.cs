namespace InventoryWebApplication.Models.Interfaces
{
    public interface IToTableRow
    {
        /// <summary>
        ///     Selects the most important fields to export
        /// </summary>
        /// <returns>
        ///     An array of the string representation of the fields matching the headers
        /// </returns>
        string[] ToTableRow();
    }
}