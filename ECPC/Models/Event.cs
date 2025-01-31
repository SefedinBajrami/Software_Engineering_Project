using ECPC.Models;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace ECPC.Models
{
    public class Event
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public string? Location { get; set; }
        [Required]
        public DateTime EventDate { get; set; }
        [Required]
        public TimeSpan Time { get; set; }
        [Required]
        public string? CreatedBy { get; set; }

        [Required]
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Category { get; set; }
        public int? MaxParticipants { get; set; }
        [Required]
        public bool IsPublic { get; set; }
        public string? Image { get; set; }
        [Required]
        public string? Grouping { get; set; }
        // Navigation property
        public ApplicationUser? User { get; set; }

        public ICollection<Comment>? Comments { get; set; }
        public ICollection<EventParticipant>? Participants { get; set; }
    }
}