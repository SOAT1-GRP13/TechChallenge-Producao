using API.Data;
using API.Setup;
using Infra.Pedidos;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Application.Pedidos.AutoMapper;
using Swashbuckle.AspNetCore.Filters;
using Domain.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Configuration.AddAmazonSecretsManager("us-west-2", "soat1-grp13");
builder.Services.Configure<Secrets>(builder.Configuration);

var connectionString = builder.Configuration.GetSection("ConnectionString").Value;

string secret = builder.Configuration.GetSection("ClientSecret").Value;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));

builder.Services.AddDbContext<PedidosContext>(options =>
        options.UseNpgsql(connectionString));

builder.Services.AddAuthenticationJWT(secret);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenConfig();

builder.Services.AddAutoMapper(typeof(PedidosMappingProfile));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseReDoc(c =>
{
    c.DocumentTitle = "REDOC API Documentation";
    c.SpecUrl = "/swagger/v1/swagger.json";
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await using var scope = app.Services.CreateAsyncScope();
using var dbApplication = scope.ServiceProvider.GetService<ApplicationDbContext>();
using var dbPedidos = scope.ServiceProvider.GetService<PedidosContext>();

await dbApplication!.Database.MigrateAsync();
await dbPedidos!.Database.MigrateAsync();

app.Run();
