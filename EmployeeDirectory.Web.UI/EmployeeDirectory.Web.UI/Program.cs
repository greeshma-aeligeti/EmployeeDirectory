using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using EmployeeDirectory.API.APIServices;
using EmployeeDirectory.BLL.IServices;
using EmployeeDirectory.BLL.Services;
using EmployeeDirectory.Web.UI.Authentication;
using EmployeeDirectory.Web.UI.Client.Pages;
using EmployeeDirectory.Web.UI.Components;
using EmployeeDirectory.Web.UI.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDirectory.Web.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();
/*            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
*/            builder.Services.AddDbContext<ApplicationDbContext>(options=>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<EmployeeAPIService>();
/*            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
*/

            builder.Services.AddHttpClient<EmployeeAPIService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7168"); // Replace with your actual API URL
            });
            builder.Services.AddHttpClient("API", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7168"); // API base URL
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true; // Prevents client-side access to the cookie
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Set the cookie expiration time
                options.SlidingExpiration = true; // Enables sliding expiration
                options.LoginPath = "/Account/Login"; // Path to redirect when not authenticated
                options.LogoutPath = "/Account/Logout"; // Path for logging out
                options.AccessDeniedPath = "/Account/AccessDenied"; // Path for access denied
            });
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddIdentityCookies();

            builder.Services.AddServerSideBlazor().AddCircuitOptions(options =>
            {
                options.DetailedErrors = true;
            });


            builder.Services
      .AddBlazorise()
      .AddBootstrap5Providers()  // Add Bootstrap providers
      .AddFontAwesomeIcons();
            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddAuthorization();

            builder.Services.AddScoped<AuthenticationStateProvider, PersistAuthStateProvider>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);
            app.MapSignoutEndpoit();
            app.Run();
        }
    }
}
