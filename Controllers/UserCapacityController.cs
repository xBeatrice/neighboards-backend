using System.Collections.Generic;
using System.Web.Http.Cors;
using System.Web.Mvc;
using WebApplication3.Core;
using WebApplication3.Core.Interfaces;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class UserCapacityController : Controller
    {
        private IUserCapacityManager userCapacityManager = new UserCapacityManager();

        [HttpGet]
        [Route("capacity/get/{iterationId}")]
        public ActionResult Get(string iterationId)
        {
            var list = userCapacityManager.GetByIterationId(iterationId);

            return Json(list, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [Route("capacity/update")]
        public void Update(UserCapacity capacity)
        {
            userCapacityManager.Update(capacity);
        }
    }
}