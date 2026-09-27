using Microsoft.EntityFrameworkCore;
using ABMTech.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuração da base de dados com SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=abmtech.db"));

// 2. Adicionar suporte a Controllers e OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// 3. Adicionar política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("PermitirTudo");
app.UseAuthorization();
app.MapControllers();

app.Run();