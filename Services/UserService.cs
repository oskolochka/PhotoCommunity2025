using PhotoCommunity2025.Models;
using Microsoft.EntityFrameworkCore;
using PhotoCommunity2025.Data; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoCommunity2025.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context; 

        public UserService(AppDbContext context)
        {
            _context = context; 
        }

        public async Task RegisterAsync(User user)
        {

            if (await _context.Users.AnyAsync(u => u.Login == user.Login))
            {
                throw new ArgumentException("Этот логин уже занят");
            }

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

   
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> LoginAsync(string login, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id); 
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == username);
        }

        public async Task UpdateUserAsync(User user)
        {

            if (string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.MiddleName) ||
                string.IsNullOrWhiteSpace(user.Cameras) ||
                string.IsNullOrWhiteSpace(user.Lenses))
            {
                throw new ArgumentException("Все поля должны быть заполнены.");
            }

            var existingUser = await GetUserByIdAsync(user.UserId);
            if (existingUser != null)
            {
                existingUser.LastName = user.LastName;
                existingUser.FirstName = user.FirstName;
                existingUser.MiddleName = user.MiddleName;
                existingUser.Cameras = user.Cameras;
                existingUser.Lenses = user.Lenses;

                await _context.SaveChangesAsync(); 
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user); 
                await _context.SaveChangesAsync(); 
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync(); 
        }
    }
}
