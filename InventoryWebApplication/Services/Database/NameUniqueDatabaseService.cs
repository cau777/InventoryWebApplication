using System.Linq;
using System.Threading.Tasks;
using InventoryWebApplication.DatabaseContexts;
using InventoryWebApplication.Models.Interfaces;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryWebApplication.Services.Database
{
    public abstract class NameUniqueDatabaseService<T> : DatabaseService<T>
        where T : class, IIdBasedModel, INameBasedModel
    {
        private readonly ILogger<DatabaseService<T>> _logger;

        protected NameUniqueDatabaseService(DbSet<T> itemSet, DatabaseContext databaseContext,
            ILogger<DatabaseService<T>> logger) : base(itemSet, databaseContext, logger)
        {
            _logger = logger;
        }

        [ItemCanBeNull]
        public async Task<T> GetByName(string name)
        {
            string lowerName = name.ToLower();
            return await ItemSet.FirstOrDefaultAsync(o => o.Name.ToLower() == lowerName);
        }

        public async Task<bool> UpdateByName(T values)
        {
            return await Update(await GetByName(values.Name), values);
        }

        public async Task<bool> DeleteByName(string name)
        {
            T element = await GetByName(name);

            if (element is not null) return await Delete(element);

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