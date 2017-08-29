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
    public class CellController : Controller
    {
        private EBinderContext db = new EBinderContext();

        //
        // GET: /Cell/

        public ActionResult Index()
        {
            IQueryable<Cell> cells;
            if (Session["Region"] != null)
            {
                string region = Session["Region"].ToString();
                if (Session["Plant"] != null)
                {
                    string plant = Session["Plant"].ToString();
                    if(Session["Department"] != null)
                    {
                        string dept = Session["Department"].ToString();
                        if (dept != "All")
                        {
                            cells = db.Cells.Where(a => a.Department.Name == dept);
                            return View(cells.ToList());
                        }
                    }
                    if (plant != "All")
                    {
                        cells = db.Cells.Where(a => a.Department.Plant.Name == plant);
                        return View(cells.ToList());
                    }
                }

                if (region != "All")
                {
                    cells = db.Cells.Where(a => a.Department.Plant.Region.Name == region);
                    return View(cells.ToList());
                }
            }

            cells = db.Cells.Include(d => d.Department);
            return View(cells.ToList());
        }

        //
        // GET: /Cell/Details/5

        public ActionResult Links(int id = 0)
        {
            Cell cell = db.Cells.Find(id);
            ViewData["Links"] = cell.References.OrderBy(a=>a.referenceType.TypeName).ToList();
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
            TempData["message"] = "Could not delete reference";
            return RedirectToAction("Links", new { id = CellId });
        }

        //
        // GET: /Cell/Create

        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name");
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
            if (cell == null)
            {
                return HttpNotFound();
            }
            return View(cell);
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