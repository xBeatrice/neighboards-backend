using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class UserStory : Item
    {
        public int StoryPoints { get; set; } // New property for StoryPoints

        public List<string> Tasks { get; set; } // New property for Tasks array
    }

}