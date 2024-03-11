using API.Setup;
using Infra.RabbitMQ;
using System.Reflection;
using Domain.Configuration;
using Infra.RabbitMQ.Consumers;
using Application.Pedidos.AutoMapper;
using Swashbuckle.AspNetCore.Filters;
using Domain.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Configurando LoggerFactory e criando uma inst�ncia de ILogger
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
    builder.AddDebug();
});
var logger = loggerFactory.CreateLogger<Program>();

// Add services to the container.
builder.Services.AddControllers();

string connectionString = "";
string secret = "";

if (builder.Environment.IsProduction())
{
    logger.LogInformation("Ambiente de Producao detectado.");

    builder.Configuration.AddAmazonSecretsManager("us-west-2", "producao-secret");

    connectionString = builder.Configuration.GetSection("ConnectionString").Value ?? string.Empty;

    secret = builder.Configuration.GetSection("ClientSecret").Value ?? string.Empty;
}
else
{
    logger.LogInformation("Ambiente de Desenvolvimento/Local detectado.");

    connectionString = builder.Configuration.GetSection("ConnectionString").Value ?? string.Empty;

    secret = builder.Configuration.GetSection("ClientSecret").Value ?? string.Empty;
}

builder.Services.Configure<Secrets>(builder.Configuration);

builder.Services.AddAuthenticationJWT(secret);
builder.Services.AddMemoryCache();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenConfig();

builder.Services.AddAutoMapper(typeof(PedidosMappingProfile));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddSingleton<RabbitMQModelFactory>();
builder.Services.AddSingleton(serviceProvider =>
{
    var modelFactory = serviceProvider.GetRequiredService<RabbitMQModelFactory>();
    return modelFactory.CreateModel();
});
builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();
builder.Services.AddHostedService<PedidoPagoSubscriber>();
builder.Services.AddHostedService<PedidoRecusadoSubscriber>();

builder.Services.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UsePathBase(new PathString("/producao"));
app.UseRouting();

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

app.Run();