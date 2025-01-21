using Microsoft.EntityFrameworkCore;
using PhotoCommunity2025.Data;
using PhotoCommunity2025.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoCommunity2025.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly AppDbContext _context;

        public PhotoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task UploadPhotoAsync(Photo photo)
        {
            if (string.IsNullOrWhiteSpace(photo.Title))
            {
                throw new ArgumentException("Название фото не может быть пустым");
            }

            if (string.IsNullOrWhiteSpace(photo.FilePath))
            {
                throw new ArgumentException("Путь к фото не может быть пустым");
            }

            await _context.Photos.AddAsync(photo);
            await _context.SaveChangesAsync(); // Сохранение изменений в БД
        }

        public async Task<Photo> GetPhotoByIdAsync(int id)
        {
            return await _context.Photos.FindAsync(id); // Поиск фото по ID
        }

        public async Task<IEnumerable<Photo>> SearchPhotosAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Enumerable.Empty<Photo>();
            }

            return await _context.Photos
                .Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                .ToListAsync(); // Поиск фото по заголовку
        }

        public async Task DeletePhotoAsync(int id)
        {
            var photo = await GetPhotoByIdAsync(id);
            if (photo != null)
            {
                _context.Photos.Remove(photo); // Удаление фото
                await _context.SaveChangesAsync(); // Сохранение изменений в БД
            }
        }

        public async Task<IEnumerable<Photo>> GetUserPhotosAsync(int userId)
        {
            return await _context.Photos
                .Where(p => p.UserId == userId)
                .ToListAsync(); // Получение фотографий пользователя
        }
    }
}