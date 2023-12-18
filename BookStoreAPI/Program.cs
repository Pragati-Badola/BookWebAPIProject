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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using BookStoreAPI.Filters;


var builder = WebApplication.CreateBuilder(args);

//var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
//var jwtAudience = builder.Configuration.GetSection("Jwt:Audience").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(
        options =>
        {
        options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(jwtKey))
            };
        });


builder.Services.AddSingleton<IJwtAuthenticationManager>(new JwtAuthenticationManager(jwtKey));


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
//builder.Services.AddControllers(options =>
//{
    //options.Filters.Add(new MySampleActionFilterAttribute("Global"));
    //options.Filters.Add(new MySampleResourceFilterAttribute("Global"));
    //options.Filters.AddService<MySampleResultFilterAttribute>();
//});
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        }
    );
    option.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        }
    );
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddSingleton<IActionResultExecutor<ObjectResult>, Response>();
builder.Services.AddScoped<IValidator<Book>, BookValidator>();
builder.Services.AddTransient<IGuidService, GuidService>();
//builder.Services.AddSingleton<MySampleResultFilterAttribute>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalErrorHandler>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
