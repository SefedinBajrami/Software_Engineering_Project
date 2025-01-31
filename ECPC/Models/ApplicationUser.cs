using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECPC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ProfileImageUrl { get; set; } = "/Uploads/default-profile.png"; // Default value

    }
}
