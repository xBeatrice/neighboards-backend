using System;
using System.Collections.Generic;
using WebApplication3.Models;

namespace WebApplication3.Core.Interfaces
{
    public interface IUserCapacityManager
    {
        List<UserCapacity> GetByIterationId(string iterationId);

        void Update(UserCapacity capacity);
    }
}