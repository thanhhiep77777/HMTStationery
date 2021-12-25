using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HMTStationery.General;
using HMTStationery.Models;
using PagedList;

namespace HMTStationery.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class StationeryController : Controller
    {
        HMT_StationeryMntEntities db = new HMT_StationeryMntEntities();
        // GET: Stationery
        public ActionResult Index(string SearchString)
        {            
            var lstStat = new List<Stationery>();
            if (!string.IsNullOrEmpty(SearchString))
            {
                lstStat = db.Stationeries.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstStat = db.Stationeries.ToList();
            }
            return View(lstStat);
        }
       
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(StationeryStatus)).Cast<StationeryStatus>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");
            return View();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Stationery objStat)
        {
            
            if (ModelState.IsValid)
            {
                HttpPostedFileBase postedfile = objStat.ImageUpload;
                Bitmap SocialMedia = new Bitmap(postedfile.InputStream);
                string ext = Path.GetExtension(postedfile.FileName);
                string fileName = "";
                if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                {
                    string uniqueNumber = Guid.NewGuid().ToString();
                    fileName = uniqueNumber + postedfile.FileName;
                    SocialMedia.Save(Server.MapPath("~/Storage/Images/" + fileName));
                    objStat.Image = fileName;
                }
                else
                {
                    ModelState.AddModelError("", "File type not allowed (Must be jpg,jpeg,png,gif.)");
                    return View();
                }

                try
                {
                    db.Stationeries.Add(objStat);
                    db.SaveChanges();
                    
                }
                catch (Exception)
                {
                    return View();
                }
            }
            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objStat = db.Stationeries.Where(n => n.ID == id).FirstOrDefault();
            return View(objStat);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objStat = db.Stationeries.Where(n => n.ID == id).FirstOrDefault();
            return View(objStat);
        }

        [HttpPost]
        public ActionResult Delete(Stationery objsta)
        {
            var objStat = db.Stationeries.Where(n => n.ID == objsta.ID).FirstOrDefault();

            db.Stationeries.Remove(objStat);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objStat = db.Stationeries.Where(n => n.ID == id).FirstOrDefault();

            ViewBag.Status = new SelectList(Enum.GetValues(typeof(StationeryStatus)).Cast<StationeryStatus>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text", objStat.Status);
            return View(objStat);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id,Stationery objsta)
        {
            string currentImagePath = objsta.Image;
            if (objsta.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objsta.ImageUpload.FileName);
                string extension = Path.GetExtension(objsta.ImageUpload.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objsta.Image = fileName;
                objsta.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Storage/Images/"), fileName));
                
                string currentFileName = ("~/Storage/Images/" + currentImagePath);
                if (currentFileName != null || currentFileName != string.Empty)
                {
                    if ((System.IO.File.Exists(currentFileName)))
                    {
                        System.IO.File.Delete(currentFileName);
                    }

                }
            }
            db.Entry(objsta).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}