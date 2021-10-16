using DBConect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vss_portal_web.Areas.Admin.Controllers
{
    public class ViewDetailRealTalkController : BaseResolverTruth
    {
        // GET: Admin/ViewDetailRealTalk
        public ActionResult Index(int id)
        {
            if(TempData["redirectAction"] == "Handletruth")
            {
                ViewData["tabBarSelection"] = "responsibleTruth";
            }
            else
            {
                ViewData["tabBarSelection"] = "truth";
            }
            var action = new ActionPost();
            var detalConten = action.DetailRealTalk(id);

            ViewData["processTruth"] = action.GetCmtProcess(id);
            return View(detalConten);
        }
    }
}