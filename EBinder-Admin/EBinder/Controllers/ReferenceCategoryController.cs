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
    public class ReferenceCategoryController : BaseController
    {
        private EbinderContext db = new EbinderContext();

        //
        // GET: /ReferenceCategory/

        public ActionResult Index()
        {
            return View(db.ReferenceCategories.ToList());
        }

        //
        // GET: /ReferenceCategory/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ReferenceCategory/Create

        [HttpPost]
        public ActionResult Create(ReferenceCategory referencecategory)
        {
            if (ModelState.IsValid)
            {
                db.ReferenceCategories.Add(referencecategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(referencecategory);
        }

        //
        // GET: /ReferenceCategory/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ReferenceCategory referencecategory = db.ReferenceCategories.Find(id);
            if (referencecategory == null)
            {
                return HttpNotFound();
            }
            return View(referencecategory);
        }

        //
        // POST: /ReferenceCategory/Edit/5

        [HttpPost]
        public ActionResult Edit(ReferenceCategory referencecategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(referencecategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(referencecategory);
        }

        //
        // GET: /ReferenceCategory/Delete/5

        public ActionResult Delete(int id = 0)
        {
            /*ReferenceCategory referencecategory = db.ReferenceCategories.Find(id);
            if (referencecategory == null)
            {
                return HttpNotFound();
            }
            return View(referencecategory);*/
            ReferenceCategory referencecategory = db.ReferenceCategories.Find(id);
            db.ReferenceCategories.Remove(referencecategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // POST: /ReferenceCategory/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ReferenceCategory referencecategory = db.ReferenceCategories.Find(id);
            db.ReferenceCategories.Remove(referencecategory);
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