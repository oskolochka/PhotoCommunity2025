using PhotoCommunity2025.Models;

namespace PhotoCommunity2025.Services
{
    public interface ICommentService
    {
        void AddComment(Comment comment);
        void DeleteComment(int id);
        IEnumerable<Comment> GetCommentsByPhotoId(int photoId);
    }
}