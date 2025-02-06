using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Identity;
using ECPC.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ECPC.Data;
using Microsoft.AspNetCore.Authentication;

namespace ECPC.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context; // Add the database context
        private readonly DataSeeder _dataSeeder;

        public AuthController(
              UserManager<ApplicationUser> userManager,
              SignInManager<ApplicationUser> signInManager,
              ApplicationDbContext context,
              DataSeeder dataSeeder) // Combine all dependencies into one constructor
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _dataSeeder = dataSeeder;
        }


        // Login Page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    return View();
                }

                var userProfile = await _context.UserProfiles
                    .FirstOrDefaultAsync(up => up.UserId == user.Id);

                if (userProfile != null)
                {
                    var claims = new List<Claim>
            {
                new Claim("ProfileImageUrl", userProfile.ProfileImageUrl ?? "/Uploads/default-profile.png")
            };

                    await _signInManager.SignInWithClaimsAsync(user, isPersistent: false, claims);
                }

                return RedirectToAction("UserProfile", new { id = user.Id });
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme); // Logs out the user
            return RedirectToAction("Login", "Auth"); // Redirect to login page
        }

        // Create User Page
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }





        [HttpPost]
        public async Task<IActionResult> CreateUser(string username, string email, string password)
        {
            var user = new ApplicationUser { UserName = username, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Create a default UserProfile entry for the new user
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
                    ProfileImageUrl = "C:\\Users\\hakan\\C#2024\\ECPC\\ECPC\\wwwroot\\Uploads\\Screenshot 2025-01-16 163419.png",
                    Following = 0,
                    Followers = 0,
                    Connections = 0
                };

                _context.UserProfiles.Add(userProfile);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }




        // User Profile
        public IActionResult UserProfile(string id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userProfile = _context.UserProfiles
                .Include(u => u.User)
                .FirstOrDefault(up => up.UserId == id);

            if (userProfile == null)
            {
                return NotFound("User profile not found.");
            }

            var isFollowing = _context.Followers
                .Any(f => f.FollowerId == currentUserId && f.FollowingId == id);

            var model = new UserProfileViewModel
            {
                UserName = userProfile.User?.UserName ?? "Unknown User",
                ProfileUserId = userProfile.UserId ?? string.Empty,
                Bio = userProfile.Bio ?? "No bio provided.",
                ProfileImageUrl = userProfile.ProfileImageUrl ?? "/Uploads/default-profile.png",
                Facebook = userProfile.Facebook ?? string.Empty,
                Instagram = userProfile.Instagram ?? string.Empty,
                Twitter = userProfile.Twitter ?? string.Empty,
                Followers = userProfile.Followers ?? 0,
                Following = userProfile.Following ?? 0,
                Connections = userProfile.Connections ?? 0,
                EventsAttended = userProfile.EventsAttended ?? 0,
                EventsCreated = userProfile.EventsCreated ?? 0,
                AttendancePercentage = userProfile.AttendancePercentage ?? 0f,
                IsFollowing = isFollowing
            };

            return View(model);
        }



        public IActionResult Connect()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return BadRequest("Current user is not authenticated.");
            }

            var users = _context.Users
                .Where(u => u.Id != currentUserId)
                .Join(
                    _context.UserProfiles,
                    user => user.Id,
                    profile => profile.UserId,
                    (user, profile) => new UserProfileViewModel
                    {
                        UserName = user.UserName ?? "Unknown User", // Default to "Unknown User" if null
                        ProfileUserId = user.Id,
                        Bio = profile.Bio ?? "No bio available.", // Default bio
                        ProfileImageUrl = profile.ProfileImageUrl ?? "C:\\Users\\hakan\\C#2024\\ECPC\\ECPC\\wwwroot\\Uploads\\Screenshot 2025-01-16 163419.png", // Default image
                        Following = profile.Following ?? 0, // Default to 0
                        Followers = profile.Followers ?? 0  // Default to 0
                    })
                .OrderBy(x => Guid.NewGuid())
                .Take(20)
                .ToList();

            return View(users);
        }


        public IActionResult OtherProfiles(string id)
        {
            var userProfile = _context.UserProfiles
                .Include(u => u.User)
                .FirstOrDefault(up => up.UserId == id);

            if (userProfile == null)
            {
                return NotFound("User profile not found.");
            }

            var model = new UserProfileViewModel
            {
                UserName = userProfile.User?.UserName ?? "Unknown User", // Handle null UserName
                ProfileUserId = userProfile.UserId ?? string.Empty, // Handle null UserId
                Bio = userProfile.Bio ?? "No bio provided.", // Handle null Bio
                ProfileImageUrl = userProfile.ProfileImageUrl ?? "C:\\Users\\hakan\\C#2024\\ECPC\\ECPC\\wwwroot\\Uploads\\Screenshot 2025-01-16 163419.png", // Handle null ProfileImageUrl
                Facebook = userProfile.Facebook ?? string.Empty, // Handle null Facebook
                Instagram = userProfile.Instagram ?? string.Empty, // Handle null Instagram
                Twitter = userProfile.Twitter ?? string.Empty, // Handle null Twitter
                Followers = userProfile.Followers ?? 0, // Handle null Followers
                Following = userProfile.Following ?? 0, // Handle null Following
                Connections = userProfile.Connections ?? 0, // Handle null Connections
                EventsAttended = userProfile.EventsAttended ?? 0, // Handle null EventsAttended
                EventsCreated = userProfile.EventsCreated ?? 0, // Handle null EventsCreated
                AttendancePercentage = userProfile.AttendancePercentage ?? 0f // Handle null AttendancePercentage
            };

            return View(model);
        }




        public IActionResult RunSeed()
        {
            _dataSeeder.SeedUserProfiles(); // Call the seeding method from DataSeeder
            return Ok("User profiles seeded.");
        }
        [HttpGet("/Auth/EditProfile")]



        public IActionResult EditProfile()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userProfile = _context.UserProfiles
                .FirstOrDefault(up => up.UserId == currentUserId);

            if (userProfile == null)
            {
                return NotFound("User profile not found.");
            }

            return View(userProfile);
        }





        [HttpPost("/Auth/EditProfile")]
        public async Task<IActionResult> EditProfile(UserProfileViewModel model, IFormFile profilePicture)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userProfile = _context.UserProfiles.FirstOrDefault(up => up.UserId == currentUserId);


 

            if (string.IsNullOrEmpty(currentUserId)) // Ensure currentUserId is not null
            {
                return Unauthorized("User is not logged in.");
            }
            if (userProfile == null)
            {
                return NotFound("User profile not found.");
            }

            // Update bio and social links
            userProfile.Bio = model.Bio;
            userProfile.Facebook = model.Facebook ?? userProfile.Facebook; // Retain old value if null
            userProfile.Instagram = model.Instagram ?? userProfile.Instagram;
            userProfile.Twitter = model.Twitter ?? userProfile.Twitter;

            if (!string.IsNullOrEmpty(model.UserName)) // Check and update username
            {
                var user = await _userManager.FindByIdAsync(currentUserId);
                if (user != null)
                {
                    user.UserName = model.UserName;
                    await _userManager.UpdateAsync(user); // Update the username in AspNetUsers
                }
            }

            // Handle profile picture upload
            if (profilePicture != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(profilePicture.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ProfileImageUrl", "Only image files are allowed.");
                    return View(model);
                }

                var fileName = Path.GetFileName(profilePicture.FileName);
                var filePath = Path.Combine("wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }

                userProfile.ProfileImageUrl = $"/uploads/{fileName}";
            }

            _context.UserProfiles.Update(userProfile);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Profile updated successfully" });
        }



    }
}
