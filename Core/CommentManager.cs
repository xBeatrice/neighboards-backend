using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using WebApplication3.Core.Interfaces;
using WebApplication3.Models;

namespace WebApplication3.Core
{
    public class CommentManager : ICommentManager
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

        public void CreateComment(Comment comment)
        {
            comment.Id = Guid.NewGuid().ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO Comments (" +
                        $"{nameof(Comment.Id)}, " +
                        $"{nameof(Comment.ItemId)}, " +
                        $"{nameof(Comment.UserId)}, " +
                        $"{nameof(Comment.Content)}, " +
                        $"{nameof(Comment.Date)}, " +
                        $"{nameof(Comment.IsEdited)}) " +
                        "VALUES (@Id, @ItemId, @UserId, @Content, @Date, @IsEdited)";

                    command.Parameters.AddWithValue("@Id", comment.Id);
                    command.Parameters.AddWithValue("@ItemId", comment.ItemId);
                    command.Parameters.AddWithValue("@UserId", comment.UserId);
                    command.Parameters.AddWithValue("@Content", comment.Content);
                    command.Parameters.AddWithValue("@Date", comment.Date);
                    command.Parameters.AddWithValue("@IsEdited", comment.IsEdited);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateComment(string commentId, Comment comment)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"UPDATE Comments SET " +
                        $"{nameof(Comment.Content)} = @Content, " +
                        $"{nameof(Comment.IsEdited)} = @IsEdited " +
                        $"WHERE {nameof(Comment.Id)} = @Id";

                    command.Parameters.AddWithValue("@Id", commentId);
                    command.Parameters.AddWithValue("@Content", comment.Content);
                    command.Parameters.AddWithValue("@IsEdited", comment.IsEdited);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"Comment with ID {commentId} does not exist.");
                    }
                }
            }
        }

        public void DeleteComment(string commentId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"DELETE FROM Comments WHERE {nameof(Comment.Id)} = @Id";
                    command.Parameters.AddWithValue("@Id", commentId);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"Comment with ID {commentId} does not exist.");
                    }
                }
            }
        }

        public List<Comment> GetComments(string itemId)
        {
            List<Comment> comments = new List<Comment>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT {nameof(Comment.Id)}, " +
                        $"{nameof(Comment.ItemId)}, " +
                        $"{nameof(Comment.UserId)}, " +
                        $"{nameof(Comment.Content)}, " +
                        $"{nameof(Comment.Date)}, " +
                        $"{nameof(Comment.IsEdited)} " +
                        $"FROM Comments WHERE {nameof(Comment.ItemId)} = @ItemId";

                    command.Parameters.AddWithValue("@ItemId", itemId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Comment comment = new Comment
                            {
                                Id = reader.GetString(0),
                                ItemId = reader.GetString(1),
                                UserId = reader.GetString(2),
                                Content = reader.GetString(3),
                                Date = reader.GetDateTime(4),
                                IsEdited = reader.GetBoolean(5)
                            };

                            comments.Add(comment);
                        }
                    }
                }
            }

            return comments;
        }
    }
}
