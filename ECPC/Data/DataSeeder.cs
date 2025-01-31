using ECPC.Data;
using ECPC.Models;
using Microsoft.AspNetCore.Identity;

public class DataSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public DataSeeder(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public void SeedUserProfiles()
    {
        var users = _userManager.Users.ToList();
        foreach (var user in users)
        {
            if (!_context.UserProfiles.Any(up => up.UserId == user.Id))
            {
                var userProfile = new UserProfile
                {
                    UserId = user.Id,
                    Bio = "This is your bio.",
                    Facebook = string.Empty,
                    Instagram = string.Empty,
                    Twitter = string.Empty,
                    EventsAttended = 0,
                    EventsCreated = 0,
                    AttendancePercentage = 0,
                    ProfileImageUrl = "/path/to/default/image.jpg",
                    Following = 0,
                    Followers = 0,
                    Connections = 0
                };

                _context.UserProfiles.Add(userProfile);
            }
        }
        _context.SaveChanges();
    }
}
