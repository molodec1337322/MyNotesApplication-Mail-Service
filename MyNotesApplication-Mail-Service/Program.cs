using MyNotesApplication_Mail_Service.Services;
using MyNotesApplication_Mail_Service.Services.Interfaces;
using MyNotesApplication_Mail_Service.Services.RabbitMQBroker;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

IConfiguration _configuration = builder.Configuration;

builder.Services.AddScoped<IMessageBrokerPersistentConnection, RabbitMQPersistentConnection>();
builder.Services.AddScoped<RabbitMQListener>();
builder.Services.AddScoped<EmailSender>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
