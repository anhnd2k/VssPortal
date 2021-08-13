using DBConect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using vss_portal_web.Areas.Admin.Models;

namespace vss_portal_web.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        [Authorize]
        // GET: Admin/HomeAdmin
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            string UserName = "";
            string cookieName = FormsAuthentication.FormsCookieName;
            if (HttpContext.Request.Cookies[cookieName] != null)
            {
                HttpCookie authCookie = HttpContext.Request.Cookies[cookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                UserName = ticket.Name;

            }

            ViewData["UserName"] = UserName;

            var list = new ActionPost();

            var allListPostNews = list.GetListPostsNew(page, pageSize);

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