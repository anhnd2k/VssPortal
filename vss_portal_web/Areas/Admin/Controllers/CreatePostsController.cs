using DBConect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using vss_portal_web.Areas.Admin.Code;
using vss_portal_web.Areas.Admin.Models;
using vss_portal_web.Controllers;

namespace vss_portal_web.Areas.Admin.Controllers
{
    public class CreatePostsController : BaseAdminController
    {
        // GET: Admin/OnboadingPosts
        public ActionResult Index()
        {
            TempData["myDataRedirect"] = CustomerRedirectLogin.CustomRedirects("Index", "Admin/CreatePosts");

            string UserName = "";
            string cookieName = FormsAuthentication.FormsCookieName;
            if (HttpContext.Request.Cookies[cookieName] != null)
            {
                HttpCookie authCookie = HttpContext.Request.Cookies[cookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                UserName = ticket.Name;

            }
            ViewData["tabBarSelection"] = "addPost";
            ViewData["UserName"] = UserName;

            var ActionPost = new ActionPost();

            var model = ActionPost.getCategory();

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(AddPostsModel model, HttpPostedFileBase ThumbNail )
        {
            var ActionPost = new ActionPost();
            var ListCategory = ActionPost.getCategory();
            string UserName = SessionHelper.GetSessionRoleAdmin().fullName;
            if (ModelState.IsValid && ThumbNail != null && model.PostTitle != null && model.PostConten != null)
            {
                try
                {
                    FileInfo imgPath = new FileInfo(ThumbNail.FileName);
                    string pathConver = Guid.NewGuid().ToString("N") + imgPath.Extension;
                    ThumbNail.SaveAs(Server.MapPath("~/ImageUpload/Thumbnal/" + pathConver));
                    model.ThumbNail = pathConver;
                    model.Status = model.checkBox == true ? 1 : 0;

                    foreach (var i in ListCategory)
                    {
                        if (i.CategoryId.ToString() == model.CheckValueCategory)
                        {
                            model.Category = (int)i.CategoryId;
                        }
                    }
                    TempData["MessSuccess"] = "success";
                    new ActionPost().AddPostsNew(model.PostTitle, model.PostConten, UserName, model.ThumbNail, model.Description, model.Status, model.Category);
                    return RedirectToAction("Index","HomeAdmin");
                }
                catch
                {
                    ViewData["errCreatePost"] = "err";
                    return View(model);
                }
            }
            return View(model);
        }
    }
}