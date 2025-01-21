using PhotoCommunity2025.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoCommunity2025.Services
{
    public interface IUserService
    {
        Task RegisterAsync(User user);
        Task<User> LoginAsync(string login, string password);
        Task<User> GetUserByIdAsync(int id);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByUsernameAsync(string username);
    }
}
