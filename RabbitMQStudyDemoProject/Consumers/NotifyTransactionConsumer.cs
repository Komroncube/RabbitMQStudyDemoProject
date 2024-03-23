using MassTransit;
using RabbitMQStudyDemoProject.Models;

namespace RabbitMQStudyDemoProject.Consumers;

public class NotifyTransactionConsumer : IConsumer<NotifyTransaction>
{
    private readonly ILogger<NotifyTransactionConsumer> _logger;

    public NotifyTransactionConsumer(ILogger<NotifyTransactionConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<NotifyTransaction> context)
    {
        _logger.LogInformation($"Consume notify about {context.Message.CardNumber}");

        //todo: do something with data
        return Task.CompletedTask;
    }
}
