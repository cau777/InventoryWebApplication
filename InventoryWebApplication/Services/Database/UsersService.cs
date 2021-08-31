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

        /// <summary>
        /// Checks whether the password is valid
        /// </summary>
        /// <param name="password">The password to be checked</param>
        /// <returns>True if the password is valid</returns>
        public static bool IsPasswordValid([NotNull] string password)
        {
            return PasswordRegex.IsMatch(password);
        }

        private static string GetPasswordHashString([NotNull] string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] passwordHash = SHA256.HashData(passwordBytes);
            return Encoding.UTF8.GetString(passwordHash);
        }

        /// <summary>
        /// Gets a User with the specified name and password
        /// </summary>
        /// <param name="name">Name of the user to find</param>
        /// <param name="password">Password of the user to find</param>
        /// <returns>Element with the provided name and password or null if not found</returns>
        [ItemCanBeNull]
        public async Task<User> GetByNameAndPassword([NotNull] string name, [NotNull] string password)
        {
            string passwordHash = GetPasswordHashString(password);
            string lowerUsername = name.ToLower();

            // ReSharper disable once SpecifyStringComparison
            return await ItemSet.FirstOrDefaultAsync(o =>
                o.Name.ToLower() == lowerUsername &&
                o.Password == passwordHash);
        }

        /// <summary>
        /// Deletes an element of the table, ignoring a name. This is used to prevent a User from deleting itself
        /// </summary>
        /// <param name="id">Id of the element to delete</param>
        /// <param name="ignoreName">Name to ignore</param>
        /// <returns>True if the element was found and deleted</returns>
        public async Task<bool> Delete(int id, string ignoreName)
        {
            User element = await GetById(id);

            if (element is null)
            {
                _logger.LogWarning($"Could not add item to table User. Id not found.");
                return false;
            }

            if (string.Equals(element.Name, ignoreName, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("Could not update item on table User. User was ignored");
                return false;
            }

            await base.Delete(element);
            return true;
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