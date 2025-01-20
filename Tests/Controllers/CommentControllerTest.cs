using Moq;
using NUnit.Framework;
using PhotoCommunity2025.Controllers;
using PhotoCommunity2025.Models;
using PhotoCommunityWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PhotoCommunity2025.Tests
{
    [TestFixture]
    public class CommentControllerTests
    {
        private Mock<ICommentService> _commentServiceMock;
        private CommentController _controller;

        [SetUp]
        public void Setup()
        {
            _commentServiceMock = new Mock<ICommentService>();
            _controller = new CommentController(_commentServiceMock.Object);
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
        public void AddComment_PhotoDetails_CommentIsValid()
        {
            var comment = new Comment { CommentId = 1, PhotoId = 1, CommentText = "Отличное фото!" };

            var result = _controller.AddComment(comment) as RedirectToActionResult;

            // Assert
            _commentServiceMock.Verify(s => s.AddComment(comment), Times.Once); 
            Assert.IsNotNull(result); 
            Assert.That(result.ActionName, Is.EqualTo("PhotoDetails"));
            Assert.That(result.ControllerName, Is.EqualTo("Photo"));
            Assert.That(result.RouteValues["id"], Is.EqualTo(comment.PhotoId));
        }

        [Test]
        public void AddComment_InvalidComment_ReturnsRedirectToPhotoDetailsWithError()
        {
            var comment = new Comment { CommentId = 1, PhotoId = 1, CommentText = "" }; 
            _commentServiceMock.Setup(s => s.AddComment(comment)).Throws(new ArgumentException("Текст комментария не может быть пустым"));

            var result = _controller.AddComment(comment) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.That(result.ActionName, Is.EqualTo("PhotoDetails")); 
            Assert.That(result.RouteValues["id"], Is.EqualTo(comment.PhotoId));
            Assert.That(_controller.ModelState.IsValid, Is.False); 
            Assert.That(_controller.ModelState[""].Errors.Count, Is.GreaterThan(0)); 
            Assert.That(_controller.ModelState[""].Errors[0].ErrorMessage, Is.EqualTo("Текст комментария не может быть пустым")); 

        }

        [Test]
        public void DeleteComment_ValidId_DeletesCommentAndRedirectsToPhotoDetails()
        { 
            var result = _controller.DeleteComment(1, 1) as RedirectToActionResult;

            _commentServiceMock.Verify(s => s.DeleteComment(1), Times.Once);
            Assert.IsNotNull(result); 
            Assert.That(result.ActionName, Is.EqualTo("PhotoDetails")); 
            Assert.That(result.ControllerName, Is.EqualTo("Photo")); 
            Assert.That(result.RouteValues["id"], Is.EqualTo(1)); 
        }

        [Test]
        public void GetComments_ValidPhotoId_ReturnsViewWithComments()
        {
            var comments = new List<Comment>
            {
                new Comment { CommentId = 1, PhotoId = 1, CommentText = "Отличное фото!" },
                new Comment { CommentId = 2, PhotoId = 1, CommentText = "Прекрасно!" }
            };
            _commentServiceMock.Setup(s => s.GetCommentsByPhotoId(1)).Returns(comments);

            var result = _controller.GetComments(1) as ViewResult;

            Assert.IsNotNull(result); 
            Assert.That(result.Model, Is.EqualTo(comments)); 
        }
    }
}