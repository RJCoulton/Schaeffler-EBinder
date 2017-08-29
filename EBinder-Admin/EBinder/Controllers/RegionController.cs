using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBinder.Models;
using EBinder.App_GlobalResources;

namespace EBinder.Controllers
{
    [SessionExpire]
    public class RegionController : BaseController
    {
        private EbinderContext db = new EbinderContext();

        //
        // GET: /Region/

        public ActionResult Index()
        {
            return View(db.Regions.ToList());
        }

        //
        // GET: /Region/Create

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Links(int id = 0)
        {
            Region region = db.Regions.Find(id);
            ViewData["Links"] = region.References.OrderBy(a => a.ReferenceType.TypeName_en).ToList();
            if (region == null)
            {
                return HttpNotFound();
            }
            return View(region);
        }

        public ActionResult RemoveLink(int RefId, int regionId)
        {
            Region region = db.Regions.Find(regionId);
            Reference reference = db.References.Find(RefId);

            if (region != null && reference != null)
            {
                region.References.Remove(reference);
                db.SaveChanges();
                return RedirectToAction("Links", new { id = regionId });
            }
            TempData["message"] = Resource.DeleteRefError;
            return RedirectToAction("Links", new { id = regionId });
        }
        //
        // POST: /Region/Create

        [HttpPost]
        public ActionResult Create(Region region)
        {
            if (ModelState.IsValid)
            {
                db.Regions.Add(region);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(region);
        }

        //
        // GET: /Region/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Region region = db.Regions.Find(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return View(region);
        }

        //
        // POST: /Region/Edit/5

        [HttpPost]
        public ActionResult Edit(Region region)
        {
            if (ModelState.IsValid)
            {
                db.Entry(region).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(region);
        }

        //
        // GET: /Region/Delete/5

        public ActionResult Delete(int id = 0)
        {
            /*Region region = db.Regions.Find(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return View(region);*/
            Region region = db.Regions.Find(id);
            db.Regions.Remove(region);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // POST: /Region/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Region region = db.Regions.Find(id);
            db.Regions.Remove(region);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}