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
    public class DepartmentController : BaseController
    {
        private EbinderContext db = new EbinderContext();

        //
        // GET: /Department/

        public ActionResult Index()
        {
            IQueryable<Department> departments;

            Region region = Session["Region"] as Region;
            Plant plant = Session["Plant"] as Plant;

            if (region != null)
            {
                if (plant != null)
                {
                    if (plant.Name != Resource.All)
                    {
                        departments = db.Departments.Where(a => a.Plant.Name == plant.Name);
                        return View(departments.ToList());
                    }
                }

                if (region.Name != Resource.All)
                {
                    departments = db.Departments.Where(a => a.Plant.Region.Name == region.Name);
                    return View(departments.ToList());
                }
            }

            departments = db.Departments.Include(d => d.Plant);
            return View(departments.ToList());
        }

        public ActionResult Links(int id = 0)
        {
            Department dept = db.Departments.Find(id);
            ViewData["Links"] = dept.References.OrderBy(a => a.ReferenceType.TypeName_en).ToList();
            if (dept == null)
            {
                return HttpNotFound();
            }
            return View(dept);
        }

        public ActionResult RemoveLink(int RefId, int DeptId)
        {
            Department dept = db.Departments.Find(DeptId);
            Reference reference = db.References.Find(RefId);

            if (dept != null && reference != null)
            {
                dept.References.Remove(reference);
                db.SaveChanges();
                return RedirectToAction("Links", new { id = DeptId });
            }
            TempData["message"] = Resource.DeleteRefError;
            return RedirectToAction("Links", new { id = DeptId });
        }


        //
        // GET: /Department/Create

        public ActionResult Create()
        {
            Plant plant = Session["Plant"] as Plant;
            if (plant != null && plant.Name != Resource.All)
                ViewBag.PlantID = new SelectList(db.Plants.Where(a => a.PlantID == plant.PlantID), "PlantID", "Name", plant.PlantID);
            else
                ViewBag.PlantID = new SelectList(db.Plants, "PlantID", "Name");

            return View();
        }

        //
        // POST: /Department/Create

        [HttpPost]
        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PlantID = new SelectList(db.Plants, "PlantID", "Name", department.PlantID);
            return View(department);
        }

        //
        // GET: /Department/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            Plant plant = Session["Plant"] as Plant;
            if (plant != null && plant.Name != Resource.All)
                ViewBag.PlantID = new SelectList(db.Plants.Where(a => a.PlantID == plant.PlantID), "PlantID", "Name", department.PlantID);
            else
                ViewBag.PlantID = new SelectList(db.Plants, "PlantID", "Name",department.PlantID);
            return View(department);
        }

        //
        // POST: /Department/Edit/5

        [HttpPost]
        public ActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PlantID = new SelectList(db.Plants, "PlantID", "Name", department.PlantID);
            return View(department);
        }

        //
        // GET: /Department/Delete/5

        public ActionResult Delete(int id = 0)
        {
            /*Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);*/
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // POST: /Department/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
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