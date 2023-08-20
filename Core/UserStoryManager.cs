using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication3.Core.Interfaces;
using WebApplication3.Models;

namespace WebApplication3.Core
{
    public class UserStoryManager : IUserStoryManager

    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
        public void CreateUserStory(UserStory userStory)
        {
            userStory.Id = Guid.NewGuid().ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO UserStories (Id, Title, DueDate, Iteration, State, Description, StoryPoints) " +
                                           "VALUES (@Id, @Title, @DueDate, @Iteration, @State, @Description, @StoryPoints)";
                    command.Parameters.AddWithValue("@Id", userStory.Id);
                    command.Parameters.AddWithValue("@Title", userStory.Title);
                    command.Parameters.AddWithValue("@DueDate", userStory.DueDate);
                    command.Parameters.AddWithValue("@State", userStory.State);
                    command.Parameters.AddWithValue("@Description", userStory.Description);
                    command.Parameters.AddWithValue("@StoryPoints", userStory.StoryPoints);
                    command.ExecuteNonQuery();

                    // Insert task IDs associated with the user story
                    if (userStory.Tasks != null && userStory.Tasks.Length > 0)
                    {
                        foreach (string taskId in userStory.Tasks)
                        {
                            using (SqlCommand taskCommand = connection.CreateCommand())
                            {
                                taskCommand.CommandText = "INSERT INTO UserStoryTasks (UserStoryId, TaskId) " +
                                                           "VALUES (@UserStoryId, @TaskId)";
                                taskCommand.Parameters.AddWithValue("@UserStoryId", userStory.Id);
                                taskCommand.Parameters.AddWithValue("@TaskId", taskId);
                                taskCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }

        public void UpdateUserStory(string userStoryId, UserStory userStoryModel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE UserStories SET Title = @Title, DueDate = @DueDate, " +
                                           "Iteration = @Iteration, State = @State, Description = @Description, " +
                                           "StoryPoints = @StoryPoints " +
                                           "WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", userStoryId);
                    command.Parameters.AddWithValue("@Title", userStoryModel.Title);
                    command.Parameters.AddWithValue("@DueDate", userStoryModel.DueDate);

                    command.Parameters.AddWithValue("@State", userStoryModel.State);
                    command.Parameters.AddWithValue("@Description", userStoryModel.Description);
                    command.Parameters.AddWithValue("@StoryPoints", userStoryModel.StoryPoints);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"UserStory with ID {userStoryId} does not exist.");
                    }

                    // Update task associations for the user story
                    if (userStoryModel.Tasks != null)
                    {
                        // Remove existing task associations
                        using (SqlCommand deleteCommand = connection.CreateCommand())
                        {
                            deleteCommand.CommandText = "DELETE FROM UserStoryTasks WHERE UserStoryId = @UserStoryId";
                            deleteCommand.Parameters.AddWithValue("@UserStoryId", userStoryId);
                            deleteCommand.ExecuteNonQuery();
                        }

                        // Insert new task associations
                        foreach (string taskId in userStoryModel.Tasks)
                        {
                            using (SqlCommand taskCommand = connection.CreateCommand())
                            {
                                taskCommand.CommandText = "INSERT INTO UserStoryTasks (UserStoryId, TaskId) " +
                                                           "VALUES (@UserStoryId, @TaskId)";
                                taskCommand.Parameters.AddWithValue("@UserStoryId", userStoryId);
                                taskCommand.Parameters.AddWithValue("@TaskId", taskId);
                                taskCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }

        public UserStory GetUserStory(string userStoryId)
        {
            UserStory userStory = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Title, IsBug, DueDate, UserId, HoursCompleted, HoursRemaining, " +
                                          "Iteration, State, AreaPath, Description FROM UserStories WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", userStoryId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userStory = new UserStory
                            {
                                Id = reader.GetString(0),
                                Title = reader.GetString(1),
                                DueDate = reader.GetDateTime(3),
                                State = reader.GetString(8),
                                Description = reader.GetString(10),
                                StoryPoints = reader.GetInt32(6)
                            };
                        }
                    }
                }
            }
            return userStory;
        }

        public void DeleteUserStory(string userStoryId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM UserStories WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", userStoryId);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"UserStory with ID {userStoryId} does not exist.");
                    }
                }
            }
        }
        public List<UserStory> GetUserStories()
        {
            List<UserStory> userStories = new List<UserStory>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Title, DueDate, StoryPoints, State, Description, Tasks FROM UserStories";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserStory userStory = new UserStory
                            {
                                Id = reader.GetString(0),
                                Title = reader.GetString(1),
                                DueDate = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
                                StoryPoints = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                                State = reader.GetString(4),
                                Description = reader.GetString(5),
                                Tasks = reader.GetString(6).Split(',')
                            };

                            userStories.Add(userStory);
                        }
                    }
                }
            }

            return userStories;
        }
    }
}