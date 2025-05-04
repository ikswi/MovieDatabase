using MovieWebApplication.Data;
using MovieWebApplication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Упрощенная регистрация аутентификации
builder.Services.AddAuthentication("Basic")
    .AddScheme<CustomAuthOptions, CustomAuthHandler>("Basic", _ => { });

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();