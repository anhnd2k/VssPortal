using DBConect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using vss_portal_web.Controllers;

namespace vss_portal_web.Areas.Admin.Controllers
{
    public class HomeAdminController : BaseAdminController
    {
        public ActionResult Index(string searchString, int idStatus=0, int page = 1, int pageSize = 10)
        {
            //create customer ridirect
            TempData["myDataRedirect"] = CustomerRedirectLogin.CustomRedirects("Index", "Admin/HomeAdmin");

            var list = new ActionPost();
            var listcategory = list.getCategory();
            ViewData["tabBarSelection"] = "Post";
            ViewData["finterCategory"] = listcategory;
            ViewData["IdStatusFinterHomeAdmin"] = idStatus;
            ViewData["searchStringHomeAdmin"] = searchString;

            var allListPostNews = list.GetListPostsNew(searchString, idStatus, page, pageSize);

            return View(allListPostNews);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new ActionPost().Delete(id);
            return RedirectToAction("Index", "HomeAdmin");
        }
    }
}