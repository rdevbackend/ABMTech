using Microsoft.EntityFrameworkCore;
using ABMTech.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Registo do DbContext com SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// 2. Configuração do CORS
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

// 3. Força a criação do banco de dados e das tabelas no arranque do servidor
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        
        // Garante a criação da diretoria do ficheiro .db se não existir
        var dbPath = context.Database.GetDbConnection().DataSource;
        if (!string.IsNullOrEmpty(dbPath))
        {
            var dbDir = Path.GetDirectoryName(dbPath);
            if (!string.IsNullOrEmpty(dbDir) && !Directory.Exists(dbDir))
            {
                Directory.CreateDirectory(dbDir);
            }
        }
        
        // Cria a base de dados e a tabela 'OrdensServico' imediatamente
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao criar base de dados: {ex.Message}");
    }
}

app.UseCors("AllowAll");

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();
app.MapControllers();

app.Run();