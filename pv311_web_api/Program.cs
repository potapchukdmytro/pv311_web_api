using Microsoft.EntityFrameworkCore;
using pv311_web_api.BLL.Services.Account;
using pv311_web_api.BLL.Services.Role;
using pv311_web_api.DAL;
using pv311_web_api.DAL.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddControllers();
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
    })
    .AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();