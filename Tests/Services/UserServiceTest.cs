using NUnit.Framework;
using PhotoCommunity2025.Models;
using PhotoCommunity2025.Services;
using System;


namespace PhotoCommunity2025.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userService = new UserService();
        }

        [Test]
        public void RegisterUser_True_UserIsValid()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };

            _userService.Register(user);

            var registeredUser = _userService.GetUserById(1);
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

            var ex = Assert.Throws<ArgumentException>(() => _userService.Register(user));
            Assert.That(ex.Message, Is.EqualTo("Все поля должны быть заполнены"));
        }

        [Test]
        public void RegisterUser_False_LoginIsNotUnique()
        {
            var user1 = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };
            _userService.Register(user1);

            var user2 = new User { UserId = 2, LastName = "Петров", FirstName = "Петр", MiddleName = "Петрович", Login = "ivanov", Password = "password456", Cameras = "Canon", Lenses = "50mm" };

            var ex = Assert.Throws<ArgumentException>(() => _userService.Register(user2));
            Assert.That(ex.Message, Is.EqualTo("Этот логин уже занят"));
        }

        [Test]
        public void RegisterUser_False_PasswordIsTooShort()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "short", Cameras = "Canon", Lenses = "50mm" };

            var ex = Assert.Throws<ArgumentException>(() => _userService.Register(user));
            Assert.That(ex.Message, Is.EqualTo("Пароль должен состоять минимум из 8 символов"));
        }

        [Test]
        public void Login_User_AccountIsValid()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };
            _userService.Register(user);

            var result = _userService.Login("ivanov", "password123");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Login, Is.EqualTo("ivanov"));
        }

        [Test]
        public void Login_Null_AccountIsInvalid()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };
            _userService.Register(user);

            var result = _userService.Login("ivanov", "wrongpassword");

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetUser_User_UserExists()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };
            _userService.Register(user);

            var result = _userService.GetUserById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserId, Is.EqualTo(1));
        }

        [Test]
        public void GetUser_Null_UserDoesNotExist()
        {
            var result = _userService.GetUserById(1);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void UpdateUser_True_UserIsUpdated()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };
            _userService.Register(user);

            var updatedUser = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "newpassword123", Cameras = "Canon", Lenses = "50mm" };
            _userService.UpdateUser(updatedUser);

            var retrievedUser = _userService.GetUserById(1);
            Assert.That(retrievedUser, Is.Not.Null);
            Assert.That(retrievedUser.LastName, Is.EqualTo("Иванов"));
            Assert.That(retrievedUser.FirstName, Is.EqualTo("Иван"));
            Assert.That(retrievedUser.MiddleName, Is.EqualTo("Иванович"));
            Assert.That(retrievedUser.Login, Is.EqualTo("ivanov"));
            Assert.That(retrievedUser.Password, Is.EqualTo("newpassword123"));
            Assert.That(retrievedUser.Cameras, Is.EqualTo("Canon"));
            Assert.That(retrievedUser.Lenses, Is.EqualTo("50mm"));
        }

        [Test]
        public void UpdateUser_False_UserDoesNotExist()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };

            _userService.UpdateUser(user);

            var retrievedUser = _userService.GetUserById(1);
            Assert.That(retrievedUser, Is.Null);
        }

        [Test]
        public void DeleteUser_True_UserExists()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" }; ;
            _userService.Register(user);

            _userService.DeleteUser(1);

            var retrievedUser = _userService.GetUserById(1);

            Assert.That(retrievedUser, Is.Null);
        }

        [Test]
        public void DeleteUser_False_UserDoesNotExist()
        { 
             var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" }; ;
            _userService.Register(user);
     
            var initialUserCount = _userService.GetAllUsers().Count();

            _userService.DeleteUser(2); 

            var finalUserCount = _userService.GetAllUsers().Count();

            Assert.That(finalUserCount, Is.EqualTo(initialUserCount));
        }
    }
}
