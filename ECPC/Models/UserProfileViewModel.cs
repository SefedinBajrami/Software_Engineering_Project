namespace ECPC.Models
{

        public class UserProfileViewModel
        {
            public string UserName { get; set; } = string.Empty; // Default to an empty string
            public string ProfileUserId { get; set; } = string.Empty; // Default to an empty string
            public string? Bio { get; set; } // Nullable
            public string ProfileImageUrl { get; set; } = "wwwroot/Uploads/Screenshot 2025-01-16 163419.png"; // Default image path
            public int? Following { get; set; } // Nullable
            public int? Followers { get; set; } // Nullable
            public int? Connections { get; set; } // Nullable
            public string? Facebook { get; set; } // Nullable
            public string? Instagram { get; set; } // Nullable
            public string? Twitter { get; set; } // Nullable
            public int? EventsAttended { get; set; } // Nullable
            public int? EventsCreated { get; set; } // Nullable
            public float? AttendancePercentage { get; set; } // Nullable
            public bool IsFollowing { get; set; } // Indicates if the current user is following this profile
        }


}
