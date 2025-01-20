using Microsoft.AspNetCore.Mvc;
using PhotoCommunity2025.Models;
using PhotoCommunity2025.Services;

namespace PhotoCommunity2025.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public IActionResult AddComment(Comment comment)
        {
            try
            {
                _commentService.AddComment(comment);
                return RedirectToAction("PhotoDetails", "Photo", new { id = comment.PhotoId });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("PhotoDetails", "Photo", new { id = comment.PhotoId });
            }
        }

        [HttpPost]
        public IActionResult DeleteComment(int id, int photoId)
        {
            _commentService.DeleteComment(id);
            return RedirectToAction("PhotoDetails", "Photo", new { id = photoId });
        }

        [HttpGet]
        public IActionResult GetComments(int photoId)
        {
            var comments = _commentService.GetCommentsByPhotoId(photoId);
            return View(comments);
        }
    }
}