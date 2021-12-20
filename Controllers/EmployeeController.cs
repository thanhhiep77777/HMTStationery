﻿using HMTStationery.App_Start;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HMTStationery.Models;
using System.Security.Claims;
using System;

namespace HMTStationery.Controllers
{
    [CustomAuthorize(Roles = "Employee")]
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
            ViewBag.Prepare = (List<PreparingStationery>)Session["prepare"];
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyRequest(Request request)
        {
            if (ModelState.IsValid)
            {
                //get preparing stationeries
                List<PreparingStationery> prepare = (List<PreparingStationery>)Session["prepare"]??new List<PreparingStationery>();
                //get user email
                var claimsIdentity = User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claims = claimsIdentity.Claims;
                string email = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();
                //Check empty preparing stationeries
                if (prepare.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, 
                        "No stationery chosen, please choose some stationery for this request");
                    return View();
                }
                //Check eligibillity
                if (db.Users.First(x => x.Email == email).Role1.Eligibility < prepare.Sum(x => x.Item.Price * x.Quantity))
                {

                }

            }

            return View();
        }
        //Check available email
        [HttpPost]
        public JsonResult IsReciverExist(string Email)
        {

            return Json(db.Users.FirstOrDefault(x => x.Email == Email && x.Role1.Name != "Admin") != null);

        }
        [HttpPost]
        public ActionResult _SearchStationery(string name)
        {
            List<Stationery> list = db.Stationeries
                .Where(x => x.Name.ToLower().Contains(name.ToLower()) && x.Stock > 0 && x.Status == 1)
                .Take(5).ToList();
            return PartialView("Partial/LiveSearch", list);
        }

        public ActionResult _LoadChosenStationery()
        {
            List<RequestDetail> list = new List<RequestDetail>();
            List<PreparingStationery> prepareList = (List<PreparingStationery>)Session["prepare"];
            if (prepareList != null)
                foreach (var item in prepareList)
                {
                    RequestDetail detail = new RequestDetail();
                    detail.Stationery = db.Stationeries.Find(item.Item.ID);
                    detail.Quantity = item.Quantity;
                    detail.Price = item.Item.Price;
                    list.Add(detail);
                }
            return PartialView("Partial/ChosenStationeries", list);
        }
        [HttpPost]
        public JsonResult _AddStationery(int id)
        {
            List<PreparingStationery> prepare = new List<PreparingStationery>();
            if (Session["prepare"] == null)
            {
                prepare.Add(new PreparingStationery { Item = db.Stationeries.Find(id), Quantity = 1 });
                Session["prepare"] = prepare;
            }
            else
            {
                prepare = (List<PreparingStationery>)Session["prepare"];
                int index = isExist(id);
                if (index != -1)
                {
                    prepare[index].Quantity++;
                }
                else
                {
                    prepare.Add(new PreparingStationery { Item = db.Stationeries.Find(id), Quantity = 1 });
                }
                Session["prepare"] = prepare;
            }
            return Json(prepare, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult _UpdateQuantity(int ID, int Quantity)
        {
            List<PreparingStationery> prepare = (List<PreparingStationery>)Session["prepare"];
            int index = isExist(ID);
            string message = "";
            if (index != -1 && Quantity > 0)
            {
                prepare[index].Quantity = Quantity;
                message = "Success";
            }
            else
            {
                message = "Failed, Invalid quantity" + ID + "-" + Quantity;
            }
            Session["prepare"] = prepare;
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult _RemoveStationery(int ID)
        {
            List<PreparingStationery> prepare = (List<PreparingStationery>)Session["prepare"];
            int index = isExist(ID);
            if (index != -1)
            {
                prepare.RemoveAt(index);
                Session["prepare"] = prepare;
            }
            return Json("OK");
        }
        private int isExist(int id)
        {
            List<PreparingStationery> prepare = (List<PreparingStationery>)Session["prepare"];
            for (int i = 0; i < prepare.Count; i++)
                if (prepare[i].Item.ID.Equals(id))
                    return i;
            return -1;
        }

        public ActionResult ViewProfile()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = claimsIdentity.Claims;
            string email = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();

            User user = db.Users.FirstOrDefault(x => x.Email == email);

            ViewBag.Name = user.Name;
            ViewBag.Email = user.Email;
            ViewBag.RoleName = user.Role1.Name;
            ViewBag.Status = user.Status;
            ViewBag.RoleDesc = user.Role1.Description;
            ViewBag.Eligibility = user.Role1.Eligibility;
            ViewBag.Superior = user.SuperiorID;
            if (user.SuperiorID >= 1)
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