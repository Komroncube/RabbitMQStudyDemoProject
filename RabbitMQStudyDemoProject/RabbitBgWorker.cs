using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQStudyDemoProject;

public class RabbitBgWorker : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitBgWorker()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += Consume;
        _channel.BasicConsume("ControllerFirstQueue", false, consumer);

        return Task.CompletedTask;
    }

    private void Consume(object? sender, BasicDeliverEventArgs e)
    {
        var content = Encoding.UTF8.GetString(e.Body.ToArray());
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(content);
        _channel.BasicAck(e.DeliveryTag, false);
    }
}
