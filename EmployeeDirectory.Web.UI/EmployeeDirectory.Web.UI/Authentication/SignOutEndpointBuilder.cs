using EmployeeDirectory.Web.UI.Data;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace EmployeeDirectory.Web.UI.Authentication
{
   internal static class SignOutEndpointBuilder
    {
        public static IEndpointConventionBuilder MapSignoutEndpoit(this IEndpointRouteBuilder endpoints)
        {
            var accountGroup = endpoints.MapGroup("/Account");
            accountGroup.MapPost("/Logout", async
                (
                ClaimsPrincipal user,
                SignInManager<ApplicationUser> signInManager
                ) =>
            {
                await signInManager.SignOutAsync();
                return TypedResults.LocalRedirect("/");
            });
            return accountGroup;
        }
    }
}
