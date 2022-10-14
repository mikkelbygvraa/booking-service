using MongoDB.Driver;
using BookingHandler.Models;

namespace BookingHandler.Data
{
    public interface IBookingContext
    {
        IMongoCollection<BookingDTO> Bookings { get; }
    }

    public class BookingContext : IBookingContext
    {
        public BookingContext(IConfiguration configuration) 
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var db = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        }

        public IMongoCollection<BookingDTO> Bookings { get; }
    }
}
