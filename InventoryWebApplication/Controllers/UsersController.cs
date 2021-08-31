using System.Linq;
using System.Threading.Tasks;
using InventoryWebApplication.Models;
using InventoryWebApplication.Models.Database;
using InventoryWebApplication.Operations;
using InventoryWebApplication.Services.Database;
using InventoryWebApplication.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryWebApplication.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly UsersService _usersService;

        public UsersController(UsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        [Authorize(Roles = Role.HrManager)]
        public IActionResult ListUsers()
        {
            return View();
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = Role.HrManager)]
        public async Task<IActionResult> AddUser([FromForm] string name, [FromForm] string password,
            [FromForm] string role)
        {
            role = role.ToLower();

            if (string.IsNullOrWhiteSpace(name))
                return View("AddUserForm", new MessageOperation("Name is required"));

            if (!string.IsNullOrWhiteSpace(password) && !UsersService.IsPasswordValid(password))
                return View("AddUserForm", new MessageOperation("Invalid password"));

            if (string.IsNullOrWhiteSpace(role))
                return View("AddUserForm", new MessageOperation("Role is required"));

            if (await _usersService.GetByName(name) is not null)
                return View("AddUserForm", new MessageOperation("This user already exists"));

            if (!Role.AvailableRoles.Contains(role))
                return View("AddUserForm", new MessageOperation("Invalid role"));

            if (await _usersService.Add(new User(name: name, password: password, role: role)))
                return View("AddUserForm", new MessageOperation($"Successfully added {name}", MessageSeverity.info));

            return View("AddUserForm", new MessageOperation($"Failed to add {name}"));
        }

        [HttpGet]
        [Route("add")]
        [Authorize(Roles = Role.HrManager)]
        public IActionResult AddUserForm()
        {
            return View(MessageOperation.Empty);
        }

        [HttpDelete]
        [Route("delete/{id:int}")]
        [Authorize(Roles = Role.HrManager)]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            bool result = await _usersService.Delete(id, User.Claims.GetName());
            return result ? Ok() : NotFound();
        }

        [HttpGet]
        [Route("edit/{id:int}")]
        [Authorize(Roles = Role.HrManager)]
        public IActionResult EditUserForm([FromRoute] int id)
        {
            return View(new MessageIdOperation(id));
        }

        [HttpPost]
        [Route("edit/{id:int}")]
        [Authorize(Roles = Role.HrManager)]
        public async Task<IActionResult> EditUser([FromRoute] int id, [FromForm] string name,
            [FromForm] string password, [FromForm] string role)
        {
            role = role.ToLower();

            if (string.IsNullOrWhiteSpace(name))
                return View("EditUserForm", new MessageIdOperation(id, "Name is required"));

            if (!string.IsNullOrWhiteSpace(password) && !UsersService.IsPasswordValid(password))
                return View("EditUserForm", new MessageIdOperation(id, "Invalid password"));

            if (string.IsNullOrWhiteSpace(role))
                return View("EditUserForm", new MessageIdOperation(id, "Role is required"));

            if (!Role.AvailableRoles.Contains(role))
                return View("EditUserForm", new MessageIdOperation(id, "Invalid role"));

            if (await _usersService.UpdateById(id, new User(id, name, password, role)))
                return View("EditUserForm", new MessageIdOperation(id, "Changes saved", MessageSeverity.info));

            return View("EditUserForm", new MessageIdOperation(id, "Failed to update user"));
        }
    }
}