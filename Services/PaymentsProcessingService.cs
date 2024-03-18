
namespace FazUmPix.Services;

using System.Text;
using System.Text.Json;
using FazUmPix.DTOs;
using FazUmPix.Models;
using RabbitMQ.Client;

public class PaymentsProcessingService
{
    public PaymentsProcessingService()
    {
        _factory = new ConnectionFactory() { HostName = "localhost", UserName = "rabbit", Password = "mq" };
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(
            queue: "payments",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

    }

    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public void Process(ProcessPaymentDTO dto)
    {
        string message = JsonSerializer.Serialize(dto);

        _channel.BasicPublish(
            exchange: String.Empty,
            routingKey: "payments",
            basicProperties: null,
            body: Encoding.UTF8.GetBytes(message)
        );

        Console.WriteLine(" [x] Sent message to consumer!");
    }

}