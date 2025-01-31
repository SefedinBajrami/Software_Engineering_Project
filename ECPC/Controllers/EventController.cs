using ECPC.Data;
using ECPC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Security.Claims;
using ECPC.Models.DTO;
using ECPC.ViewModels;




namespace ECPC.Controllers
{
    [Authorize] // Ensure only authenticated users can access
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .Select(e => new EventViewModel
                {
                    ID = e.ID,
                    Title = e.Title ?? string.Empty, // Handle possible null values
                    Description = e.Description ?? string.Empty,
                    Location = e.Location ?? string.Empty,
                    EventDate = e.EventDate,
                    Time = e.Time,
                    Category = e.Category ?? string.Empty,
                    Country = e.Country ?? string.Empty,
                    City = e.City,
                    District = e.District,
                    MaxParticipants = e.MaxParticipants,
                    IsPublic = e.IsPublic,
                    Image = e.Image ?? "/images/default-event.png",
                    Grouping = e.Grouping ?? string.Empty,
                    UserName = _context.Users
                        .Where(u => u.Id == e.CreatedBy)
                        .Select(u => u.UserName)
                        .FirstOrDefault() ?? "Unknown User",
                    ProfileImageUrl = _context.UserProfiles
                        .Where(p => p.UserId == e.CreatedBy)
                        .Select(p => p.ProfileImageUrl)
                        .FirstOrDefault() ?? "/images/default-user.png",
                    Comments = _context.Comments
                        .Where(c => c.EventID == e.ID)
                        .Select(c => new CommentViewModel
                        {
                            UserName = _context.Users
                                .Where(u => u.Id == c.UserID)
                                .Select(u => u.UserName)
                                .FirstOrDefault() ?? "Unknown User",
                            ProfileImageUrl = _context.UserProfiles
                                .Where(p => p.UserId == c.UserID)
                                .Select(p => p.ProfileImageUrl)
                                .FirstOrDefault() ?? "/images/default-user.png",
                            CommentText = c.CommentText,
                            CreatedAt = c.CreatedAt
                        })
                        .ToList()
                })
                .ToListAsync();

            return View(events);
        }




        // GET: Event/Create
        public IActionResult Create()
        {
            TempData["ErrorMessage"] = null; // Clear any previous error messages
            TempData["SuccessMessage"] = null; // Clear any previous success messages
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel eventViewModel)
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "User is not authenticated.";
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Failed to retrieve User ID.";
                return Unauthorized();
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                TempData["ErrorMessage"] = "The user does not exist.";
                return View(eventViewModel);
            }

            var eventItem = new Event
            {
                Title = eventViewModel.Title,
                Description = eventViewModel.Description,
                Location = eventViewModel.Location,
                EventDate = eventViewModel.EventDate,
                Time = eventViewModel.Time,
                CreatedBy = userId,
                Country = eventViewModel.Country,
                City = eventViewModel.City,
                District = eventViewModel.District,
                Category = eventViewModel.Category,
                MaxParticipants = eventViewModel.MaxParticipants,
                IsPublic = eventViewModel.IsPublic,
                Image = eventViewModel.Image ?? "/images/default-event.png",
                Grouping = eventViewModel.Grouping
            };

            ModelState.Remove("CreatedBy");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Events.Add(eventItem);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Event created successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An unexpected error occurred: {ex.Message}";
                    Console.WriteLine($"[DEBUG] Exception: {ex}");
                }
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"[DEBUG] ModelState Error: {error.ErrorMessage}");
                }
            }

            return View(eventViewModel);
        }




        // Other actions remain unchanged
        // GET: Event/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }



        // POST: Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event eventItem)
        {
            if (id != eventItem.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventItem);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Event updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(eventItem.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(eventItem);
        }



        // GET: Event/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var eventItem = await _context.Events.FirstOrDefaultAsync(m => m.ID == id);
            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        // GET: Event/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }



        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                TempData["ErrorMessage"] = "The event to delete was not found.";
                return RedirectToAction(nameof(Index));
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Event deleted successfully!";
            return RedirectToAction(nameof(Index));
        }


        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.ID == id);
        }


        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] AddCommentRequest request)
        {
            try
            {
                Console.WriteLine($"[DEBUG] Event ID: {request.EventId}, Comment Text: {request.CommentText}");

                if (string.IsNullOrWhiteSpace(request.CommentText))
                {
                    return BadRequest("Comment cannot be empty.");
                }

                // Check if the event exists
                var eventExists = await _context.Events.AnyAsync(e => e.ID == request.EventId);
                if (!eventExists)
                {
                    return NotFound("The specified event does not exist.");
                }

                // Get the logged-in user's ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("You must be logged in to comment.");
                }

                // Validate if the user exists in AspNetUsers
                var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
                if (!userExists)
                {
                    return NotFound("The user does not exist.");
                }

                // Add the comment
                var comment = new Comment
                {
                    EventID = request.EventId,
                    UserID = userId,
                    CommentText = request.CommentText,
                    CreatedAt = DateTime.UtcNow
                };

                Console.WriteLine($"[DEBUG] Preparing to save Comment: EventID={comment.EventID}, UserID={comment.UserID}, Text={comment.CommentText}");

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                Console.WriteLine($"[DEBUG] Comment saved successfully: ID={comment.ID}");

                return Ok(new
                {
                    commentId = comment.ID,
                    userId = comment.UserID,
                    commentText = comment.CommentText,
                    createdAt = comment.CreatedAt
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }




    }




}


