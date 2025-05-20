using Microsoft.EntityFrameworkCore;
using PruebaBackend.Data;

var builder = WebApplication.CreateBuilder(args);

// Configura el DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔴 Comentado porque puede causar error en Docker si no hay HTTPS
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

// 🔥 Docker acepte peticiones externas al contenedor
app.Urls.Add("http://0.0.0.0:80");

app.Run();
