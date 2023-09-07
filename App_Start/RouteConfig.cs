using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "GetUserStory",
              url: "userStories/{userStoryId}",
              defaults: new { controller = "UserStories", action = "GetUserStory" }
          );

            routes.MapRoute(
            name: "GetUserStories",
            url: "userStories",
            defaults: new { controller = "UserStories", action = "GetUserStories" }
        );

            routes.MapRoute(
                name: "CreateUserStory",
                url: "userStories/create",
                defaults: new { controller = "UserStories", action = "CreateUserStory" }
            );

            routes.MapRoute(
                name: "UpdateUserStory",
                url: "userStories/update/{userStoryId}",
                defaults: new { controller = "UserStories", action = "UpdateUserStory" }
            );

            routes.MapRoute(
                name: "DeleteUserStory",
                url: "userStories/{userStoryId}",
                defaults: new { controller = "UserStories", action = "DeleteUserStory" }
            );

            routes.MapRoute(
                name: "GetTask",
                url: "tasks/get/{taskId}",
                defaults: new { controller = "Tasks", action = "GetTask" }
            );

            routes.MapRoute(
                name: "GetTasks",
                url: "tasks/get/{userId}",
                defaults: new { controller = "Tasks", action = "GetTasks", userId = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "GetAllTasks",
              url: "tasks",
              defaults: new { controller = "Tasks", action = "GetAllTasks"}
          );

            routes.MapRoute(
                name: "CreateTask",
                url: "tasks/create",
                defaults: new { controller = "Tasks", action = "CreateTask" }
            );

            routes.MapRoute(
                name: "UpdateTask",
                url: "tasks/update/{taskId}",
                defaults: new { controller = "Tasks", action = "UpdateTask" }
            );

            routes.MapRoute(
                name: "DeleteTask",
                url: "tasks/delete/{taskId}",
                defaults: new { controller = "Tasks", action = "DeleteTask" }
            );

            routes.MapRoute(
                name: "GetUser",
                url: "users/{userName}",
                defaults: new { controller = "User", action = "getUser" }
            );

            routes.MapRoute(
                name: "GetUsers",
                url: "users",
                defaults: new { controller = "User", action = "GetUsers" }
            );


            routes.MapRoute(
                name: "CreateUser",
                url: "users",
                defaults: new { controller = "User", action = "CreateUser" }
            );

            routes.MapRoute(
                name: "UpdateUser",
                url: "users/update/{userId}",
                defaults: new { controller = "User", action = "UpdateUser" }
            );

            routes.MapRoute(
                name: "DeleteUser",
                url: "users/{userName}",
                defaults: new { controller = "User", action = "DeleteUser" }
            );

            routes.MapRoute(
                name: "GetComments",
                url: "comments/get/{itemId}",
                defaults: new { controller = "Comments", action = "GetComments", itemId = UrlParameter.Optional }
          );

            routes.MapRoute(
                name: "CreateComment",
                url: "comments/create",
                defaults: new { controller = "Comments", action = "CreateComment" }
          );

            routes.MapRoute(
                name: "UpdateComment",
                url: "comments/update/{itemId}",
                defaults: new { controller = "Comments", action = "UpdateComment", itemId = UrlParameter.Optional }
          );

            routes.MapRoute(
                name: "DeleteComment",
                url: "comments/delete/{itemId}",
                defaults: new { controller = "Comments", action = "DeleteComment", itemId = UrlParameter.Optional }
          );
            routes.MapRoute(
                name: "Get",
                url: "capacity/get/{iterationId}",
                defaults: new { controller = "UserCapacity", action = "Get", iterationId = UrlParameter.Optional }
          );
            routes.MapRoute(
                name: "Update",
                url: "capacity/update",
                defaults: new { controller = "UserCapacity", action = "Update" }
          );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
