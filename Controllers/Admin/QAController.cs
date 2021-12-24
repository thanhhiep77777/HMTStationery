﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Mvc;
using HMTStationery.Models;

namespace HMTStationery.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class QAController : Controller
    {
        private HMT_StationeryMntEntities db = new HMT_StationeryMntEntities();
        public int GetUserID()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = claimsIdentity.Claims;
            string givenName = claims.Where(c => c.Type == ClaimTypes.GivenName)
                .Select(c => c.Value).SingleOrDefault();
            return int.Parse(givenName);
        }
        public string GetUserEmail()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = claimsIdentity.Claims;
            string email = claims.Where(c => c.Type == ClaimTypes.Email)
                .Select(c => c.Value).SingleOrDefault();
            return email;
        }
        // GET: QA
        public ActionResult Index()
        {
            var QAs = db.QAs;
            return View(QAs.ToList());
        }

        // GET: QA/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QA qA = db.QAs.Find(id);
            if (qA == null)
            {
                return HttpNotFound();
            }
            return View(qA);
        }

        // GET: QA/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: QA/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserID,Date,Question,Answer,Status")] QA qA)
        {
            if (ModelState.IsValid)
            {
                qA.UserID = GetUserID();
                qA.Date = System.DateTime.Now;
                db.QAs.Add(qA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "ID", "Name", qA.UserID);
            return View(qA);
        }

        // GET: QA/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QA qA = db.QAs.Find(id);
            if (qA == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "ID", "Name", qA.UserID);
            return View(qA);
        }

        // POST: QA/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserID,Date,Question,Answer,Status")] QA qA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(qA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "ID", "Name", qA.UserID);
            return View(qA);
        }

        // GET: QA/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QA qA = db.QAs.Find(id);
            if (qA == null)
            {
                return HttpNotFound();
            }
            return View(qA);
        }

        // POST: QA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QA qA = db.QAs.Find(id);
            db.QAs.Remove(qA);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
