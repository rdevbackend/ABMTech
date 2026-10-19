using Microsoft.EntityFrameworkCore;
using ABMTech.Api.Data;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Configura o caminho absoluto exato para o SQLite no diretório da aplicação
var folder = AppDomain.CurrentDomain.BaseDirectory;
var dbPath = Path.Join(folder, "abmtech.db");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? $"Data Source={dbPath}"));

builder.Services.AddControllers();

// Configuração ampla de CORS
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

// Inicialização e criação forçada da base de dados e tabela no arranque
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    
    // Garante a criação via EF Core
    context.Database.EnsureCreated();

    // Garante a criação via SQL bruto para nunca dar erro de tabela inexistente
    context.Database.ExecuteSqlRaw(@"
        CREATE TABLE IF NOT EXISTS ""OrdensServico"" (
            ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_OrdensServico"" PRIMARY KEY AUTOINCREMENT,
            ""CodigoRastreio"" TEXT NULL,
            ""NomeCliente"" TEXT NULL,
            ""WhatsappCliente"" TEXT NULL,
            ""ModeloAparelho"" TEXT NULL,
            ""DefeitoRelatado"" TEXT NULL,
            ""PrecoPecaFornecedor"" TEXT NULL,
            ""MargemLucroPecaPorcentagem"" TEXT NULL,
            ""ValorMaoDeObra"" TEXT NULL,
            ""ValorTotalCalculado"" TEXT NULL,
            ""Status"" INTEGER NOT NULL,
            ""DataCriacao"" TEXT NOT NULL
        );
    ");
}

app.UseCors("AllowAll");
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();
app.MapControllers();

app.Run();