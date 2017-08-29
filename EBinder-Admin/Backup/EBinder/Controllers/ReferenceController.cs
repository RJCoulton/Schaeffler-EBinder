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
    public class ReferenceController : Controller
    {
        private EBinderContext db = new EBinderContext();

        //
        // GET: /Reference/

        public ActionResult Index()
        {
            var references = db.References.Include(r => r.referenceType);


            return View(references.ToList());
        }

        //
        // GET: /Reference/Details/5

        public ActionResult Details(int id = 0)
        {
            Reference reference = db.References.Find(id);
            ViewData["LinkCells"] = reference.Cells.ToList();
            ViewData["LinkDept"] = reference.Departments.ToList();
            ViewData["LinkPlants"] = reference.Plants.ToList();
            ViewData["LinkRegions"] = reference.Regions.ToList();
            ViewData["LinkParts"] = reference.Parts.ToList();

            if (reference == null)
            {
                return HttpNotFound();
            }
            return View(reference);
        }

        //
        // GET: /Reference/Create

        public ActionResult Create()
        {
            ViewBag.ReferenceTypeID = new SelectList(db.ReferenceTypes, "ReferenceTypeID", "TypeName");
            return View();
        }

        //
        // POST: /Reference/Create

        [HttpPost]
        public ActionResult Create(Reference reference)
        {
            if (ModelState.IsValid)
            {
                db.References.Add(reference);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ReferenceTypeID = new SelectList(db.ReferenceTypes, "ReferenceTypeID", "TypeName", reference.ReferenceTypeID);
            return View(reference);
        }

        //
        // GET: /Reference/Create

        public ActionResult CreateLink(int id)
        {
            ViewData["RefId"] = id;
            string region = Session["Region"].ToString();
            string plant = Session["Plant"].ToString();
            string department = Session["Department"].ToString();
            string cell = Session["Cell"].ToString();
            string part = Session["Part"].ToString();

            List<SelectListItem> type = new List<SelectListItem>();
            type.Add(new SelectListItem { Text = "Region", Value = "Region" });
            type.Add(new SelectListItem { Text = "Plant", Value = "Plant" });
            type.Add(new SelectListItem { Text = "Department", Value = "Department" });
            type.Add(new SelectListItem { Text = "Cell", Value = "Cell", Selected = true });
            type.Add(new SelectListItem { Text = "Part", Value = "Part" });
            ViewData["Type"] = type;

            IQueryable<Cell> cells;

            if (department == "All")
            {
                if (plant == "All")
                {
                    if (region == "All")
                    {
                        cells = db.Cells;
                        ViewData["cells"] = cells.ToList().Select(x => new SelectListItem
                        {
                            Value = x.CellID.ToString(),
                            Text = x.Name,
                            Selected = (x.Name == cell)
                        }).ToList();
                        return View();
                    }
                    cells = db.Cells.Where(a => a.Department.Plant.Region.Name == region);
                    ViewData["cells"] = cells.ToList().Select(x => new SelectListItem
                    {
                        Value = x.CellID.ToString(),
                        Text = x.Name,
                        Selected = (x.Name == cell)
                    }).ToList();
                    return View();
                }
                cells = db.Cells.Where(a => a.Department.Plant.Name == plant);
                ViewData["cells"] = cells.ToList().Select(x => new SelectListItem
                {
                    Value = x.CellID.ToString(),
                    Text = x.Name,
                    Selected = (x.Name == cell)
                }).ToList();
                return View();
            }
            cells = db.Cells.Where(a => a.Department.Name == department);
            ViewData["cells"] = cells.ToList().Select(x => new SelectListItem
            {
                Value = x.CellID.ToString(),
                Text = x.Name,
                Selected = (x.Name == cell)
            }).ToList();
            return View();
        }

        //
        // POST: /Reference/Create

        [HttpPost]
        public ActionResult CreateLink(int id, string level, int linkLoc)
        {
            Reference reference = db.References.Find(id);
            switch (level)
            {
                case "Region":
                    Region region = db.Regions.Find(linkLoc);
                    if(IsDuplicate(level, reference, region))
                    {
                        TempData["message"] = "This reference already belongs to this area";
                        return RedirectToAction("CreateLink");
                    }
                    reference.Regions.Add(region);
                    break;
                case "Plant":
                    Plant plant = db.Plants.Find(linkLoc);
                    if (IsDuplicate(level, reference, plant))
                    {
                        TempData["message"] = "This reference already belongs to this area";
                        return RedirectToAction("CreateLink");
                    }
                    reference.Plants.Add(plant);
                    break;
                case "Department":
                    Department department = db.Departments.Find(linkLoc);
                    if (IsDuplicate(level, reference, department))
                    {
                        TempData["message"] = "This reference already belongs to this area";
                        return RedirectToAction("CreateLink");
                    }
                    reference.Departments.Add(department);
                    break;
                case "Cell":
                    Cell cell = db.Cells.Find(linkLoc);
                    if (IsDuplicate(level, reference, cell))
                    {
                        TempData["message"] = "This reference already belongs to this area";
                        return RedirectToAction("CreateLink");
                    }
                    reference.Cells.Add(cell);
                    break;
                case "Part":
                    Part part = db.Parts.Find(linkLoc);
                    if (reference.Parts.Contains(part))
                    {
                        TempData["message"] = "This reference already belongs to this area";
                        return RedirectToAction("CreateLink");
                    }
                    reference.Parts.Add(part);
                    break;
            }

            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        private bool IsDuplicate(string level, Reference reference, Model model)
        {
            //Checks higher levels for duplicates and returns true if anu duplicates are found
            //Checks lower levels for duplicates and deletes any before adding to higher level
            if(level == "Region")
            {
                Region region = (Region)model;
                //Check current level and above
                if (region.References.Contains(reference))
                    return true;
                //check lower levels
                foreach (Plant plant in region.Plants)
                {
                    if (plant.References.Contains(reference))
                    {
                        plant.References.Remove(reference);
                    }
                    else
                    {
                        foreach (Department dept in plant.Departments)
                        {
                            if (dept.References.Contains(reference))
                            {
                                dept.References.Remove(reference);
                            }
                            else
                            {
                                foreach (Cell cell in dept.Cells)
                                {
                                    if (cell.References.Contains(reference))
                                    {
                                        cell.References.Remove(reference);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if(level == "Plant")
            {
                Plant plant = (Plant)model;
                //check current level and above
                if (plant.References.Contains(reference) || 
                    plant.Region.References.Contains(reference))
                    return true;
                //check lower levels and remove any duplicates
                foreach (Department dept in plant.Departments)
                {
                    if (dept.References.Contains(reference))
                    {
                        dept.References.Remove(reference);
                    }
                    else
                    {
                        foreach (Cell cell in dept.Cells)
                        {
                            if (cell.References.Contains(reference))
                            {
                                cell.References.Remove(reference);
                            }
                        }
                    }
                }
            }
            else if(level == "Department")
            {
                Department dept = (Department)model;
                //check current level and above
                if (dept.References.Contains(reference) || 
                    dept.Plant.References.Contains(reference) || 
                    dept.Plant.Region.References.Contains(reference))
                    return true;

                foreach (Cell cell in dept.Cells)
                {
                    if (cell.References.Contains(reference))
                    {
                        cell.References.Remove(reference);
                    }
                }
            }
            else if (level == "Cell")
            {
                Cell cell = (Cell)model;
                if (cell.References.Contains(reference) || 
                    cell.Department.References.Contains(reference) || 
                    cell.Department.Plant.References.Contains(reference) || 
                    cell.Department.Plant.Region.References.Contains(reference))
                    return true;
            }

            return false;
        }

        //
        // GET: /Reference/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Reference reference = db.References.Find(id);
            if (reference == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReferenceTypeID = new SelectList(db.ReferenceTypes, "ReferenceTypeID", "TypeName", reference.ReferenceTypeID);
            return View(reference);
        }

        //
        // POST: /Reference/Edit/5

        [HttpPost]
        public ActionResult Edit(Reference reference)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reference).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ReferenceTypeID = new SelectList(db.ReferenceTypes, "ReferenceTypeID", "TypeName", reference.ReferenceTypeID);
            return View(reference);
        }

        public JsonResult GetList(string id)
        {
            if (id != null)
            {
                string region = Session["Region"].ToString();
                string plant = Session["Plant"].ToString();
                string department = Session["Department"].ToString();
                string cell = Session["Cell"].ToString();
                List<SelectListItem> list;
                switch (id)
                {
                    case "Region":
                        if (region == "All")
                        {
                            var regions = db.Regions;
                            list = regions.ToList().Select(x => new SelectListItem
                            {
                                Value = x.RegionID.ToString(),
                                Text = x.Name
                            }).ToList();
                            return Json(new SelectList(list, "Value", "Text"));
                        }
                        else
                        {
                            var regions = db.Regions.Where(a=>a.Name == region);
                            list = regions.ToList().Select(x => new SelectListItem
                            {
                                Value = x.RegionID.ToString(),
                                Text = x.Name
                            }).ToList();
                            return Json(new SelectList(list, "Value", "Text"));
                        }
                    case "Plant":
                        if (region == "All")
                        {
                            var plants = db.Plants;
                            list = plants.ToList().Select(x => new SelectListItem
                            {
                                Value = x.PlantID.ToString(),
                                Text = x.Name
                            }).ToList();
                            return Json(new SelectList(list, "Value", "Text"));
                        }
                        else
                        {
                            var plants = db.Plants.Where(a => a.Name == plant);
                            list = plants.ToList().Select(x => new SelectListItem
                            {
                                Value = x.PlantID.ToString(),
                                Text = x.Name
                            }).ToList();
                            return Json(new SelectList(list, "Value", "Text"));
                        }
                    case "Department":
                        if (plant == "All")
                        {
                            if (region == "All")
                            {
                                var depts = db.Departments;
                                list = depts.ToList().Select(x => new SelectListItem
                                {
                                    Value = x.DepartmentID.ToString(),
                                    Text = x.Name
                                }).ToList();
                                return Json(new SelectList(list, "Value", "Text"));
                            }
                            else
                            {
                                var depts = db.Departments.Where(a => a.Plant.Region.Name == region);
                                list = depts.ToList().Select(x => new SelectListItem
                                {
                                    Value = x.DepartmentID.ToString(),
                                    Text = x.Name
                                }).ToList();
                                return Json(new SelectList(list, "Value", "Text"));
                            }
                        }
                        else
                        {
                            var depts = db.Departments.Where(a => a.Plant.Name == plant);
                            list = depts.ToList().Select(x => new SelectListItem
                            {
                                Value = x.DepartmentID.ToString(),
                                Text = x.Name
                            }).ToList();
                            return Json(new SelectList(list, "Value", "Text"));
                        }
                    case "Cell":
                        if (department == "All")
                        {
                            if (plant == "All")
                            {
                                if (region == "All")
                                {
                                    var cells = db.Cells;
                                    list = cells.ToList().Select(x => new SelectListItem
                                    {
                                        Value = x.CellID.ToString(),
                                        Text = x.Name
                                    }).ToList();
                                    return Json(new SelectList(list, "Value", "Text"));
                                }
                                else
                                {
                                    var cells = db.Cells.Where(a => a.Department.Plant.Region.Name == region);
                                    list = cells.ToList().Select(x => new SelectListItem
                                    {
                                        Value = x.CellID.ToString(),
                                        Text = x.Name
                                    }).ToList();
                                    return Json(new SelectList(list, "Value", "Text"));
                                }
                            }
                            else
                            {
                                var cells = db.Cells.Where(a => a.Department.Plant.Name == plant);
                                list = cells.ToList().Select(x => new SelectListItem
                                {
                                    Value = x.CellID.ToString(),
                                    Text = x.Name
                                }).ToList();
                                return Json(new SelectList(list, "Value", "Text"));
                            }
                        }
                        else
                        {
                            var cells = db.Cells.Where(a => a.Department.Name == department);
                            list = cells.ToList().Select(x => new SelectListItem
                            {
                                Value = x.CellID.ToString(),
                                Text = x.Name
                            }).ToList();
                            return Json(new SelectList(list, "Value", "Text"));
                        }
                    case "Part":
                        break;
                }
            }
            return null;
        }

        public ActionResult Unlink(int refId, int locId, string loc)
        {
            var reference = db.References.Find(refId);
            switch (loc)
            {
                case "Region":
                    var region = db.Regions.Find(locId);
                    if(reference.Regions.Contains(region))
                        reference.Regions.Remove(region);
                    else
                        TempData["message"] = "Failed to remove link";
                    break;
                case "Plant":
                    var plant = db.Plants.Find(locId);
                    if(reference.Plants.Contains(plant))
                        reference.Plants.Remove(plant);
                    else
                        TempData["message"] = "Failed to remove link";
                    break;
                case "Department":
                    var dept = db.Departments.Find(locId);
                    if(reference.Departments.Contains(dept))
                        reference.Departments.Remove(dept);
                    else
                        TempData["message"] = "Failed to remove link";
                    break;
                case "Cell":
                    var cell = db.Cells.Find(locId);
                    if(reference.Cells.Contains(cell))
                        reference.Cells.Remove(cell);
                    else
                        TempData["message"] = "Failed to remove link";
                    break;
                case "Part":
                    var part = db.Parts.Find(locId);
                    if(reference.Parts.Contains(part))
                        reference.Parts.Remove(part);
                    else
                        TempData["message"] = "Failed to remove link";
                    break;
            }
            db.SaveChanges();
            return RedirectToAction("Details", new { id = refId });
        }

        //
        // GET: /Reference/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Reference reference = db.References.Find(id);
            if (reference == null)
            {
                return HttpNotFound();
            }
            return View(reference);
        }

        //
        // POST: /Reference/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Reference reference = db.References.Find(id);
            db.References.Remove(reference);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}