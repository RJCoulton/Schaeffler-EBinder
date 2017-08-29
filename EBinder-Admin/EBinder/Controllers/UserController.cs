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
    public class UserController : BaseController
    {
        private EbinderContext db = new EbinderContext();

        //
        // GET: /User/

        public ActionResult Index()
        {
            IEnumerable<User> users;
            Cell cell = Session["Cell"] as Cell;
            Department dept = Session["Department"] as Department;
            Plant plant = Session["Plant"] as Plant;
            Region region = Session["Region"] as Region;

            if (cell!=null && cell.Name != Resource.All)
            {
                users = db.Users.Where(a => a.CellID == cell.CellID);
            }
            else
            {
                if(dept != null && dept.Name != Resource.All)
                {
                    users = db.Users.Where(a => a.Cell.DepartmentID == dept.DepartmentID);
                }
                else
                {
                    if (plant != null && plant.Name != Resource.All)
                    {
                        users = db.Users.Where(a => a.Cell.Department.PlantID == plant.PlantID);
                    }
                    else
                    {
                        if (region != null && region.Name != Resource.All)
                        {
                            users = db.Users.Where(a => a.Cell.Department.Plant.RegionID == region.RegionID);
                        }
                        else
                        {
                            users = db.Users;
                        }
                    }
                }
            }
        
            users = users.OrderBy(u=>u.UserType.Name).ThenBy(u=>u.Username);
            return View(users.ToList());
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(string id = null)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            ViewBag.CellID = FilteredCellList(null);
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Name");
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(User user)
        {
            user.Username = user.Username.ToLower();
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CellID = FilteredCellList(user.CellID);
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Name", user.UserTypeID);
            return View(user);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(string id = null)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CellID = FilteredCellList(user.CellID);
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Name", user.UserTypeID);
            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CellID = FilteredCellList(user.CellID);
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Name", user.UserTypeID);
            return View(user);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(string id = null)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private SelectList FilteredCellList(int? SelectedID)
        {
            Department dept = Session["Department"] as Department;
            Plant plant = Session["Plant"] as Plant;
            Region region = Session["Region"] as Region;

            if (dept != null && dept.Name != Resource.All)
            {
                return new SelectList(db.Cells.Where(a => a.DepartmentID == dept.DepartmentID), "CellID", "Name", SelectedID);
            }
            else
            {
                if (plant != null && plant.Name != Resource.All)
                {
                    return new SelectList(db.Cells.Where(a => a.Department.PlantID == plant.PlantID), "CellID", "Name", SelectedID);
                }
                else
                {
                    if (region != null && region.Name != Resource.All)
                    {
                        return new SelectList(db.Cells.Where(a => a.Department.Plant.RegionID == region.RegionID), "UserTypeID", "Name", SelectedID);
                    }
                    else
                    {
                        return new SelectList(db.Cells, "UserTypeID", "Name", SelectedID);
                    }
                }
            }
        }
    }
}