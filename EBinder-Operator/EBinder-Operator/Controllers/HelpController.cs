using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBinder_Operator.Models;
using System.Net.Mail;
using EBinder_Operator.App_GlobalResources;

namespace EBinder_Operator.Controllers
{
    public class HelpController : BaseController
    {
        private EBinderContext db = new EBinderContext();

        [SessionExpire]
        public ActionResult Help()
        {
            return View();
        }


        [SessionExpire]
        [HttpPost]
        public ActionResult Help(string Name, string Issue)
        {
            Plant plant = Session["Plant"] as Plant;
            Department dept = Session["Department"] as Department;
            Cell cell = Session["Cell"] as Cell;
            try
            {
                MailMessage mail = new MailMessage();

                SmtpClient smtpServer = new SmtpClient("mail.na.luk.com");
                smtpServer.Port = 587; 

                mail.From = new MailAddress("EBinder-Support@schaeffler.com");
                mail.To.Add(plant.ContactEmail);
                mail.Subject = "EBinder+ " + Resource.Support;
                mail.Body = Resource.Department + ": " + dept.Name + "\r\n" + Resource.Cell + ": " + cell.Name + "\r\n" + Resource.Name + ": " + Name + "\r\n" + Resource.Issue + ": " + Issue;

                smtpServer.Send(mail);
            }
            catch(Exception ex)
            {
                ViewData["Error"] = "Emailed failed to send.";
            }

            return View();
        }
    }
}
