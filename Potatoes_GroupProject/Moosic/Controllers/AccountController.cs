using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moosic.Models;

/*
* Group Name: Potatoes
* Project Name: Moosic
* 
* Created By: Alexander Bascevan
* 
* Created On: April 7, 2025
* Updated On: 
* 
* Purpose: Account Controller Class to handle regisration and login, as well as the user dashboard
* 
*/

namespace Moosic.Controllers
{
    public class AccountController : Controller
    {
        private readonly MoosicDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public AccountController(MoosicDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        // GET: /Account/SignUp
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        // POST: /Account/SignUp
        [HttpPost]
        public async Task<IActionResult> SignUp(User user)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.email == user.email))
                {
                    ModelState.AddModelError("email", "Email already exists.");
                    return View(user);
                }

                user.password = _passwordHasher.HashPassword(user, user.password);
                user.LibraryItems = null;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("email", "Email and password are required.");
                return View();
            }

            var user = _context.Users.FirstOrDefault(u => u.email == email);
            if (user == null)
            {
                ViewData["Error"] = "Invalid email or password.";
                return View();
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.password, password);
            if (result == PasswordVerificationResult.Success)
            {
                TempData["Message"] = $"Welcome, {user.userName}!";
                return RedirectToAction("Dashboard", "Account"); // redirect to dashboard
            }

            ViewData["Error"] = "Invalid email or password.";
            return View();
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            ViewBag.UserName = TempData["UserName"];
            return View();
        }



    }
}
