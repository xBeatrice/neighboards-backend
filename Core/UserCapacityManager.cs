using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using WebApplication3.Core.Interfaces;
using WebApplication3.Models;

namespace WebApplication3.Core
{
    public class UserCapacityManager : IUserCapacityManager
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

        public List<UserCapacity> GetByIterationId(string iterationId)
        {
            List<UserCapacity> userCapacityList = new List<UserCapacity>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT " +
                                          $"{nameof(UserCapacity.IterationId)}, " +
                                          $"{nameof(UserCapacity.UserId)}, " +
                                          $"{nameof(UserCapacity.Hours)} " +
                                          $"FROM UserCapacity WHERE {nameof(UserCapacity.IterationId)} = @IterationId";
                    
                    command.Parameters.AddWithValue("@IterationId", iterationId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserCapacity userCapacity = new UserCapacity
                            {
                                IterationId = reader.GetString(0),
                                UserId = reader.GetString(1),
                                Hours = reader.GetInt32(2)
                            };

                            userCapacityList.Add(userCapacity);
                        }
                    }
                }
            }

            return userCapacityList;
        }

        public void Update(UserCapacity capacity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"UPDATE UserCapacity SET " +
                                           $"{nameof(UserCapacity.Hours)} = @Hours " +
                                           $"WHERE {nameof(UserCapacity.IterationId)} = @IterationId AND {nameof(UserCapacity.UserId)} = @UserId";

                    command.Parameters.AddWithValue("@UserId", capacity.UserId);
                    command.Parameters.AddWithValue("@IterationId", capacity.IterationId);
                    command.Parameters.AddWithValue("@Hours", capacity.Hours == 0 ? 8 : capacity.Hours);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        command.CommandText = $"INSERT INTO UserCapacity (" +
                            $"{nameof(UserCapacity.IterationId)}, " +
                            $"{nameof(UserCapacity.UserId)}, " +
                            $"{nameof(UserCapacity.Hours)}) " +
                            $"VALUES (@IterationId, @UserId, @Hours)";

                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}