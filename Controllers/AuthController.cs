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

namespace HMTStationery.Controllers
{
    public class AuthController : Controller
    {
        private HMT_StationeryMntEntities db = new HMT_StationeryMntEntities();
        // GET: Auth
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string email, string password, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.FirstOrDefault(x => x.Email == email);
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

                          new Claim(ClaimTypes.Name,email),

                          // optionally you could add roles if any
                          new Claim(ClaimTypes.Role, role),


                      },
                      DefaultAuthenticationTypes.ApplicationCookie);

                     HttpContext.GetOwinContext().Authentication.SignIn(
                       new AuthenticationProperties { IsPersistent = false }, ident);
                    return Redirect($"~/{returnUrl??""}"); // auth succeed 
                }
            }
            return View();
        }
        public ActionResult AccessDenied()
        {
            return View();
        }
        public ActionResult Logout()
        {
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
       public ActionResult TestPassword(string email, string pass)
        {
            return Content(EncryptPassword(pass, email));
        }
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