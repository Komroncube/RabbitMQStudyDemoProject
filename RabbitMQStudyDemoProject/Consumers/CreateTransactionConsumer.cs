using MassTransit;
using RabbitMQStudyDemoProject.Models;

namespace RabbitMQStudyDemoProject.Consumers;

public class CreateTransactionConsumer : IConsumer<CreateTransaction>
{
    private readonly ILogger<CreateTransactionConsumer> _logger;

    public CreateTransactionConsumer(ILogger<CreateTransactionConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<CreateTransaction> context)
    {
        _logger.LogInformation($"Consume data from createTransactionQueue {context.Message.CardNumber}");
        return Task.CompletedTask;
    }
}
