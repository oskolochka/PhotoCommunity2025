using PhotoCommunity2025.Models;

namespace PhotoCommunity2025.Services
{
    public interface IUserService
    {
        void Register(User user);
        User Login(string login, string password);
        User GetUserById(int id);
        void UpdateUser(User user);
        void DeleteUser(int id);
        IEnumerable<User> GetAllUsers();
    }
}
