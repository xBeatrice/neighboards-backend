using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Task : Item
    {
        public bool IsBug { get; set; }

        public int HoursCompleted { get; set; }

        public int HoursRemaining { get; set; }

        public string AreaPath { get; set; }

        public string UserId { get; set; }

    }
}