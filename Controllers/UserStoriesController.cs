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
    public class UserStoriesController : Controller
    {
        private IUserStoryManager userStoriesManager = new UserStoryManager();

        [HttpGet]
        [Route("userStories/{userStoryId}")]
        public UserStory GetUserStory(string userStoryId)
        {
            return userStoriesManager.GetUserStory(userStoryId);
        }

        [HttpGet]
        [Route("userStories")]
        public JsonResult GetUserStories()
        {
            List<UserStory> userStories = userStoriesManager.GetUserStories();
            return Json(userStories, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Route("userStories")]
        public void CreateUserStory(UserStory userStory)
        {
            userStoriesManager.CreateUserStory(userStory);
        }

     

        [HttpPost]
        [Route("userStories/{userStoryId}")]
        public void UpdateUserStory(string userStoryId, UserStory userStory)
        {
            userStoriesManager.UpdateUserStory(userStoryId, userStory);
        }

        [HttpDelete]
        [Route("userStories/{userStoryId}")]
        public void DeleteUserStory(string userStoryId)
        {
            userStoriesManager.DeleteUserStory(userStoryId);
        }
    }
}