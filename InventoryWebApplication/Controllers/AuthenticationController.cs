using System.Security.Claims;
using System.Threading.Tasks;
using InventoryWebApplication.Models.Database;
using InventoryWebApplication.Operations;
using InventoryWebApplication.Services.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryWebApplication.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UsersService _usersService;

        public AuthenticationController(UsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("auth")]
        public async Task<IActionResult> Authenticate([FromForm] string name, [FromForm] string password)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
                return RedirectToFailedLogin();

            User user = await _usersService.GetByNameAndPassword(name, password);
            if (user is null) return RedirectToFailedLogin();

            ClaimsIdentity claimsIdentity = new(new Claim[]
            {
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Role, user.Role)
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal claimsPrincipal = new(new[]
            {
                claimsIdentity
            });

            await HttpContext.SignInAsync(claimsPrincipal);

            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectToFailedLogin()
        {
            return View("Login", new MessageOperation("Wrong username or password"));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("login")]
        public IActionResult Login()
        {
            return View(MessageOperation.Empty);
        }

        [HttpGet]
        [Authorize]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Authentication");
        }
    }
}