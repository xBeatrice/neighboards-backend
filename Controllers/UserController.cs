using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using WebApplication3.Core;
using WebApplication3.Core.Interfaces;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class UserController : Controller
    {
        private IUserManager userManager = new UserManager();

        [HttpGet]
        [Route("users/{userId}")]
        public User GetUser(string groupName, string userId)
        {
            return userManager.GetUser(userId);
        }

        [HttpGet]
        [Route("users")]
        public ActionResult GetUsers()
        {
            var users = userManager.GetUsers(); // Get the list of users
            return Json(users, JsonRequestBehavior.AllowGet); // Return the users as JSON response
        }

        [HttpPost]
        [Route("users")]
        public void CreateUser(string groupName, User user)
        {
            userManager.CreateUser(user);
        }

        [HttpPost]
        [Route("users/{userId}")]
        public void UpdateUser(string groupName, string userId, User user)
        {
            userManager.UpdateUser(userId, user);
        }

        [HttpDelete]
        [Route("users/{userId}")]
        public void DeleteUser(string groupName, string userId)
        {
            userManager.DeleteUser(userId);
        }
    }
}