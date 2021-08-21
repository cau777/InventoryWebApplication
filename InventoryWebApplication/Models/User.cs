using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace InventoryWebApplication.Models
{
    public class User : IIdBasedModel, INameBasedModel
    {
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