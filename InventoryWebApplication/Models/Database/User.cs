using System.ComponentModel.DataAnnotations;
using System.Globalization;
using InventoryWebApplication.Models.Interfaces;
using Newtonsoft.Json;

namespace InventoryWebApplication.Models.Database
{
    /// <summary>
    ///     Stores information about a user of the service
    /// </summary>
    public class User : IIdBasedModel, INameBasedModel, ITableRow
    {
        public User() { }

        public User(int id = default, string name = null, string password = null, string role = null)
        {
            Id = id;
            Name = name;
            Password = password;
            Role = role;
        }

        [MaxLength(32)] public string Password { get; set; }
        public string Role { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public string[] TableRowHeaders => new[] { "Id", "Name", "Role" };

        public string[] ToTableRow()
        {
            return new[]
            {
                Id.ToString(CultureInfo.InvariantCulture),
                Name,
                Role
            };
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}