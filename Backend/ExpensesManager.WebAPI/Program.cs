﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using ExpensesManager.DB;
using ExpensesManger.Services.BuisnessLogic.Map;
using ExpensesManger.Services.Contracts;
using ExpensesManger.Services.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("Configurations/appsettings.json", optional: true, reloadOnChange: true);
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddTransient<IExpenseReaderService, ExpenseReadService> ();
builder.Services.AddTransient<IExpenseMapperService, ExpenseMapperService>();
builder.Services.AddTransient<ITotalExpensesPerCategoryService, TotalExpensesPerCategoryService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ISplitwiseExpensesService, SplitewiseExpenseService>();
builder.Services.AddTransient<IRecalculatedExpenseService, RecalculatedExpenseService>();
builder.Services.AddScoped<IExpenseMapperFactory, ExpenseMapperFactory>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => 
    {
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    });

var app = builder.Build();

if(app.Environment.IsDevelopment())
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

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();

