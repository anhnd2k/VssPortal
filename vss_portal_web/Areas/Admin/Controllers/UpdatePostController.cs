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

namespace vss_portal_web.Areas.Admin.Controllers
{
    public class UpdatePostController : BaseAdminController
    {
      
        // GET: Admin/UpdatePost
        public ActionResult Index(int id)
        {
            ViewData["UserName"] = SessionHelper.GetSessionRoleAdmin()?.fullName;
            ViewData["tabBarSelection"] = "Post";
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

            string UserName = SessionHelper.GetSessionRoleAdmin()?.fullName;
            model.Status = model.checkBox == true ? 1 : 0;
            foreach (var i in ListCategory)
            {
                if (i.CategoryId.ToString() == model.CheckValueCategory)
                {
                    model.Category = (int)i.CategoryId;
                }
            }
            //cập nhật khi người dùng thay đổi ảnh nền post
            if (ModelState.IsValid && ThumbNail != null && model.PostTitle != null && model.PostConten != null)
            {
                try
                {
                    FileInfo imgPath = new FileInfo(ThumbNail.FileName);
                    string pathConver = Guid.NewGuid().ToString("N") + imgPath.Extension;
                    ThumbNail.SaveAs(Server.MapPath("~/ImageUpload/Thumbnal/" + pathConver));
                    model.ThumbNail = pathConver;


                    new ActionPost().UpdatePost(model.id, model.PostTitle, model.PostConten, model.Description, UserName, model.ThumbNail, model.Status, model.Category);
                    TempData["UpdateSuccess"] = "successUpdate";
                    return RedirectToAction("Index", "HomeAdmin");
                }
                catch
                {
                    ViewData["errUpdatePost"] = "err";
                    return View(model);
                }
            }
            //cập nhật khi người dùng ko thay đổi ảnh nền
            if(ModelState.IsValid && ThumbNail == null && model.PostTitle != null && model.PostConten != null)
            {
                try
                {
                    model.ThumbNail = TempData["imgPath"].ToString();
                    new ActionPost().UpdatePost(model.id, model.PostTitle, model.PostConten, model.Description, UserName, model.ThumbNail, model.Status, model.Category);
                    TempData["UpdateSuccess"] = "successUpdate";
                    return RedirectToAction("Index", "HomeAdmin");
                }
                catch
                {
                    ViewData["errUpdatePost"] = "err";
                    return View(model);
                }
            }
            return View(model);
        }
    }
}