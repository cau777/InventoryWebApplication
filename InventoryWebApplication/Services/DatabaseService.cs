using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryWebApplication.DatabaseContexts;
using InventoryWebApplication.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryWebApplication.Services
{
    public abstract class DatabaseService<T> where T : class, IIdBasedModel
    {
        public int Count => ItemSet.Count();

        protected readonly DbSet<T> ItemSet;

        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<DatabaseService<T>> _logger;
        private readonly string _tableName;

        protected DatabaseService(DbSet<T> itemSet, DatabaseContext databaseContext, ILogger<DatabaseService<T>> logger)
        {
            ItemSet = itemSet;
            _databaseContext = databaseContext;
            _logger = logger;
            _tableName = itemSet.EntityType.Name;
            _tableName = _tableName[_tableName.LastIndexOf(".", StringComparison.Ordinal) ..];
        }

        public abstract bool IsPresent(T element);
        protected abstract bool CanBeAdded(T element);
        protected abstract bool CanBeEdited(T target, T values);
        protected abstract void SetValues(T target, T values);

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
                _logger.LogWarning($"Could not add item to table {_tableName}");
                return false;
            }

            ItemSet.Add(element);
            await _databaseContext.SaveChangesAsync();

            _logger.LogInformation($"Successfully added target to table {_tableName}");
            return true;
        }

        public async Task<bool> Update(T values)
        {
            T target = await GetById(values.Id);

            if (target is null)
            {
                _logger.LogWarning($"Could not update item on table {_tableName}. Element does not exist");
                return false;
            }

            SetValues(target, values);

            if (!CanBeEdited(target, values))
            {
                _logger.LogWarning($"Could not update item on table {_tableName}. Element can't be edited");
                return false;
            }

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            T element = await GetById(id);

            if (element is not null) return await Delete(element);

            _logger.LogWarning($"Could not delete item to table {_tableName}. Id not found");
            return false;
        }

        public async Task<bool> Delete([NotNull] T element)
        {
            ItemSet.Remove(element);
            await _databaseContext.SaveChangesAsync();
            return true;
        }
    }
}