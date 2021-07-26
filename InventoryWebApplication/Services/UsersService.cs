using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using InventoryWebApplication.Models;
using InventoryWebApplication.Models.DatabaseContexts;

namespace InventoryWebApplication.Services
{
    public class UsersService
    {
        public static string[] AvailableRoles => new[] {"manager"};
        public int UserCount => _databaseContext.Users.Count();
        public int ProductsCount => _databaseContext.Products.Count();

        private readonly DatabaseContext _databaseContext;

        public UsersService(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
        }

        public async Task<bool> AddUser(string username, string password, string role)
        {
            bool alreadyExists = UsernameExists(username);
            if (alreadyExists) return false;

            string passwordHash = GetPasswordHashString(password);
            User user = new()
            {
                Name = username,
                Password = passwordHash,
                Role = role
            };

            _databaseContext.Users.Add(user);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public User FindUser(string username, string password)
        {
            string passwordHash = GetPasswordHashString(password);
            string lowerUsername = username.ToLower();

            // ReSharper disable once SpecifyStringComparison
            User found = _databaseContext.Users.FirstOrDefault(o =>
                o.Name.ToLower() == lowerUsername &&
                o.Password == passwordHash);
            return found;
        }

        public bool UsernameExists(string username)
        {
            string lowerUsername = username.ToLower();
            return _databaseContext.Users.FirstOrDefault(o => o.Name.ToLower() == lowerUsername) is not null;
        }

        public IEnumerable<User> GetUsers()
        {
            foreach (User user in _databaseContext.Users)
            {
                // Password is hidden
                yield return new User
                {
                    Id = user.Id,
                    Name = user.Name,
                    Role = user.Role
                };
            }
        }

        private static string GetPasswordHashString(string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] passwordHash = SHA256.HashData(passwordBytes);
            return Encoding.UTF8.GetString(passwordHash);
        }
    }
}