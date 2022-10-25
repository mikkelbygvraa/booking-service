using Microsoft.AspNetCore.Mvc;

using BookingProducer.Models;
using BookingProducer.Services;

namespace BookingProducer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly ILogger<BookingController> _logger;

        private static long _bookingId = 1;

        public BookingController(IMessageService messageService, ILogger<BookingController> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        // -----------------------------------------------------------------

        // GET: api/<BookingController>/version
        [HttpGet("version")] 
        public IEnumerable<string> Get() 
        { 
            var properties = new List<string>(); 
            var assembly = typeof(Program).Assembly; 
            foreach (var attribute in assembly.GetCustomAttributesData()) 
            { 
                properties.Add($"{attribute.AttributeType.Name} - {attribute.ToString()}"); 
            } 
            return properties; 
        }

        // POST api/<BookingController>
        [HttpPost]
        public void Post(string customerName, DateTime startDate, string startAddress, string endAddress)
        {
            var booking = new Booking()
            {
                BookingId = _bookingId++,
                CustomerName = customerName,
                StartDate = startDate,
                StartAddress = startAddress,
                EndAddress = endAddress,
                Timestamp = DateTime.Now 
            };

            _messageService.Enqueue(booking);
        }

        // -----------------------------------------------------------------
    }
}
