namespace InventoryWebApplication.Models.Interfaces
{
    public interface INameBasedModel
    {
        /// <summary>
        ///     A unique name
        /// </summary>
        string Name { get; }
    }
}