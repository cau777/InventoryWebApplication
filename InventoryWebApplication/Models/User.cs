using Newtonsoft.Json;

namespace InventoryWebApplication.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public User Clone()
        {
            return new()
            {
                Id = Id,
                Name = Name,
                Password = Password,
                Role = Role
            };
        }

        public User CloneHidePassword()
        {
            return new()
            {
                Id = Id,
                Name = Name,
                Role = Role
            };
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}