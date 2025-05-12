using TaskFlow.Infrastructure.Messaging.Abstract;
using TaskFlow.Infrastructure.Messaging.Concrete;
using TaskFlow.Infrastructure.Redis.Abstract;
using TaskFlow.Infrastructure.Redis.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Swagger ve controller'larý ekle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// RabbitMQ Publisher servis kaydý
builder.Services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>(); 
builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
