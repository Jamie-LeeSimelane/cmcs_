using Microsoft.AspNetCore.Mvc;
using cmcs_.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

public class HomeController : Controller
{
    private readonly IClaimRepository _claimRepository; // Injected repository for claims

    // Simulated user data. Replace with your actual user retrieval logic.
    private readonly List<Person> _users = new List<Person>
    {
        new Person { UserName = "john.doe@example.com", Password = HashPassword("password1"), Role = "Lecturer" },
        new Person { UserName = "harry.brodersen@example.com", Password = HashPassword("password2"), Role = "Coordinator" },
        new Person { UserName = "manager@example.com", Password = HashPassword("password3"), Role = "Manager" },
        new Person { UserName = "admin@example.com", Password = HashPassword("password4"), Role = "Coordinator" }
    };

    // Constructor to inject the claims repository
    public HomeController(IClaimRepository claimRepository)
    {
        _claimRepository = claimRepository; // Assign the injected repository
    }

    public IActionResult Index()
    {
        return View(); // Return the welcome page
    }

    public IActionResult Privacy()
    {
        return View(); // Ensure you have Privacy.cshtml in Views/Home
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(); // Return the login page
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Hash the input password before checking
            var hashedPassword = HashPassword(model.Password);
            var user = _users.FirstOrDefault(u => u.UserName == model.UserName && u.Password == hashedPassword);

            if (user != null)
            {
                // Store user details in session
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("Role", user.Role);

                // Redirect based on the user's role
                if (user.Role == "Lecturer")
                {
                    return RedirectToAction("ClaimStatus", "Lecturer");
                }
                else if (user.Role == "Coordinator")
                {
                    return RedirectToAction("Claims", "Coordinator");
                }
                else if (user.Role == "Manager")
                {
                    return RedirectToAction("Claims", "Manager");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt. Please try again.");
            }
        }
        return View(model); // Return to login page if login failed
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clear session data on logout
        return RedirectToAction("Index", "Home"); // Redirect to welcome page after logout
    }

    // Password hashing utility method
    private static string HashPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }

    [HttpGet]
    public IActionResult VerifyClaims()
    {
        // Check if the user is authorized to view the claims
        var role = HttpContext.Session.GetString("Role");
        if (role != "Coordinator" && role != "Manager")
        {
            // Redirect to the Login page if the user is not authorized
            return RedirectToAction("Login");
        }

        var claims = _claimRepository.GetAllClaims(); // Use the injected claims repository
        return View(claims); // Return the claims for authorized users
    }
}
