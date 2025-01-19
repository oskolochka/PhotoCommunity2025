using PhotoCommunity2025.Models;

namespace PhotoCommunity2025.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly List<Photo> _photos = new List<Photo>();
        private int _nextId = 1;

        public void UploadPhoto(Photo photo)
        {

            if (string.IsNullOrWhiteSpace(photo.Title))
            {
                throw new ArgumentException("Название фото не может быть пустым");
            }

            if (string.IsNullOrWhiteSpace(photo.FilePath))
            {
                throw new ArgumentException("Путь к фото не может быть пустым");
            }

            photo.PhotoId = _nextId++;
            _photos.Add(photo);
        }

        public Photo GetPhotoById(int id)
        {
            return _photos.FirstOrDefault(p => p.PhotoId == id);
        }

        public IEnumerable<Photo> SearchPhotos(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Enumerable.Empty<Photo>();
            }

            return _photos.Where(p => p.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        public void DeletePhoto(int id)
        {
            var photo = GetPhotoById(id);
            if (photo != null)
            {
                _photos.Remove(photo);
            }
        }

        public IEnumerable<Photo> GetUserPhotos(int userId)
        {
            return _photos.Where(p => p.UserId == userId).ToList();
        }
    }
}
