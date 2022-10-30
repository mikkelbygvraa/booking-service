using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingHandler.Models
{
    [Serializable, BsonIgnoreExtraElements]
    public class Booking
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string BookingId { get; set; }

        [BsonElement("customer_name")]
        public string? CustomerName { get; set; }

        [BsonElement("start_date"), BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? StartDate { get; set; }

        [BsonElement("start_address")]
        public string? StartAddress { get; set; }

        [BsonElement("end_address")]
        public string? EndAddress { get; set; }

        [BsonElement("time_stamp"), BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? Timestamp { get; set; }
    }
}