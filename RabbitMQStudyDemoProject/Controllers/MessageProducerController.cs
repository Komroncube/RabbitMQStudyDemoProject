using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace RabbitMQStudyDemoProject.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MessageProducerController : ControllerBase
{

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
}
