using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using InventoryWebApplication.DatabaseContexts;
using InventoryWebApplication.Models.Database;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryWebApplication.Services.Database
{
    public class UsersService : NameUniqueDatabaseService<User>
    {
        private static readonly Regex PasswordRegex = new("^\\w{4,}$", RegexOptions.Compiled);
        private readonly ILogger<DatabaseService<User>> _logger;

        public UsersService(DatabaseContext databaseContext, ILogger<DatabaseService<User>> logger) : base(
            databaseContext.Users, databaseContext, logger)
        {
            _logger = logger;
        }
        
        public static bool IsPasswordValid(string password)
        {
            return PasswordRegex.IsMatch(password);
        }
        
        private static string GetPasswordHashString(string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] passwordHash = SHA256.HashData(passwordBytes);
            return Encoding.UTF8.GetString(passwordHash);
        }

        [ItemCanBeNull]
        public async Task<User> FindUserWithPassword(string username, string password)
        {
            string passwordHash = GetPasswordHashString(password);
            string lowerUsername = username.ToLower();

            // ReSharper disable once SpecifyStringComparison
            return await ItemSet.FirstOrDefaultAsync(o =>
                o.Name.ToLower() == lowerUsername &&
                o.Password == passwordHash);
        }

        public async Task<bool> Delete(int id, string ignoreUsername)
        {
            User element = await GetById(id);

            if (element is null)
            {
                _logger.LogWarning($"Could not add item to table User. Id not found.");
                return false;
            }

            if (string.Equals(element.Name, ignoreUsername, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("Could not update item on table User. User was ignored");
                return false;
            }

            return await base.Delete(element);
        }

        public override Task<bool> Add(User element)
        {
            element.Password = GetPasswordHashString(element.Password);
            return base.Add(element);
        }

        protected override void SetValues(User target, User values)
        {
            target.Name = values.Name;
            if (!string.IsNullOrWhiteSpace(values.Password))
                target.Password = GetPasswordHashString(values.Password);
            target.Role = values.Role;
        }
    }
}