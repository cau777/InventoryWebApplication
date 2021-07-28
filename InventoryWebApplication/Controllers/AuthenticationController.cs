using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InventoryWebApplication.Models;
using InventoryWebApplication.Models.Operations;
using InventoryWebApplication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace InventoryWebApplication.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly UsersService _usersService;

        public AuthenticationController(ILogger<AuthenticationController> logger, UsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("auth")]
        public async Task<IActionResult> Authenticate([FromForm] Dictionary<string, string> model)
        {
            string username;
            string password;

            try
            {
                username = model["username"];
                password = model["password"];
            }
            catch (KeyNotFoundException)
            {
                return RedirectToFailedLogin();
            }

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return RedirectToFailedLogin();

            User user = _usersService.FindUserWithPassword(username, password);
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