using PhotoCommunity2025.Models;


namespace PhotoCommunity2025.Services
{
    public class CommentService : ICommentService
    {
        private readonly List<Comment> _comments = new List<Comment>();
        private int _nextId = 1;

        public void AddComment(Comment comment)
        {
            if (string.IsNullOrWhiteSpace(comment.CommentText))
            {
                throw new ArgumentException("Текст комментария не может быть пустым");
            }

            comment.CommentId = _nextId++;
            _comments.Add(comment);
        }

        public void DeleteComment(int id)
        {
            var comment = _comments.FirstOrDefault(c => c.CommentId == id);
            if (comment != null)
            {
                _comments.Remove(comment);
            }
        }

        public IEnumerable<Comment> GetCommentsByPhotoId(int photoId)
        {
            return _comments.Where(c => c.PhotoId == photoId).ToList();
        }
    }
}