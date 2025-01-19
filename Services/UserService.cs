using PhotoCommunity2025.Models;

namespace PhotoCommunity2025.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new List<User>();
        private int _nextId = 1;

        public void Register(User user)
        {
            if (_users.Any(u => u.Login == user.Login))
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

            user.UserId = _nextId++;
            _users.Add(user);
        }

        public User Login(string login, string password)
        {
            return _users.FirstOrDefault(u => u.Login == login && u.Password == password);
        }

        public User GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.UserId == id);
        }

        public void UpdateUser(User user)
        {
            var existingUser = GetUserById(user.UserId);
            if (existingUser != null)
            {
                existingUser.LastName = user.LastName;
                existingUser.FirstName = user.FirstName;
                existingUser.MiddleName = user.MiddleName;
                existingUser.Login = user.Login;
                existingUser.Password = user.Password;
                existingUser.Cameras = user.Cameras;
                existingUser.Lenses = user.Lenses;
            }
        }

        public void DeleteUser(int id)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                _users.Remove(user);
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users;
        }
    }
}
