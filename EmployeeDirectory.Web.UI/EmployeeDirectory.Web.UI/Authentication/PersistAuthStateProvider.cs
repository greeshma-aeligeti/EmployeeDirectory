using EmployeeDirectory.Web.UI.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Principal;
using System.Diagnostics;
using EmployeeDirectory.Web.UI.Client.Authentication;

namespace EmployeeDirectory.Web.UI.Authentication
{
    public class PersistAuthStateProvider : RevalidatingServerAuthenticationStateProvider
    {

        private readonly IServiceScopeFactory scopeFactory; 
        private readonly PersistentComponentState state;
        private readonly IdentityOptions options;
        private readonly PersistingComponentStateSubscription subscription;
        private Task<AuthenticationState>? authenticationStateTask;
        public PersistAuthStateProvider(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory serviceScopeFactory, PersistentComponentState persistentComponentState, IOptions<IdentityOptions> optionsAccessor)
        : base(loggerFactory)
        {
            scopeFactory = serviceScopeFactory; 

            state = persistentComponentState;
            options = optionsAccessor.Value;
            AuthenticationStateChanged += OnAuthenticationStateChanged;
            subscription = state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
        }
        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

        protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            // Get the user manager from a new scope to ensure it fetches fresh data
            await using var scope = scopeFactory.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            return await ValidateSecurityStampAsync(userManager, authenticationState.User);
        }
        private async Task<bool> ValidateSecurityStampAsync(UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
        {

            var user = await userManager.GetUserAsync(principal); 
            if (user is null)
            {
                return false;
            }
            else if (!userManager.SupportsUserSecurityStamp)
            {
                return true;
            }
            else
            {
                var principalStamp = principal.FindFirstValue(options.ClaimsIdentity.SecurityStampClaimType);
                var userStamp = await userManager.GetSecurityStampAsync(user);
                return principalStamp == userStamp;
            }
        }
        private void OnAuthenticationStateChanged(Task<AuthenticationState> task)
        {
            authenticationStateTask = task;
        }

        private async Task OnPersistingAsync()
        {
            if (authenticationStateTask is null)
            {

                throw new UnreachableException($"Authentication state not set in {nameof(OnPersistingAsync)}().");
            }
            var authenticationState = await authenticationStateTask;
            var principal = authenticationState.User;
            if (principal.Identity?.IsAuthenticated == true)
            {
                // Gather what you want to display in the Client UI
                var fullname = principal.Claims.First(_ => _.Type == "Fullname").Value;
                var email = principal.FindFirst(options.ClaimsIdentity.EmailClaimType)?.Value;
                if (fullname != null && email != null) {
                    state.PersistAsJson(nameof(UserSession), new UserSession
                    {
                        FullName = fullname,
                        Email = email,
                    });
                }
            } }

        protected override void Dispose(bool disposing)
        {
            subscription.Dispose();
            AuthenticationStateChanged -= OnAuthenticationStateChanged;
            base.Dispose(disposing);

        }
    }
}
