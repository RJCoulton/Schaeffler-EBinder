using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBinder_Operator.Models;

namespace EBinder_Operator.Controllers
{
    
    public class FilterController : Controller
    {
        private EBinderContext db = new EBinderContext();

        [SessionExpire]
        public ActionResult Filter()
        {
            string region = Session["Region"].ToString();
            string plant = Session["Plant"].ToString();
            string department = Session["Department"].ToString();
            string cell = Session["Cell"].ToString();
            string part = Session["Part"].ToString();


            var regions = db.Regions;


            ViewData["Regions"] = RebuildList(regions.ToList().Select(x => new SelectListItem
            {
                Value = x.RegionID.ToString(),
                Text = x.Name,
                Selected = (x.Name == region)
            }).ToList());


            IQueryable<Plant> plants;
            if (region == "All")
                plants = db.Plants;
            else
                plants = db.Plants.Where(a => a.Region.Name == region);

            ViewData["Plants"] = RebuildList(plants.ToList().Select(x => new SelectListItem
            {
                Value = x.PlantID.ToString(),
                Text = x.Name,
                Selected = (x.Name == plant)
            }).ToList());

            if (plant == "All")
            {
                ViewData["Departments"] = RebuildList(new List<SelectListItem>());
            }
            else
            {
                var depts = db.Departments.Where(a => a.Plant.Name == plant);
                ViewData["Departments"] = RebuildList(depts.ToList().Select(x => new SelectListItem
                {
                    Value = x.DepartmentID.ToString(),
                    Text = x.Name,
                    Selected = (x.Name == department)
                }).ToList());
            }

            if (department == "All")
            {
                ViewData["Cells"] = RebuildList(new List<SelectListItem>());
            }
            else
            {
                var cells = db.Cells.Where(a => a.Department.Name == department);
                ViewData["Cells"] = RebuildList(cells.ToList().Select(x => new SelectListItem
                {
                    Value = x.CellID.ToString(),
                    Text = x.Name,
                    Selected = (x.Name == cell)
                }).ToList());
            }

            if (cell == "All")
            {
                ViewData["Parts"] = RebuildList(new List<SelectListItem>());
            }
            else
            {
                var cells = db.Cells.Where(a => a.Department.Name == department && a.Name == cell);
                var parts = db.Parts.Where(a => a.Cells.Contains(cells.FirstOrDefault()));
                ViewData["Parts"] = RebuildList(parts.ToList().Select(x => new SelectListItem
                {
                    Value = x.PartID.ToString(),
                    Text = x.PartNumber,
                    Selected = (x.PartNumber == part)
                }).ToList());
            }


            return View();
        }


        public JsonResult GetPlants(int id)
        {
            IQueryable<Plant> plants;
            if (id == -1)
                plants = db.Plants;
            else
                plants = db.Plants.Where(a => a.RegionID == id);

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

            return Json(new SelectList(list, "Value", "Text"));
        }

        public JsonResult GetDepartments(int id, int regionID)
        {
            IQueryable<Department> depts;
            if (id == -1)
            {
                if (regionID == -1)
                    depts = db.Departments;
                else
                    depts = db.Departments.Where(a => a.Plant.RegionID == regionID);
            }
            else
            {
                depts = db.Departments.Where(a => a.Plant.PlantID == id);
            }

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

            return Json(new SelectList(list, "Value", "Text"));
        }

        public JsonResult GetCells(int id, int plantID, int regionID)
        {
            IQueryable<Cell> cells;
            if (id == -1)
            {
                if (plantID == -1)
                {
                    if (regionID == -1)
                        cells = db.Cells;
                    else
                        cells = db.Cells.Where(a => a.Department.Plant.RegionID == regionID);
                }
                else
                {
                    cells = db.Cells.Where(a => a.Department.PlantID == plantID);
                }
            }
            else
                cells = db.Cells.Where(a => a.Department.DepartmentID == id);

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

            return Json(new SelectList(list, "Value", "Text"));
        }

        public static SelectList RebuildList(List<SelectListItem> newList)
        {
            newList.Insert(0, new SelectListItem { Value = "-1", Text = "All" });

            var selectedItem = newList.FirstOrDefault(item => item.Selected);
            var selectedItemValue = String.Empty;
            if (selectedItem != null)
            {
                selectedItemValue = selectedItem.Value;
            }

            return new SelectList(newList, "Value", "Text", selectedItemValue);
        }
    }
}
