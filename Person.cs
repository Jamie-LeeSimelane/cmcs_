using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography; // Required for SHA256
using System.Text; // Required for Encoding

namespace cmcs_.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Username cannot be longer than 100 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; }

        // Method to hash the password
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2")); // Convert to hexadecimal
                }
                return builder.ToString();
            }
        }

        // Optional: Method to validate password
        public bool ValidatePassword(string inputPassword)
        {
            return HashPassword(inputPassword) == Password;
        }
    }
}
