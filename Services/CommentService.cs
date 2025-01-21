using Microsoft.EntityFrameworkCore;
using PhotoCommunity2025.Data;
using PhotoCommunity2025.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoCommunity2025.Services
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;

        public CommentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddCommentAsync(Comment comment)
        {
            if (string.IsNullOrWhiteSpace(comment.CommentText))
            {
                throw new ArgumentException("Текст комментария не может быть пустым");
            }

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync(); // Сохранение изменений в БД
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment); // Удаление комментария
                await _context.SaveChangesAsync(); // Сохранение изменений в БД
            }
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPhotoIdAsync(int photoId)
        {
            return await _context.Comments
                .Where(c => c.PhotoId == photoId)
                .ToListAsync(); // Получение комментариев по ID фотографии
        }
    }
}