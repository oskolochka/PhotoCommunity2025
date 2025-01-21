
using PhotoCommunity2025.Models;

public class Photo
{
    public int PhotoId { get; set; } // Автоинкрементный первичный ключ
    public int UserId { get; set; } // Внешний ключ
    public string Title { get; set; } // Заголовок
    public string Description { get; set; } // Описание
    public string Tags { get; set; } // Теги
    public string FilePath { get; set; } // Путь к файлу

    // Связь с пользователем
    public virtual User User { get; set; } // Навигационное свойство

    // Связь с комментариями
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

}