using System;
using System.Threading.Channels;
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
        private readonly IModel _channel;

        private static readonly string hostName = "localhost";
        private static readonly string queueName = "booking";

        public MessageService(IModel channel)
        {
            _channel = channel;

            var factory = new ConnectionFactory
            {
                HostName = hostName,
            };

            _connection = factory.CreateConnection();

            // Opretter en channel til at sende beskeder gennem
            channel = _connection.CreateModel();
            {
                // Opretter en kø
                channel.QueueDeclare(queue: queueName,
                                      durable: false,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);
            }

        }

        public void Enqueue(Booking booking)
        {
            // Serialisering af Booking-objekt
            var body = JsonSerializer.SerializeToUtf8Bytes(booking);

            // Sender 'body'-datastrømmen ind i køen
            _channel.BasicPublish(exchange: "",
                                  routingKey: queueName,
                                  basicProperties: null,
                                  body: body);

            // Udskriver til konsolen, at vi har sendt en booking
            Console.WriteLine(" [x] Published booking: {0}", booking.BookingId);
        }
    }
}
