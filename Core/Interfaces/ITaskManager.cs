using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Core.Interfaces
{
    public interface ITaskManager
    {
        void CreateTask(Task task);

        void UpdateTask(string taskId, Task task);

        Task GetTask(string taskId);

        void DeleteTask(string taskId);

        List<Task> GetTasks(string userId);

        List<Task> GetAllTasks();
    }
}