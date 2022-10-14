using System.Text;
using System.Text.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using BookingHandler.Models;
using BookingHandler.Repositories;

namespace BookingHandler.Services;

/// <summary>
/// Consumes messages from the common message queue.
/// </summary>
public class BookingWorker : BackgroundService
{
    private readonly ILogger<BookingWorker> _logger;
    private readonly IConnection _connection;

    private readonly IBookingRepository _repository;

    private static readonly string queueName = "booking";
    private static int _nextId = 1;

    /// <summary>
    /// Create a worker service that receives a ILogger and 
    /// environment configuration instance.
    /// <link>https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-6.0</link>
    /// </summary>
    /// <param name="logger"></param>
    public BookingWorker(ILogger<BookingWorker> logger, IConfiguration configuration, IBookingRepository repository)
    {
        _logger = logger;
        _repository = repository;

        var mqhostname = configuration["BookingBrokerHost"];

        // Hvis mphostname er tom, så falder vi tilbage på localhost. Dette er "dårlig" fejlhåndtering, og er den hurtige løsning.
        if (string.IsNullOrEmpty(mqhostname))
        {
            mqhostname = "localhost";
        }

        var factory = new ConnectionFactory() { HostName = mqhostname };
        _connection = factory.CreateConnection();

        _logger.LogInformation($"Booking worker listening on host at {mqhostname}");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Opretter en channel til at sende beskeder gennem
        var channel = _connection.CreateModel();
        {
            // Opretter en kø
            channel.QueueDeclare(queue: queueName,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
        }

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            // Deserialisering af datastrømmen til et Booking-objekt
            var booking = JsonSerializer.Deserialize<BookingDTO>(message);

            if (booking is not null)
            {
                booking.BookingId = _nextId++;
                _logger.LogInformation("Processing booking {id} from {customer} ", booking.BookingId, booking.CustomerName);

                _repository.Create(booking);

            }
            else
            {
                _logger.LogWarning($"Could not deserialize message with body: {message}");
            }

        };

        channel.BasicConsume(queue: queueName,
                             autoAck: true,
                             consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}
