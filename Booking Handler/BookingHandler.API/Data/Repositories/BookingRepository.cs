using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MongoDB.Driver;

using BookingHandler.Models;

namespace BookingHandler.Data.Repositories
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAll();
        Task<IEnumerable<Booking>> GetAllByStartDate();
        Task<Booking> Get(string id);

        Task Create(Booking booking);
        Task<bool> Update(Booking booking);
        Task<bool> Delete(string id);
    }

    /// <summary>
    /// Implementation of the repository-pattern as described here:
    /// <link>https://learn.microsoft.com/en-us/previous-versions/msp-n-p/ff649690(v=pandp.10)?redirectedfrom=MSDN</link>
    /// 
    /// Part of this article:
    /// <link>https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application</link>
    /// </summary>
    public class BookingRepository : IBookingRepository
    {
        private readonly ILogger<BookingRepository> _logger;
        private readonly IBookingContext _context;

        public BookingRepository(ILogger<BookingRepository> logger, IBookingContext context)
        {
            _logger = logger;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<IEnumerable<Booking>> GetAll()
        {
            return await _context
                .BookingCollection
                .Find(b => true)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllByStartDate()
        {
            return await _context
                .BookingCollection
                .Find(b => true)
                .SortBy(b => b.StartDate)
                .ToListAsync();
        }

        public async Task<Booking> Get(string id)
        {
            return await _context
                .BookingCollection
                .Find(b => b.BookingId == id)
                .FirstOrDefaultAsync();
        }

        public async Task Create(Booking booking)
        {
            await _context
                .BookingCollection
                .InsertOneAsync(booking);
        }

        public async Task<bool> Update(Booking booking)
        {
            var result = await _context
                .BookingCollection
                .ReplaceOneAsync(filter: b => b.BookingId == booking.BookingId, replacement: booking);

            return result.IsAcknowledged
                && result.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            var result = await _context
                .BookingCollection
                .DeleteOneAsync(b => b.BookingId == id);

            return result.IsAcknowledged
                && result.DeletedCount > 0;
        }
    }
}