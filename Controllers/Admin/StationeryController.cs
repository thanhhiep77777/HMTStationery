using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HMTStationery.Models;
using PagedList;

namespace HMTStationery.Controllers.Admin
{
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
        /*public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            var lstStat = new List<Stationery>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                lstStat = db.Stationeries.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstStat = db.Stationeries.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            lstStat = lstStat.OrderByDescending(n => n.ID).ToList();
            return View(lstStat.ToPagedList(pageNumber,pageSize));
        }*/

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Stationery objStat)
        {
            try
            {
                if (objStat.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objStat.ImageUpload.FileName);
                    string extension = Path.GetExtension(objStat.ImageUpload.FileName);
                    fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                    objStat.Image = fileName;
                    objStat.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));

                }
                db.Stationeries.Add(objStat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

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
            return View(objStat);
        }

        [HttpPost]
        public ActionResult Edit(int id,Stationery objsta)
        {
            if (objsta.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objsta.ImageUpload.FileName);
                string extension = Path.GetExtension(objsta.ImageUpload.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objsta.Image = fileName;
                objsta.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));

            }
            db.Entry(objsta).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        /* public ActionResult Create(Stationery objStat)
         {
             try
             {
                 HttpPostedFileBase postedfile = objStat.ImageUpload;
                 Bitmap SocialMedia = new Bitmap(postedfile.InputStream);
                 string ext = Path.GetExtension(postedfile.FileName);
                 string fileName = "";
                 if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                 {
                     string uniqueNumber = Guid.NewGuid().ToString();
                     fileName = uniqueNumber + postedfile.FileName;
                     SocialMedia.Save(Server.MapPath("~/Content/images/" + fileName));
                     objStat.Image = fileName;
                     db.Stationeries.Add(objStat);
                     db.SaveChanges();
                 }
                 return RedirectToAction("Index");
             }                
             catch (Exception)
             {
                 return View();
             }

         }*/
    }
}