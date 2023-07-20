# Web Notes Application Email service
Микросервис для отправки электронных писем для приложения по организации личных дел.

+ .NET 6.0 Framework <br />
+ MailKit
+ RabbitMQ

Для корректной сборки проекта в корневом каталоге проекта необходимо добавить следущие файлы файлы с указанным содержимым:
+ appsettings.json - конфигурация приложения


### appsettings.json
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "HostEmail": "Ваша почта здесь",
  "HostPassword": "Пароль от почты здесь",
  "BrokerMessageQueue": "my_notes_app_message_broker",
  "EmailServiceName": "EMAIL_SERVICE",
  "ConnectionsRetry": 5
}
```
