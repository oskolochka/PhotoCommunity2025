using NUnit.Framework;
using PhotoCommunity2025.Models;
using PhotoCommunity2025.Services;
using System;


namespace PhotoCommunity2025.Tests.Services
{
    [TestFixture]
    public class PhotoServiceTests
    {
        private PhotoService _photoService;

        [SetUp]
        public void Setup()
        {
            _photoService = new PhotoService();
        }

        [Test]
        public void UploadPhoto_Photo_PhotoIsValid()
        {
            var photo = new Photo { UserId = 1, Title = "Закат", FilePath = "путь/к/фото.jpg" };

            _photoService.UploadPhoto(photo);

            var retrievedPhoto = _photoService.GetPhotoById(1);
            Assert.That(retrievedPhoto, Is.Not.Null);
            Assert.That(retrievedPhoto.Title, Is.EqualTo("Закат"));
        }

        [Test]
        public void UploadPhoto_Null_PhotoHasEmptyTitle()
        {
            var photo = new Photo { UserId = 1, Title = "", FilePath = "путь/к/фото.jpg" };

            var ex = Assert.Throws<ArgumentException>(() => _photoService.UploadPhoto(photo));
            Assert.That(ex.Message, Is.EqualTo("Название фото не может быть пустым"));
        }

        [Test]
        public void UploadPhoto_Null_PhotoHasEmptyFilePath()
        {
            var photo = new Photo { UserId = 1, Title = "Закат", FilePath = "" };

            var ex = Assert.Throws<ArgumentException>(() => _photoService.UploadPhoto(photo));
            Assert.That(ex.Message, Is.EqualTo("Путь к фото не может быть пустым"));
        }

        [Test]
        public void GetPhoto_Photo_PhotoExists()
        {
            var photo = new Photo { UserId = 1, Title = "Закат", FilePath = "путь/к/фото.jpg" };
            _photoService.UploadPhoto(photo);

            var retrievedPhoto = _photoService.GetPhotoById(1);

            Assert.That(retrievedPhoto, Is.Not.Null);
            Assert.That(retrievedPhoto.Title, Is.EqualTo("Закат"));
        }

        [Test]
        public void GetPhoto_Null_PhotoDoesNotExist()
        {
            var retrievedPhoto = _photoService.GetPhotoById(1);

            Assert.That(retrievedPhoto, Is.Null);
        }

        [Test]
        public void GetUserPhotos_Photos_UserHasPhotos()
        {
            var photo1 = new Photo { UserId = 1, Title = "Закат", FilePath = "путь/к/фото1.jpg" };
            var photo2 = new Photo { UserId = 1, Title = "Пляж", FilePath = "путь/к/фото2.jpg" };
            var photo3 = new Photo { UserId = 2, Title = "Гора", FilePath = "путь/к/фото3.jpg" };

            _photoService.UploadPhoto(photo1);
            _photoService.UploadPhoto(photo2);
            _photoService.UploadPhoto(photo3);

            var photos = _photoService.GetUserPhotos(1);

            Assert.That(photos.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetUserPhotos_Null_UserHasNoPhotos()
        {
            var photos = _photoService.GetUserPhotos(1);

            Assert.That(photos, Is.Empty);
        }

        [Test]
        public void DeletePhoto_True_PhotoExists()
        {
            var photo = new Photo { UserId = 1, Title = "Закат", FilePath = "путь/к/фото.jpg" };
            _photoService.UploadPhoto(photo);

            _photoService.DeletePhoto(1);

            var retrievedPhoto = _photoService.GetPhotoById(1);
            Assert.That(retrievedPhoto, Is.Null);
        }

        [Test]
        public void DeletePhoto_False_PhotoDoesNotExist()
        {
            Assert.That(() => _photoService.DeletePhoto(1), Throws.Nothing);
        }

        [Test]
        public void SearchPhotos_Photos_SearchEqualTitle()
        {
            var photo1 = new Photo { UserId = 1, Title = "Закат", Description = "Красивый закат", Tags = "Красный", FilePath = "путь/к/файлу.jpg" };
            var photo2 = new Photo { UserId = 1, Title = "Пляж", Description = "Солнечный пляж", Tags = "Голубой", FilePath = "путь/к/файлу.jpg" };

            _photoService.UploadPhoto(photo1);
            _photoService.UploadPhoto(photo2);

            var searchResults = _photoService.SearchPhotos("закат");

            Assert.That(searchResults.Count(), Is.EqualTo(1)); 
            Assert.That(searchResults.First().Title, Is.EqualTo("Закат")); 
        }

        [Test]
        public void SearchPhotos_EmptyList_SearchNoEqualTitle()
        {
            var photo1 = new Photo { UserId = 1, Title = "Закат", Description = "Красивый закат", Tags = "Красный", FilePath = "путь/к/файлу.jpg" };
            var photo2 = new Photo { UserId = 1, Title = "Пляж", Description = "Солнечный пляж", Tags = "Голубой", FilePath = "путь/к/файлу.jpg" };

            _photoService.UploadPhoto(photo1);
            _photoService.UploadPhoto(photo2);

            var searchResults = _photoService.SearchPhotos("Гора");

            Assert.That(searchResults.Count(), Is.EqualTo(0));
        }
    }
}
