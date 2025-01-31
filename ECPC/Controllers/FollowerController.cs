using ECPC.Data;
using ECPC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECPC.Controllers
{
    public class FollowController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FollowController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Follow(string userId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Ensure both IDs are not null
            if (currentUserId == null || userId == null)
            {
                return BadRequest("Invalid user information.");
            }

            if (!_context.Followers.Any(f => f.FollowerId == currentUserId && f.FollowingId == userId))
            {
                _context.Followers.Add(new Follower { FollowerId = currentUserId, FollowingId = userId });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("UserProfile", new { id = userId });
        }


        [HttpPost]
        public async Task<IActionResult> Unfollow(string userId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var follower = _context.Followers
                .FirstOrDefault(f => f.FollowerId == currentUserId && f.FollowingId == userId);

            if (follower != null)
            {
                _context.Followers.Remove(follower);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("UserProfile", new { id = userId });
        }
    }
}
