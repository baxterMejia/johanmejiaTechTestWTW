using DataAccess.Models;
using Domain.Interfaces.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.Interfaces;
using Repository.Order;
using Repository.Product;
using Service.Interfaces;
using Service.Order;
using Service.Product;
using System.Security.Claims;
using System.Text;
using Utilities;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        //El key debe ser el mismo como variable de entorno en el servidor
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY", EnvironmentVariableTarget.Machine)))
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var userId = context.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Asume que el correo o usuario está en este claim.

            // Obtener el servicio ITokenService del contenedor de inyección de dependencias
            var tokenService = context.HttpContext.RequestServices.GetService<IUserService>();
            var userService = context.HttpContext.RequestServices.GetService<IUserRepository>();

            // Ejemplo: Verificar si la sesion del usuario existe en la base de datos            
            bool exists = await userService.userStateSession(userId);

            if (!exists)
            {
                context.Fail("Usuario no encontrado o sesion deprecada");
                return;
            }

            //Renovar el token si está a punto de expirar
            var expirationClaim = context.Principal.FindFirst(ClaimTypes.Expiration)?.Value;
            if (expirationClaim != null)
            {
                DateTime expirationDate = DateTime.Parse(expirationClaim);
                if (expirationDate.AddMinutes(-5) <= DateTime.UtcNow)
                {
                    // Generar un nuevo token usando el método GenerateToken de ITokenService
                    var newToken = tokenService.GenerateToken(userId);
                    
                    context.HttpContext.Response.Headers.Add("New-Token", newToken);
                }
            }
        },
    };
});


builder.Services.AddControllers();
builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});


builder.Services.AddScoped<Repository.Interfaces.IUserRepository, Repository.User.UserRepository>();
builder.Services.AddScoped<Service.Interfaces.IUserService, Service.User.UserService>();
builder.Services.AddScoped<Domain.Interfaces.Domain.IUserDomain, Domain.UserDomain>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Agregar la cadena de conexión desde appsettings.json o configuración
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

//Ajustar origen de Politica CORS para el proyecto
app.UseCors(builder =>
{
    //Aqui se configuran los origenes separados por , en la variable de entorno ORIGIN_API_OWASP o la que tenga lugar ejemplo: "http://localhost:3000", "https://otro-sitio-web.com"
    //builder.WithOrigins(Environment.GetEnvironmentVariable("ORIGIN_API_OWASP", EnvironmentVariableTarget.Machine))
    //        .AllowAnyMethod()
    //        .AllowAnyHeader();
    builder.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
});



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();
