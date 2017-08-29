using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBinder_Operator.Models;
using System.Diagnostics;
using EBinder_Operator.App_GlobalResources;

namespace EBinder_Operator.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private EBinderContext db = new EBinderContext();


        public ActionResult Index()
        {
            User curUser = Session["User"] as User;
            if (curUser == null) curUser = new User();
            string username = User.Identity.Name.Split('\\')[1].ToLower();
            
            if (curUser.Username != username)
            {
                User user = db.Users.FirstOrDefault(a => a.Username == username);
                if (user != null)
                {
                    Session["Region"] = user.Cell.Department.Plant.Region;
                    Session["Plant"] = user.Cell.Department.Plant;
                    Session["Departments"] = db.Departments; //May be able to remove these
                    Session["Department"] = user.Cell.Department;
                    Session["Cells"] = db.Cells; //May be able to remove these
                    Session["Cell"] = user.Cell;
                    Session["Parts"] = db.Parts; //May be able to remove these
                    Part part = new Part();
                    part.PartDesc = Resource.All;
                    Session["Part"] = part;
                    string lang = user.Cell.Department.Plant.Language;
                    SiteLanguages sl = new SiteLanguages();
                    if (SiteLanguages.IsLanguageAvailable(lang))
                        new SiteLanguages().SetLanguage(lang);
                    else
                        new SiteLanguages().SetLanguage(SiteLanguages.GetDefaultLanguage());
                    Session["UserType"] = user.UserType.Name;
                    Session["User"] = user;
                }
                else
                {
                    TempData["message"] = Resource.UsernameError;
                    return View();
                }
            }

            ViewData["Categories"] = GetIconList();
            return View();
        }

        public JsonResult ChangeLang(string lang)
        {
            string all = Resource.All;
            Region region = Session["Region"] as Region;
            Plant plant = Session["Plant"] as Plant;
            Department dept = Session["Department"] as Department;
            Cell cell = Session["Cell"] as Cell;
            Part part = Session["Part"] as Part;

            new SiteLanguages().SetLanguage(lang);

            if (region.Name == all)
            {
                region.Name = Resource.All;
                Session["Region"] = region;
            }
            if (plant.Name == all)
            {
                plant.Name = Resource.All;
                Session["Plant"] = plant;
            }
            if (dept.Name == all)
            {
                dept.Name = Resource.All;
                Session["Department"] = dept;
            }
            if (cell.Name == all)
            {
                cell.Name = Resource.All;
                Session["Cell"] = cell;
            }
            if (part.PartDesc == all)
            {
                part.PartDesc = Resource.All;
                Session["Part"] = part;
            }

            return Json("success");
        }

        [SessionExpire]
        public ActionResult ReferenceList(int iconId)
        {
            ViewData["RefType"] = db.ReferenceTypes.Find(iconId);
            Cell cell = Session["Cell"] as Cell;
            Department dept = Session["Department"] as Department;
            Plant plant = Session["Plant"] as Plant;
            Region region = Session["Region"] as Region;
            Part part = Session["Part"] as Part;

            var allRefs = db.References.Where(a => a.ReferenceTypeID == iconId);
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
                refs = refs.Concat(allRefs.Where(a => a.Cells.Any(b => b.DepartmentID  == dept.DepartmentID)));
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

            if (refs != null)
            {
                if (refs.Count() >= 1)
                {
                    return View(refs);
                }
            }

            TempData["message"] = Resource.NoRefError + ": " + db.ReferenceTypes.Find(iconId).TypeName_en;
            return RedirectToAction("Index");
        }

        private List<Category> GetIconList()
        {
            List<Category> categories = new List<Category>();

            var sesPlant = Session["Plant"] as Plant;
            if (sesPlant == null || sesPlant.Name == Resource.All)
            {
                foreach (ReferenceCategory rc in db.ReferenceCategories)
                {
                    if(Session["Language"].ToString() == "es")
                        categories.Add(new Category(rc.CategoryName_es, db.ReferenceTypes.Where(a => a.ReferenceCategoryID == rc.ReferenceCategoryID).ToList()));
                    else
                        categories.Add(new Category(rc.CategoryName_en, db.ReferenceTypes.Where(a => a.ReferenceCategoryID == rc.ReferenceCategoryID).ToList()));
                }

                return categories;
            }


            var plant = db.Plants.Find(sesPlant.PlantID);
            categories.Add(new Category(Resource.Home, plant.HomePageReferences.ToList()));

            foreach (ReferenceCategory rc in db.ReferenceCategories)
            {
                if (Session["Language"].ToString() == "es")
                    categories.Add(new Category(rc.CategoryName_es, plant.ReferenceTypes.Where(a => a.ReferenceCategoryID == rc.ReferenceCategoryID).ToList()));
                else
                    categories.Add(new Category(rc.CategoryName_en, plant.ReferenceTypes.Where(a => a.ReferenceCategoryID == rc.ReferenceCategoryID).ToList()));
            }

            return categories;
        }
    }
}

namespace EBinder_Operator.Models
{
    public class Category
    {
        public string Name { get; set; }
        public List<ReferenceType> List { get; set; }

        public Category(string Name, List<ReferenceType> List)
        {
            this.Name = Name;
            this.List = List;
        }
    }
}
