﻿using Microsoft.AspNetCore.Mvc;
using PhotoCommunity2025.Models;
using PhotoCommunity2025.Services;

namespace PhotoCommunity2025.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(Photo photo)
        {
            try
            {
                _photoService.UploadPhotoAsync(photo);
                return RedirectToAction("Profile", new { userId = photo.UserId }); 
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(photo);
            }
        }

        [HttpGet]
        public IActionResult PhotoDetails(int id)
        {
            var photo = _photoService.GetPhotoByIdAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            return View(photo);
        }


        [HttpGet]
        public IActionResult DeletePhoto(int id)
        {
            _photoService.DeletePhotoAsync(id);
            return RedirectToAction("Profile");
        }


        [HttpGet]
        public IActionResult Search(string title)
        {
            var photos = _photoService.SearchPhotosAsync(title);
            return View(photos);
        }

        [HttpGet]
        public IActionResult UserPhotos(int userId)
        {
            var photos = _photoService.GetUserPhotosAsync(userId);
            return View(photos);
        }
    }
}