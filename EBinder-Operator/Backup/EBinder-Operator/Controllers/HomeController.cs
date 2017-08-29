using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBinder_Operator.Models;
using System.Diagnostics;

namespace EBinder_Operator.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private EBinderContext db = new EBinderContext();


        public ActionResult Index()
        {
            //return RedirectToAction("Index", "User", null);
            if (Session["Cell"] == null)
            {
                string username = User.Identity.Name.Split('\\')[1];
                var user = db.Users.First(a => a.Username == username);
                if (user != null)
                {
                    Session["Region"] = user.Cell.Department.Plant.Region;
                    Session["Plant"] = user.Cell.Department.Plant;
                    Session["Departments"] = db.Departments;
                    Session["Department"] = user.Cell.Department;
                    Session["Cells"] = db.Cells;
                    Session["Cell"] = user.Cell;
                    Session["Parts"] = db.Parts;
                    Session["Part"] = null;
                    Session["Language"] = "en";
                }
                else
                {
                    TempData["message"] = "Your username is not yet registered with the EBinder+. Please contact your local EBinder+ specialist.";
                    return View();
                }
            }
                    
            Plant plant = Session["Plant"] as Plant;

            //Check to see if plant data is up to date and update if necessary
            if (db.Plants.Find(plant.PlantID).HomePageReferences != plant.HomePageReferences)
            {
                plant = db.Plants.Find(plant.PlantID);
            }

            ViewData["Categories"] = GetIconList(plant);


            return View();
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

            if (part == null)
            {
                part = new Part();
                part.PartID = -1;
            }


            var refs = db.References.Where(a => a.ReferenceTypeID == iconId)
                                    .Where(a => a.Cells.Any(b=>b.CellID == cell.CellID) ||
                                        a.Departments.Any(b => b.DepartmentID == dept.DepartmentID) ||
                                        a.Plants.Any(b => b.PlantID == plant.PlantID) ||
                                        a.Regions.Any(b => b.RegionID == region.RegionID) ||
                                        a.Parts.Any(b => b.PartID == part.PartID));

            if (refs != null)
            {
                if (refs.Count() == 1)
                {
                    OpenReference(refs.First().ReferenceID);
                    return RedirectToAction("Index");
                }
                else if(refs.Count() > 1)
                {
                    return View(refs);
                }
            }

            TempData["message"] = "There doesnt appear to be any references of the type: " + db.ReferenceTypes.Find(iconId).TypeName;
            return RedirectToAction("Index");
        }

        public JsonResult OpenReference(int refId)
        {
            string src = db.References.Find(refId).URL;
            string ext = System.IO.Path.GetExtension(src).ToLower();

            Microsoft.Office.Interop.Word.Document wordDocument;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            Microsoft.Office.Interop.PowerPoint.Presentation powerpoint;

            if (ext == ".doc" || ext == ".docx")
            {
                Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
                wordDocument = appWord.Documents.Open(src, ReadOnly: true);
                wordDocument.Activate();
                appWord.Visible = true;
            }
            else if (ext == ".xlsx" || ext == ".xlsm" || ext == ".xls")
            {
                Microsoft.Office.Interop.Excel.Application appExcel = new Microsoft.Office.Interop.Excel.Application();
                workbook = appExcel.Workbooks.Open(src, ReadOnly: true);
                workbook.Activate();
                appExcel.Visible = true;
            }
            else if (ext == ".ppt" || ext == ".pptx")
            {
                Microsoft.Office.Interop.PowerPoint.Application appPP = new Microsoft.Office.Interop.PowerPoint.Application();
                appPP.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
                appPP.Activate();
                Microsoft.Office.Interop.PowerPoint.Presentations ps = appPP.Presentations;
                powerpoint = ps.Open(src, ReadOnly: Microsoft.Office.Core.MsoTriState.msoTrue);
            }
            else if (ext == ".pdf" || ext == ".exe")
            {
                Process.Start(src);
            }
            else if (ext == ".accdb" || ext == ".mdb")
            {
                var access = new Microsoft.Office.Interop.Access.Application();
                access.OpenCurrentDatabase(src);
                access.Visible = true;
            }
            else
            {
                TempData["message"] = "There was a problem opening the link: " + src;
            }
            return Json(null);
        }

        private List<Category> GetIconList(Plant plant)
        {
            List<Category> categories = new List<Category>();
            
            
            categories.Add(new Category("Home", plant.HomePageReferences.ToList()));

            foreach(ReferenceCategory rc in db.ReferenceCategory)
            {
                categories.Add(new Category(rc.CategoryName, plant.ReferenceTypes.Where(a => a.ReferenceCategoryID == rc.ReferenceCategoryID).ToList()));
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
