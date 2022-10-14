using MongoDB.Driver;

using BookingHandler.Models;
using BookingHandler.Data;

namespace BookingHandler.Repositories
{
    public interface IBookingRepository : IDisposable
    {
        Task<IEnumerable<BookingDTO>> GetAll();
        Task<IEnumerable<BookingDTO>> GetAllByStartDate();
        Task<BookingDTO> Get(long id);
        Task Create(BookingDTO booking);
        Task<bool> Update(BookingDTO booking);
        Task<bool> Delete(long id);
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

        private List<BookingDTO> _bookings;

        public BookingRepository(ILogger<BookingRepository> logger, IBookingContext context)
        {
            _logger = logger;
            _context = context ?? throw new ArgumentNullException(nameof(context));

            _bookings = new List<BookingDTO>();
        }


        public async Task<IEnumerable<BookingDTO>> GetAll()
        {
            return await _context
                .Bookings
                .Find(b => true)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookingDTO>> GetAllByStartDate()
        {
            return await _context
                .Bookings
                .Find(b => true)
                .SortBy(b => b.StartDate)
                .ToListAsync();
        }

        public async Task<BookingDTO> Get(long id)
        {
            return await _context
                .Bookings
                .Find(b => b.BookingId == id)
                .FirstOrDefaultAsync();
        }

        public async Task Create(BookingDTO booking)
        {
            await _context
                .Bookings
                .InsertOneAsync(booking);
        }

        public async Task<bool> Update(BookingDTO booking)
        {
            var result = await _context
                .Bookings
                .ReplaceOneAsync(filter: b => b.BookingId == booking.BookingId, replacement: booking);

            return result.IsAcknowledged
                && result.ModifiedCount > 0;
        }

        public async Task<bool> Delete(long id)
        {
            var result = await _context
                .Bookings
                .DeleteOneAsync(b => b.BookingId == id);

            return result.IsAcknowledged
                && result.DeletedCount > 0;
        }


        #region IDisposable

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _bookings.Clear();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}