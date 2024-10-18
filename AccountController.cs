using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using cmcs_.Models; // Ensure this matches your namespace for the LoginViewModel

namespace cmcs_.Controllers
{
    public class AccountController : Controller
    {
        // GET: Login page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login page
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Call the ValidateUser method to check user credentials
                var user = ValidateUser(model.UserName, model.Password);

                if (user != null)
                {
                    // Create claims for the authenticated user
                    var claims = new List<Models.Claim>
                    {
                        new Models.Claim(ClaimTypes.Name, user.UserName), // Adjust based on your User model
                        new Models.Claim(ClaimTypes.Role, user.Role) // Add any additional claims as needed
                    };

                    var claimsIdentity = new ClaimsIdentity((IEnumerable<System.Security.Claims.Claim>?)claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe // Use RememberMe from the LoginViewModel
                    };

                    // Sign in the user with the specified claims
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    return RedirectToAction("Index", "Home"); // Redirect to home or desired action
                }

                ViewData["LoginError"] = "Invalid username or password.";
            }
            return View(model);
        }

        // This method validates the user credentials
        private User ValidateUser(string username, string password)
        {
            // Example logic for user validation
            // Replace with your own logic to validate the user, for example:
            // Check against a database or in-memory store

            // Placeholder for user retrieval, replace with actual logic
            if (username == "testuser" && password == "password123") // Example credentials
            {
                return new User { UserName = username, Role = "User" }; // Example user with a role
            }

            return null; // Return null if validation fails
        }

        // You may need an action for logout as well
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }

    // Example User model
    public class User
    {
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}
