using MyNotesApplication_Mail_Service.Services;
using MyNotesApplication_Mail_Service.Services.Interfaces;
using MyNotesApplication_Mail_Service.Services.RabbitMQBroker;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

IConfiguration _configuration = builder.Configuration;

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddControllers();

builder.Services.AddScoped<IMessageBrokerPersistentConnection, RabbitMQPersistentConnection>();
builder.Services.AddScoped<RabbitMQListener>();
builder.Services.AddScoped<EmailSender>();

var app = builder.Build();

app.UseSession();
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();
