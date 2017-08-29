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
    public class PlantController : Controller
    {
        private EBinderContext db = new EBinderContext();

        //
        // GET: /Plant/

        public ActionResult Index()
        {
            IQueryable<Plant> plants;
            
            if (Session["Region"] != null)
            {
                string region = Session["Region"].ToString();
                if (region != "All")
                {
                    plants = db.Plants.Where(a => a.Region.Name == region);
                    return View(plants.ToList());
                }
            }

            plants = db.Plants.Include(p => p.Region);
            return View(plants.ToList());
        }

        //
        // GET: /Plant/Create

        public ActionResult Create()
        {
            ViewBag.RegionID = new SelectList(db.Regions, "RegionID", "Name");
            return View();
        }

        //
        // POST: /Plant/Create

        [HttpPost]
        public ActionResult Create(Plant plant)
        {
            //Add all reference types to the plant
            //Admins will then remove the unnecesary ones
            

            if (ModelState.IsValid)
            {
                foreach (ReferenceType r in db.ReferenceTypes.ToList())
                {
                    r.Plants.Add(plant);
                    //plant.ReferenceTypes.Add(r);
                }
                db.Plants.Add(plant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RegionID = new SelectList(db.Regions, "RegionID", "Name", plant.RegionID);
            return View(plant);
        }

        //
        // GET: /Plant/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Plant plant = db.Plants.Find(id);
            if (plant == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegionID = new SelectList(db.Regions, "RegionID", "Name", plant.RegionID);
            return View(plant);
        }

        //
        // POST: /Plant/Edit/5

        [HttpPost]
        public ActionResult Edit(Plant plant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RegionID = new SelectList(db.Regions, "RegionID", "Name", plant.RegionID);
            return View(plant);
        }

        //
        // GET: /Plant/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Plant plant = db.Plants.Find(id);
            if (plant == null)
            {
                return HttpNotFound();
            }
            return View(plant);
        }

        //
        // POST: /Plant/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Plant plant = db.Plants.Find(id);
            db.Plants.Remove(plant);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Icons()
        {
            Plant plant;

            if (Session["Plant"] != null)
            {
                string plantName = Session["Plant"].ToString();
                if (plantName != "All")
                {
                    plant = db.Plants.First(a => a.Name == plantName);
                    ViewData["iconId"] = new SelectList(db.ReferenceTypes, "ReferenceTypeID", "TypeName");
                    return View(plant.ReferenceTypes.ToList());
                }
                else
                {
                    TempData["message"] = "Please select a plant from the filter menu";
                }
            }
            else
            {
                TempData["message"] = "Couldn't resolve plant name";
            }
            return RedirectToAction("Index", "Home", null);
        }
        
        [HttpPost]
        public ActionResult AddIcon(int iconId)
        {
            if (Session["Plant"] != null)
            {
                string plantName = Session["Plant"].ToString();
                if (plantName != "All")
                {
                    var plant = db.Plants.First(a => a.Name == plantName);
                    var refType = db.ReferenceTypes.Find(iconId);
                    if (!plant.ReferenceTypes.Contains(refType))
                    {
                        plant.ReferenceTypes.Add(refType);
                        db.SaveChanges();
                    }
                    else
                    {
                        TempData["message"] = "The plant already uses this icon";
                    }
                    return RedirectToAction("Icons");
                }
            }
            return RedirectToAction("Icons");
        }

        public ActionResult DeleteIcon(int IconId)
        {
            string plantName = Session["Plant"].ToString();

            if (plantName != null)
            {
                Plant plant = db.Plants.First(a => a.Name == plantName);
                ReferenceType refType = db.ReferenceTypes.Find(IconId);
                if (plant == null)
                {
                    return HttpNotFound();
                }

                plant.ReferenceTypes.Remove(refType);
                db.SaveChanges();
            }
            else
            {
                TempData["message"] = "Couldn't resolve plant name";
            }
            return RedirectToAction("Icons");
        }

        public ActionResult HomeIcons()
        {
            Plant plant;

            if (Session["Plant"] != null)
            {
                string plantName = Session["Plant"].ToString();
                if (plantName != "All")
                {
                    plant = db.Plants.First(a => a.Name == plantName);
                    ViewData["iconId"] = new SelectList(plant.ReferenceTypes, "ReferenceTypeID", "TypeName");
                    return View(plant.HomePageReferences.ToList());
                }
                else
                {
                    TempData["message"] = "Please select a plant from the filter menu";
                }
            }
            else
            {
                TempData["message"] = "Couldn't resolve plant name";
            }
            return RedirectToAction("Index", "Home", null);
        }

        [HttpPost]
        public ActionResult AddHomeIcon(int iconId)
        {
            if (Session["Plant"] != null)
            {
                string plantName = Session["Plant"].ToString();
                if (plantName != "All")
                {
                    var plant = db.Plants.First(a => a.Name == plantName);
                    var refType = db.ReferenceTypes.Find(iconId);
                    if (plant.HomePageReferences.Count < 8)
                    {
                        if (!plant.HomePageReferences.Contains(refType))
                        {
                            plant.HomePageReferences.Add(refType);
                            db.SaveChanges();
                        }
                        else
                        {
                            TempData["message"] = "The plant already uses this icon";
                        }
                    }
                    else
                    {
                        TempData["message"] = "There can only be 8 home page icons. Consider deleting one before you add another.";
                    }
                    return RedirectToAction("HomeIcons");
                }
            }
            return RedirectToAction("HomeIcons");
        }

        public ActionResult DeleteHomeIcon(int IconId)
        {
            string plantName = Session["Plant"].ToString();

            if (plantName != null)
            {
                Plant plant = db.Plants.First(a => a.Name == plantName);
                ReferenceType refType = db.ReferenceTypes.Find(IconId);
                if (plant == null)
                {
                    return HttpNotFound();
                }

                plant.HomePageReferences.Remove(refType);
                db.SaveChanges();
            }
            else
            {
                TempData["message"] = "Couldn't resolve plant name";
            }
            return RedirectToAction("HomeIcons");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}