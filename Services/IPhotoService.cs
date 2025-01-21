using PhotoCommunity2025.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoCommunity2025.Services
{
    public interface IPhotoService
    {
        Task UploadPhotoAsync(Photo photo);
        Task<Photo> GetPhotoByIdAsync(int id);
        Task<IEnumerable<Photo>> SearchPhotosAsync(string title);
        Task DeletePhotoAsync(int id);
        Task<IEnumerable<Photo>> GetUserPhotosAsync(int userId);
    }
}
