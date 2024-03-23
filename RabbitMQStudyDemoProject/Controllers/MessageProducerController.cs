using System.Text;
using System.Text.Json;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQStudyDemoProject.Models;

namespace RabbitMQStudyDemoProject.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MessageProducerController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MessageProducerController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost("send")]
    public IActionResult Send(string message)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "ControllerFirstQueue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            //var body = Encoding.UTF8.GetBytes(message);
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            channel.BasicPublish(exchange: "",
                routingKey:"ControllerFirstQueue",
                basicProperties:null,
                body:body);

        }   
        return Ok();
    }

    [HttpPost("transactions/notify")]
    public IActionResult SendNotify(NotifyTransaction transaction)
    {
        _publishEndpoint.Publish(transaction);
        return Ok();
    }
    [HttpPost("transactions/create")]
    public IActionResult SendTransaction(CreateTransaction transaction)
    {
        _publishEndpoint.Publish(transaction);
        return Ok();
    }
}
