namespace ECPC.Models
{
    public class Request
    {
        public int ID { get; set; }
        public string? UserID { get; set; } // FK to AspNetUsers, nullable
        public int EventID { get; set; } // FK to EventDatabase
        public bool IsApproved { get; set; }

        // Navigation properties
        public Event? Event { get; set; } // Nullable navigation property
    }
}
