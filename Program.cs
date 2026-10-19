using Microsoft.EntityFrameworkCore;
using ABMTech.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

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

// Garante a criação da base de dados e da tabela de forma segura
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    
    var dbPath = context.Database.GetDbConnection().DataSource;
    if (!string.IsNullOrEmpty(dbPath))
    {
        var dbDir = Path.GetDirectoryName(dbPath);
        if (!string.IsNullOrEmpty(dbDir) && !Directory.Exists(dbDir))
        {
            Directory.CreateDirectory(dbDir);
        }
    }
    
    context.Database.EnsureCreated();

    // Garante que a tabela OrdensServico existe mesmo se o ficheiro SQLite já existir vazio
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