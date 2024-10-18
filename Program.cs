using cmcs_;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace cmcs_Views.Lecturer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Build and run the web host
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args) // Create a default host with configuration
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); // Specify the Startup class for application configuration
                });
    }
}
