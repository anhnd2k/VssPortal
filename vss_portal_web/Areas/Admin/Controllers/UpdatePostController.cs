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
    public class UpdatePostController : Controller
    {
        // GET: Admin/UpdatePost
        public ActionResult Index(int id)
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
            var postDetail = action.DetailPost(id);
            var listCategory = action.getCategory();

            ModelControllerAdmin controller = new ModelControllerAdmin();
            controller.ListPost = postDetail;
            controller.Category = listCategory;

            return View(controller);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(AddPostsModel model, HttpPostedFileBase ThumbNail, int id)
        {
            var action = new ActionPost();
            var postDetail = action.DetailPost(id);
            var ListCategory = action.getCategory();
            TempData["imgPath"] = postDetail.ThumbNail;

            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = HttpContext.Request.Cookies[cookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string UserName = ticket.Name;
            model.Status = model.checkBox == true ? 1 : 0;
            foreach (var i in ListCategory)
            {
                if (i.CategoryId.ToString() == model.CheckValueCategory)
                {
                    model.Category = (int)i.CategoryId;
                }
            }

            if (ModelState.IsValid && ThumbNail != null && model.PostTitle != null && model.PostConten != null)
            {

                FileInfo imgPath = new FileInfo(ThumbNail.FileName);
                string pathConver = Guid.NewGuid().ToString("N") + imgPath.Extension;
                ThumbNail.SaveAs(Server.MapPath("~/ImageUpload/Thumbnal/" + pathConver));
                model.ThumbNail = pathConver;


                new ActionPost().UpdatePost(model.id, model.PostTitle, model.PostConten, model.Description,UserName, model.ThumbNail, model.Status, model.Category);
                TempData["UpdateSuccess"] = "CẬP NHẬT BÀI VIẾT THÀNH CÔNG!";
                return RedirectToAction("Index", "HomeAdmin");
            }
            if(ModelState.IsValid && ThumbNail == null && model.PostTitle != null && model.PostConten != null)
            {
                model.ThumbNail = TempData["imgPath"].ToString();
                new ActionPost().UpdatePost(model.id, model.PostTitle, model.PostConten, model.Description, UserName, model.ThumbNail, model.Status, model.Category);
                TempData["UpdateSuccess"] = "CẬP NHẬT BÀI VIẾT THÀNH CÔNG!";
                return RedirectToAction("Index", "HomeAdmin");
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng điền đủ thông tin!");
            }
            return View(model);
        }
    }
}