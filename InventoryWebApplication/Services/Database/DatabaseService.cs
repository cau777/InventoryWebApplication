using System;
using System.Collections.Generic;
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
    /// Default class used to access a database table
    /// </summary>
    /// <typeparam name="T">The type stored in the database table that has a unique id</typeparam>
    public abstract class DatabaseService<T> where T : class, IIdBasedModel
    {
        /// <summary>
        /// Total elements in the table
        /// </summary>
        public int Count => ItemSet.Count();

        protected readonly DbSet<T> ItemSet;
        protected readonly DatabaseContext DatabaseContext;
        protected readonly string TableName;

        private readonly ILogger<DatabaseService<T>> _logger;

        protected DatabaseService(DbSet<T> itemSet, DatabaseContext databaseContext, ILogger<DatabaseService<T>> logger)
        {
            ItemSet = itemSet;
            DatabaseContext = databaseContext;
            _logger = logger;
            TableName = itemSet.EntityType.Name;
            TableName = TableName[(1 + TableName.LastIndexOf(".", StringComparison.Ordinal)) ..];
        }

        /// <summary>
        /// Checks whether the element can be added to the table
        /// </summary>
        /// <param name="element">Element to analyse</param>
        /// <returns>True if the element can be added</returns>
        protected abstract bool CanBeAdded([NotNull] T element);
        
        /// <summary>
        /// Checks whether the element can be updated to the values
        /// </summary>
        /// <param name="target">Element to analyse</param>
        /// <param name="values">New values</param>
        /// <returns>True if the element can be added</returns>
        protected abstract bool CanBeEdited([NotNull] T target, [NotNull] T values);
        
        /// <summary>
        /// Sets target's fields to the provided values
        /// </summary>
        /// <param name="target">Element to update values</param>
        /// <param name="values">New values</param>
        protected abstract void SetValues([NotNull] T target, [NotNull] T values);

        /// <summary>
        /// Get an element of the table with the provided id
        /// </summary>
        /// <param name="id">A unique id</param>
        /// <returns>Element with the provided id or null if not found</returns>
        [ItemCanBeNull]
        public async Task<T> GetById(int id)
        {
            return await ItemSet.FirstOrDefaultAsync(o => o.Id == id);
        }

        /// <summary>
        /// Gets all elements of the table, including related entities
        /// </summary>
        /// <returns>An IEnumerable that contains all elements and all related entities</returns>
        [ItemNotNull]
        public virtual IEnumerable<T> GetAll()
        {
            return ItemSet;
        }

        /// <summary>
        /// Adds an element to the table
        /// </summary>
        /// <param name="element">The element to add</param>
        /// <returns>True if the element was added</returns>
        public virtual async Task<bool> Add([NotNull] T element)
        {
            element.Id = default;

            if (!CanBeAdded(element))
            {
                _logger.LogWarning($"Could not add item to table {TableName}");
                return false;
            }

            ItemSet.Add(element);
            await DatabaseContext.SaveChangesAsync();

            _logger.LogInformation($"Successfully added target to table {TableName}");
            return true;
        }

        /// <summary>
        /// Edits an element of the table, replacing its values with new ones
        /// </summary>
        /// <param name="target">The element to be edited</param>
        /// <param name="values">Object containing the new values</param>
        /// <returns>True if the element was found and edited</returns>
        public async Task<bool> Update([CanBeNull] T target, [NotNull] T values)
        {
            if (target is null)
            {
                _logger.LogWarning($"Could not update item on table {TableName}. Element does not exist");
                return false;
            }

            SetValues(target, values);

            if (!CanBeEdited(target, values))
            {
                _logger.LogWarning($"Could not update item on table {TableName}. Element can't be edited");
                return false;
            }

            await DatabaseContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Updates an element of the table, replacing its values with new ones
        /// </summary>
        /// <param name="id">Id of the element to update</param>
        /// <param name="values">Object containing the new values</param>
        /// <returns>True if the element was found and edited</returns>
        public async Task<bool> UpdateById(int id, [NotNull] T values)
        {
            return await Update(await GetById(id), values);
        }

        /// <summary>
        /// Deletes an element of the table
        /// </summary>
        /// <param name="element">Element to delete</param>
        public async Task Delete([NotNull] T element)
        {
            ItemSet.Remove(element);
            await DatabaseContext.SaveChangesAsync();
        }
        
        /// <summary>
        /// Deletes an element of the table
        /// </summary>
        /// <param name="id">Id of the element to delete</param>
        /// <returns>True if the element was found and deleted</returns>
        public async Task<bool> DeleteById(int id)
        {
            T element = await GetById(id);

            if (element is not null)
            {
                await Delete(element);
                return true;
            }

            _logger.LogWarning($"Could not delete item to table {TableName}. Id not found");
            return false;
        }
    }
}