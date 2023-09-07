using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Item
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime DueDate { get; set; }

        public List<Comment> Comments { get; set; }

        public int Iteration { get; set; }

        public string State { get; set; }

        public string Description { get; set; }
    }
}
