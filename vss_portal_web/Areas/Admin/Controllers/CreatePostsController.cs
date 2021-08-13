using DBConect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using vss_portal_web.Areas.Admin.Models;

namespace vss_portal_web.Areas.Admin.Controllers
{
    public class CreatePostsController : Controller
    {
        // GET: Admin/OnboadingPosts
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

            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = HttpContext.Request.Cookies[cookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string UserName = ticket.Name;
            if (ModelState.IsValid && ThumbNail != null && model.PostTitle != null && model.PostConten != null)
            {

                FileInfo imgPath = new FileInfo(ThumbNail.FileName);
                string pathConver = Guid.NewGuid().ToString("N") + imgPath.Extension;
                ThumbNail.SaveAs(Server.MapPath("~/ImageUpload/Thumbnal/" + pathConver));
                model.ThumbNail = pathConver;
                model.Status = model.checkBox == true ? 1 : 0;

                foreach(var i in ListCategory)
                {
                    if(i.CategoryId.ToString() == model.CheckValueCategory)
                    {
                        model.Category = (int)i.CategoryId;
                    }
                }
                TempData["MessSuccess"] = "THÊM MỚI BÀI VIẾT THÀNH CÔNG!";
                new ActionPost().AddPostsNew(model.PostTitle, model.PostConten, UserName, model.ThumbNail, model.Description, model.Status, model.Category);
                return RedirectToAction("Index", "HomeAdmin");
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng điền đủ thông tin!");
                ViewData["errPosts"] = "Vui lòng điền đủ thông tin!";
            }
            return View(model);
        }
    }
}