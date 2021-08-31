namespace InventoryWebApplication.Models.Interfaces
{
    public interface IIdBasedModel
    {
        /// <summary>
        /// A unique id
        /// </summary>
        public int Id { get; set; }
    }
}