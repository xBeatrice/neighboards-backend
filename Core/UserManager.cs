using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using WebApplication3.Core.Interfaces;
using WebApplication3.Models;

namespace WebApplication3.Core
{
    public class UserManager : IUserManager
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

        public void CreateUser(User user)
        {
            user.Id = Guid.NewGuid().ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO Users ({nameof(User.Id)}, {nameof(User.Name)}, {nameof(User.Activity)}) " +
                                          $"VALUES (@Id, @Name, @Activity)";

                    command.Parameters.AddWithValue("@Id", user.Id);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Activity", user.Activity);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateUser(string userId, User userModel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"UPDATE Users SET {nameof(User.Name)} = @Name, {nameof(User.Activity)} = @Activity " +
                                          $"WHERE {nameof(User.Id)} = @Id";

                    command.Parameters.AddWithValue("@Id", userId);
                    command.Parameters.AddWithValue("@Name", userModel.Name);
                    command.Parameters.AddWithValue("@Activity", userModel.Activity);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"User with ID {userId} does not exist.");
                    }
                }
            }
        }

        public User GetUser(string userId)
        {
            User user = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT {nameof(User.Id)}, {nameof(User.Name)}, {nameof(User.Activity)} " +
                                          $"FROM Users WHERE {nameof(User.Id)} = @Id";

                    command.Parameters.AddWithValue("@Id", userId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Id = reader.GetString(0),
                                Name = reader.GetString(1),
                                Activity = reader.GetString(2)
                            };
                        }
                    }
                }
            }
            return user;
        }

        public void DeleteUser(string userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"DELETE FROM Users WHERE {nameof(User.Id)} = @Id";

                    command.Parameters.AddWithValue("@Id", userId);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"User with ID {userId} does not exist.");
                    }
                }
            }
        }

        public List<User> GetUsers() // Change the return type to List<User>
        {
            List<User> users = new List<User>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT {nameof(User.Id)}, {nameof(User.Name)}, {nameof(User.Activity)} FROM Users";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {
                                Id = reader.GetString(0),
                                Name = reader.GetString(1),
                                Activity = reader.GetString(2)
                            };

                            users.Add(user);
                        }
                    }
                }
            }

            return users; // Return the list of users directly
        }
    }
}
