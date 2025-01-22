
namespace PhotoCommunity2025.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int PhotoId { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; }

        public virtual Photo Photo { get; set; }

        public virtual User User { get; set; }
    }
}
