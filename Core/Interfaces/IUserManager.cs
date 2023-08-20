using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Core.Interfaces
{
    public interface IUserManager
    {
        void CreateUser( User user);

        void UpdateUser(string userId, User user);

        User GetUser(string userId);

        void DeleteUser(string userId);

        List<User> GetUsers();
    }
}