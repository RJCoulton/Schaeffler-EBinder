using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBinder.Models;

namespace EBinder.Controllers
{
    [SessionExpire]
    public class ReferenceTypeController : Controller
    {
        private EBinderContext db = new EBinderContext();

        //
        // GET: /ReferenceType/

        public ActionResult Index()
        {
            var referencetypes = db.ReferenceTypes.Include(r => r.ReferenceCategory);
            return View(referencetypes.ToList());
        }

        //
        // GET: /ReferenceType/Create

        public ActionResult Create()
        {
            ViewBag.ReferenceCategoryID = new SelectList(db.ReferenceCategory, "ReferenceCategoryID", "CategoryName");
            return View();
        }

        //
        // POST: /ReferenceType/Create

        [HttpPost]
        public ActionResult Create(ReferenceType referencetype)
        {
            if (ModelState.IsValid)
            {
                db.ReferenceTypes.Add(referencetype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ReferenceCategoryID = new SelectList(db.ReferenceCategory, "ReferenceCategoryID", "CategoryName", referencetype.ReferenceCategoryID);
            return View(referencetype);
        }

        //
        // GET: /ReferenceType/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ReferenceType referencetype = db.ReferenceTypes.Find(id);
            if (referencetype == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReferenceCategoryID = new SelectList(db.ReferenceCategory, "ReferenceCategoryID", "CategoryName", referencetype.ReferenceCategoryID);
            return View(referencetype);
        }

        //
        // POST: /ReferenceType/Edit/5

        [HttpPost]
        public ActionResult Edit(ReferenceType referencetype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(referencetype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ReferenceCategoryID = new SelectList(db.ReferenceCategory, "ReferenceCategoryID", "CategoryName", referencetype.ReferenceCategoryID);
            return View(referencetype);
        }

        //
        // GET: /ReferenceType/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ReferenceType referencetype = db.ReferenceTypes.Find(id);
            if (referencetype == null)
            {
                return HttpNotFound();
            }
            return View(referencetype);
        }

        //
        // POST: /ReferenceType/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ReferenceType referencetype = db.ReferenceTypes.Find(id);
            db.ReferenceTypes.Remove(referencetype);
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