using Microsoft.AspNetCore.Mvc;
using PhotoCommunity2025.Models;
using PhotoCommunity2025.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoCommunity2025.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            // Проверяем модель на валидность
            if (!TryValidateModel(user))
            {
                return View(user); // Если модель не валидна, возвращаем представление с ошибками
            }

            try
            {
                await _userService.RegisterAsync(user);
                return RedirectToAction("Login");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(user);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            var user = await _userService.LoginAsync(login, password);
            if (user != null)
            {
                return RedirectToAction("Profile", new { username = user.Login });
            }
            ModelState.AddModelError("", "Неверный логин или пароль");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string username)
        {
            // Получаем пользователя по логину
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(string username)
        {
            // Получаем пользователя по логину
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(User user)
        {
            if (!ModelState.IsValid)
            {
                return NotFound(); ; // Если модель не валидна, возвращаем представление с ошибками
            }

            await _userService.UpdateUserAsync(user);
            return RedirectToAction("Profile", new { username = user.Login }); // Перенаправляем на профиль
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            await _userService.DeleteUserAsync(userId);
            return RedirectToAction("Login");
        }
    }
}