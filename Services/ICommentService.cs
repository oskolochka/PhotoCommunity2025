using PhotoCommunity2025.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoCommunity2025.Services
{
    public interface ICommentService
    {
        Task AddCommentAsync(Comment comment);
        Task DeleteCommentAsync(int id);
        Task<IEnumerable<Comment>> GetCommentsByPhotoIdAsync(int photoId);
    }
}