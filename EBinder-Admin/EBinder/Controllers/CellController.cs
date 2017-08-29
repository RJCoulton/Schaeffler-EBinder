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
    public class CellController : BaseController
    {
        private EbinderContext db = new EbinderContext();
        

        //
        // GET: /Cell/

        public ActionResult Index()
        {
            IQueryable<Cell> cells;
            Region region = Session["Region"] as Region;
            Plant plant = Session["Plant"] as Plant;
            Department dept = Session["Department"] as Department;

            if (region != null)
            {
                if (Session["Plant"] != null)
                {
                    if(Session["Department"] != null)
                    {
                        if (dept.Name != Resource.All)
                        {
                            cells = db.Cells.Where(a => a.Department.Name == dept.Name).OrderBy(a=>a.Department.Name).ThenBy(a=>a.Name);
                            return View(cells.ToList());
                        }
                    }
                    if (plant.Name != Resource.All)
                    {
                        cells = db.Cells.Where(a => a.Department.Plant.Name == plant.Name).OrderBy(a => a.Department.Name).ThenBy(a => a.Name);
                        return View(cells.ToList());
                    }
                }

                if (region.Name != Resource.All)
                {
                    cells = db.Cells.Where(a => a.Department.Plant.Region.Name == region.Name).OrderBy(a => a.Department.Name).ThenBy(a => a.Name);
                    return View(cells.ToList());
                }
            }

            cells = db.Cells.Include(d => d.Department).OrderBy(a => a.Department.Name).ThenBy(a => a.Name);
            return View(cells.ToList());
        }

        //
        // GET: /Cell/Details/5

        public ActionResult Links(int id = 0)
        {
            Cell cell = db.Cells.Find(id);
            ViewData["Links"] = cell.References.OrderBy(a=>a.ReferenceType.TypeName_en).ToList();
            if (cell == null)
            {
                return HttpNotFound();
            }
            return View(cell);
        }

        public ActionResult RemoveLink(int RefId, int CellId)
        {
            Cell cell = db.Cells.Find(CellId);
            Reference reference = db.References.Find(RefId);

            if (cell != null && reference != null)
            {
                cell.References.Remove(reference);
                db.SaveChanges();
                return RedirectToAction("Links", new { id = CellId });
            }
            TempData["message"] = Resource.DeleteRefError;
            return RedirectToAction("Links", new { id = CellId });
        }

        //
        // GET: /Cell/Create

        public ActionResult Create()
        {
            Plant plant = Session["Plant"] as Plant;
            Department dept = Session["Department"] as Department;
            int deptId = -1;
            if(dept != null) deptId = dept.DepartmentID;
            if (plant != null && plant.Name != Resource.All)
                ViewBag.DepartmentID = new SelectList(db.Departments.Where(a => a.Plant.PlantID == plant.PlantID), "DepartmentID", "Name", deptId);
            else
                ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", deptId);
            return View();
        }

        //
        // POST: /Cell/Create

        [HttpPost]
        public ActionResult Create(Cell cell)
        {
            if (ModelState.IsValid)
            {
                db.Cells.Add(cell);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", cell.DepartmentID);
            return View(cell);
        }

        //
        // GET: /Cell/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Cell cell = db.Cells.Find(id);
            if (cell == null)
            {
                return HttpNotFound();
            }
            Plant plant = Session["Plant"] as Plant;
            if (plant != null && plant.Name != Resource.All)
                ViewBag.DepartmentID = new SelectList(db.Departments.Where(a => a.Plant.PlantID == plant.PlantID), "DepartmentID", "Name", cell.DepartmentID);
            else
                ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", cell.DepartmentID);
            return View(cell);
        }

        //
        // POST: /Cell/Edit/5

        [HttpPost]
        public ActionResult Edit(Cell cell)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cell).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", cell.DepartmentID);
            return View(cell);
        }

        //
        // GET: /Cell/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Cell cell = db.Cells.Find(id);
            db.Cells.Remove(cell);
            db.SaveChanges();
            return RedirectToAction("Index");

            /*
            Cell cell = db.Cells.Find(id);
            if (cell == null)
            {
                return HttpNotFound();
            }
            return View(cell);*/
        }

        //
        // POST: /Cell/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Cell cell = db.Cells.Find(id);
            db.Cells.Remove(cell);
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