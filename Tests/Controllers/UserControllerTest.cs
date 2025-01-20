using Moq;
using NUnit.Framework;
using PhotoCommunity2025.Controllers;
using PhotoCommunity2025.Models;
using PhotoCommunity2025.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PhotoCommunity2025.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
       
            if (_controller is IDisposable disposableController)
            {
                disposableController.Dispose();
            }
        }

        [Test]
        public void Register_Login_UserIsValid()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };

            var result = _controller.Register(user) as RedirectToActionResult;

            _userServiceMock.Verify(s => s.Register(user), Times.Once);
            Assert.That(result.ActionName, Is.EqualTo("Login"));
        }

        [Test]
        public void Register_ErrorMessage_UserIsNoValid()
        {
            var user1 = new User { UserId = 1, LastName = "", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };
           
            _userServiceMock.Setup(s => s.Register(user1)).Throws(new ArgumentException("Все поля должны быть заполнены"));
            var result1 = _controller.Register(user1) as ViewResult;

            Assert.That(_controller.ModelState.IsValid, Is.False);
            Assert.That(result1.Model, Is.EqualTo(user1));
            Assert.That(_controller.ModelState[""].Errors.Count, Is.GreaterThan(0));
            Assert.That(_controller.ModelState[""].Errors[0].ErrorMessage, Is.EqualTo("Все поля должны быть заполнены"));

            _controller.ModelState.Clear();

            var user2 = new User { UserId = 2, LastName = "Петров", FirstName = "Петр", MiddleName = "Петрович", Login = "petrov", Password = "short", Cameras = "Canon", Lenses = "50mm" };

            _userServiceMock.Setup(s => s.Register(user2)).Throws(new ArgumentException("Пароль должен состоять минимум из 8 символов"));
            var result2 = _controller.Register(user2) as ViewResult;

            Assert.That(_controller.ModelState.IsValid, Is.False);
            Assert.That(result2.Model, Is.EqualTo(user2));
            Assert.That(_controller.ModelState[""].Errors.Count, Is.GreaterThan(0));
            Assert.That(_controller.ModelState[""].Errors[0].ErrorMessage, Is.EqualTo("Пароль должен состоять минимум из 8 символов"));

            _controller.ModelState.Clear();

            var user3 = new User { UserId = 3, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };

            _userServiceMock.Setup(s => s.Register(user3)).Throws(new ArgumentException("Этот логин уже занят"));
            var result3 = _controller.Register(user3) as ViewResult;

            Assert.That(_controller.ModelState.IsValid, Is.False);
            Assert.That(result3.Model, Is.EqualTo(user3));
            Assert.That(_controller.ModelState[""].Errors.Count, Is.GreaterThan(0));
            Assert.That(_controller.ModelState[""].Errors[0].ErrorMessage, Is.EqualTo("Этот логин уже занят"));
            
        }

        [Test]
        public void Login_Profile_ProfileExists()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };
            _userServiceMock.Setup(s => s.Login(user.Login, user.Password)).Returns(user);

            var result = _controller.Login(user.Login, user.Password) as RedirectToActionResult;

            Assert.That(result.ActionName, Is.EqualTo("Profile"));

        }

        [Test]
        public void Login_ErrorMessage_ProfileDoesNotExist()
        {
            _userServiceMock.Setup(s => s.Login("ivanov", "password123")).Returns((User)null);

            var result = _controller.Login("ivanov", "password123") as ViewResult;

            Assert.That(_controller.ModelState.IsValid, Is.False);
            Assert.That(_controller.ModelState[""].Errors[0].ErrorMessage, Is.EqualTo("Неверный логин или пароль"));
        }

        [Test]
        public void EditProfile_User_UpdatesUser()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };

            var result = _controller.EditProfile(user) as RedirectToActionResult;

            _userServiceMock.Verify(s => s.UpdateUser(user), Times.Once);
            Assert.That(result.ActionName, Is.EqualTo("Profile"));
        }

        [Test]
        public void DeleteAccount_Login_ProfileIsDelete()
        {
            int userId = 1;

            var result = _controller.DeleteAccount(userId) as RedirectToActionResult;

            _userServiceMock.Verify(s => s.DeleteUser(userId), Times.Once);
            Assert.That(result.ActionName, Is.EqualTo("Login"));
        }
    }
}