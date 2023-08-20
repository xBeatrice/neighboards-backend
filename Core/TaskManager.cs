using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebApplication3.Core.Interfaces;
using WebApplication3.Models;

namespace WebApplication3.Core
{
    public class TaskManager : ITaskManager
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

        public void CreateTask(Task task)
        {
            task.Id = Guid.NewGuid().ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Tasks (Id, Title, IsBug, DueDate, UserId, HoursCompleted, HoursRemaining, Iteration, State, AreaPath, Description) " +
                                          "VALUES (@Id, @Title, @IsBug, @DueDate, @UserId, @HoursCompleted, @HoursRemaining, @Iteration, @State, @AreaPath, @Description)";
                    command.Parameters.AddWithValue("@Id", task.Id);
                    command.Parameters.AddWithValue("@Title", task.Title);
                    command.Parameters.AddWithValue("@IsBug", task.IsBug);
                    command.Parameters.AddWithValue("@DueDate", task.DueDate);
                    command.Parameters.AddWithValue("@UserId", task.UserId);
                    command.Parameters.AddWithValue("@HoursCompleted", task.HoursCompleted);
                    command.Parameters.AddWithValue("@HoursRemaining", task.HoursRemaining);
                    command.Parameters.AddWithValue("@Iteration", task.Iteration);
                    command.Parameters.AddWithValue("@State", task.State);
                    command.Parameters.AddWithValue("@AreaPath", task.AreaPath);
                    command.Parameters.AddWithValue("@Description", task.Description);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateTask(string taskId, Task taskModel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Tasks SET Title = @Title, IsBug = @IsBug, DueDate = @DueDate, " +
                                          "UserId = @UserId, HoursCompleted = @HoursCompleted, HoursRemaining = @HoursRemaining, " +
                                          "Iteration = @Iteration, State = @State, AreaPath = @AreaPath, Description = @Description " +
                                          "WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", taskId);
                    command.Parameters.AddWithValue("@Title", taskModel.Title);
                    command.Parameters.AddWithValue("@IsBug", taskModel.IsBug);
                    command.Parameters.AddWithValue("@DueDate", taskModel.DueDate);
                    command.Parameters.AddWithValue("@UserId", taskModel.UserId);
                    command.Parameters.AddWithValue("@HoursCompleted", taskModel.HoursCompleted);
                    command.Parameters.AddWithValue("@HoursRemaining", taskModel.HoursRemaining);
                    command.Parameters.AddWithValue("@Iteration", taskModel.Iteration);
                    command.Parameters.AddWithValue("@State", taskModel.State);
                    command.Parameters.AddWithValue("@AreaPath", taskModel.AreaPath);
                    command.Parameters.AddWithValue("@Description", taskModel.Description);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"Task with ID {taskId} does not exist.");
                    }
                }
            }
        }

        public Task GetTask(string taskId)
        {
            Task task = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Title, IsBug, DueDate, UserId, HoursCompleted, HoursRemaining, " +
                                          "Iteration, State, AreaPath, Description FROM Tasks WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", taskId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            task = new Task
                            {
                                Id = reader.GetString(0),
                                Title = reader.GetString(1),
                                IsBug = reader.GetBoolean(2),
                                DueDate = reader.GetDateTime(3),
                                UserId = reader.GetString(4),
                                HoursCompleted = reader.GetInt32(5),
                                HoursRemaining = reader.GetInt32(6),
                                Iteration = reader.GetInt32(7),
                                State = reader.GetString(8),
                                AreaPath = reader.GetString(9),
                                Description = reader.GetString(10)
                            };
                        }
                    }
                }
            }
            return task;
        }

        public void DeleteTask(string taskId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Tasks WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", taskId);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"Task with ID {taskId} does not exist.");
                    }
                }
            }
        }

        public List<Task> GetTasks(string userId)
        {
            List<Task> tasks = new List<Task>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Title, IsBug, DueDate, HoursCompleted, HoursRemaining, Iteration, State, AreaPath, Description " +
                                          "FROM Tasks " +
                                          "WHERE UserId = @UserId";
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Task task = new Task
                            {
                                Id = reader.GetString(0),
                                Title = reader.GetString(1),
                                IsBug = reader.GetBoolean(2),
                                DueDate = reader.GetDateTime(3),
                                HoursCompleted = reader.GetInt32(4),
                                HoursRemaining = reader.GetInt32(5),
                                Iteration = reader.GetInt32(6),
                                State = reader.GetString(7),
                                AreaPath = reader.GetString(8),
                                Description = reader.GetString(9)
                            };

                            tasks.Add(task);
                        }
                    }
                }
            }

            return tasks;
        }
        public List<Task> GetAllTasks()
        {
            List<Task> tasks = new List<Task>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Title, IsBug, DueDate, UserId, HoursCompleted, HoursRemaining, Iteration, State, AreaPath, Description, Comments " +
                                          "FROM Tasks";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Task task = new Task
                            {
                                Id = reader.GetString(0),
                                Title = reader.GetString(1),
                                IsBug = reader.GetBoolean(2),
                                DueDate = reader.GetDateTime(3),
                                UserId = reader.IsDBNull(4) ? null : reader.GetString(4), // Handle nullable column
                                HoursCompleted = reader.GetInt32(5),
                                HoursRemaining = reader.GetInt32(6),
                                Iteration = reader.GetInt32(7),
                                State = reader.GetString(8),
                                AreaPath = reader.GetString(9),
                                Description = reader.GetString(10),
                                Comments = new List<Comment>() // Initialize the comments list
                            };

                            if (!reader.IsDBNull(11))
                            {
                                string commentsJson = reader.GetString(11);
                                List<Comment> comments = JsonConvert.DeserializeObject<List<Comment>>(commentsJson);
                                task.Comments = comments; // Assign the 'comments' list to the task property.
                            }

                            tasks.Add(task);
                        }
                    }
                }
            }

            return tasks;
        }


    }
}
