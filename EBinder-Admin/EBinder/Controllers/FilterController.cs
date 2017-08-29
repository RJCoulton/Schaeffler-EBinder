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
    public class FilterController : BaseController
    {
        private EbinderContext db = new EbinderContext();
        private CDContext cd = new CDContext();

        [SessionExpire]
        public ActionResult Filter()
        {
            //Grab object from session
            var region = Session["Region"] as Region;
            //If object is null set a name for display purposes
            if (region == null)
            {
                region = new Region();
                region.Name = Resource.All;
            }
            var plant = Session["Plant"] as Plant;
            if (plant == null)
            {
                plant = new Plant();
                plant.Name = Resource.All;
            }
            var department = Session["Department"] as Department;
            if (department == null)
            {
                department = new Department();
                department.Name = Resource.All;
            }
            var cell = Session["Cell"] as Cell;
            if (cell == null)
            {
                cell = new Cell();
                cell.Name = Resource.All;
            }
            var part = Session["Part"] as Part;
            if (part == null)
            {
                part = new Part();
                part.PartDesc = Resource.All;
            }


            var regions = db.Regions;


            ViewData["Regions"] = RebuildList(regions.ToList().Select(x => new SelectListItem
            {
                Value = x.RegionID.ToString(),
                Text = x.Name,
                Selected = (x.Name == region.Name)
            }).ToList());


            IQueryable<Plant> plants;
            if (region == null || region.Name == Resource.All)
                plants = db.Plants;
            else
                plants = db.Plants.Where(a => a.Region.Name == region.Name);

            ViewData["Plants"] = RebuildList(plants.ToList().Select(x => new SelectListItem
            {
                Value = x.PlantID.ToString(),
                Text = x.Name,
                Selected = (x.Name == plant.Name)
            }).ToList());

            if (plant == null || plant.Name == Resource.All)
            {
                ViewData["Departments"] = RebuildList(new List<SelectListItem>());
            }
            else
            {
                var depts = db.Departments.Where(a => a.Plant.Name == plant.Name);
                ViewData["Departments"] = RebuildList(depts.ToList().Select(x => new SelectListItem
                {
                    Value = x.DepartmentID.ToString(),
                    Text = x.Name,
                    Selected = (x.Name == department.Name)
                }).ToList());
                IEnumerable<SAPPart> parts = Enumerable.Empty<SAPPart>().AsEnumerable();
                if (plant.CompanyNumbers != null)
                {
                    foreach (string num in plant.CompanyNumbers.Split(','))
                    {
                        parts = parts.Concat(cd.SAPParts.Where(a => a.CompanyNumber == num.Trim() && a.SAPMaterialNum.Substring(9) != "8000"));
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

            if (department == null || department.Name == Resource.All)
            {
                ViewData["Cells"] = RebuildList(new List<SelectListItem>());
            }
            else
            {
                var cells = db.Cells.Where(a => a.Department.Name == department.Name);
                ViewData["Cells"] = RebuildList(cells.ToList().Select(x => new SelectListItem
                {
                    Value = x.CellID.ToString(),
                    Text = x.Name,
                    Selected = (x.Name == cell.Name)
                }).ToList());
            }
            return View();
        }


        public JsonResult GetPlants(int id)
        {
            var plants = db.Plants.Where(a => a.RegionID == id);
            SelectList list;
            if (plants.Count() > 0)
            {
                list = RebuildList(plants.ToList().Select(x => new SelectListItem
                {
                    Value = x.PlantID.ToString(),
                    Text = x.Name
                }).ToList());
            }
            else
            {
                list = RebuildList(new List<SelectListItem>());
            }

            return Json(new SelectList(list, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartments(int id, int regionID)
        {
            var depts = db.Departments.Where(a => a.Plant.PlantID == id);
            SelectList list;
            if (depts.Count() > 0)
            {
                list = RebuildList(depts.ToList().Select(x => new SelectListItem
                {
                    Value = x.PlantID.ToString(),
                    Text = x.Name
                }).ToList());
            }
            else
            {
                list = RebuildList(new List<SelectListItem>());
            }

            return Json(new SelectList(list, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCells(int id, int? plantID, int? regionID)
        {
            if (plantID == null || regionID == null)
            {
                Region region = Session["Region"] as Region;
                Plant plant = Session["Plant"] as Plant;

                plantID = plant.PlantID;
                regionID = region.RegionID;
            }

            var cells = db.Cells.Where(a => a.Department.DepartmentID == id);
            SelectList list;
            if (cells.Count() > 0)
            {
                list = RebuildList(cells.ToList().Select(x => new SelectListItem
                {
                    Value = x.CellID.ToString(),
                    Text = x.Name
                }).ToList());
            }
            else
            {
                list = RebuildList(new List<SelectListItem>());
            }

            return Json(new SelectList(list, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        [HttpPost]
        public ActionResult Filter(int? Regions, int? Plants, int Departments, int Cells, string PartDesc)
        {
            if (Regions != null)
            {
                if (Regions == -1)
                {
                    var region = new Region();
                    region.Name = Resource.All;
                    Session["Region"] = region;
                }
                else
                    Session["Region"] = db.Regions.Find(Regions);
            }

            if (Plants != null)
            {
                if (Plants == -1)
                {
                    var plant = new Plant();
                    plant.Name = Resource.All;
                    Session["Plant"] = plant;
                }
                else
                    Session["Plant"] = db.Plants.Find(Plants);
            }

            if (Departments == -1)
            {
                var dept = new Department();
                dept.Name = Resource.All;
                Session["Department"] = dept;
            }
            else
                Session["Department"] = db.Departments.Find(Departments);

            if (Cells == -1)
            {
                var cell = new Cell();
                cell.Name = Resource.All;
                Session["Cell"] = cell;
            }
            else
                Session["Cell"] = db.Cells.Find(Cells);

            if(cd.SAPParts.Where(a=>a.PartDesc == PartDesc).Count() > 0)
            {
                var part = new Part();
                part.PartDesc = PartDesc;
                Session["Part"] = part;
            }
            else
            {
                var part = new Part();
                part.PartDesc = Resource.All;
                Session["Part"] = part;
            }

            return RedirectToAction("Index", "Home");
        }

        public static SelectList RebuildList(List<SelectListItem> newList)
        {
            newList.Insert(0, new SelectListItem { Value = "-1", Text = Resource.All });

            var selectedItem = newList.FirstOrDefault(item => item.Selected);
            var selectedItemValue = String.Empty;
            if (selectedItem != null)
            {
                selectedItemValue = selectedItem.Value;
            }

            return new SelectList(newList, "Value", "Text", selectedItemValue);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}