namespace BookingProducer.Models
{
    public class Booking
    {
        public long BookingId { get; set; }
        public string? CustomerName { get; set; }
        public DateTime? StartDate { get; set; }
        public string? StartAddress { get; set; }
        public string? EndAddress { get; set; }
        public DateTime Timestamp { get; set; }
    }
}