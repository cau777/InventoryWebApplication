using System.Linq;
using System.Threading.Tasks;
using InventoryWebApplication.DatabaseContexts;
using InventoryWebApplication.Models.Interfaces;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryWebApplication.Services.Database
{
    /// <summary>
    ///     Class used to access a database table that can't have the same name
    /// </summary>
    /// <typeparam name="T">The type stored in the database table that has a unique id and a unique name</typeparam>
    public abstract class NameUniqueDatabaseService<T> : DatabaseService<T>
        where T : class, IIdBasedModel, INameBasedModel
    {
        private readonly ILogger<DatabaseService<T>> _logger;

        protected NameUniqueDatabaseService(DbSet<T> itemSet, DatabaseContext databaseContext,
            ILogger<DatabaseService<T>> logger) : base(itemSet, databaseContext, logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///     Get an element of the table with the provided name
        /// </summary>
        /// <param name="name">A unique name</param>
        /// <returns>Element with the provided name or null if not found</returns>
        [ItemCanBeNull]
        public async Task<T> GetByName(string name)
        {
            string lowerName = name.ToLower();
            return await ItemSet.FirstOrDefaultAsync(o => o.Name.ToLower() == lowerName);
        }

        /// <summary>
        ///     Updates an element of the table, replacing its values with new ones
        /// </summary>
        /// <param name="name">Name of the element to update</param>
        /// <param name="values">Object containing the new values</param>
        /// <returns>True if the element was found and edited</returns>
        public async Task<bool> UpdateByName(string name, T values)
        {
            return await Update(await GetByName(name), values);
        }

        /// <summary>
        ///     Deletes an element of the table
        /// </summary>
        /// <param name="name">Name of the element to delete</param>
        /// <returns>True if the element was found and deleted</returns>
        public async Task<bool> DeleteByName(string name)
        {
            T element = await GetByName(name);

            if (element is not null)
            {
                await Delete(element);
                return true;
            }

            _logger.LogWarning($"Could not delete item to table {TableName}. Name not found");
            return false;
        }

        protected override bool CanBeAdded(T element)
        {
            string lowerName = element.Name.ToLower();
            return !ItemSet.Any(o => lowerName == o.Name.ToLower());
        }

        protected override bool CanBeEdited(T target, T values)
        {
            string lowerName = values.Name.ToLower();
            return GetAll().All(o => o.Id == target.Id || o.Name.ToLower() != lowerName);
        }
    }
}