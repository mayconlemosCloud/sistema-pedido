using API.Data;
using API.Repositories;
using API.Services;
using API.Events;
using API.Mappings;
using API.Validators.Produto;
using API.Validators.Pedido;
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

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
builder.Services.AddSingleton<IEventPublisher, EventPublisher>();

// Registrar AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container.
builder.Services.AddValidatorsFromAssemblyContaining<CreateProdutoRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Auto-aplicar migrations ao iniciar
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
