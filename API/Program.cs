using API.Data;
using API.Repositories;
using API.Services;
using API.Events;
using Serilog;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog para logging
builder.Host.UseSerilog((context, config) =>
    config
        .MinimumLevel.Information()
        .WriteTo.Console()
        .WriteTo.File("logs/api-.txt", rollingInterval: RollingInterval.Day)
);

// Registrar o DbContext com SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString)
);

// Registrar Repositories
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

// Registrar Services
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<PedidoService>();

// Registrar Event Publisher
builder.Services.AddSingleton<EventPublisher>();

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
