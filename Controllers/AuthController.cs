using HMTStationery.Models;
using System;

using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

using System.Web.Security;
using System.Text;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Security.Claims;
using Microsoft.Owin.Security;
using HMTStationery.General;
using HMTStationery.App_Start;
using System.Collections.Generic;
using HMTStationery.Hubs;
using System.Collections.Concurrent;
using System.Runtime.Remoting.Contexts;
using Microsoft.AspNet.SignalR;

namespace HMTStationery.Controllers
{
    public class AuthController : Controller
    {
        private HMT_StationeryMntEntities db = new HMT_StationeryMntEntities();
        private static ConcurrentDictionary<string, string> clients = new ConcurrentDictionary<string, string>();
        // GET: Auth
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                User user = db.Users.FirstOrDefault(x => x.Email == email && x.Status == (int)UserStatus.ENABLE);
                if (user == null)
                {
                    ViewBag.Message = "Email is not exist";
                    return View();
                }
                else if (user.Password != EncryptPassword(password, email))
                {
                    ViewBag.Message = "Password is not correct";
                    return View();
                }
                else
                {
                    
                    string role = user.Role1.Name == "Admin" ? "Admin" : "Employee";
                    var ident = new ClaimsIdentity(
                      new[] { 
                          // adding following 2 claim just for supporting default antiforgery provider
                          new Claim(ClaimTypes.NameIdentifier, email),
                          new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

                          new Claim(ClaimTypes.Name,user.Name),
                          new Claim(ClaimTypes.Email,email),
                          new Claim(ClaimTypes.GivenName,user.ID.ToString()),
                          // optionally you could add roles if any
                          new Claim(ClaimTypes.Role, role),
                      },
                      DefaultAuthenticationTypes.ApplicationCookie);
                    //Login
                    HttpContext.GetOwinContext().Authentication.SignIn(
                      new AuthenticationProperties { IsPersistent = false }, ident);
                    //Connect to signalr
                   
                   
                    return Redirect($"~/{(returnUrl!="" ?returnUrl:role)}"); // auth succeed 
                }
            }
            return View();
        }
        [CustomAuthorize(Roles = "Admin,Employee")]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [CustomAuthorize(Roles = "Admin,Employee")]
        public ActionResult ChangePassword(string Password, string NewPassword)
        {

            var claimsIdentity = User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = claimsIdentity.Claims;
            string email = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();

            string hashPassword = EncryptPassword(Password, email);
            User user = db.Users.FirstOrDefault(x => x.Password == hashPassword);
            if (user == null)
            {
                ViewBag.Message = "Incorect curent password";
                return View();
            }
            else
            {
                try
                {
                    user.Password = EncryptPassword(NewPassword, email);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    ViewBag.Message = "Unknown error";
                    return View();
                }

            }
            ViewBag.Message = "Password changed successfully";
            return View();
        }
        public ActionResult AccessDenied()
        {
            return View();
        }
        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Employee");
        }
        //public ActionResult TestPassword(string email, string pass)
        //{
        //    return Content(EncryptPassword(pass, email));
        //}
        public static string EncryptPassword(string password, string saltorusername)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = string.Format("{0}{1}", saltorusername, password);
                byte[] saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
                return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordAsBytes));
            }
        }
    }
}