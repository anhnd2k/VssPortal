using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vss_portal_web.Areas.Admin.Code;

namespace vss_portal_web.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewData["fullNameUser"] = CheckLoginRole.getUserName();
            ViewData["checkRoleLogin"] = CheckLoginRole.check();
            ViewData["departmentUser"] = SessionHelper.GetSession()?.Department;
            ViewData["headerActive"] = "active";
            var session = SessionHelper.GetSession();
            if(session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "LoginForGuest", action = "Index"}));
            }
            base.OnActionExecuted(filterContext);
        }
    }
}