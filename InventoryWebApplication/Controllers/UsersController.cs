using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryWebApplication.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "manager")]
        public IActionResult ListUsers()
        {
            return View();
        }
        
        [HttpGet]
        [Route("add")]
        [Authorize(Roles = "manager")]
        public IActionResult AddUserForm()
        {
            return View();
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "manager")]
        public IActionResult AddUser()
        {
            throw new System.NotImplementedException();
        }
    }
}