using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingHandler.Models
{
    public class BookingDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public long? BookingId { get; set; }
        public string? CustomerName { get; set; }
        public DateTime? StartDate { get; set; }
        public string? StartAddress { get; set; }
        public string? EndAddress { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}