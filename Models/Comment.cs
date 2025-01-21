
namespace PhotoCommunity2025.Models
{
    public class Comment
    {
        public int CommentId { get; set; } // Автоинкрементный первичный ключ
        public int PhotoId { get; set; } // Внешний ключ
        public int UserId { get; set; } // Внешний ключ
        public string CommentText { get; set; } // Текст комментария

        // Связь с фотографией
        public virtual Photo Photo { get; set; } // Навигационное свойство

        // Связь с пользователем
        public virtual User User { get; set; } // Навигационное свойство
    }
}
