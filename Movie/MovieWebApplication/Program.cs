using MovieWebApplication.Data;
using MovieWebApplication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication("Basic")
    .AddScheme<CustomAuthOptions, CustomAuthHandler>("Basic", _ => { });

builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");  // Базовый маршрут
app.MapGet("/test", () => "Test works!");  // Тестовый маршрут

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();