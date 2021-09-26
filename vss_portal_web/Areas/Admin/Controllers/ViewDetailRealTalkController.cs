using DBConect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vss_portal_web.Areas.Admin.Controllers
{
    public class ViewDetailRealTalkController : BaseAdminController
    {
        // GET: Admin/ViewDetailRealTalk
        public ActionResult Index(int id)
        {
            ViewData["tabBarSelection"] = "truth";
            var action = new ActionPost();
            var detalConten = action.DetailRealTalk(id);
            return View(detalConten);
        }
    }
}