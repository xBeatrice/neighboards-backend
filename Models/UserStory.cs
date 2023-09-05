using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class UserStory
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public int StoryPoints { get; set; } // New property for StoryPoints
        public string State { get; set; }
        public string Description { get; set; }
        public List<string> Tasks { get; set; } // New property for Tasks array
    }

}