using MongoDB.Driver;
using BookingHandler.Models;
using System;

namespace BookingHandler.Data
{
    public interface IBookingContext
    {
        IMongoCollection<Booking> BookingCollection { get; }
    }

    public class BookingContext : IBookingContext
    {
        public BookingContext(IConfiguration configuration)
        {
            var isRunningInContainer = configuration.GetValue<bool>("DOTNET_RUNNING_IN_CONTAINER");

            var connectionString = isRunningInContainer ?
                configuration.GetValue<string>("DatabaseSettings:ConnectionStrings:container") :
                configuration.GetValue<string>("DatabaseSettings:ConnectionStrings:local");

            Console.WriteLine(connectionString);

            var client = new MongoClient(connectionString);

            var db = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            BookingCollection = db.GetCollection<Booking>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));

            // SeedData(BookingCollection);
        }

        public IMongoCollection<Booking> BookingCollection { get; }

        public static void SeedData(IMongoCollection<Booking> bookingCollection)
        {
            bool existBooking = bookingCollection.Find(b => true).Any();
            
            if (existBooking is false)
            {
                bookingCollection.InsertManyAsync(new List<Booking>()
                {
                    new Booking()
                    {
                        BookingId = 1,
                        CustomerName = "Simon",
                        StartDate = DateTime.Today,
                        StartAddress = "Start St. 1",
                        EndAddress = "End St. 2",
                        Timestamp = DateTime.Now
                    },
                    new Booking()
                    {
                        BookingId = 2,
                        CustomerName = "Søren",
                        StartDate = DateTime.Today,
                        StartAddress = "Start St. 10",
                        EndAddress = "End St. 20",
                        Timestamp = DateTime.Now
                    }
                });
            }
        }
    }
}
