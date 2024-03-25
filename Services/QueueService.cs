
namespace FazUmPix.Services;

using System.Text;
using System.Text.Json;
using FazUmPix.DTOs;
using FazUmPix.Models;
using RabbitMQ.Client;

public class QueueService
{
    public QueueService()
    {
        _factory = new ConnectionFactory() { HostName = "localhost", UserName = "rabbit", Password = "mq" };
        _connection = _factory.CreateConnection();

        _paymentsChannel = _connection.CreateModel();
        _concilliationsChannel = _connection.CreateModel();

        _paymentsChannel.QueueDeclare(
            queue: "payments",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        _concilliationsChannel.QueueDeclare(
            queue: "conciliations",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

    }

    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private readonly IModel _paymentsChannel;
    private readonly IModel _concilliationsChannel;

    public void PublishPayment(ProcessPaymentDTO dto)
    {
        string message = JsonSerializer.Serialize(dto);

        _paymentsChannel.BasicPublish(
            exchange: String.Empty,
            routingKey: "payments",
            basicProperties: null,
            body: Encoding.UTF8.GetBytes(message)
        );

        Console.WriteLine("[x] Payment published to consumer!");
    }

    public void PublishConcilliation(QueueConcilliationInputDTO dto)
    {
        string message = JsonSerializer.Serialize(dto);

        _paymentsChannel.BasicPublish(
            exchange: String.Empty,
            routingKey: "concilliations",
            basicProperties: null,
            body: Encoding.UTF8.GetBytes(message)
        );

        Console.WriteLine("[x] Concilliation published to consumer!");
    }

}