namespace ECPC.Models.DTO
{
    public class AddCommentRequest
    {
        public int EventId { get; set; }
        public string CommentText { get; set; } = string.Empty;
    }
}
