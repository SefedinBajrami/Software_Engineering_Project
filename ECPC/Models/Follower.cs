using System.ComponentModel.DataAnnotations;

public class Follower
{
    [Key]
    public int Id { get; set; }

    public string? FollowerId { get; set; } // User who follows, nullable
    public string? FollowingId { get; set; } // User being followed, nullable
}
