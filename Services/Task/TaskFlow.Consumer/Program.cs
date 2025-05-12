using TaskFlow.Consumer;
using TaskFlow.Consumer.Consumers;
using TaskFlow.Infrastructure.Redis.Abstract;
using TaskFlow.Infrastructure.Redis.Concrete;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<TaskCreatedConsumer>(); 
        services.AddHostedService<Worker>();
        services.AddSingleton<IRedisCacheService, RedisCacheService>();
    })
    .Build();

host.Run();
