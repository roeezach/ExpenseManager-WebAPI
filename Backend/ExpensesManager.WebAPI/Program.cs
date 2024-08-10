using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using ExpensesManager.DB;
using ExpensesManager.Services.BuisnessLogic.Map;
using ExpensesManager.Services.Contracts;
using ExpensesManager.Services.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using ExpensesManager.BuisnessLogic.Core;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Check if running inside a Docker container
bool isDocker = Utils.IsAppInContainer();

if (!isDocker)
{
    // Add appsettings.json only if not running in Docker
    builder.Configuration.AddJsonFile("Configurations/appsettings.json", optional: true, reloadOnChange: true);
}

// Always add environment variables
builder.Configuration.AddEnvironmentVariables();

// builder.Configuration.AddJsonFile("Configurations/appsettings.json", optional: true, reloadOnChange: true)
// .AddEnvironmentVariables();

builder.Services.AddControllers();


builder.Services.AddDbContext<AppDbContext>();

// builder.Services.AddDbContext<AppDbContextAutomation>();

builder.Services.AddTransient<IExpenseReaderService, ExpenseReadService>();
builder.Services.AddTransient<IExpenseMapperService, ExpenseMapperService>();
builder.Services.AddTransient<ITotalExpensesPerCategoryService, TotalExpensesPerCategoryService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ISplitwiseExpensesService, SplitewiseExpenseService>();
builder.Services.AddTransient<IRecalculatedExpenseService, RecalculatedExpenseService>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ExpenseMapperFactory>();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
    {
        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    });

string secret = Utils.GetSecret(builder.Configuration);
string issuer = Utils.GetIssuer(builder.Configuration);

// if (!isDocker)
// {
//     secret = builder.Configuration.GetSection("Secrets")["JWT_SECRET"];
//     issuer = builder.Configuration.GetSection("Secrets")["JWT_ISSUER"];
// }

// else
// {
//     secret = Environment.GetEnvironmentVariable("JWT_SECRET");
//     issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
// }

// // Load secrets from configuration
// if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(issuer))
// {
//     throw new ArgumentNullException("JWT_SECRET and JWT_ISSUER must be provided");
// }


builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; ;
    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(options =>
{
    options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

// builder.Services.AddCors(options =>
// {
//     options.AddDefaultPolicy(builder =>
//     {
//         builder.AllowAnyOrigin()
//                .AllowAnyMethod()
//                .AllowAnyHeader();
//     });
// });

// // ... later in the middleware pipeline ...
// app.UseCors();


app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     dbContext.Database.Migrate();
// }

app.Run();