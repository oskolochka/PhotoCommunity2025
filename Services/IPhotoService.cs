using PhotoCommunity2025.Models;

namespace PhotoCommunity2025.Services
{
    public interface IPhotoService
    {
        void UploadPhoto(Photo photo);
        Photo GetPhotoById(int id);
        IEnumerable<Photo> SearchPhotos(string title);
        void DeletePhoto(int id);
        IEnumerable<Photo> GetUserPhotos(int userId);
    }
}
