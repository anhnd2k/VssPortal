using DBConect;
using DBConect.conmon;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using vss_portal_web.Areas.Admin.Code;
using vss_portal_web.Areas.Admin.Models;
using vss_portal_web.Models;

namespace vss_portal_web.Controllers
{
    public class LoginForGuestController : Controller
    {
        // GET: LoginForGuest
        public ActionResult Index()
        {
            var cookieName = SessionHelper.GetSession();
            var action = new ActionPost();
            var listPost = action.GetListPost();
            if (cookieName != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(listPost);
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            var action = new ActionPost();
            var listPost = action.GetListPost();

            RedirectModel dataRedirect = TempData["myDataRedirect"] as RedirectModel;
            if (new LdapAuthentication().AuthenticateUserV2(model.UserName, model.PassWord) && ModelState.IsValid)
            {
                if (dataRedirect != null)
                {
                    return RedirectToAction(dataRedirect.IndexRedirect, dataRedirect.ControllerRedirect);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.checkPermision = "";
                ViewBag.showModel = true;
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            }
            return View(listPost);
        }
    }
}