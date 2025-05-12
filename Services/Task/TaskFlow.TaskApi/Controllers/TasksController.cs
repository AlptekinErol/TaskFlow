using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using TaskFlow.Infrastructure.Messaging.Abstract;
using TaskFlow.Infrastructure.Redis.Abstract;
using TaskFlow.SharedResources;

namespace TaskFlow.TaskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly IRabbitMqPublisher publisher;
    private readonly IRedisCacheService redisCacheService;

    public TasksController(IRabbitMqPublisher publisher, IRedisCacheService redisCacheService)
    {
        this.publisher = publisher;
        this.redisCacheService = redisCacheService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaskCreatedEvent task)
    {
        task.TaskId = Guid.NewGuid();
        task.CreatedAt = DateTime.UtcNow;

        await this.publisher.PublishAsync(task, "task_created");

        return Ok(new { message = "Task sent to queue.", taskId = task.TaskId });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var json = await this.redisCacheService.GetTaskAsync(id);
        if (string.IsNullOrEmpty(json))
            return NotFound(new { message = $"Task {id} not found in Redis." });

        var task = JsonSerializer.Deserialize<TaskCreatedEvent>(json);
        return Ok(task);
    }
}
