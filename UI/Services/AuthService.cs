using System.Net.Http.Json;
using JwtAuthDemoUI.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace JwtAuthDemoUI.Services
{
    public class AuthService
    {
        private readonly HttpClient _http; // HttpClient used to send requests to the backend API
        private readonly ILocalStorageService _localStorage; // Local storage service for persisting JWT tokens in the browser
        private readonly CustomAuthStateProvider _authStateProvider; // Custom authentication state provider to notify Blazor about login/logout events

        public string JwtToken { get; private set; } // The current JWT token issued by the backend after login.

        // Constructor: injects HttpClient, LocalStorage, and CustomAuthStateProvider.
        public AuthService(HttpClient http, ILocalStorageService localStorage, CustomAuthStateProvider authStateProvider)
        {
            _http = http;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        // Attempts to log in with the given username and password.
        // If successful, stores the JWT token, sets the Authorization header, and notifies Blazor that the user is authenticated.
        public async Task<bool> LoginAsync(string username, string password)
        {
            // Send login request to backend
            var response = await _http.PostAsJsonAsync("user/login", new LoginDto
            {
                Username = username,
                Password = password
            });

            // If login fails, return false
            if (!response.IsSuccessStatusCode) return false;

            // Deserialize response into LoginResponse (contains token + user info)
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            JwtToken = result.Token;

            // Save token in browser local storage for persistence across page reloads
            await _localStorage.SetItemAsync("jwt", JwtToken);

            // Attach token to HttpClient for subsequent API requests
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JwtToken);

            // Notify Blazor that authentication state has changed (user is logged in)
            _authStateProvider.NotifyUserAuthentication(JwtToken);

            return true;
        }

        // Logs out the current user by clearing the JWT token, removing it from local storage, and notifying Blazor that the user is now anonymous.
        public async Task LogoutAsync()
        {
            JwtToken = null; // Clear token in memory
            await _localStorage.RemoveItemAsync("jwt"); // Remove token from local storage
            _http.DefaultRequestHeaders.Authorization = null; // Remove Authorization header

            // Notify Blazor that authentication state has changed (user logged out)
            _authStateProvider.NotifyUserLogout();
        }
    }
}
