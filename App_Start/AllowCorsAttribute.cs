using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication3
{
    public class AllowCorsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Allow all origins (replace * with specific origins in production)
            filterContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");

            // Allow specific HTTP methods (e.g., GET, POST, OPTIONS)
            filterContext.HttpContext.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");

            // Allow specific headers (e.g., Content-Type)
            filterContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type");

            // Optional: Allow credentials (e.g., cookies) to be included in the CORS request
            // filterContext.HttpContext.Response.AddHeader("Access-Control-Allow-Credentials", "true");

            base.OnActionExecuting(filterContext);
        }
    }
}
