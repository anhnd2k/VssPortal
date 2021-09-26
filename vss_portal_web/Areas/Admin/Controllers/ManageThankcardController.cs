using DBConect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vss_portal_web.Controllers;
using vss_portal_web.Models;

namespace vss_portal_web.Areas.Admin.Controllers
{
    public class ManageThankcardController : BaseAdminController
    {
      
        // GET: Admin/FeedBack
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            TempData["myDataRedirect"] = CustomerRedirectLogin.CustomRedirects("Index", "Admin/ManageThankcard");

            var action = new ActionPost();
            var res = action.GetListThankCard(page, pageSize);
            ViewData["tabBarSelection"] = "Thankcard";
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