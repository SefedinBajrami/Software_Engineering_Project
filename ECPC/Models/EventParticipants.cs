namespace ECPC.Models
{
    public class EventParticipant
    {
        public int ID { get; set; }
        public int EventID { get; set; } // FK to EventDatabase
        public string? UserID { get; set; } // FK to AspNetUsers, nullable
        public bool Request { get; set; }
        public bool Confirm { get; set; }

        // Navigation properties
        public Event? Event { get; set; } // Nullable navigation property
    }
}
