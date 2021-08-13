using DBConect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace vss_portal_web.Controllers
{
    public class DetailPostController : Controller
    {
        // GET: DetailPost
        public ActionResult Index(int id)
        {
            var action = new ActionPost();
            var postDetail = action.DetailPost(id);

            string UserName = "";
            string cookieName = FormsAuthentication.FormsCookieName;
            if (HttpContext.Request.Cookies[cookieName] != null)
            {
                HttpCookie authCookie = HttpContext.Request.Cookies[cookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                UserName = ticket.Name;

            }

            ViewData["UserName"] = UserName;

            return View(postDetail);
        }

        public ActionResult OnboadingPost(int id)
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

            var action = new ActionPost();
            var OnboadingDeatail = action.DetailOnBoadingPost(id);

            return View(OnboadingDeatail);
        }
    }
}