using JwtAuthDemoUI;
using JwtAuthDemoUI.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
// Configure HttpClient with the base address of the backend API for making HTTP requests.
// BaseAddress should point to the backend API URL.
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7241/") // your API base URL
});

// Register Blazored.LocalStorage for managing local storage in the browser, which is used to store the JWT token.
builder.Services.AddBlazoredLocalStorage();
// Register the custom authentication state provider and the authentication state provider service to manage user authentication state in the Blazor application.
builder.Services.AddScoped<CustomAuthStateProvider>();
// The AuthenticationStateProvider is a service that provides the current authentication state of the user. By registering it with the custom implementation, we ensure that the Blazor application uses our custom logic to determine if the user is authenticated and what their roles are.
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());
// Register the AuthService, which contains methods for handling authentication-related operations such as login, logout, and token management.
builder.Services.AddScoped<AuthService>();
// Add authorization services to enable role-based or policy-based authorization in the Blazor application.
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
