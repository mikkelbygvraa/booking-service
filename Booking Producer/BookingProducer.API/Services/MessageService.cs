using System.Text;
using System.Text.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using BookingProducer.Models;

namespace BookingProducer.Services
{
    public interface IMessageService
    {
        void Enqueue(Booking booking);
    }

    public class MessageService : IMessageService
    {
        private readonly IConnection _connection;

        private static readonly string queueName = "booking";

        public MessageService(IConfiguration configuration)
        {
            var mqhostname = configuration["BookingBrokerHost"];

            // Hvis mphostname er tom, så falder vi tilbage på localhost. Dette er "dårlig" fejlhåndtering, og er den hurtige løsning.
            if (string.IsNullOrEmpty(mqhostname))
            {
                mqhostname = "localhost";
            }

            var factory = new ConnectionFactory { HostName = mqhostname };
            _connection = factory.CreateConnection();
        }

        public void Enqueue(Booking booking)
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

                // Serialisering af Booking-objekt
                var body = JsonSerializer.SerializeToUtf8Bytes(booking);

                // Sender 'body'-datastrømmen ind i køen
                channel.BasicPublish(exchange: "",
                                        routingKey: queueName,
                                        basicProperties: null,
                                        body: body);

                // Udskriver til konsolen, at vi har sendt en booking
                Console.WriteLine(" [x] Published booking: {0}", booking.BookingId);
            }
        }
    }
}
