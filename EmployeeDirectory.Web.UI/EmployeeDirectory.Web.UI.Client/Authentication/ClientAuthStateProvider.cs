using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
namespace EmployeeDirectory.Web.UI.Client.Authentication
{
    public class ClientAuthStateProvider : AuthenticationStateProvider
    {
        private static readonly Task<AuthenticationState> defaultUnauthenticatedTask =
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

        private readonly Task<AuthenticationState> authenticationStateTask = defaultUnauthenticatedTask;

        public ClientAuthStateProvider(PersistentComponentState state)
        {
            if(!state.TryTakeFromJson<UserSession>(nameof(UserSession),out var userInfo) || userInfo is null)
            {
                return;
            }
            Claim[] claims = [
                new Claim(ClaimTypes.Name, userInfo.FullName),
            new Claim(ClaimTypes.Email, userInfo.Email!)
            ];

            authenticationStateTask = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: nameof(ClientAuthStateProvider)))));
        }


        public override Task<AuthenticationState> GetAuthenticationStateAsync() => authenticationStateTask;
       
    }
}
