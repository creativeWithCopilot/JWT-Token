using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace JwtAuthDemoUI.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        // Holds the current user (ClaimsPrincipal). Starts as an anonymous user.
        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        // Blazor calls this method to get the current authentication state.
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // It returns the ClaimsPrincipal representing the current user.
            return Task.FromResult(new AuthenticationState(_currentUser));
        }

        // Called when a user successfully logs in.
        // Updates the ClaimsPrincipal with identity and role claims, then notifies Blazor that the authentication state has changed.
        public void NotifyUserAuthentication(string token)
        {
            // TODO: Parse the JWT token to extract real claims (username, role, etc.)
            // For now, we hardcode "User" and "Admin" as placeholders.
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "User"),
                new Claim(ClaimTypes.Role, "Admin") // replace with parsed role
            }, "jwt");

            _currentUser = new ClaimsPrincipal(identity);
            // Notify Blazor that the authentication state has changed (user is now authenticated)
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }

        // Called when a user logs out.
        // Resets the ClaimsPrincipal to an empty identity (anonymous user), then notifies Blazor that the authentication state has changed.
        public void NotifyUserLogout()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            // Notify Blazor that the authentication state has changed (user is now anonymous)
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }
    }
}
