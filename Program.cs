var builder = WebApplication.CreateBuilder(args);

// 1. Configuração de Controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Configuração do CORS para permitir requisições externas
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 3. Configuração de porta dinâmica para o Render (lê a variável PORT do ambiente)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

// 4. Configuração do Swagger (Habilitado no Render e em desenvolvimento)
app.UseSwagger();
app.UseSwaggerUI();

// 5. Aplicação das políticas de segurança e CORS
app.UseCors("AllowAll");

// 6. Suporte a Arquivos Estáticos (Servir o Frontend da pasta wwwroot)
app.UseDefaultFiles(); // Procura automaticamente por index.html
app.UseStaticFiles();  // Permite servir ficheiros HTML, CSS, JS, imagens, etc.

app.UseAuthorization();

// 7. Mapeamento das rotas da API
app.MapControllers();

app.Run();