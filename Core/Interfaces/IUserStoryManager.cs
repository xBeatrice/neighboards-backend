using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Core.Interfaces
{
    public interface IUserStoryManager
    {
        void CreateUserStory(UserStory userStory);

        void UpdateUserStory(string userStoryId, UserStory userStory);

        UserStory GetUserStory(string userStoryId);

        void DeleteUserStory(string userStoryId);

        List<UserStory> GetUserStories();

    }
}