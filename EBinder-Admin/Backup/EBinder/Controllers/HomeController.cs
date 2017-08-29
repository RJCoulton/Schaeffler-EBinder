using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBinder.Models;


namespace EBinder.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private EBinderContext db = new EBinderContext();
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (Session["Cell"] == null)
            {
                string username = User.Identity.Name.Split('\\')[1];
                User user = db.Users.Find(username);

                if (user == null)
                {
                    //Create new user and flag
                    TempData["message"] = "Your username is not yet registered. Please contact an admin to fix this issue.";
                    Session["Cell"] = "Unknown";
                    Session["Department"] = "Unknown";
                    Session["Plant"] = "Unknown";
                    Session["Region"] = "Unknown";
                    Session["UserType"] = "Unknown";
                    Session["Part"] = "Unknown";
                    return View();
                }

                Session["Cell"] = user.Cell.Name;
                Session["Department"] = user.Cell.Department.Name;
                Session["Plant"] = user.Cell.Department.Plant.Name;
                Session["Region"] = user.Cell.Department.Plant.Region.Name;
                Session["UserType"] = user.userType.Name;
                Session["Part"] = "All";
            }
            
            return View();
        }

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
                ViewData["Cells"] = RebuildList(cells.ToList().Select(x=> new SelectListItem
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

        [SessionExpire]
        [HttpPost]
        public ActionResult Filter(int Regions, int Plants, int Departments, int Cells, int Parts)
        {
            if(Regions == -1)
                Session["Region"] = "All";
            else
                Session["Region"] = db.Regions.Find(Regions).Name;

            if (Plants == -1)
                Session["Plant"] = "All";
            else
                Session["Plant"] = db.Plants.Find(Plants).Name;

            if (Departments == -1)
                Session["Department"] = "All";
            else
                Session["Department"] = db.Departments.Find(Departments).Name;

            if (Cells == -1)
                Session["Cell"] = "All";
            else
                Session["Cell"] = db.Cells.Find(Cells).Name;

            if (Parts == -1)
                Session["Part"] = "All";
            else
                Session["Part"] = db.Parts.Find(Parts).PartNumber;

            return View("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
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
