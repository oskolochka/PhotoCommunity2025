using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoCommunity2025.Models
{
public class User
{
    public int UserId { get; set; } // Автоинкрементный первичный ключ
    public string LastName { get; set; } // Фамилия
    public string FirstName { get; set; } // Имя
    public string MiddleName { get; set; } // Отчество
    public string Login { get; set; } // Уникальный логин
    public string Password { get; set; } // Пароль
    public string Cameras { get; set; } // Камеры
    public string Lenses { get; set; } // Объективы

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
}