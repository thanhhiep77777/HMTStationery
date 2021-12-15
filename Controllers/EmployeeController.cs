using HMTStationery.App_Start;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HMTStationery.Models;
using System.Security.Claims;

namespace HMTStationery.Controllers
{
    [CustomAuthorize(Roles ="Employee")]
    public class EmployeeController : Controller
    {
        private HMT_StationeryMntEntities db = new HMT_StationeryMntEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult QA()
        {
            return View();
        }
       
        public ActionResult ApplyRequest()
        {

            return View();
        }
        public ActionResult ViewProfile()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = claimsIdentity.Claims;           
            string email = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();

            User user = db.Users.FirstOrDefault(x=>x.Email==email);

            ViewBag.Name = user.Name;
            ViewBag.Email = user.Email;
            ViewBag.RoleName = user.Role1.Name;
            ViewBag.Status = user.Status;
            ViewBag.RoleDesc = user.Role1.Description;
            ViewBag.Eligibility = user.Role1.Eligibility;
            ViewBag.Superior = user.SuperiorID;
            if (user.SuperiorID>=1)
            {
                User supUser = db.Users.FirstOrDefault(x => x.ID == user.SuperiorID);
                ViewBag.SupName = supUser.Name;
                ViewBag.SupEmail = supUser.Email;
                ViewBag.SupRole = supUser.Role1.Name;
            }
            return View();
        }
        
    }
}