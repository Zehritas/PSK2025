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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IValidator<UserLoginRequest>, UserLoginRequestValidator>();
builder.Services.AddScoped<IValidator<RegisterNewUserRequest>, RegisterUserRequestValidator>();


builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        NameClaimType = JwtRegisteredClaimNames.Sub,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        ),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Authentication failed: " + context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token is valid.");
            return Task.CompletedTask;
        }
    };
});


builder.Services.AddEndpoints();

builder.Services.AddEndpointsApiExplorer();
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

builder.AddServiceDefaults();

//builder.Services.AddApplication(builder.Configuration);


builder.AddNpgsqlDbContext<AppDbContext>(connectionName: "postgresdb");

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

builder.Services.AddAuthorization();
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IRouteGroup, AuthRouteGroup>();

builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IUserProjectService, UserProjectService>();
// builder.Services.AddScoped<IUserProjectRepo, >();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));


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

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}


app.MapDefaultEndpoints();


app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DataSeeder.SeedAsync(services);
}


app.MapGroupedEndpoints();
app.UseCors("AllowWeb");

app.Run();

