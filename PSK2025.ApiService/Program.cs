using Microsoft.AspNetCore.Identity;
using PSK2025.Data.Contexts;
using PSK2025.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PSK2025.ApiService.Services;
using PSK2025.ApiService.Services.Interfaces;
using PSK2025.ApiService.Settings;
using System.IdentityModel.Tokens.Jwt;
using PSK2025.Data.Seeding;
using PSK2025.ApiService.Extensions;
using PSK2025.ApiService.Controllers.Auth;
using PSK2025.ApiService.Interfaces;
using PSK2025.Data.Repositories;
using PSK2025.Data.Repositories.Interfaces;
using PSK2025.MigrationService.Abstractions;
using PSK2025.ApiService.Validators.Auth;
using FluentValidation;
using PSK2025.Data.Requests.Auth;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IValidator<UserLoginRequest>, UserLoginRequestValidator>();
builder.Services.AddScoped<IValidator<RegisterNewUserRequest>, RegisterUserRequestValidator>();


builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


builder.Services.AddEndpoints();

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        NameClaimType = JwtRegisteredClaimNames.Sub,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))


    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"error\":\"Authentication failed: " + context.Exception.Message + "\"}");
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token is valid.");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();


builder.AddServiceDefaults();

//builder.Services.AddApplication(builder.Configuration);


builder.AddNpgsqlDbContext<AppDbContext>(connectionName: "postgresdb");

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();


builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IRouteGroup, AuthRouteGroup>();

builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddScoped<ITaskService,  TaskService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowWeb",
        configurePolicy: policy =>
        {
            policy.WithOrigins(builder.Configuration["ALLOWED_ORIGIN"] ?? "")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Your API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your token. Example: 'ey...'"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}


app.MapDefaultEndpoints();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeeder.SeedRolesAsync(roleManager);
    await DataSeeder.SeedAsync(services);
}


app.MapGroupedEndpoints();
app.UseCors("AllowWeb");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/debug", [Authorize] (ClaimsPrincipal user) =>
{
    var claims = user.Claims.Select(c => new { c.Type, c.Value }).ToList();
    return Results.Ok(claims);
});
app.Run();

