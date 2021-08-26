using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryWebApplication.DatabaseContexts;
using InventoryWebApplication.Models;
using InventoryWebApplication.Models.Interfaces;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryWebApplication.Services
{
    public abstract class DatabaseService<T> where T : class, IIdBasedModel
    {
        public int Count => ItemSet.Count();

        protected readonly DbSet<T> ItemSet;

        private readonly ILogger<DatabaseService<T>> _logger;
        protected readonly DatabaseContext DatabaseContext;
        protected readonly string TableName;

        protected DatabaseService(DbSet<T> itemSet, DatabaseContext databaseContext, ILogger<DatabaseService<T>> logger)
        {
            ItemSet = itemSet;
            DatabaseContext = databaseContext;
            _logger = logger;
            TableName = itemSet.EntityType.Name;
            TableName = TableName[(1 + TableName.LastIndexOf(".", StringComparison.Ordinal)) ..];
        }
        
        protected abstract bool CanBeAdded([NotNull] T element);
        protected abstract bool CanBeEdited([NotNull] T target, [NotNull] T values);
        protected abstract void SetValues([NotNull] T target, [NotNull] T values);

        [ItemCanBeNull]
        public async Task<T> GetById(int id)
        {
            return await ItemSet.FirstOrDefaultAsync(o => o.Id == id);
        }

        [ItemNotNull]
        public IEnumerable<T> GetAll()
        {
            return ItemSet;
        }

        public virtual async Task<bool> Add(T element)
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
        
        public async Task<bool> Update(T target, T values)
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

        public async Task<bool> UpdateById(T values)
        {
            return await Update(await GetById(values.Id), values);
        }

        public async Task<bool> DeleteById(int id)
        {
            T element = await GetById(id);

            if (element is not null) return await Delete(element);

            _logger.LogWarning($"Could not delete item to table {TableName}. Id not found");
            return false;
        }

        public async Task<bool> Delete([NotNull] T element)
        {
            ItemSet.Remove(element);
            await DatabaseContext.SaveChangesAsync();
            return true;
        }
    }
}