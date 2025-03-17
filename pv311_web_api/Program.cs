using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using pv311_web_api.BLL;
using pv311_web_api.BLL.DTOs.Account;
using pv311_web_api.BLL.Services.Account;
using pv311_web_api.BLL.Services.Cars;
using pv311_web_api.BLL.Services.Email;
using pv311_web_api.BLL.Services.Image;
using pv311_web_api.BLL.Services.Manufactures;
using pv311_web_api.BLL.Services.Role;
using pv311_web_api.BLL.Services.User;
using pv311_web_api.DAL;
using pv311_web_api.DAL.Entities;
using pv311_web_api.DAL.Repositories.Cars;
using pv311_web_api.DAL.Repositories.Manufactures;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IManufactureService, ManufactureService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ICarService, CarService>();

// Add repositories
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IManufactureRepository, ManufactureRepository>();

builder.Services.AddControllers();

// Add fluent validation
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

// Add automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql("name=NpgSqlLocal");
});

// Add identity
builder.Services
    .AddIdentity<AppUser, AppRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("localhost3000", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("localhost3000");

// Static files
var rootPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
var imagesPath = Path.Combine(rootPath, Settings.ImagesPath);
Settings.FilesRootPath = builder.Environment.ContentRootPath;

if (!Directory.Exists(rootPath))
{
    Directory.CreateDirectory(rootPath);
}

if (!Directory.Exists(imagesPath))
{
    Directory.CreateDirectory(imagesPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesPath),
    RequestPath = "/images"
});

app.UseAuthorization();

app.MapControllers();

app.Run();