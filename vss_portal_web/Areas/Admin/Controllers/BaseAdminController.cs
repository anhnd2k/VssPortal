using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vss_portal_web.Areas.Admin.Code;

namespace vss_portal_web.Areas.Admin.Controllers
{
    public class BaseAdminController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            ViewData["UserName"] = SessionHelper.GetSessionRoleAdmin()?.fullName;
            var session = SessionHelper.GetSessionRoleAdmin();
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Admin", action = "Index" }));
            }
            base.OnActionExecuted(filterContext);
        }
    }
}