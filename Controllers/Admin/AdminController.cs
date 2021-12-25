using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HMTStationery.Models;

namespace HMTStationery.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private HMT_StationeryMntEntities db = new HMT_StationeryMntEntities();
        // GET: Admin
        public ActionResult Index()
        {

            ViewBag.UserNumber = db.Users.Count();
            ViewBag.StationeryNumber = db.Stationeries.Count();
            return View();
        }
    }
}