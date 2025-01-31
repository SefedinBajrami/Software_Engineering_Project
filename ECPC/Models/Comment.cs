using System;
using System.ComponentModel.DataAnnotations;

namespace ECPC.Models
{
    public class Comment
    {
        public int ID { get; set; }

        [Required] // Non-nullable
        public int EventID { get; set; }

        [Required] // Non-nullable
        public string UserID { get; set; } = string.Empty;

        [Required] // Non-nullable
        public string CommentText { get; set; } = string.Empty;

        [Required] // Non-nullable
        public DateTime CreatedAt { get; set; }

        // Non-nullable navigation property
        [Required]
        public Event? Event { get; set; }


        // Default constructor to avoid nullable warnings
        public class CommentViewModel
        {
            public string UserName { get; set; } = "Unknown User";
            public string ProfileImageUrl { get; set; } = "/images/default-user.png";
            public string CommentText { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; }
        }

    }
}
