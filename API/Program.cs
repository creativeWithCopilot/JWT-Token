using System.Text;
using JwtAuthDemo.Data;
using JwtAuthDemo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add DbContext with SQLite 
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register TokenService 
builder.Services.AddSingleton<TokenService>();

// Bind JWT settings from configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
        };
    });

// Configure CORS (Cross-Origin Resource Sharing) policy.
// This allows the Blazor frontend (running on https://localhost:7091) to call the API.
builder.Services.AddCors(options => 
{ 
    options.AddPolicy("AllowFrontend", 
        policy => 
        { 
            policy.WithOrigins("https://localhost:7046") // Blazor WASM port 
            .AllowAnyHeader() 
            .AllowAnyMethod(); 
        }); 
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(); // Add controllers for handling authentication and other API endpoints
builder.Services.AddAuthorization(); // Add authorization services to enable role-based or policy-based authorization

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply the CORS policy so the frontend can access the API.
app.UseCors("AllowFrontend");
app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization(); // Enable authorization middleware
app.MapControllers(); // Map controller routes
app.Run();
