using System.ComponentModel.DataAnnotations;
using InventoryWebApplication.Models.Interfaces;
using Newtonsoft.Json;

namespace InventoryWebApplication.Models
{
    public class User : IIdBasedModel, INameBasedModel
    {
        public static readonly User Unknown = new(-1, "Unknown");
        public int Id { get; set; } 
        public string Name { get; set; }
        [MaxLength(32)] public string Password { get; set; }
        public string Role { get; set; }

        public User() { }

        public User(int id = default, string name = null, string password = null, string role = null)
        {
            Id = id;
            Name = name;
            Password = password;
            Role = role;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}