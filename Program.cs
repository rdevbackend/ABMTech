using Microsoft.EntityFrameworkCore;
using ABMTech.Api.Data; // Ajusta este namespace caso o teu DbContext esteja noutra pasta

var builder = WebApplication.CreateBuilder(args);

// 1. Registo do Base de Dados / DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// 2. Configuração ampla de CORS para evitar qualquer bloqueio
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();
app.MapControllers();

app.Run();