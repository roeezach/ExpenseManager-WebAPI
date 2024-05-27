using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
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

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("Configurations/appsettings.json", optional: true, reloadOnChange: true);
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddTransient<IExpenseReaderService, ExpenseReadService>();
builder.Services.AddTransient<IExpenseMapperService, ExpenseMapperService>();
builder.Services.AddTransient<ITotalExpensesPerCategoryService, TotalExpensesPerCategoryService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ISplitwiseExpensesService, SplitewiseExpenseService>();
builder.Services.AddTransient<IRecalculatedExpenseService, RecalculatedExpenseService>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IExpenseMapperFactory, ExpenseMapperFactory>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
    {
        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    });

string secret = builder.Configuration.GetSection("Secrets")["JWT_SECRET"];
string issuer = builder.Configuration.GetSection("Secrets")["JWT_ISSUER"];

builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; ;
    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
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



app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();