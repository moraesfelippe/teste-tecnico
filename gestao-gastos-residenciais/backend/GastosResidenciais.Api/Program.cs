using Microsoft.EntityFrameworkCore;
using GastosResidenciais.Api.Data;
using GastosResidenciais.Api.Middleware;
using GastosResidenciais.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// --- Controllers e serialização JSON ---
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Serializa enums como texto (ex.: "Despesa" em vez de 1), o que
        // facilita tanto a leitura no Swagger quanto o consumo pelo front-end.
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// --- Documentação/teste manual da API (Swagger) ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Banco de dados ---
// SQLite foi escolhido por gravar tudo em um único arquivo (gastos.db),
// dispensando a instalação de um servidor de banco separado, e por garantir
// que os dados persistam entre execuções da aplicação (requisito do desafio).
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=gastos.db";
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

// --- Injeção de dependência dos serviços de domínio ---
builder.Services.AddScoped<IPessoaService, PessoaService>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();
builder.Services.AddScoped<ITotaisService, TotaisService>();

// --- CORS para o front-end React (Vite roda por padrão em http://localhost:5173) ---
const string frontEndPolicy = "FrontEnd";
builder.Services.AddCors(options =>
{
    options.AddPolicy(frontEndPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Garante que o banco e as tabelas existam ao subir a aplicação. Como o
// desafio não exige migrations formais, EnsureCreated() cria o schema a
// partir do modelo do EF Core já na primeira execução.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Trata exceções de domínio (regra de negócio / não encontrado) de forma
// centralizada, antes de chegar aos controllers.
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors(frontEndPolicy);
app.UseAuthorization();
app.MapControllers();

app.Run();
