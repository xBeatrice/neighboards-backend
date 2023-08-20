using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication3.Core.Interfaces
{
    public interface IFileManager 
    {
        string GetContent(string groupName);

        void WriteContent(string groupName, string content);
    }
}