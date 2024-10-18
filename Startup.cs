using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore; // For Entity Framework Core
using cmcs_.Models; // Include your models namespace for the DbContext
using System;
using cmcs_.Services;

namespace cmcs_
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration; // Configuration settings
        }

        public IConfiguration Configuration { get; }

        // This method is called by the runtime to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(); // Add MVC services

            // Register the DbContext with a SQL Server provider
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Register the ClaimRepository for Dependency Injection
            services.AddSingleton<IClaimRepository, ClaimRepository>(); // Registering the repository

            // Register other services for Dependency Injection
            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<IDocumentService, DocumentService>();

            // Configure authentication (e.g., Cookie authentication)
            services.AddAuthentication("YourAuthenticationScheme") // Change to your preferred authentication scheme
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login"; // Set your login path
                    options.AccessDeniedPath = "/Account/AccessDenied"; // Set your access denied path
                });

            // Configure session management with a timeout setting
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout to 30 minutes
                options.Cookie.HttpOnly = true; // Makes the cookie accessible only through HTTP requests
                options.Cookie.IsEssential = true; // Marks the cookie as essential
            });

            // Optional: Configure Antiforgery for additional security (especially with forms)
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN"; // Add custom header for antiforgery token
            });
        }

        // This method is called by the runtime to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Show detailed error pages in development
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // Redirect to error handler in production
                app.UseHsts(); // Enable HTTP Strict Transport Security in production
            }

            app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
            app.UseStaticFiles(); // Serve static files (e.g., CSS, JS, images)

            app.UseRouting(); // Enable routing middleware

            // Enable authentication and authorization middleware
            app.UseAuthentication(); // Enable authentication
            app.UseAuthorization(); // Enable authorization

            // Enable session middleware
            app.UseSession();

            // Define endpoint routing
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"); // Set default route

                // Add specific route for claims
                endpoints.MapControllerRoute(
                    name: "claims",
                    pattern: "Claims/{action=Index}/{id?}", // Corrected to point to Claims controller
                    defaults: new { controller = "Claims" });
            });
        }
    }
}
