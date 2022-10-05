using BookingHandler.Models;

namespace BookingHandler.Services
{
    public interface IBookingRepository : IDisposable
    {
        IEnumerable<BookingDTO> GetAll();
        IEnumerable<BookingDTO> GetAllByStartDate();
        void Create(BookingDTO booking);
        BookingDTO? Update(BookingDTO booking);
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
        //private readonly ILogger<BookingRepository> _logger;
        private List<BookingDTO> _bookings;

        public BookingRepository(/*ILogger<BookingRepository> logger*/)
        {
            //_logger = logger;
            _bookings = new List<BookingDTO>();
        }

        public IEnumerable<BookingDTO> GetAll()
        {
            return _bookings
                .ToList();
        }

        public IEnumerable<BookingDTO> GetAllByStartDate()
        {
            return _bookings
                .OrderBy(b => b.StartDate)
                .ToList();
        }

        public void Create(BookingDTO booking)
        {
            _bookings
                .Add(booking);
        }

        public BookingDTO? Update(BookingDTO booking)
        {
            var result = _bookings
                .FirstOrDefault(b => b.BookingId == booking.BookingId);

            if (result is not null)
            {
                result.CustomerName = booking.CustomerName;
                result.StartDate = booking.StartDate;
                result.StartAddress = booking.StartAddress;
                result.EndAddress = booking.EndAddress;

                return result;
            }
            return null;
        }

        #region IDisposable

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _bookings.Clear();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}