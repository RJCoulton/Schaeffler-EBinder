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
    public class ReferenceController : BaseController
    {
        private EbinderContext db = new EbinderContext();
        private CDContext cd = new CDContext();

        //
        // GET: /Reference/

        public ActionResult Index()
        {
            var references = GetRefsByFilter();
            if (Session["Language"].ToString() == "es")
            {
                ViewBag.ReferenceTypeID = db.ReferenceTypes.ToList().Select(x => new SelectListItem
                {
                    Value = x.ReferenceTypeID.ToString(),
                    Text = x.TypeName_es != null ? x.TypeName_es : x.TypeName_en
                }).ToList();
            }
            else
            {
                ViewBag.ReferenceTypeID = new SelectList(db.ReferenceTypes, "ReferenceTypeID", "TypeName_en");
            }
            ViewBag.GlobalFilter = "global";
            return View(references.ToList());
        }

        [SessionExpire]
        [HttpPost]
        public ActionResult FilterIndex(int? ReferenceTypeID, bool Flagged, string GlobalFilter, string Search)
        {

            IEnumerable<Reference> references = db.References;

            if (GlobalFilter == "global")
            {
                references = GetRefsByFilter();
            }
            else
            {
                references = references.Where(r => r.Regions.Count < 1 &&
                                                    r.Plants.Count < 1 &&
                                                    r.Departments.Count < 1 &&
                                                    r.Cells.Count < 1 &&
                                                    r.Parts.Count < 1);
                Plant plant = Session["Plant"] as Plant;
                if (plant != null && plant.Name != Resource.All)
                    references = references.Where(r => r.DefaultPlantID == plant.PlantID); 
            }

            if (ReferenceTypeID != null)
                references = references.Where(r => r.ReferenceTypeID == ReferenceTypeID);
            if (Flagged)
                references = references.Where(r => r.FlagDate > DateTime.Now);
            if (Search != "")
                references = references.Where(r => r.Title.ToLower().Contains(Search.ToLower()) || r.Number.ToLower().Contains(Search.ToLower()));


            if (Session["Language"].ToString() == "es")
            {
                ViewBag.ReferenceTypeID = db.ReferenceTypes.ToList().Select(x => new SelectListItem
                {
                    Value = x.ReferenceTypeID.ToString(),
                    Text = x.TypeName_es != null ? x.TypeName_es : x.TypeName_en
                }).ToList();
            }
            else
            {
                ViewBag.ReferenceTypeID = new SelectList(db.ReferenceTypes, "ReferenceTypeID", "TypeName_en", ReferenceTypeID);
            }
            ViewBag.Flagged = Flagged;
            ViewBag.GlobalFilter = GlobalFilter;

            return View("Index", references.ToList());
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

        public ActionResult Flag(int id)
        {
            Reference reference = db.References.Find(id);
            reference.FlagDate = DateTime.Now.AddDays(7);
            db.Entry(reference).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }


        //
        // GET: /Reference/Create

        public ActionResult Create()
        {
            if (Session["Language"].ToString() == "es")
            {
                ViewBag.ReferenceTypeID = db.ReferenceTypes.ToList().Select(x => new SelectListItem
                {
                    Value = x.ReferenceTypeID.ToString(),
                    Text = x.TypeName_es != null ? x.TypeName_es : x.TypeName_en
                }).ToList();
            }
            else
            {
                ViewBag.ReferenceTypeID = new SelectList(db.ReferenceTypes, "ReferenceTypeID", "TypeName_en");
            }
            return View();
        }

        //
        // POST: /Reference/Create

        [HttpPost]
        public ActionResult Create(Reference reference)
        {
            DateTime flagDate = DateTime.Now.AddDays(7);
            reference.FlagDate = flagDate;

            Plant plant = Session["Plant"] as Plant;
            if (plant != null && plant.Name != Resource.All)
                reference.DefaultPlantID = plant.PlantID;
            else
                reference.DefaultPlantID = -1;

                if (ModelState.IsValid)
            {
                db.References.Add(reference);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = reference.ReferenceID });
            }

            if (Session["Language"].ToString() == "es")
            {
                ViewBag.ReferenceTypeID = db.ReferenceTypes.ToList().Select(x => new SelectListItem
                {
                    Value = x.ReferenceTypeID.ToString(),
                    Text = x.TypeName_es != null ? x.TypeName_es : x.TypeName_en
                }).ToList();
            }
            else
            {
                ViewBag.ReferenceTypeID = new SelectList(db.ReferenceTypes, "ReferenceTypeID", "TypeName_en", reference.ReferenceTypeID);
            }
            return View(reference);
        }

        //
        // GET: /Reference/Create

        public ActionResult CreateLink(int id)
        {
            
            List<SelectListItem> type = new List<SelectListItem>();
            type.Add(new SelectListItem { Text = Resource.Region, Value = "Region" });
            type.Add(new SelectListItem { Text = Resource.Plant, Value = "Plant" });
            type.Add(new SelectListItem { Text = Resource.Department, Value = "Department" });
            type.Add(new SelectListItem { Text = Resource.Cell, Value = "Cell", Selected = true });
            type.Add(new SelectListItem { Text = Resource.Part, Value = "Part" });

            ViewData["Type"] = type;
            ViewData["RefId"] = id;
            ViewData["cells"] = GetListFromFilter("Cell");
            Plant plant = Session["Plant"] as Plant;
            if(plant!=null)
            {
                IEnumerable<SAPPart> parts = Enumerable.Empty<SAPPart>().AsEnumerable();
                if (plant.CompanyNumbers != null)
                {
                    foreach (string num in plant.CompanyNumbers.Split(','))
                    {
                        parts = parts.Concat(cd.SAPParts.Where(a => a.CompanyNumber == num.Trim() && a.SAPMaterialNum.Substring(a.SAPMaterialNum.Length - 4) != "8000"));
                    }
                }
                var data = new List<string>();
                foreach (var p in parts)
                {
                    data.Add(p.PartDesc);
                }
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                ViewData["Parts"] = serializer.Serialize(data);
            }
            

            return View();
        }

        //
        // POST: /Reference/Create

        [HttpPost]
        public ActionResult CreateLink(int id, string level, int linkLoc, string partDesc)
        {
            Reference reference = db.References.Find(id);
            switch (level)
            {
                case "Region":
                    Region region = db.Regions.Find(linkLoc);
                    if(IsDuplicate(level, reference, region))
                    {
                        TempData["message"] = Resource.LinkExistsError;
                        return RedirectToAction("CreateLink");
                    }
                    reference.Regions.Add(region);
                    break;
                case "Plant":
                    Plant plant = db.Plants.Find(linkLoc);
                    if (IsDuplicate(level, reference, plant))
                    {
                        TempData["message"] = Resource.LinkExistsError;
                        return RedirectToAction("CreateLink");
                    }
                    reference.Plants.Add(plant);
                    break;
                case "Department":
                    Department department = db.Departments.Find(linkLoc);
                    if (IsDuplicate(level, reference, department))
                    {
                        TempData["message"] = Resource.LinkExistsError;
                        return RedirectToAction("CreateLink");
                    }
                    reference.Departments.Add(department);
                    break;
                case "Cell":
                    Cell cell = db.Cells.Find(linkLoc);
                    if (IsDuplicate(level, reference, cell))
                    {
                        TempData["message"] = Resource.LinkExistsError;
                        return RedirectToAction("CreateLink");
                    }
                    reference.Cells.Add(cell);
                    break;
                case "Part":
                    SAPPart part = cd.SAPParts.FirstOrDefault(a => a.PartDesc == partDesc);
                    if (part == null)
                    {
                        return RedirectToAction("CreateLink");
                    }

                    Part p = db.Parts.FirstOrDefault(a => a.PartDesc == part.PartDesc);
                    if(p==null)
                    {
                        p = new Part();
                        p.PartDesc = part.PartDesc;
                        db.Parts.Add(p);
                        db.SaveChanges();
                    }

                    if(reference.Parts.Contains(p))
                    {
                        TempData["message"] = Resource.LinkExistsError;
                        return RedirectToAction("CreateLink");
                    }
                    reference.Parts.Add(p);
                    break;
            }

            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        private bool IsDuplicate(string level, Reference reference, object model)
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
            if (Session["Language"].ToString() == "es")
            {
                ViewBag.ReferenceTypeID = db.ReferenceTypes.ToList().Select(x => new SelectListItem
                {
                    Value = x.ReferenceTypeID.ToString(),
                    Text = x.TypeName_es != null ? x.TypeName_es : x.TypeName_en,
                    Selected = x.ReferenceTypeID == reference.ReferenceTypeID
                }).ToList();
            }
            else
            {
                ViewBag.ReferenceTypeID = new SelectList(db.ReferenceTypes, "ReferenceTypeID", "TypeName_en", reference.ReferenceTypeID);
            }
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
            if(Session["Language"].ToString() == "es")
            {
                ViewBag.ReferenceTypeID = db.ReferenceTypes.ToList().Select(x => new SelectListItem
                {
                    Value = x.ReferenceTypeID.ToString(),
                    Text = x.TypeName_es != null ? x.TypeName_es : x.TypeName_en
                }).ToList();
            }
            else
            {
                ViewBag.ReferenceTypeID = new SelectList(db.ReferenceTypes, "ReferenceTypeID", "TypeName_en", reference.ReferenceTypeID);
            }

            return View(reference);
        }

        public JsonResult GetList(string id)
        {
            if (id != null)
            {
                var list = GetListFromFilter(id);
                if (list == null) return null;
                return Json(new SelectList(list, "Value", "Text"));
            }
            return null;
        }

        public ActionResult Unlink(int refId, string locId, string loc)
        {
            var reference = db.References.Find(refId);
            switch (loc)
            {
                case "Region":
                    var region = db.Regions.Find(Convert.ToInt32(locId));
                    if(reference.Regions.Contains(region))
                        reference.Regions.Remove(region);
                    else
                        TempData["message"] = Resource.FailedToUnlinkError;
                    break;
                case "Plant":
                    var plant = db.Plants.Find(Convert.ToInt32(locId));
                    if(reference.Plants.Contains(plant))
                        reference.Plants.Remove(plant);
                    else
                        TempData["message"] = Resource.FailedToUnlinkError;
                    break;
                case "Department":
                    var dept = db.Departments.Find(Convert.ToInt32(locId));
                    if(reference.Departments.Contains(dept))
                        reference.Departments.Remove(dept);
                    else
                        TempData["message"] = Resource.FailedToUnlinkError;
                    break;
                case "Cell":
                    var cell = db.Cells.Find(Convert.ToInt32(locId));
                    if(reference.Cells.Contains(cell))
                        reference.Cells.Remove(cell);
                    else
                        TempData["message"] = Resource.FailedToUnlinkError;
                    break;
                case "Part":
                    var part = db.Parts.FirstOrDefault(a => a.PartDesc == locId);
                    if (reference.Parts.Contains(part))
                        reference.Parts.Remove(part);
                    else
                        TempData["message"] = Resource.FailedToUnlinkError;
                    break;
            }
            db.SaveChanges();
            return RedirectToAction("Details", new { id = refId });
        }

        //
        // GET: /Reference/Delete/5

        public ActionResult Delete(int id = 0)
        {
            /*Reference reference = db.References.Find(id);
            if (reference == null)
            {
                return HttpNotFound();
            }
            return View(reference);*/
            Reference reference = db.References.Find(id);
            db.References.Remove(reference);
            db.SaveChanges();
            return RedirectToAction("Index");
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

        private List<SelectListItem> GetListFromFilter(string id)
        {
            Region region = Session["Region"] as Region;
            Plant plant = Session["Plant"] as Plant;
            Department department = Session["Department"] as Department;
            Cell cell = Session["Cell"] as Cell;
            Part part = Session["Part"] as Part;

            switch (id)
            {
                case "Region":
                    if (region.Name == Resource.All)
                    {
                        var regions = db.Regions;
                        return regions.ToList().Select(x => new SelectListItem
                        {
                            Value = x.RegionID.ToString(),
                            Text = x.Name
                        }).ToList();
                    }
                    else
                    {
                        var regions = db.Regions.Where(a => a.Name == region.Name);
                        return regions.ToList().Select(x => new SelectListItem
                        {
                            Value = x.RegionID.ToString(),
                            Text = x.Name
                        }).ToList();
                    }
                case "Plant":
                    if (region.Name == Resource.All)
                    {
                        var plants = db.Plants;
                        return plants.ToList().Select(x => new SelectListItem
                        {
                            Value = x.PlantID.ToString(),
                            Text = x.Name
                        }).ToList();
                    }
                    else
                    {
                        var plants = db.Plants.Where(a => a.Name == plant.Name);
                        return plants.ToList().Select(x => new SelectListItem
                        {
                            Value = x.PlantID.ToString(),
                            Text = x.Name
                        }).ToList();
                    }
                case "Department":
                    if (plant.Name == Resource.All)
                    {
                        if (region.Name == Resource.All)
                        {
                            var depts = db.Departments;
                            return depts.ToList().Select(x => new SelectListItem
                            {
                                Value = x.DepartmentID.ToString(),
                                Text = x.Name
                            }).ToList();
                        }
                        else
                        {
                            var depts = db.Departments.Where(a => a.Plant.Region.Name == region.Name);
                            return depts.ToList().Select(x => new SelectListItem
                            {
                                Value = x.DepartmentID.ToString(),
                                Text = x.Name
                            }).ToList();
                        }
                    }
                    else
                    {
                        var depts = db.Departments.Where(a => a.Plant.Name == plant.Name);
                        return depts.ToList().Select(x => new SelectListItem
                        {
                            Value = x.DepartmentID.ToString(),
                            Text = x.Name
                        }).ToList();
                    }
                case "Cell":
                    if (department.Name == Resource.All)
                    {
                        if (plant.Name == Resource.All)
                        {
                            if (region.Name == Resource.All)
                            {
                                var cells = db.Cells;
                                return cells.ToList().Select(x => new SelectListItem
                                {
                                    Value = x.CellID.ToString(),
                                    Text = x.Name
                                }).ToList();
                            }
                            else
                            {
                                var cells = db.Cells.Where(a => a.Department.Plant.Region.Name == region.Name);
                                return cells.ToList().Select(x => new SelectListItem
                                {
                                    Value = x.CellID.ToString(),
                                    Text = x.Name
                                }).ToList();
                            }
                        }
                        else
                        {
                            var cells = db.Cells.Where(a => a.Department.Plant.Name == plant.Name);
                            return cells.ToList().Select(x => new SelectListItem
                            {
                                Value = x.CellID.ToString(),
                                Text = x.Name
                            }).ToList();
                        }
                    }
                    else
                    {
                        var cells = db.Cells.Where(a => a.Department.Name == department.Name);
                        return cells.ToList().Select(x => new SelectListItem
                        {
                            Value = x.CellID.ToString(),
                            Text = x.Name
                        }).ToList();
                    }
                case "Part":
                    return null;
            }
            return null;
        }

        private IEnumerable<Reference> GetRefsByFilter()
        {
            Cell cell = Session["Cell"] as Cell;
            Department dept = Session["Department"] as Department;
            Plant plant = Session["Plant"] as Plant;
            Region region = Session["Region"] as Region;
            Part part = Session["Part"] as Part;

            var allRefs = db.References;
            IEnumerable<Reference> refs = Enumerable.Empty<Reference>().AsEnumerable();

            if (region == null || region.Name == Resource.All)
            {
                refs = refs.Concat(allRefs);
            }
            else if (plant == null || plant.Name == Resource.All)
            {
                refs = refs.Concat(allRefs.Where(a => a.Regions.Any(b => b.Name == region.Name)));
                refs = refs.Concat(allRefs.Where(a => a.Plants.Any(b => b.RegionID == region.RegionID)));
                refs = refs.Concat(allRefs.Where(a => a.Departments.Any(b => b.Plant.RegionID == region.RegionID)));
                refs = refs.Concat(allRefs.Where(a => a.Cells.Any(b => b.Department.Plant.RegionID == region.RegionID)));
            }
            else if (dept == null || dept.Name == Resource.All)
            {
                refs = refs.Concat(allRefs.Where(a => a.Regions.Any(b => b.Name == region.Name)));
                refs = refs.Concat(allRefs.Where(a => a.Plants.Any(b => b.Name == plant.Name)));
                refs = refs.Concat(allRefs.Where(a => a.Departments.Any(b => b.PlantID == plant.PlantID)));
                refs = refs.Concat(allRefs.Where(a => a.Cells.Any(b => b.Department.PlantID == plant.PlantID)));
            }
            else if (cell == null || cell.Name == Resource.All)
            {
                refs = refs.Concat(allRefs.Where(a => a.Regions.Any(b => b.Name == region.Name)));
                refs = refs.Concat(allRefs.Where(a => a.Plants.Any(b => b.Name == plant.Name)));
                refs = refs.Concat(allRefs.Where(a => a.Departments.Any(b => b.Name == dept.Name)));
                refs = refs.Concat(allRefs.Where(a => a.Cells.Any(b => b.DepartmentID == dept.DepartmentID)));
            }
            else
            {
                refs = refs.Concat(allRefs.Where(a => a.Regions.Any(b => b.Name == region.Name)));
                refs = refs.Concat(allRefs.Where(a => a.Plants.Any(b => b.Name == plant.Name)));
                refs = refs.Concat(allRefs.Where(a => a.Departments.Any(b => b.Name == dept.Name)));
                refs = refs.Concat(allRefs.Where(a => a.Cells.Any(b => b.Name == cell.Name)));
            }

            if (part != null && part.PartDesc != Resource.All)
            {
                refs = refs.Concat(allRefs.Where(a => a.Parts.Any(b => b.PartDesc == part.PartDesc)));
            }

            return refs;
        }
    }
}