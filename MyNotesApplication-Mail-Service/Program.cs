using MyNotesApplication_Mail_Service.Services.Interfaces;
using MyNotesApplication_Mail_Service.Services.RabbitMQBroker;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMessageBrokerPersistentConnection, RabbitMQPersistedConnection>();
builder.Services.AddScoped<RabbitMQListener>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
