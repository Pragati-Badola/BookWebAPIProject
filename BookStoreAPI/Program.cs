using BookStoreAPI.Middleware;
using BookStoreAPI.Models;
using BookStoreAPI.Repository;
using BookStoreAPI.Shared;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers(config =>
//{

//}).AddNewtonsoftJson().AddFluentValidation(options =>
//{
//    // Validate child properties and root collection elements
//    options.ImplicitlyValidateChildProperties = true;
//    options.ImplicitlyValidateRootCollectionElements = true;
//    // Automatic registration of validators in assembly
//    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
//}); 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddSingleton<IActionResultExecutor<ObjectResult>, Response>();
builder.Services.AddScoped<IValidator<Book>, BookValidator>();
builder.Services.AddTransient<IGuidService, GuidService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalErrorHandler>();

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
