using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vss_portal_web.Areas.Admin.Code;

namespace vss_portal_web.Areas.Admin.Controllers
{
    public class BaseResolverTruth : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var sessionAdmin = SessionHelper.GetSessionRoleAdmin();
            var sessionResolver = SessionHelper.GetSessionRoleResolverTruth();
            if(sessionAdmin != null)
            {
                ViewData["UserName"] = sessionAdmin?.fullName;
            }
            else
            {
                ViewData["UserName"] = sessionResolver?.fullName;
            }
            if (sessionAdmin == null && sessionResolver == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Admin", action = "Index" }));
            }
            base.OnActionExecuted(filterContext);
        }
    }
}