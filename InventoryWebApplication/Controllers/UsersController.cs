using System.Linq;
using System.Threading.Tasks;
using InventoryWebApplication.Models;
using InventoryWebApplication.Services;
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
        [Authorize(Roles = "manager")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> AddUser([FromForm] string username, [FromForm] string password, [FromForm] string role)
        {
            role = role.ToLower();

            if (string.IsNullOrWhiteSpace(username))
                return View("AddUserForm", new OperationResult("Name is required"));

            if (string.IsNullOrWhiteSpace(password))
                return View("AddUserForm", new OperationResult("Password is required"));

            if (string.IsNullOrWhiteSpace(role))
                return View("AddUserForm", new OperationResult("Role is required"));

            if (_usersService.UsernameExists(username))
                return View("AddUserForm", new OperationResult("This user already exists"));

            if (!UsersService.AvailableRoles.Contains(role))
                return View("AddUserForm", new OperationResult("Invalid role"));

            if (await _usersService.AddUser(username, password, role))
                return View("AddUserForm", new OperationResult($"Successfully added {username}"));

            return View("AddUserForm", new OperationResult($"Failed to add {username}"));
        }

        [HttpGet]
        [Route("add")]
        [Authorize(Roles = "manager")]
        public IActionResult AddUserForm()
        {
            return View(OperationResult.Empty);
        }
    }
}