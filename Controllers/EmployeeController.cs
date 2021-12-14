using HMTStationery.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HMTStationery.Controllers
{
    //[CustomAuthorize(Roles ="Employee")]
    public class EmployeeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ApplyRequest()
        {

            return View();
        }
        public ActionResult ViewProfile()
        {
            
            return View();
        }
    }
}