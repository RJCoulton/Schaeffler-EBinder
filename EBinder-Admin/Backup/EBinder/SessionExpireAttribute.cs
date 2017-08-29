using System.Web.Mvc;
using System.Web;

public class SessionExpireAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        HttpContext ctx = HttpContext.Current;

        // check  sessions here
        if (HttpContext.Current.Session["Cell"] == null)
        {
            filterContext.Result = new RedirectResult("~/Home/Index");
            return;
        }

        base.OnActionExecuting(filterContext);
    }
}