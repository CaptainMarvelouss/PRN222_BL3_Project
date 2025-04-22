using BusinessObjects;
using DataAccessObjects;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepositories;
using Repositories.Repositories;

namespace PRN222_BL3_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IFootballFieldRepository, FootballFieldRepository>();

            builder.Services.AddDbContext<FootballFieldBookingContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("FootballDB"));
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(options =>
                 {
                     options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                     options.SlidingExpiration = true;
                     options.LoginPath = "/Auth/Login";
                     options.AccessDeniedPath = "/Forbidden/";
                 })
                 .AddGoogle(googleoptions =>
                 {
                     googleoptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                     googleoptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                 });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
