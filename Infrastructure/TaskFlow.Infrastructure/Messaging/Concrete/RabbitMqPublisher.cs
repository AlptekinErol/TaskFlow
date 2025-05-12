using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using TaskFlow.Infrastructure.Messaging.Abstract;

namespace TaskFlow.Infrastructure.Messaging.Concrete;

public class RabbitMqPublisher : IRabbitMqPublisher
{
    private readonly IConnection connection;
    private readonly IModel channel;

    public RabbitMqPublisher()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        this.connection = factory.CreateConnection();
        this.channel = this.connection.CreateModel();
    }

    public Task PublishAsync<T>(T message, string queueName)
    {
        this.channel.QueueDeclare(queue: queueName,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        this.channel.BasicPublish(exchange: "",
                              routingKey: queueName,
                              basicProperties: null,
                              body: body);

        return Task.CompletedTask;
    }
}
