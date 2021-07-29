using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using InventoryWebApplication.Models;
using InventoryWebApplication.Models.DatabaseContexts;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryWebApplication.Services
{
    public class UsersService
    {
        public int UserCount => _databaseContext.Users.Count();
        public int ProductsCount => _databaseContext.Products.Count();

        private readonly ILogger _logger;
        private readonly DatabaseContext _databaseContext;

        public UsersService(DatabaseContext databaseContext, ILogger<UsersService> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task<bool> AddUser(string username, string password, string role)
        {
            bool alreadyExists = await UserExists(username);
            if (alreadyExists)
            {
                _logger.LogWarning($"Failed to add user {username};{password};{role}. Username already exists.");
                return false;
            }

            string passwordHash = GetPasswordHashString(password);
            User user = new()
            {
                Name = username,
                Password = passwordHash,
                Role = role
            };

            _databaseContext.Users.Add(user);
            await _databaseContext.SaveChangesAsync();
            
            _logger.LogInformation($"Successfully added user {username};{password};{role} to the database.");
            return true;
        }
        
        public async Task<bool> UpdateUser(int id, string username, string role, string password = null)
        {
            User user = await GetUser(id);

            if (user is null)
            {
                _logger.LogWarning($"Failed to update user {id};{username};{role}. User does not exist.");
                return false;
            }
            
            if (_databaseContext.Users.Where(o => o != user).Any(o => o.Name == username))
            {
                _logger.LogWarning($"Failed to update user {id};{username};{role}. Username already exists.");
                return false;
            }
            
            user.Name = username;
            if (!string.IsNullOrWhiteSpace(password))
                user.Password = GetPasswordHashString(password);
            user.Role = role;
            
            await _databaseContext.SaveChangesAsync();
            
            _logger.LogInformation($"Successfully updated user {id};{username};{role}");
            return true;
        }

        [ItemCanBeNull]
        public async Task<User> FindUserWithPassword(string username, string password)
        {
            string passwordHash = GetPasswordHashString(password);
            string lowerUsername = username.ToLower();

            // ReSharper disable once SpecifyStringComparison
            User found = await _databaseContext.Users.FirstOrDefaultAsync(o =>
                o.Name.ToLower() == lowerUsername &&
                o.Password == passwordHash);
            return found?.Clone();
        }
        
        [ItemCanBeNull]
        public async Task<User> FindUser(int id)
        {
            User user = await GetUser(id);

            return user?.CloneHidePassword();
        }

        public async Task<bool> UserExists(string username)
        {
            string lowerUsername = username.ToLower();
            return await _databaseContext.Users.AnyAsync(o => o.Name.ToLower() == lowerUsername);
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

        public async Task<bool> DeleteUser(int id, string ignoreUsername = "")
        {
            User user = await GetUser(id);

            if (user is null || string.Equals(user.Name, ignoreUsername, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning($"Failed to delete user {id}. User not found.");
                return false;
            }

            _databaseContext.Users.Remove(user);
            await _databaseContext.SaveChangesAsync();
            
            _logger.LogInformation($"Successfully removed user {id}");
            return true;
        }
        
        private static string GetPasswordHashString(string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] passwordHash = SHA256.HashData(passwordBytes);
            return Encoding.UTF8.GetString(passwordHash);
        }
        
        [ItemCanBeNull]
        private async Task<User> GetUser(int id)
        {
            return await _databaseContext.Users.FirstOrDefaultAsync(o => o.Id == id);
        }
        
        [ItemCanBeNull]
        private async Task<User> GetUser(string name)
        {
            return await _databaseContext.Users.FirstOrDefaultAsync(o => o.Name == name);
        }
    }
}