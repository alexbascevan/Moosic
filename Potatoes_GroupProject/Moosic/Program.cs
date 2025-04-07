using Microsoft.EntityFrameworkCore;
using Moosic.Models;
using Moosic.Services;

namespace Moosic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add the database configuration.
            builder.Services.AddDbContext<MoosicDbContext>(opts =>
            {
                opts.UseSqlServer(builder.Configuration["ConnectionStrings:MoosicConn"]);
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient<SpotifyAuthService>();
            builder.Services.AddHttpClient<ApiService>();
            builder.Services.AddScoped<SpotifyAuthService>();
            builder.Services.AddScoped<ApiService>();

            // Add session services.
            builder.Services.AddSession(options =>
            {
                // Optionally set session options, e.g., timeout.
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            // Enable session middleware.
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
