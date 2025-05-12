using System.Text;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using TaskFlow.SharedResources;
using TaskFlow.Infrastructure.Redis.Abstract;
using StackExchange.Redis;

namespace TaskFlow.Consumer.Consumers;

public class TaskCreatedConsumer : BackgroundService
{
    private IConnection connection;
    private IModel channel;
    private readonly IRedisCacheService redisCacheService;

    public TaskCreatedConsumer(IRedisCacheService redisCacheService)
    {
        this.redisCacheService = redisCacheService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        this.connection = factory.CreateConnection();
        this.channel = this.connection.CreateModel();

        this.channel.QueueDeclare(queue: "task_created",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(this.channel);

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var taskEvent = JsonSerializer.Deserialize<TaskCreatedEvent>(json);

            Console.WriteLine($"[x] Received Task: {taskEvent?.Title} - {taskEvent?.Description}");

            if (taskEvent != null)
            {
                await this.redisCacheService.SetTaskAsync(taskEvent.TaskId.ToString(), json);
                Console.WriteLine($"[+] Task {taskEvent.TaskId} Redis'e yazıldı.");
            }
        };

        this.channel.BasicConsume(queue: "task_created",
                             autoAck: true,
                             consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        this.channel?.Close();
        this.connection?.Close();
        base.Dispose();
    }
}
