using BusinessObjects;
using DataAccessObjects;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PRN222_BL3_Project.Hubs;
using ProjectPRN222_BL3_Project.Services;
using Repositories;

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
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IVnPayService, VnPayService>();
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
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Yêu c?u HTTPS
                options.Cookie.SameSite = SameSiteMode.None; // C?n thi?t cho OAuth cross-site
                options.Cookie.Name = ".AspNetCore.Session";
            });

            builder.Services.AddSignalR();

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
            app.UseSession();

            app.MapHub<NotificationHub>("/notificationHub");

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
