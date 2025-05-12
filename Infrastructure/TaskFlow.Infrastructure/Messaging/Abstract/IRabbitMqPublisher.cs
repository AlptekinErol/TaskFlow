namespace TaskFlow.Infrastructure.Messaging.Abstract;

public interface IRabbitMqPublisher
{
    Task PublishAsync<T>(T message, string queueName);
}
