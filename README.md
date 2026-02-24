# JwtAuthDemoAPI
This is the backend API for the JwtAuthDemo project. It provides endpoints for user authentication, role-based authorization, and profile access using **JWT tokens**.
## 🚀 Features
- User login with username and password
- JWT token issuance and validation
- Role-based endpoints (Admin, User, Common)
- Profile endpoint to return current user info
- Secure authorization middleware

## 🛠️ Technologies
- ASP.NET Core Web API
- JWT Authentication (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- Entity Framework Core (optional for persistence)
- CORS enabled for Blazor WebAssembly frontend

## 📂 Endpoints
- `POST /user/login` → Authenticate user and return JWT + user info
- `GET /roletest/profile` → Returns username and role from JWT
- `GET /roletest/admin` → Accessible only by Admin role
- `GET /roletest/user` → Accessible only by User role
- `GET /roletest/common` → Accessible by both Admin and User roles

## 📘 Frontend README (`README.md` in Frontend Project)


# JwtAuthDemoUI
This is the Blazor WebAssembly frontend for the JwtAuthDemo project. It demonstrates how to integrate **JWT authentication** with Blazor, including login, logout, profile display, and role-based pages.

## 🚀 Features
- Login page with username/password
- Logout button to clear JWT and reset state
- Profile page showing username and role
- Admin-only page (restricted)
- User-only page (restricted)
- Common page (accessible to all roles)
- Role-based navigation and authorization

## 🛠️ Technologies
- Blazor WebAssembly (.NET 8)
- `Blazored.LocalStorage` for storing JWT tokens
- `AuthenticationStateProvider` for managing auth state
- `HttpClient` for API calls
- `<AuthorizeView>` for role-based UI rendering

## 📂 Pages
- `/login` → Login form
- `/logout` → Logout button
- `/profile` → Shows current user info
- `/admin` → Admin-only endpoint
- `/user` → User-only endpoint
- `/common` → Accessible to both roles
- `/` → Home page with app info
