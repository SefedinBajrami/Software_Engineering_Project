using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ECPC.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty; // Non-nullable Foreign key to AspNetUsers

        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }
        public int? EventsAttended { get; set; } = 0;
        public int? EventsCreated { get; set; } = 0;
        public string? Bio { get; set; }
        public float? AttendancePercentage { get; set; } = 0.0f;
        public string? ProfileImageUrl { get; set; } = @"C:\Users\hakan\C#2024\ECPC\ECPC\wwwroot\Uploads\Screenshot 2025-01-16 163419.png";
        public int? Following { get; set; } = 0;
        public int? Followers { get; set; } = 0;
        public int? Connections { get; set; } = 0;
  
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

    }
}
