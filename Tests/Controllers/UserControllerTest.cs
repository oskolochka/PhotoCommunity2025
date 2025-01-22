using Moq;
using NUnit.Framework;
using PhotoCommunity2025.Controllers;
using PhotoCommunity2025.Models;
using PhotoCommunity2025.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task Register_Login_UserIsValid()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };

            _userServiceMock.Setup(s => s.RegisterAsync(user)).Returns(Task.CompletedTask);

            var result = await _controller.Register(user) as RedirectToActionResult;

            _userServiceMock.Verify(s => s.RegisterAsync(user), Times.Once);
            Assert.That(result.ActionName, Is.EqualTo("Login"));
        }

        [Test]
        public async Task Register_ErrorMessage_UserIsNoValid()
        {
            var user1 = new User { UserId = 1, LastName = "", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };

            _userServiceMock.Setup(s => s.RegisterAsync(user1)).ThrowsAsync(new ArgumentException("Все поля должны быть заполнены"));
            var result1 = await _controller.Register(user1) as ViewResult;

            Assert.That(_controller.ModelState.IsValid, Is.False);
            Assert.That(result1.Model, Is.EqualTo(user1));
            Assert.That(_controller.ModelState[""].Errors.Count, Is.GreaterThan(0));
            Assert.That(_controller.ModelState[""].Errors[0].ErrorMessage, Is.EqualTo("Все поля должны быть заполнены"));

            _controller.ModelState.Clear();

            var user2 = new User { UserId = 2, LastName = "Петров", FirstName = "Петр", MiddleName = "Петрович", Login = "petrov", Password = "short", Cameras = "Canon", Lenses = "50mm" };

            _userServiceMock.Setup(s => s.RegisterAsync(user2)).ThrowsAsync(new ArgumentException("Пароль должен состоять минимум из 8 символов"));
            var result2 = await _controller.Register(user2) as ViewResult;

            Assert.That(_controller.ModelState.IsValid, Is.False);
            Assert.That(result2.Model, Is.EqualTo(user2));
            Assert.That(_controller.ModelState[""].Errors.Count, Is.GreaterThan(0));
            Assert.That(_controller.ModelState[""].Errors[0].ErrorMessage, Is.EqualTo("Пароль должен состоять минимум из 8 символов"));

            _controller.ModelState.Clear();

            var user3 = new User { UserId = 3, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };

            _userServiceMock.Setup(s => s.RegisterAsync(user3)).ThrowsAsync(new ArgumentException("Этот логин уже занят"));
            var result3 = await _controller.Register(user3) as ViewResult;

            Assert.That(_controller.ModelState.IsValid, Is.False);
            Assert.That(result3.Model, Is.EqualTo(user3));
            Assert.That(_controller.ModelState[""].Errors.Count, Is.GreaterThan(0));
            Assert.That(_controller.ModelState[""].Errors[0].ErrorMessage, Is.EqualTo("Этот логин уже занят"));
        }

        [Test]
        public async Task Login_Profile_ProfileExists()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };
            _userServiceMock.Setup(s => s.LoginAsync(user.Login, user.Password)).ReturnsAsync(user);

            var result = await _controller.Login(user.Login, user.Password) as RedirectToActionResult;

            Assert.That(result.ActionName, Is.EqualTo("Profile"));
        }

        [Test]
        public async Task Login_ErrorMessage_ProfileDoesNotExist()
        {
            _userServiceMock.Setup(s => s.LoginAsync("ivanov", "password123")).ReturnsAsync((User)null);

            var result = await _controller.Login("ivanov", "password123") as ViewResult;

            Assert.That(_controller.ModelState.IsValid, Is.False);
            Assert.That(_controller.ModelState[""].Errors[0].ErrorMessage, Is.EqualTo("Неверный логин или пароль"));
        }

        /*
        [Test]
        public async Task EditProfile_User_UpdatesUser()
        {
            var user = new User { UserId = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Login = "ivanov", Password = "password123", Cameras = "Canon", Lenses = "50mm" };
            
            _userServiceMock.Setup(s => s.UpdateUserAsync(user)).Returns(Task.CompletedTask);

            var result = await _controller.EditProfile(user) as RedirectToActionResult;

            _userServiceMock.Verify(s => s.UpdateUserAsync(user), Times.Once);
            Assert.That(result.Model, Is.EqualTo(user));
        } 
        */

        [Test]
        public async Task DeleteAccount_Login_ProfileIsDeleted()
        {
            int userId = 1;

            _userServiceMock.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(new User { UserId = userId });

            _userServiceMock.Setup(s => s.DeleteUserAsync(userId)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteAccount(userId) as RedirectToActionResult;

            _userServiceMock.Verify(s => s.DeleteUserAsync(userId), Times.Once);
            Assert.That(result.ActionName, Is.EqualTo("Login"));
        }
    }
}