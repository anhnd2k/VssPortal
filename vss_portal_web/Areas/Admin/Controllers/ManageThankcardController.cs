using DBConect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vss_portal_web.Areas.Admin.Controllers
{
    public class ManageThankcardController : Controller
    {
        // GET: Admin/FeedBack
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var action = new ActionPost();
            var res = action.GetListThankCard(page, pageSize);
            return View(res);
        }

        [HttpDelete]
        public ActionResult DeleteFeedBack(int id)
        {
            new ActionPost().DeleteFeedBack(id);
            return RedirectToAction("Index", "ManageThankcard");
        }
    }
}