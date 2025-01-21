using Microsoft.EntityFrameworkCore;
using PhotoCommunity2025.Models;

namespace PhotoCommunity2025.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка отношений
            modelBuilder.Entity<Photo>()
                .HasOne(p => p.User)
                .WithMany(u => u.Photos)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Удаление фотографий при удалении пользователя

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Photo)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PhotoId)
                .OnDelete(DeleteBehavior.Cascade); // Удаление комментариев при удалении фотографии

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany() // У пользователя может быть много комментариев, но мы не создаем навигационное свойство в User
                .HasForeignKey(c => c.UserId);
        }
    }
}
