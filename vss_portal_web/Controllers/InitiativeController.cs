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
    public class InitiativeController : BaseController
    {
        public ActionResult Index()
        {
            TempData["myDataRedirect"] = CustomerRedirectLogin.CustomRedirects("Index", "Initiative");

            string fullNameSess = string.Empty;
            var action = new ActionPost();
            var listField = action.GetField();
            var listUnitApply = action.getUnitApply();
            var listDepartment = action.getListDepartment();

            ModelController controllerModel = new ModelController();
            controllerModel.UnitApplyModel = listUnitApply;
            controllerModel.FieldIdea = listField;
            controllerModel.ListDepartments = listDepartment;

            var session = SessionHelper.GetSession();
            if(session != null)
            {
                ViewBag.nameSession = session.UserName;
            }

            return View(controllerModel);
        }

        [HttpPost]
        [ValidateInput(false)]

        // đăng ký sáng kiến
        public ActionResult Index(InitativeManage model, HttpPostedFileBase fileUpload, IdeaPlus modelPlus)
        {
            var nameSession = SessionHelper.GetSession();
            var action = new ActionPost();
            var listField = action.GetField();
            var listUnitApply = action.getUnitApply();

            ModelController controllerModel = new ModelController();
            controllerModel.UnitApplyModel = listUnitApply;
            controllerModel.FieldIdea = listField;

            //người gửi sáng kiến

            if(nameSession != null)
            {
                ViewBag.nameSession = nameSession.UserName;
                model.UserSend = nameSession.UserName;
                model.EmailUserSend = nameSession.Email;
                model.UserSendFullName = nameSession.fullName;
                model.UserSenDepartment = nameSession.Department;
            }
            if(modelPlus.InitativeDepartment == true)
            {
                model.MailOfEach = null;
            }
            else
            {
                model.InitativeOfDepartment = null;
            }

            //check sáng kiến này của cá nhân hay tập thể
            if (modelPlus.individualClick == true)
            {
                model.PersonalInitative = true;
                model.MailOfEach = null;
            }
            else
            {
                model.PersonalInitative = false;
            }
            //file upload
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                try
                {
                    FileInfo imgPath = new FileInfo(fileUpload.FileName);
                    string pathConver = Guid.NewGuid().ToString("N") + imgPath.Extension;
                    model.NameDocument = pathConver;
                    string path = Path.Combine(Server.MapPath("~/UploadFiles/FileInitative"), pathConver);
                    fileUpload.SaveAs(path);
                }
                catch
                {
                    ViewBag.successInitative = "FALSE";
                    return View(controllerModel);
                }
            }

            bool res = model.SendInitative();

            if (res)
            {
                ViewBag.successInitative = "SUCCESS";
                return RedirectToAction("ThanksForSendInitative", "Initiative");
            }
            else
            {
                ViewBag.successInitative = "FALSE";
                return View(controllerModel);
            }
        }

        public ActionResult ThanksForSendInitative()
        {
            return View();
        }

        //View sáng kiến đã đăng ký
        public ActionResult InitativeRegestered()
        {
            TempData["myDataRedirect"] = CustomerRedirectLogin.CustomRedirects("InitativeRegestered", "Initiative");
            string username = SessionHelper.GetSession()?.UserName;
            if (username != null)
            {
                var listInitativeRegestered = new ActionPost().findIinitativeRegestered(username);
                return View(listInitativeRegestered);
            }

            return RedirectToAction("Index", "LoginForGuest");
        }
        //chi tiết sáng kiến đã đăng ký
        public ActionResult DetailIntativeRegester(int id)
        {
            var detailInitativeRegester = new ActionPost().DetailInitativeRegester(id);
            return View(detailInitativeRegester);
        }

        // download document

        public FileResult Download(string imgName)
        {

            if (imgName != "")
            {
                var filePath = "~/UploadFiles/FileInitative/" + imgName;
                return File(filePath, "application/force- download", Path.GetFileName(filePath));
            }
            return null;
        }
    }

    public static class CreateInitative
    {
        public static bool SendInitative(this InitativeManage model)
        {
            try
            {
                new ActionPost().CreateInitativeRegester(model);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}