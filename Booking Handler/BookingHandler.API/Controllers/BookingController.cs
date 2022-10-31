using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

using BookingHandler.Data.Repositories;
using BookingHandler.Models;

namespace BookingHandler.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingRepository _repository;

        public BookingController(ILogger<BookingController> logger, IBookingRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        // GET Booking
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Booking>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            try
            {
                var bookings = await _repository.GetAll();
                return Ok(bookings);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving booking data");
            }
        }


        // GET Booking/startdate
        [Route("[action]", Name = "GetBookingsByStartDate")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Booking>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookingsByStartDate()
        {
            try
            {
                var bookings = await _repository.GetAllByStartDate();
                return Ok(bookings);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving booking data");
            }
        }


        // GET Booking/5
        [HttpGet("{id}", Name = "GetBooking")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Booking), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Booking>> GetBooking(string id)
        {
            try
            {
                var booking = await _repository.Get(id);

                if (booking == null)
                {
                    _logger.LogError($"Booking with id {id} not found.");
                    return NotFound();
                }

                return Ok(booking);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating booking data");
            }
        }

    }
}
