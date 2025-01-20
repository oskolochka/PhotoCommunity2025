using Microsoft.AspNetCore.Mvc;
using PhotoCommunity2025.Models;
using PhotoCommunity2025.Services;
using System.Collections.Generic;

namespace PhotoCommunity2025.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            try
            {
                _userService.Register(user);
                return RedirectToAction("Login");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(user);
            }
        }

        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            var user = _userService.Login(login, password);
            if (user != null)
            {
                return RedirectToAction("Profile");
            }
            ModelState.AddModelError("", "Неверный логин или пароль");
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditProfile(User user)
        {
            _userService.UpdateUser(user);
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public IActionResult DeleteAccount(int id)
        {
            _userService.DeleteUser(id);
            return RedirectToAction("Login");
        }
    }
}