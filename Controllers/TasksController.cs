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
    public class TasksController : Controller
    {
        private ITaskManager taskManager = new TaskManager();

        [HttpGet]
        [Route("tasks/get/{taskId}")]
        public Task GetTask(string taskId)
        {
            return taskManager.GetTask(taskId);
        }

        [HttpGet]
        [Route("tasks/get/{userId}")]
        public ActionResult GetTasks(string userId)
        {
            List<Task> tasks = taskManager.GetTasks(userId);
            return Json(tasks, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("tasks/get")]
        public ActionResult GetAllTasks()
        {
            List<Task> tasks = taskManager.GetAllTasks();
            return Json(tasks, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Route("tasks/create")]
        public void CreateTask(Task task)
        {
            taskManager.CreateTask(task);
        }

        [HttpPost]
        [Route("tasks/update/{taskId}")]
        public void UpdateTask(string taskId, Task task)
        {
            taskManager.UpdateTask(taskId, task);
        }

        [HttpGet]
        [Route("tasks/delete/{taskId}")]
        public void DeleteTask(string taskId)
        {
            taskManager.DeleteTask(taskId);
        }
    }
}