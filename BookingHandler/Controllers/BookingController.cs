using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using BookingHandler.Models;
using BookingHandler.Repositories;

namespace BookingHandler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingRepository _repository;

        public BookingController(ILogger<BookingController> logger, IBookingRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        // GET api/<BookingController>
        [HttpGet]
        public ActionResult GetBookings()
        {
            try
            {
                return Ok(_repository.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving booking data");
            }
        }

        // GET api/<BookingController/startdate>
        [HttpGet("startdate")]
        public ActionResult GetBookingsByStartDate()
        {
            try
            {
                return Ok(_repository.GetAllByStartDate());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving booking data");
            }
        }

        // GET api/<BookingController>/5
        [HttpGet("{id}")]
        public ActionResult GetBookingById(BookingDTO booking)
        {
            try
            {
                return Ok(_repository.Update(booking));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating booking data");
            }
        }

    }
}
