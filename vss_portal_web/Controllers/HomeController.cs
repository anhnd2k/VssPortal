using DBConect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using vss_portal_web.Areas.Admin.Code;
using vss_portal_web.Areas.Admin.Models;
using vss_portal_web.Models;

namespace vss_portal_web.Controllers
{
    public class HomeController : Controller
    {
        [HandleError]
        // GET: Home
        public ActionResult Index()
        {
            var cookie = SessionHelper.GetSession();
            if (cookie != null)
            {
                ViewBag.checkPermision = true;
            }

            ViewData["fullNameUser"] = CheckLoginRole.getUserName();
            ViewData["departmentUser"] = SessionHelper.GetSession()?.Department;
            ViewData["checkRoleLogin"] = CheckLoginRole.check();

            var list = new ActionPost();
            var resListPost = list.HomeGetLimedPost();
            var onBoadingPost = list.ListPostOnboading();
            var listThankCard = list.getCountThankCard();
            var listTalkReal = list.getListTalkReal();

            var listIdeas = list.GetAllListIdeaRegester();
            var listInitatives = list.GetAllListInitativeRegester();

            ModelController controllerModel = new ModelController();
            controllerModel.ListPost = resListPost;
            controllerModel.OnboadingPost = onBoadingPost;
            controllerModel.ListhankCard = listThankCard;
            controllerModel.ListTalkReal = listTalkReal;
            controllerModel.ListIdea = listIdeas;
            controllerModel.ListInitative = listInitatives;

            return View(controllerModel);
        }
        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            var list = new ActionPost();
            var resListPost = list.HomeGetLimedPost();
            var onBoadingPost = list.ListPostOnboading();
            var listThankCard = list.getCountThankCard();
            var listTalkReal = list.getListTalkReal();

            var listIdeas = list.GetAllListIdeaRegester();
            var listInitatives = list.GetAllListInitativeRegester();

            ModelController controllerModel = new ModelController();
            controllerModel.ListPost = resListPost;
            controllerModel.OnboadingPost = onBoadingPost;
            controllerModel.ListhankCard = listThankCard;
            controllerModel.ListTalkReal = listTalkReal;
            controllerModel.ListIdea = listIdeas;
            controllerModel.ListInitative = listInitatives;

            var action = new AccountModel();

            //test
            //var resTest = action.GetAUser();

            //if (action.Login(model.UserName, model.PassWord) && ModelState.IsValid)
            //{
            //    SessionHelper.resetSession();
            //    SessionHelper.SetSecssion(new UserSession() { UserName = model.UserName });
            //    //admin
            //    FormsAuthentication.SetAuthCookie(model.UserName, true);
            //    return RedirectToAction("Index", model.RedirectName);
            //}
            //if (action.AuthenticateUserV2(model.UserName, model.PassWord) && ModelState.IsValid)
            //{
            //    SessionHelper.resetSession();
            //    //FormsAuthentication.SetAuthCookie(model.UserName, true);
            //    SessionHelper.SetSecssion(new UserSession() { UserName = model.UserName });
            //    return RedirectToAction("Index", model.RedirectName);
            //}
            if (new LdapAuthentication().AuthenticateUserV2(model.UserName, model.PassWord) && ModelState.IsValid)
            {
                return RedirectToAction(model.RedirectAction, model.RedirectName);
            }
            else
            {
                ViewBag.checkPermision = "";
                ViewBag.showModel = true;
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            }
            return View(controllerModel);
        }
    }

    public static class CheckLoginRole
    {
        public static string check()
        {
            var cookie = SessionHelper.GetSession();
            var cookieAdmin = SessionHelper.GetSessionRoleAdmin();
            var cookieResolver = SessionHelper.GetSessionRoleResolverTruth();
            if (cookie != null && cookieAdmin == null && cookieResolver == null)
            {
                return "Guest";
            }

            if (cookieAdmin != null)
            {
                return "Admin";
            }

            if(cookieResolver != null)
            {
                return "Resolver";
            }
            return "noLogin";
        }
        public static string getUserName()
        {
            var cookie = SessionHelper.GetSession();
            if (cookie != null )
            {
                return cookie.fullName;
            }

            return "";
        }
    }

    public static class CustomerRedirectLogin
    {
        public static RedirectModel CustomRedirects (string index, string controllers)
        {
            RedirectModel customerRedirect = new RedirectModel()
            {
                IndexRedirect = index,
                ControllerRedirect = controllers
            };
            return customerRedirect;
        }
    }

}