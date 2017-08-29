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
    public class PlantController : BaseController
    {
        private EbinderContext db = new EbinderContext();

        //
        // GET: /Plant/

        public ActionResult Index()
        {
            IQueryable<Plant> plants;

            Region region = Session["Region"] as Region;
            Plant plant = Session["Plant"] as Plant;
            Department dept = Session["Department"] as Department;
            Cell cell = Session["Cell"] as Cell;

            if (region != null)
            {
                if (region.Name != Resource.All)
                {
                    plants = db.Plants.Where(a => a.Region.Name == region.Name);
                    return View(plants.ToList());
                }
            }

            plants = db.Plants.Include(p => p.Region);
            return View(plants.ToList());
        }

        public ActionResult Links(int id = 0)
        {
            Plant plant = db.Plants.Find(id);
            ViewData["Links"] = plant.References.OrderBy(a => a.ReferenceType.TypeName_en).ToList();
            if (plant == null)
            {
                return HttpNotFound();
            }
            return View(plant);
        }

        public ActionResult RemoveLink(int RefId, int plantId)
        {
            Department dept = db.Departments.Find(plantId);
            Reference reference = db.References.Find(RefId);

            if (dept != null && reference != null)
            {
                dept.References.Remove(reference);
                db.SaveChanges();
                return RedirectToAction("Links", new { id = plantId });
            }
            TempData["message"] = Resource.DeleteRefError;
            return RedirectToAction("Links", new { id = plantId });
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
        public ActionResult Create(Plant plant, string lang)
        {
            //Add all reference types to the plant
            //Admins will then remove the unnecesary ones

            plant.Language = lang;
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
        public ActionResult Edit(Plant plant, string lang)
        {
            plant.Language = lang;
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
            /*Plant plant = db.Plants.Find(id);
            if (plant == null)
            {
                return HttpNotFound();
            }
            return View(plant);*/
            Plant plant = db.Plants.Find(id);
            db.Plants.Remove(plant);
            db.SaveChanges();
            return RedirectToAction("Index");
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
            Plant plant = Session["Plant"] as Plant;

            if (plant != null || plant.Name!= Resource.All)
            {
                plant = db.Plants.Find(plant.PlantID);
                if (Session["Language"].ToString() == "es")
                {
                    ViewData["iconId"] = db.ReferenceTypes.ToList().Select(x => new SelectListItem
                    {
                        Value = x.ReferenceTypeID.ToString(),
                        Text = x.TypeName_es != null ? x.TypeName_es : x.TypeName_en
                    }).ToList();
                }
                else
                {
                    ViewData["iconId"] = new SelectList(db.ReferenceTypes, "ReferenceTypeID", "TypeName_en");
                }
                return View(plant.ReferenceTypes.ToList());
            }
            else
            {
                TempData["message"] = Resource.SelectAPlantError;
            }
            return RedirectToAction("Index", "Home", null);
        }
        
        [HttpPost]
        public ActionResult AddIcon(int iconId)
        {
            Plant plant = Session["Plant"] as Plant;

            if (plant != null || plant.Name != Resource.All)
            {
                plant = db.Plants.Find(plant.PlantID);
                var refType = db.ReferenceTypes.Find(iconId);
                if (!plant.ReferenceTypes.Contains(refType))
                {
                    plant.ReferenceTypes.Add(refType);
                    Session["Plant"] = plant;
                    db.SaveChanges();
                }
                else
                {
                    TempData["message"] = Resource.PlantIconError;
                }
                return RedirectToAction("Icons");
            }
            return RedirectToAction("Index", "Home", null);
        }

        public ActionResult DeleteIcon(int IconId)
        {
            Plant plant = Session["Plant"] as Plant;

            if (plant != null || plant.Name!= Resource.All)
            {
                plant = db.Plants.Find(plant.PlantID);
                ReferenceType refType = db.ReferenceTypes.Find(IconId);
                plant.ReferenceTypes.Remove(refType);
                Session["Plant"] = plant;
                db.SaveChanges();
            }
            else
            {
                TempData["message"] = Resource.IconDeleteError;
            }
            return RedirectToAction("Icons");
        }

        public ActionResult HomeIcons()
        {
            Plant plant = Session["Plant"] as Plant;

            if (plant != null || plant.Name != Resource.All)
            {
                plant = db.Plants.Find(plant.PlantID);
                if (Session["Language"].ToString() == "es")
                {
                    ViewData["iconId"] = plant.ReferenceTypes.ToList().Select(x => new SelectListItem
                    {
                        Value = x.ReferenceTypeID.ToString(),
                        Text = x.TypeName_es != null ? x.TypeName_es : x.TypeName_en
                    }).ToList();
                }
                else
                {
                    ViewData["iconId"] = new SelectList(plant.ReferenceTypes, "ReferenceTypeID", "TypeName_en");
                }
                return View(plant.HomePageReferences.ToList());
            }
            else
            {
                TempData["message"] = Resource.SelectAPlantError;
            }
            return RedirectToAction("Index", "Home", null);
        }

        [HttpPost]
        public ActionResult AddHomeIcon(int iconId)
        {
            Plant plant = Session["Plant"] as Plant;

            if (plant != null || plant.Name != Resource.All)
            {
                plant = db.Plants.Find(plant.PlantID);
                var refType = db.ReferenceTypes.Find(iconId);
                if (plant.HomePageReferences.Count < 8)
                {
                    if (!plant.HomePageReferences.Contains(refType))
                    {
                        plant.HomePageReferences.Add(refType);
                        Session["Plant"] = plant;
                        db.SaveChanges();
                    }
                    else
                    {
                        TempData["message"] = Resource.PlantIconError;
                    }
                }
                else
                {
                    TempData["message"] = Resource.HomePageMaxIconError;
                }
            }
            return RedirectToAction("HomeIcons");
        }

        public ActionResult DeleteHomeIcon(int IconId)
        {
            Plant plant = Session["Plant"] as Plant;

            if (plant != null || plant.Name != Resource.All)
            {
                plant = db.Plants.Find(plant.PlantID);
                ReferenceType refType = db.ReferenceTypes.Find(IconId);
                plant.HomePageReferences.Remove(refType);
                Session["Plant"] = plant;
                db.SaveChanges();
            }
            else
            {
                TempData["message"] = Resource.SelectAPlantError;
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