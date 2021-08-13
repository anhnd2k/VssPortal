using DBConect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using vss_portal_web.Models;

namespace vss_portal_web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
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
            var resListPost = list.HomeGetLimedPost();
            var onBoadingPost = list.ListPostOnboading();
            var listThankCard = list.getCountThankCard();

            ModelController controllerModel = new ModelController();
            controllerModel.ListPost = resListPost;
            controllerModel.OnboadingPost = onBoadingPost;
            controllerModel.ListhankCard = listThankCard;
            return View(controllerModel);
        }
    }
}