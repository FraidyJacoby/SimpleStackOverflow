using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SimpleStackOverflow.Data;

namespace SimpleStackOverflow.Web.Controllers
{
    public class AccountController : Controller
    {
        private string _connectionString;

        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(User user, string password)
        {
            var repo = new UsersRepository(_connectionString);
            repo.AddUser(user, password);
            return Redirect("/account/login");
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(string email, string password)
        {
            var repo = new UsersRepository(_connectionString);
            var user = repo.LogIn(email, password);
            if (user == null)
            {
                return Redirect("/account/login");
            }

            var claims = new List<Claim>
            {
                new Claim("user", email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return Redirect("/home/index");
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
    }
}
