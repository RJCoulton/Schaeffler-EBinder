using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBinder.Models;
using EBinder.App_GlobalResources;

namespace EBinder.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {

        private EbinderContext db = new EbinderContext();
        //
        // GET: /Home/

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
                    SAPPart part = new SAPPart();
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
                    TempData["message"] = Resource.UsernameNotRegistered;
                    return View();
                }
        
        }
            
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }

        public JsonResult ChangeLang(string lang)
        {
            string all = Resource.All;
            Region region = Session["Region"] as Region;
            Plant plant = Session["Plant"] as Plant;
            Department dept = Session["Department"] as Department;
            Cell cell = Session["Cell"] as Cell;
            SAPPart part = Session["Part"] as SAPPart;

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
            if (part == null) part = new SAPPart();
            if (part.PartDesc == all)
            {
                part.PartDesc = Resource.All;
                Session["Part"] = part;
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }
       

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
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
    }
}
