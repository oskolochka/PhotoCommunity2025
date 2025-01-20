using Moq;
using NUnit.Framework;
using PhotoCommunity2025.Controllers;
using PhotoCommunity2025.Models;
using PhotoCommunity2025.Services;
using Microsoft.AspNetCore.Mvc;

namespace PhotoCommunity2025.Tests
{
    [TestFixture]
    public class PhotoControllerTests
    {
        private Mock<IPhotoService> _photoServiceMock;
        private PhotoController _controller;

        [SetUp]
        public void Setup()
        {
            _photoServiceMock = new Mock<IPhotoService>();
            _controller = new PhotoController(_photoServiceMock.Object);
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
        public void Upload_PhotoProfile_PhotoIsValid()
        {
            var photo = new Photo { PhotoId = 1, UserId = 1, Title = "Закат", FilePath = "путь/к/фото.jpg" };

            var result = _controller.Upload(photo) as RedirectToActionResult;

            _photoServiceMock.Verify(s => s.UploadPhoto(photo), Times.Once);
            Assert.IsNotNull(result);
            Assert.That(result.ActionName, Is.EqualTo("Profile")); 
            Assert.That(result.RouteValues["userId"], Is.EqualTo(photo.UserId));

            _controller.ModelState.Clear();
        }

        [Test]
        public void Upload_ErrorMessage_PhotoIsNoValid()
        {
            var photo1 = new Photo { PhotoId = 1, UserId = 1, Title = "Закат", FilePath = "путь/к/фото.jpg" };
            _photoServiceMock.Setup(s => s.UploadPhoto(photo1)).Throws(new ArgumentException("Название фото не может быть пустым"));

            var result1 = _controller.Upload(photo1) as ViewResult;

            Assert.That(_controller.ModelState.IsValid, Is.False); 
            Assert.That(result1.Model, Is.EqualTo(photo1));
            Assert.That(_controller.ModelState[""].Errors.Count, Is.GreaterThan(0)); 
            Assert.That(_controller.ModelState[""].Errors[0].ErrorMessage, Is.EqualTo("Название фото не может быть пустым"));

            _controller.ModelState.Clear();

            var photo2 = new Photo { PhotoId = 2, UserId = 1, Title = "Закат", FilePath = ""}; 
            _photoServiceMock.Setup(s => s.UploadPhoto(photo2)).Throws(new ArgumentException("Путь к фото не может быть пустым"));

            var result2 = _controller.Upload(photo2) as ViewResult;

            Assert.That(_controller.ModelState.IsValid, Is.False); 
            Assert.That(result2.Model, Is.EqualTo(photo2));
            Assert.That(_controller.ModelState[""].Errors.Count, Is.GreaterThan(0)); 
            Assert.That(_controller.ModelState[""].Errors[0].ErrorMessage, Is.EqualTo("Путь к фото не может быть пустым")); 
        }

        [Test]
        public void PhotoDetails_Photo_PhotoExist()
        {
            var photo = new Photo { PhotoId = 1, UserId = 1, Title = "Закат", FilePath = "путь/к/фото.jpg" };
            _photoServiceMock.Setup(s => s.GetPhotoById(1)).Returns(photo);

            var result = _controller.PhotoDetails(1) as ViewResult;

            Assert.IsNotNull(result);
            Assert.That(result.Model, Is.EqualTo(photo));
        }

        [Test]
        public void DeletePhoto_Profile_PhotoIsDelete()
        {
            var photo = new Photo { PhotoId = 1, UserId = 1, Title = "Закат", FilePath = "путь/к/фото.jpg" };

            var result = _controller.DeletePhoto(1) as RedirectToActionResult;

            _photoServiceMock.Verify(s => s.DeletePhoto(1), Times.Once); 
            Assert.IsNotNull(result); 
            Assert.That(result.ActionName, Is.EqualTo("Profile")); 
        }

        [Test]
        public void Search_Photos_PhotosFound()
        {
            var photos = new List<Photo>
            {
                new Photo { PhotoId = 1, Title = "Закат", FilePath = "путь/к/фото.jpg" },
                new Photo { PhotoId = 2, Title = "Пляж", FilePath = "путь/к/фото.jpg" }
            };
            _photoServiceMock.Setup(s => s.SearchPhotos("Закат")).Returns(photos);

            var result = _controller.Search("Закат") as ViewResult;

            Assert.IsNotNull(result);
            Assert.That(result.Model, Is.EqualTo(photos)); 
        }

        [Test]
        public void Search_EmptyList_NoPhotosFound()
        {
            _photoServiceMock.Setup(s => s.SearchPhotos("Закат")).Returns(new List<Photo>());

            var result = _controller.Search("Закат") as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Photo>>(result.Model);
            Assert.IsEmpty(result.Model as List<Photo>); 
        }

        [Test]
        public void UserPhotos_Photos_PhotosExists()
        {
            var photos = new List<Photo>
            {
                new Photo { PhotoId = 1, Title = "Закат", FilePath = "путь/к/фото.jpg", UserId = 1 },
                new Photo { PhotoId = 2, Title = "Пляж", FilePath = "путь/к/фото.jpg", UserId = 1 }
            };
            _photoServiceMock.Setup(s => s.GetUserPhotos(1)).Returns(photos);

            var result = _controller.UserPhotos(1) as ViewResult;

            Assert.IsNotNull(result);
            Assert.That(result.Model, Is.EqualTo(photos)); 
        }

        [Test]
        public void UserPhotos_EmptyList_NoPhotosExist()
        {
            _photoServiceMock.Setup(s => s.GetUserPhotos(1)).Returns(new List<Photo>());

            var result = _controller.UserPhotos(1) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Photo>>(result.Model); 
            Assert.IsEmpty(result.Model as List<Photo>); 
        }
    }
}