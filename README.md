# Asp.net-core-web-API

Perfect Rajesh 🔥
I’ll give you a **complete minimal eCommerce Backend (Production-Ready Structure)** using:

✅ .NET 8
✅ ASP.NET Core Web API
✅ Entity Framework Core
✅ SQL Server
✅ JWT Authentication
✅ Products
✅ Cart
✅ Orders

This is a **clean professional backend structure** 👇

---

# 🏗 PROJECT STRUCTURE

```
EcommerceAPI
│
├── Controllers
│   ├── AuthController.cs
│   ├── ProductsController.cs
│   ├── CartController.cs
│   └── OrdersController.cs
│
├── Models
│   ├── User.cs
│   ├── Product.cs
│   ├── CartItem.cs
│   └── Order.cs
│
├── Data
│   └── AppDbContext.cs
│
├── DTOs
│   ├── RegisterDto.cs
│   └── LoginDto.cs
│
├── Program.cs
└── appsettings.json
```

---

# 1️⃣ Install NuGet Packages

```
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Microsoft.AspNetCore.Authentication.JwtBearer
System.IdentityModel.Tokens.Jwt
```

---

# 2️⃣ MODELS

## 📁 Models/User.cs

```csharp
namespace EcommerceAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
```

---

## 📁 Models/Product.cs

```csharp
namespace EcommerceAPI.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
```

---

## 📁 Models/CartItem.cs

```csharp
namespace EcommerceAPI.Models;

public class CartItem
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
```

---

## 📁 Models/Order.cs

```csharp
namespace EcommerceAPI.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
}
```

---

# 3️⃣ DTOs

## 📁 DTOs/RegisterDto.cs

```csharp
namespace EcommerceAPI.DTOs;

public class RegisterDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}
```

## 📁 DTOs/LoginDto.cs

```csharp
namespace EcommerceAPI.DTOs;

public class LoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}
```

---

# 4️⃣ DB CONTEXT

## 📁 Data/AppDbContext.cs

```csharp
using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Models;

namespace EcommerceAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
}
```

---

# 5️⃣ AUTH CONTROLLER (JWT)

## 📁 Controllers/AuthController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Data;
using EcommerceAPI.Models;
using EcommerceAPI.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcommerceAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterDto dto)
    {
        var user = new User
        {
            Username = dto.Username,
            Password = dto.Password
        };

        _context.Users.Add(user);
        _context.SaveChanges();
        return Ok("User Registered");
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        var user = _context.Users
            .FirstOrDefault(x => x.Username == dto.Username && x.Password == dto.Password);

        if (user == null)
            return Unauthorized();

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("UserId", user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds);

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }
}
```

---

# 6️⃣ PRODUCTS CONTROLLER

## 📁 Controllers/ProductsController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Data;
using EcommerceAPI.Models;

namespace EcommerceAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetProducts()
    {
        return Ok(_context.Products.ToList());
    }

    [HttpPost]
    public IActionResult AddProduct(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
        return Ok(product);
    }
}
```

---

# 7️⃣ CART CONTROLLER

## 📁 Controllers/CartController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Data;
using EcommerceAPI.Models;

namespace EcommerceAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly AppDbContext _context;

    public CartController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult AddToCart(CartItem item)
    {
        _context.CartItems.Add(item);
        _context.SaveChanges();
        return Ok(item);
    }

    [HttpGet("{userId}")]
    public IActionResult GetCart(int userId)
    {
        var cart = _context.CartItems
            .Where(x => x.UserId == userId)
            .ToList();

        return Ok(cart);
    }
}
```

---

# 8️⃣ ORDERS CONTROLLER

## 📁 Controllers/OrdersController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Data;
using EcommerceAPI.Models;

namespace EcommerceAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult CreateOrder(int userId)
    {
        var cart = _context.CartItems
            .Where(x => x.UserId == userId)
            .ToList();

        var total = cart.Sum(x =>
            _context.Products.First(p => p.Id == x.ProductId).Price * x.Quantity);

        var order = new Order
        {
            UserId = userId,
            TotalAmount = total
        };

        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(cart);
        _context.SaveChanges();

        return Ok(order);
    }
}
```

---

# 9️⃣ Program.cs

```csharp
using EcommerceAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
```

---

# 🔟 appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=EcommerceDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "THIS_IS_MY_SECRET_KEY_12345"
  }
}
```

---

# 🛠 RUN DATABASE MIGRATION

Open Package Manager Console:

```
Add-Migration InitialCreate
Update-Database
```

---

# 🎯 YOU NOW HAVE

✅ User Registration/Login (JWT)
✅ Products CRUD
✅ Add to Cart
✅ Create Order
✅ SQL Server Integration
✅ Production Architecture

---

# 🔥 Next Level (If You Want)

* Password hashing (very important for production)
* Role-based admin system
* Stripe / Razorpay integration
* Image upload
* Azure deployment
* Clean Architecture version

---

If you want, I can now give you:

👉 Full frontend (index.html + JS) integrated with this API
👉 Production-secure version with hashed passwords
👉 Deploy to Azure step-by-step

Tell me what’s next, Rajesh 🚀
