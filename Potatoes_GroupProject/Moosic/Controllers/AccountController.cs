/*
* Group Name: Potatoes
* Project Name: Moosic
* 
* Created By: Alexander Bascevan
* 
* Created On: April 7, 2025
* Updated On: April 7, 2025
* 
* Purpose: Account Controller Class to handle regisration and login, as well as the user dashboard
* 
* Comments: Added the Display New Releases On Dashboard - Mariah
* 
*/

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moosic.Models;
using Moosic.Services;

namespace Moosic.Controllers
{
    public class AccountController : Controller
    {
        private readonly MoosicDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        //Injecting the APIService 
        private readonly ApiService _apiService;
        public AccountController(MoosicDbContext context, ApiService apiService)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
            _apiService = apiService;
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
                TempData["UserName"] = user.userName;
                HttpContext.Session.SetInt32("UserId", user.UserId);
                return RedirectToAction("Dashboard", "Account"); // redirect to dashboard
            }

            ViewData["Error"] = "Invalid email or password.";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var releases = await _apiService.GetNewReleases();
            ViewBag.UserName = TempData["UserName"];
            return View(releases);
        }

        // GET: /Account/Library
        public async Task<IActionResult> Library()
        {
            // Get the logged-in user's id from session
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            // Load the user's library items along with associated Music details.
            var libraryItems = await _context.LibraryItems
                .Include(li => li.Music)
                .Where(li => li.UserId == userId.Value)
                .ToListAsync();

            return View(libraryItems);
        }

    }
}
