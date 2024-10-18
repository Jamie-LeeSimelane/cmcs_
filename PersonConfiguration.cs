using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography;
using System.Text;

namespace cmcs_.Models.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasData(
                new Person
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    UserName = "john.doe@example.com",
                    Password = HashPassword("password1"), // Hashing the password
                    Role = "Lecturer"
                },
                new Person
                {
                    Id = 2,
                    FirstName = "Harry",
                    LastName = "Brodersen",
                    UserName = "harry.brodersen@example.com",
                    Password = HashPassword("password2"), // Hashing the password
                    Role = "Coordinator"
                },
                new Person
                {
                    Id = 3,
                    FirstName = "Alice",
                    LastName = "Smith",
                    UserName = "alice.smith@example.com",
                    Password = HashPassword("password3"), // Hashing the password
                    Role = "Lecturer"
                },
                new Person
                {
                    Id = 4,
                    FirstName = "Bob",
                    LastName = "Johnson",
                    UserName = "bob.johnson@example.com",
                    Password = HashPassword("password4"), // Hashing the password
                    Role = "Academic Manager"
                },
                new Person
                {
                    Id = 5,
                    FirstName = "Eva",
                    LastName = "Green",
                    UserName = "eva.green@example.com",
                    Password = HashPassword("password5"), // Hashing the password
                    Role = "Coordinator"
                }
            );
        }

        private string HashPassword(string password)
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
    }
}
