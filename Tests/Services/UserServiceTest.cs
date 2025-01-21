using NUnit.Framework;
using PhotoCommunity2025.Models;
using PhotoCommunity2025.Services;
using PhotoCommunity2025.Data;
using Microsoft.EntityFrameworkCore;


namespace PhotoCommunity2025.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService;
        private AppDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _userService = new UserService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }


        [Test]
        public async Task RegisterUser_True_UserIsValid()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };

            await _userService.RegisterAsync(user);

            var registeredUser = await _userService.GetUserByIdAsync(1);
            Assert.That(registeredUser, Is.Not.Null);
            Assert.That(registeredUser.LastName, Is.EqualTo("Иванов"));
            Assert.That(registeredUser.FirstName, Is.EqualTo("Иван"));
            Assert.That(registeredUser.MiddleName, Is.EqualTo("Иванович"));
            Assert.That(registeredUser.Login, Is.EqualTo("ivanov"));
            Assert.That(registeredUser.Password, Is.EqualTo("password123"));
            Assert.That(registeredUser.Cameras, Is.EqualTo("Canon"));
            Assert.That(registeredUser.Lenses, Is.EqualTo("50mm"));
        }

        [Test]
        public void RegisterUser_False_UserHasEmptyFields()
        {
            var user = new User { UserId = 1, LastName = "", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _userService.RegisterAsync(user));
            Assert.That(ex.Message, Is.EqualTo("Все поля должны быть заполнены"));
        }

        [Test]
        public async Task RegisterUser_False_LoginIsNotUnique()
        {
            var user1 = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };
            await _userService.RegisterAsync(user1);

            var user2 = new User { UserId = 2, LastName = "Петров", FirstName = "Петр", MiddleName = "Петрович", Login = "ivanov", Password = "password456", Cameras = "Canon", Lenses = "50mm" };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _userService.RegisterAsync(user2));
            Assert.That(ex.Message, Is.EqualTo("Этот логин уже занят"));
        }

        [Test]
        public void RegisterUser_False_PasswordIsTooShort()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "short", Cameras = "Canon", Lenses = "50mm" };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _userService.RegisterAsync(user));
            Assert.That(ex.Message, Is.EqualTo("Пароль должен состоять минимум из 8 символов"));
        }

        [Test]
        public async Task Login_User_AccountIsValid()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };
            await _userService.RegisterAsync(user);

            var result = await _userService.LoginAsync("ivanov", "password123");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Login, Is.EqualTo("ivanov"));
        }

        [Test]
        public async Task Login_Null_AccountIsInvalid()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };
            await _userService.RegisterAsync(user);

            var result = await _userService.LoginAsync("ivanov", "wrongpassword");

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetUser_User_UserExists()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };
            await _userService.RegisterAsync(user);

            var result = await _userService.GetUserByIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetUser_Null_UserDoesNotExist()
        {
            var result = await _userService.GetUserByIdAsync(1);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpdateUser_True_UserIsUpdated()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };
            await _userService.RegisterAsync(user);

            var updatedUser = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Nikon", Lenses = "35mm" };
            await _userService.UpdateUserAsync(updatedUser);

            var retrievedUser = await _userService.GetUserByIdAsync(1);
            Assert.That(retrievedUser, Is.Not.Null);
            Assert.That(retrievedUser.LastName, Is.EqualTo("Иванов"));
            Assert.That(retrievedUser.FirstName, Is.EqualTo("Иван"));
            Assert.That(retrievedUser.MiddleName, Is.EqualTo("Иванович"));
            Assert.That(retrievedUser.Login, Is.EqualTo("ivanov"));
            Assert.That(retrievedUser.Password, Is.EqualTo("password123")); 
            Assert.That(retrievedUser.Cameras, Is.EqualTo("Nikon")); 
            Assert.That(retrievedUser.Lenses, Is.EqualTo("35mm")); 
        }

        [Test]
        public async Task UpdateUser_False_UserDoesNotExist()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };

            await _userService.UpdateUserAsync(user); 

            var retrievedUser = await _userService.GetUserByIdAsync(1);
            Assert.That(retrievedUser, Is.Null);
        }

        [Test]
        public async Task DeleteUser_True_UserExists()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };
            await _userService.RegisterAsync(user);

            await _userService.DeleteUserAsync(1);

            var retrievedUser = await _userService.GetUserByIdAsync(1);

            Assert.That(retrievedUser, Is.Null);
        }

        [Test]
        public async Task DeleteUser_False_UserDoesNotExist()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };

            await _userService.RegisterAsync(user);

            var initialUserCount = (await _userService.GetAllUsersAsync()).Count();

            await _userService.DeleteUserAsync(2);

            var finalUserCount = (await _userService.GetAllUsersAsync()).Count();

            Assert.That(finalUserCount, Is.EqualTo(initialUserCount));
        }
    }
}