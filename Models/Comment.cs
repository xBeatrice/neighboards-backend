using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Comment
    {
        // primary key
        public string Id { get; set; }

        // foreign key to Item table
        public string ItemId { get; set; } // representing TaskId or UserStoryId

        public string UserId { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }

        public bool IsEdited { get; set; }
    }
}