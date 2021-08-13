using DBConect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using vss_portal_web.Areas.Admin.Code;
using vss_portal_web.Areas.Admin.Models;

namespace vss_portal_web.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Admin
        [HttpGet]
        public ActionResult Index()
        {
            //check người dùng đã đăng nhập chưa và điểu hướng bỏ qua màn hình login
            string cookieName = FormsAuthentication.FormsCookieName;

            if (HttpContext.Request.Cookies[cookieName] != null)
            {
                return RedirectToAction("Index", "HomeAdmin");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index (LoginModel model)
        {
            //var res = new AccountModel().Login(model.UserName, model.PassWord);
            if (Membership.ValidateUser(model.UserName, model.PassWord) && ModelState.IsValid)
            {

                //SessionHelper.SetSecssion(new UserSession() { UserName = model.UserName });
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                return RedirectToAction("Index", "HomeAdmin");
            }
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            }
            return View(model);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Admin");
        }
    }
}