using System.Linq;
using System.Threading.Tasks;
using InventoryWebApplication.Models;
using InventoryWebApplication.Operations;
using InventoryWebApplication.Services;
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
        public async Task<IActionResult> AddUser([FromForm] string username, [FromForm] string password,
            [FromForm] string role)
        {
            role = role.ToLower();

            if (string.IsNullOrWhiteSpace(username))
                return View("AddUserForm", new MessageOperation("Name is required"));

            if (string.IsNullOrWhiteSpace(password))
                return View("AddUserForm", new MessageOperation("Password is required"));

            if (string.IsNullOrWhiteSpace(role))
                return View("AddUserForm", new MessageOperation("Role is required"));

            if (await _usersService.UserExists(username))
                return View("AddUserForm", new MessageOperation("This user already exists"));

            if (!Role.AvailableRoles.Contains(role))
                return View("AddUserForm", new MessageOperation("Invalid role"));

            if (await _usersService.AddUser(username, password, role))
                return View("AddUserForm",
                    new MessageOperation($"Successfully added {username}", MessageSeverity.info));

            return View("AddUserForm", new MessageOperation($"Failed to add {username}"));
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
            bool result = await _usersService.DeleteUser(id, User.Claims.GetName());
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
        public async Task<IActionResult> EditUser([FromRoute] int id, [FromForm] string username,
            [FromForm] string password, [FromForm] string role)
        {
            role = role.ToLower();

            if (string.IsNullOrWhiteSpace(username))
                return View("AddUserForm", new MessageIdOperation("Name is required", id));

            if (string.IsNullOrWhiteSpace(role))
                return View("AddUserForm", new MessageIdOperation("Role is required", id));

            if (!Role.AvailableRoles.Contains(role))
                return View("AddUserForm", new MessageIdOperation("Invalid role", id));

            bool result = await _usersService.UpdateUser(id, username, role, password);

            return View("EditUserForm",
                result
                    ? new MessageIdOperation("Changes saved", MessageSeverity.info, id)
                    : new MessageIdOperation("Failed to update user", id));
        }
    }
}