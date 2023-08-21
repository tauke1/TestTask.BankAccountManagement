using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TestTask.BankAccountManagement.BAL.Factories;
using TestTask.BankAccountManagement.BAL.Factories.Interfaces;
using TestTask.BankAccountManagement.BAL.MappingProfiles;
using TestTask.BankAccountManagement.BAL.Models.Settings;
using TestTask.BankAccountManagement.BAL.SecurityHashers;
using TestTask.BankAccountManagement.BAL.SecurityHashers.Interfaces;
using TestTask.BankAccountManagement.BAL.Services;
using TestTask.BankAccountManagement.BAL.Services.Interfaces;
using TestTask.BankAccountManagement.BAL.Utilities;
using TestTask.BankAccountManagement.BAL.Utilities.Interfaces;
using TestTask.BankAccountManagement.BAL.Wrappers;
using TestTask.BankAccountManagement.BAL.Wrappers.Interfaces;
using TestTask.BankAccountManagement.DAL;
using TestTask.BankAccountManagement.DAL.Contexts;
using TestTask.BankAccountManagement.DAL.Interceptors;
using TestTask.BankAccountManagement.DAL.Repositories;
using TestTask.BankAccountManagement.DAL.Repositories.Interfaces;
using TestTask.BankAccountManagement.Factories;
using TestTask.BankAccountManagement.Factories.Interfaces;
using TestTask.BankAccountManagement.Helpers;
using TestTask.BankAccountManagement.Helpers.Interfaces;
using TestTask.BankAccountManagement.Middlewares;
using TestTask.BankAccountManagement.Models.Settings;
using TestTask.BankAccountManagement.Shared.Wrappers;
using TestTask.BankAccountManagement.Shared.Wrappers.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// db context
builder.Services.AddDbContext<AppDbContext>(c => c.UseSqlServer(
    builder.Configuration.GetConnectionString("AppDbContext"))
    .AddInterceptors(new SoftDeleteInterceptor()));

// unit of work
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

// options
var bindJwtSettings = new JwtSettings();
builder.Configuration.Bind("Jwt", bindJwtSettings);
builder.Services.Configure<JwtSettings>((s) =>
{
    s.ExpiresInSeconds = bindJwtSettings.ExpiresInSeconds;
    s.Issuer = bindJwtSettings.Issuer;
    s.Key = bindJwtSettings.Key;
});
builder.Services.Configure<BcryptSettings>(builder.Configuration.GetSection("BCrypt"));

// authentication && authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = bindJwtSettings.Issuer,
            ValidAudience = bindJwtSettings.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(bindJwtSettings.Key))
        };
    });

// wrapper
builder.Services.AddTransient<IDateTimeWrapper, DateTimeWrapper>();
builder.Services.AddTransient<IRandomWrapper, RandomWrapper>();

// services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountOperationService, AccountOperationService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

// repositories
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IAccountOperationRepository, AccountOperationRepository>();
builder.Services.AddTransient<IAccountTypeSettingsRepository, AccountTypeSettingsRepository>();
builder.Services.AddTransient<ICountryRepository, CountryRepository>();
builder.Services.AddTransient<IManagerRepository, ManagerRepository>();
builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();

// factories
builder.Services.AddSingleton<IJwtTokenFactory, JwtTokenFactory>();
builder.Services.AddSingleton<IIbanGeneratorFactory, IbanGeneratorFactory>();

// hashers
builder.Services.AddSingleton<ISecurityHasher, BCryptHasher>();

// automapper
builder.Services.AddAutoMapper(typeof(MapperMarker));

// http context accessor
builder.Services.AddHttpContextAccessor();

// helpers
builder.Services.AddTransient<IUserHelper, UserHelper>();

// utilities
builder.Services.AddTransient<IPredicateBuilder, PredicateBuilder>();

// swagger
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Accounts API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionMiddleware>();

app.Run();
