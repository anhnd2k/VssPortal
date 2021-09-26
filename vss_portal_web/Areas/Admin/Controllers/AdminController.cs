using DBConect;
using DBConect.conmon;
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
            var cookieAdmin = SessionHelper.GetSessionRoleAdmin();
            var cookie = SessionHelper.GetSession();
            if (cookieAdmin != null)
            {
                return RedirectToAction("Index", "HomeAdmin");
            }
            if(cookie == null)
            {
                return RedirectToAction("Index", "LoginForGuest", new { area = "" });
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index (LoginModel model)
        {
            var action = new AccountModel();
            //var res = new AccountModel().Login(model.UserName, model.PassWord);
            if (Membership.ValidateUser(model.UserName, model.PassWord) && ModelState.IsValid)
            {
                //SessionHelper.resetSession();
                //SessionHelper.SetSecssion(new UserSession() { UserName = model.UserName });
                //admin 
                FormsAuthentication.SetAuthCookie(model.UserName, false);
                return RedirectToAction("Index", "HomeAdmin");
            }
            //if(action.AuthenticateUserV2(model.UserName, model.PassWord) && ModelState.IsValid)
            //{
            //    SessionHelper.resetSession();
            //    SessionHelper.SetSecssion(new UserSession() { UserName = model.UserName });
            //    return RedirectToAction("Index", "HomeAdmin");
            //}
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            }
            return View(model);
        }

        public ActionResult LogOut()
        {
            SessionHelper.resetSession();
            return RedirectToAction("Index", "LoginForGuest", new { area = "" });
        }
    }
}