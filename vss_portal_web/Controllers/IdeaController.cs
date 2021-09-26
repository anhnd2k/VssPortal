using DBConect;
using DBConect.Farmework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using vss_portal_web.Areas.Admin.Code;
using vss_portal_web.Models;

namespace vss_portal_web.Controllers
{
    
    public class IdeaController : BaseController
    {
        public ActionResult Index()
        {
            TempData["myDataRedirect"] = CustomerRedirectLogin.CustomRedirects("Index", "Idea");

            string ticket = string.Empty;
            var action = new ActionPost();
            var listFieldIdea = action.GetField();
            var listDepartment = action.getListDepartment();
            var session = SessionHelper.GetSession();

            ModelController controllerModel = new ModelController();
            controllerModel.FieldIdea = listFieldIdea;
            controllerModel.ListDepartments = listDepartment;

            if (session != null)
            {
                ticket = session.UserName;
            }
            ViewBag.nameSession = ticket;

            return View(controllerModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(ManageRegisterIdea model, HttpPostedFileBase fileUpload, IdeaPlus modelPlus)
        {
            var nameSession = SessionHelper.GetSession();
            ViewBag.nameSession = nameSession.UserName;
            var action = new ActionPost();
            var listField = action.GetField();

            if (modelPlus.individualClick == true)
            {
                model.IndividualIdea = true;
                model.EmailIndividual = null;
            }
            else
            {
                model.IndividualIdea = false;
                if(modelPlus.InitativeDepartment == true)
                {
                    model.EmailIndividual = null;
                }
                else
                {
                    model.IdeaOfDepartment = null;
                }
            }

            model.AuthorUserId = nameSession.UserName;
            model.AuthorFullName = nameSession.fullName;
            model.AuthorEmail = nameSession.Email;
            model.AuthorDepartment = nameSession.Department;

            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                try
                {
                    FileInfo imgPath = new FileInfo(fileUpload.FileName);
                    string pathConver = Guid.NewGuid().ToString("N") + imgPath.Extension;
                    //model.DocumentIdea = Path.GetFileName(fileUpload.FileName);
                    model.DocumentIdea = pathConver;
                    string path = Path.Combine(Server.MapPath("~/UploadFiles/fileIdea"), pathConver);
                    fileUpload.SaveAs(path);
                }
                catch 
                {
                    ViewBag.success = "FALSE";
                    return View(listField);
                }
            }

            bool res = model.Senddata();

            if (res)
            {
                ViewBag.success = "SUCCESS";
                return RedirectToAction("ThanksForSendIdea", "Idea");
            }
            else
            {
                ViewBag.success = "FALSE";
                return View(listField);
            }
        }

        public ActionResult ThanksForSendIdea()
        {
            return View();
        }

        //auto compelete email viettel
        [HttpPost]
        public JsonResult autocompeleteMail(string value)
        {
            var action = new ActionPost();
            var resEmployee = action.autocompleteEmail(value);

            return new JsonResult { Data = resEmployee, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //View sáng kiến đã đăng ký
        public ActionResult IdeaRegestered ()
        {
            TempData["myDataRedirect"] = CustomerRedirectLogin.CustomRedirects("IdeaRegestered", "Idea");
            string username = SessionHelper.GetSession()?.UserName;
            if(username != null)
            {
                var listIdeaRegestered = new ActionPost().findIdeaRegestered(username);
                return View(listIdeaRegestered);
            }
            return RedirectToAction("Index", "LoginForGuest");
        }

        //Xem thông tin chi tiết ý tưởng đã đăng ký
        public ActionResult DetailIdeaRegester (int id)
        {
            var detailIdeaRegester = new ActionPost().DetailIdeaRegester(id);
            return View(detailIdeaRegester);
        }

        //Download file document

        public FileResult Download(string imgName)
        {

            if (imgName != "")
            {
                var filePath = "~/UploadFiles/fileIdea/" + imgName;
                return File(filePath, "application/force- download", Path.GetFileName(filePath));
            }
            return null;
        }

    }

    // post ý tưởng
    public static class ADExtensionMethods
    {
        public static bool Senddata(this ManageRegisterIdea model)
        {
            try
            {
                new ActionPost().PostResigterIdea(model);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}