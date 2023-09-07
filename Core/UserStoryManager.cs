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
                    command.CommandText = $"INSERT INTO UserStories (" +
                                          $"{nameof(UserStory.Id)}, " +
                                          $"{nameof(UserStory.Title)}, " +
                                          $"{nameof(UserStory.State)}, " +
                                          $"{nameof(UserStory.Description)}, " +
                                          $"{nameof(UserStory.StoryPoints)}) " +
                                          "VALUES (@Id, @Title, @State, @Description, @StoryPoints)";

                    userStory.Tasks.ForEach(taskId =>
                    {
                        command.CommandText += $"\nINSERT INTO UserStoryTaskLink (UserStoryId, TaskId) VALUES (@Id, {taskId})";
                    });

                    command.Parameters.AddWithValue("@Id", userStory.Id);
                    command.Parameters.AddWithValue("@Title", userStory.Title);
                    command.Parameters.AddWithValue("@State", userStory.State);
                    command.Parameters.AddWithValue("@Description", userStory.Description);
                    command.Parameters.AddWithValue("@StoryPoints", userStory.StoryPoints);

                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = connection.CreateCommand())
                {
                    string commandText = "";

                    userStory.Tasks.ForEach(taskId =>
                    {
                        commandText += $"UPDATE Tasks SET UserStoryId = @Id WHERE TaskId = {taskId}\n";
                    });

                    command.CommandText = commandText;

                    command.Parameters.AddWithValue("@Id", userStory.Id);

                    command.ExecuteNonQuery();
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
                    var commandText = $"UPDATE UserStories SET " +
                                          $"{nameof(UserStory.Title)} = @Title, " +
                                          $"{nameof(UserStory.State)} = @State, " +
                                          $"{nameof(UserStory.Description)} = @Description, " +
                                          $"{nameof(UserStory.StoryPoints)} = @StoryPoints " +
                                          $"WHERE {nameof(UserStory.Id)} = @Id";

                    // Clear existing task associations for the user story
                    commandText += "\nUPDATE Tasks SET UserStoryId = null WHERE UserStoryId = @Id";

                    // Insert new task associations
                    userStoryModel.Tasks?.ForEach(taskId =>
                    {
                        commandText += $"\nUPDATE Tasks SET UserStoryId = @Id WHERE Id = {taskId}";
                    });

                    command.CommandText = commandText;

                    command.Parameters.AddWithValue("@Id", userStoryId);
                    command.Parameters.AddWithValue("@Title", userStoryModel.Title);
                    command.Parameters.AddWithValue("@State", userStoryModel.State);
                    command.Parameters.AddWithValue("@Description", userStoryModel.Description);
                    command.Parameters.AddWithValue("@StoryPoints", userStoryModel.StoryPoints);

                    command.ExecuteNonQuery();
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
                    command.CommandText = "SELECT Id, Title, IsBug, DueDate, UserId, HoursCompleted, HoursRemaining, Iteration, State, AreaPath, Description FROM UserStories WHERE Id = @Id";

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
                                StoryPoints = reader.GetInt32(6),
                                Tasks = new List<string>(),
                                Comments = new List<Comment>()
                            };
                        }
                    }
                }

                AddTasksToUserStory(connection, userStory);
                AddCommentsToUserStory(connection, userStory);
            }

            return userStory;
        }

        public void DeleteUserStory(string userStoryId)
        {
            // First remove Comments
            // delete from comments where ItemId = userStoryId

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Comments WHERE ItemId = @Id\n";

                    command.CommandText = "UPDATE Tasks SET UserStoryId = null WHERE UserStoryId = @Id";
                    
                    command.CommandText += "\nDELETE FROM UserStories WHERE Id = @Id";

                    command.Parameters.AddWithValue("@Id", userStoryId);

                    command.ExecuteNonQuery();
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
                    command.CommandText = $"SELECT " +
                     $"{nameof(UserStory.Id)}, " +
                     $"{nameof(UserStory.Title)}, " +
                     $"{nameof(UserStory.DueDate)}, " +
                     $"{nameof(UserStory.StoryPoints)}, " +
                     $"{nameof(UserStory.State)}, " +
                     $"{nameof(UserStory.Description)} " +
                     "FROM UserStories";


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
                                Tasks = new List<string>(),
                                Comments = new List<Comment>()
                            };


                            userStories.Add(userStory);
                        }
                    }
                }

                userStories.ForEach(userStory =>
                {
                    AddTasksToUserStory(connection, userStory);
                    AddCommentsToUserStory(connection, userStory);
                });
            }

            return userStories;
        }

        private void AddTasksToUserStory(SqlConnection connection, UserStory userStory)
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Id FROM Tasks WHERE UserStoryId = @Id";

                command.Parameters.AddWithValue("@Id", userStory.Id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userStory.Tasks.Add(reader.GetString(0));
                    }
                }
            }
        }

        private void AddCommentsToUserStory(SqlConnection connection, UserStory userStory)
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT " +
                     $"{nameof(Comment.Id)}, " +
                     $"{nameof(Comment.ItemId)}, " +
                     $"{nameof(Comment.Content)}, " +
                     $"{nameof(Comment.Date)}, " +
                     $"{nameof(Comment.IsEdited)} " +
                     "FROM Comments";

                command.CommandText += " WHERE ItemId = @Id";

                command.Parameters.AddWithValue("@Id", userStory.Id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Comment comment = new Comment()
                        {
                            Id = reader.GetString(0),
                            ItemId = reader.GetString(1),
                            Content = reader.GetString(2),
                            Date = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3),
                            IsEdited = reader.GetBoolean(4)
                        };

                        userStory.Comments.Add(comment);
                    }
                }
            }
        }
    }
}