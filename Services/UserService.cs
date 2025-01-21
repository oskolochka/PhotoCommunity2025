using PhotoCommunity2025.Models;
using Microsoft.EntityFrameworkCore;
using PhotoCommunity2025.Data; // Добавьте пространство имен для контекста
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoCommunity2025.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context; // Контекст базы данных

        public UserService(AppDbContext context)
        {
            _context = context; // Инициализация контекста
        }

        public async Task RegisterAsync(User user)
        {
            // Проверка на существование логина
            if (await _context.Users.AnyAsync(u => u.Login == user.Login))
            {
                throw new ArgumentException("Этот логин уже занят");
            }

            // Валидация полей пользователя
            if (string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.MiddleName) ||
                string.IsNullOrWhiteSpace(user.Login) ||
                string.IsNullOrWhiteSpace(user.Password) ||
                string.IsNullOrWhiteSpace(user.Cameras) ||
                string.IsNullOrWhiteSpace(user.Lenses))
            {
                throw new ArgumentException("Все поля должны быть заполнены");
            }

            if (user.Password.Length < 8)
            {
                throw new ArgumentException("Пароль должен состоять минимум из 8 символов");
            }

            // Сохранение пароля в открытом виде
            // Добавление пользователя в контекст
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync(); // Сохранение изменений в БД
        }

        public async Task<User> LoginAsync(string login, string password)
        {
            // Поиск пользователя по логину и паролю
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id); // Поиск пользователя по ID
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == username);
        }

        public async Task UpdateUserAsync(User user)
        {
            // Проверка на заполненность всех полей
            if (string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.MiddleName) ||
                string.IsNullOrWhiteSpace(user.Login) ||
                string.IsNullOrWhiteSpace(user.Cameras) ||
                string.IsNullOrWhiteSpace(user.Lenses))
            {
                throw new ArgumentException("Все поля должны быть заполнены.");
            }

            var existingUser = await GetUserByIdAsync(user.UserId);
            if (existingUser != null)
            {
                // Обновляем все поля
                existingUser.LastName = user.LastName;
                existingUser.FirstName = user.FirstName;
                existingUser.MiddleName = user.MiddleName;
                existingUser.Login = user.Login;
                existingUser.Password = existingUser.Password;
                existingUser.Cameras = user.Cameras;
                existingUser.Lenses = user.Lenses;

                await _context.SaveChangesAsync(); // Сохраняем изменения в БД
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user); // Удаление пользователя
                await _context.SaveChangesAsync(); // Сохранение изменений в БД
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync(); // Получение всех пользователей из БД
        }
    }
}
