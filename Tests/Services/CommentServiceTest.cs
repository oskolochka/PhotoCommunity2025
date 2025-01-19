using NUnit.Framework;
using PhotoCommunity2025.Models;
using PhotoCommunity2025.Services;
using PhotoCommunityWeb.Services;
using System;


namespace PhotoCommunityWeb.Tests.Services
{
    [TestFixture]
    public class CommentServiceTests
    {
        private CommentService _commentService;

        [SetUp]
        public void Setup()
        {
            _commentService = new CommentService();
        }

        [Test]
        public void AddComment_True_CommentIsValid()
        {
            var comment = new Comment { PhotoId = 1, UserId = 1, CommentText = "Отличное фото!" };

            _commentService.AddComment(comment);

            var retrievedComment = _commentService.GetCommentsByPhotoId(1).FirstOrDefault(c => c.CommentText == "Отличное фото!");
            Assert.That(retrievedComment, Is.Not.Null);
            Assert.That(retrievedComment.CommentText, Is.EqualTo("Отличное фото!"));
        }

        [Test]
        public void AddComment_False_CommentTextIsEmpty()
        {
            var comment = new Comment { PhotoId = 1, UserId = 1, CommentText = "" };

            _commentService.AddComment(comment);

            var ex = Assert.Throws<ArgumentException>(() => _commentService.AddComment(comment));
            Assert.That(ex.Message, Is.EqualTo("Текст комментария не может быть пустым"));
        }

        [Test]
        public void GetCommentsByPhotoId_Comments_PhotoHasComment()
        {
            var comment1 = new Comment { PhotoId = 1, UserId = 1, CommentText = "Отличное фото!" };
            var comment2 = new Comment { PhotoId = 1, UserId = 2, CommentText = "Красивый закат!" };

            _commentService.AddComment(comment1);
            _commentService.AddComment(comment2);

            var comments = _commentService.GetCommentsByPhotoId(1);

            Assert.That(comments.Count(), Is.EqualTo(2));
            Assert.That(comments.Any(c => c.CommentText == "Отличное фото!"), Is.True);
            Assert.That(comments.Any(c => c.CommentText == "Красивый закат!"), Is.True);
        }

        [Test]
        public void GetCommentsByPhotoId_Null_PhotoHasNoComment()
        {

            var comments = _commentService.GetCommentsByPhotoId(1);

            Assert.That(comments, Is.Null);
        }

        [Test]
        public void DeleteComment_True_CommentExists()
        {
            var comment = new Comment { PhotoId = 1, UserId = 1, CommentText = "Отличное фото!" };
            _commentService.AddComment(comment);

            _commentService.DeleteComment(1); 

            var comments = _commentService.GetCommentsByPhotoId(1);
            Assert.That(comments.Any(c => c.CommentText == "Отличное фото!"), Is.False); 
        }

        [Test]
        public void DeleteComment_False_CommentDoesNotExist()
        {
            Assert.That(() => _commentService.DeleteComment(1), Throws.Nothing);
        }
    }
}
