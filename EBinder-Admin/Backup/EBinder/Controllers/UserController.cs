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
    public class UserController : Controller
    {
        private EBinderContext db = new EBinderContext();

        //
        // GET: /User/

        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Cell).Include(u => u.userType);
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
            ViewBag.CellID = new SelectList(db.Cells, "CellID", "Name");
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Name");
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CellID = new SelectList(db.Cells, "CellID", "Name", user.CellID);
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
            ViewBag.CellID = new SelectList(db.Cells, "CellID", "Name", user.CellID);
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
            ViewBag.CellID = new SelectList(db.Cells, "CellID", "Name", user.CellID);
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "Name", user.UserTypeID);
            return View(user);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(string id = null)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
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
    }
}