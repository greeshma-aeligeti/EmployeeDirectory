using Blazorise;
using EmployeeDirectory.Web.UI.Client.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace EmployeeDirectory.Web.UI.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddBlazorise();

            builder.Services.AddSingleton<AuthenticationStateProvider, ClientAuthStateProvider>();
            await builder.Build().RunAsync();
        }
    }
}
