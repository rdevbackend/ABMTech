using Microsoft.EntityFrameworkCore;
using ABMTech.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuração para o Render ler a porta dinâmica
var port = Environment.GetEnvironmentVariable("PORT") ?? "5176";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}
// Adicione isto logo antes de app.Run();
app.MapGet("/", () => Results.Ok(new { status = "API ABMTech Online", version = "1.0" }));
app.UseDefaultFiles(); // Procura automaticamente pelo index.html
app.UseStaticFiles();  // Permite servir HTML, CSS, imagens e JS

app.Run();

