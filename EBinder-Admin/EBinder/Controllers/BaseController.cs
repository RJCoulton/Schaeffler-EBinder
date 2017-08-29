using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EBinder.Controllers
{
    public class BaseController : Controller
    {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            var lang = Session["Language"];
            if (lang == null)
            {
                lang = SiteLanguages.GetDefaultLanguage();
            }

            new SiteLanguages().SetLanguage(lang.ToString());

            return base.BeginExecuteCore(callback, state);
        }
    }
}