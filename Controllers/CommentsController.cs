using System.Collections.Generic;
using System.Web.Http.Cors;
using System.Web.Mvc;
using WebApplication3.Core;
using WebApplication3.Core.Interfaces;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class CommentsController : Controller
    {
        private ICommentManager commentManager = new CommentManager();

        [HttpGet]
        [Route("comments/get/{itemId}")]
        public ActionResult GetComments(string itemId)
        {
            List<Comment> comments = commentManager.GetComments(itemId);
            return Json(comments, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("comments/create")]
        public void CreateComment(Comment comment)
        {
            commentManager.CreateComment(comment);
        }

        [HttpPost]
        [Route("comments/update/{commentId}")]
        public void UpdateComment(string commentId, Comment comment)
        {
            commentManager.UpdateComment(commentId, comment);
        }

        [HttpGet]
        [Route("comments/delete/{commentId}")]
        public void DeleteComment(string commentId)
        {
            commentManager.DeleteComment(commentId);
        }
    }
}
