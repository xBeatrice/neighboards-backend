using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Core.Interfaces
{
    public interface ICommentManager
    {
        void CreateComment(Comment comment);

        void UpdateComment(string commentId, Comment comment);

        void DeleteComment(string commentId);

        List<Comment> GetComments(string itemId);
    }
}