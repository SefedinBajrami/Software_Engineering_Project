using ECPC.Models;
using System;
using System.Collections.Generic;

namespace ECPC.ViewModels
{

    public class EventViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public TimeSpan Time { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? City { get; set; }
        public string? District { get; set; }
        public int? MaxParticipants { get; set; }
        public bool IsPublic { get; set; }

        public string? Image { get; set; } = "/images/default-event.png"; // Path stored in DB
        public IFormFile? ImageFile { get; set; } // File input for upload

        public string Grouping { get; set; } = string.Empty;

        public string CreatedById { get; set; } = string.Empty;

        // Creator details
        public string UserName { get; set; } = "Unknown User";
        public string ProfileImageUrl { get; set; } = "/images/default-user.png";

        // Comments on the event
        public List<CommentViewModel> Comments { get; set; } = new();
    }

    public class CommentViewModel
    {
        public string UserName { get; set; } = "Unknown User";
        public string ProfileImageUrl { get; set; } = "/images/default-user.png";
        public string CommentText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
