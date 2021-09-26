using DBConect;
using DBConect.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vss_portal_web.Areas.Admin.Code;
using vss_portal_web.Controllers;
using vss_portal_web.Models;

namespace vss_portal_web.Areas.Admin.Controllers
{
    public class HandleTruthController : BaseAdminController
    {
        private string titleEmailhandel = "Thông báo về tiến trình phê duyệt, thực hiện - Nói thật đêêê";
        // GET: Admin/HandleTruth
        public ActionResult Index( string searchString, int idFinterTruth = 0, int page = 1, int pageSize = 10)
        {
            TempData["myDataRedirect"] = CustomerRedirectLogin.CustomRedirects("Index", "Admin/HandleTruth");

            var mailUser = SessionHelper.GetSessionRoleAdmin()?.Email;
            var action = new ActionPost();
            var listRealTalkHandel = action.GetListTalkRealAdmin(searchString, idFinterTruth, page, pageSize);
            ViewData["tabBarSelection"] = "responsibleTruth";
            ViewData["userMail"] = mailUser;
            ViewData["TruthStatusInHanderScreen"] = action.getListTruthStatus();
            ViewData["searchStringHanderScreen"] = searchString;
            ViewData["idSelectedTruthHanderScreen"] = idFinterTruth;
            ViewData["ListPersionResponsible"] = action.GetListPersionManageRealTalk();

            return View(listRealTalkHandel);
        }

        public JsonResult DoingTruth(int idPost)
        {
            bool resDoingTruth = false;
            var dataItem = new ActionPost().DetailRealTalk(idPost);
            var action = new ActionPost();
            EmailService service = new EmailService();
            string body = "Ý tưởng: " 
                          + dataItem.TitleRealTalk 
                          + " của bạn đã được phê duyệt và đang được triển khai, áp dụng </br>"
                          + " Thời gian phê duyệt thực thi: " + DateTime.Now + " " 
                          +" <br/>Người thực thi và phê duyệt:" + SessionHelper.GetSessionRoleAdmin().fullName + " <br/>";
            bool resSendMail = service.SendMailAction(dataItem.MailSender, titleEmailhandel, body);

            if (resSendMail)
            {
                resDoingTruth = action.DoingTruth(idPost);
            }

            return new JsonResult { Data = resDoingTruth, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult CloseTruth(int idPost)
        {
            bool resCloseTruth = false;
            var dataItem = new ActionPost().DetailRealTalk(idPost);
            var action = new ActionPost();
            EmailService service = new EmailService();
            string body = "Ý tưởng: " 
                          + dataItem.TitleRealTalk 
                          + " của bạn đang được xử lý thành công. <br/>"
                          + " Cảm ơn bạn vì những phản hồi tích cực đem lại lợi ích chung cho trung tâm </br>"
                          + " Thời gian phê duyệt:  " + DateTime.Now + " <br/>"
                          + "Người phê duyệt:" + SessionHelper.GetSessionRoleAdmin().fullName + " <br/>";
            bool resSendMail = service.SendMailAction(dataItem.MailSender, titleEmailhandel, body);
            if (resSendMail)
            {
                resCloseTruth = action.CloseTruth(idPost);
            }

            return new JsonResult { Data = resCloseTruth, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ThanksTruth(int idPost)
        {
            bool resThanksTruth = false;
            var dataItem = new ActionPost().DetailRealTalk(idPost);
            var action = new ActionPost();
            EmailService service = new EmailService();
            string body = "Ý tưởng: " 
                          + dataItem.TitleRealTalk 
                          + " của bạn đã bị từ chối. <br/>"
                          + " Cảm ơn bạn vì những phản hồi. </br>"
                          + " Thời gian phê duyệt:  " + DateTime.Now + " <br/>Người phê duyệt:" + SessionHelper.GetSessionRoleAdmin().fullName + " <br/>";
            bool resSendMail = service.SendMailAction(dataItem.MailSender, titleEmailhandel, body);
            if (resSendMail)
            {
                resThanksTruth = action.ThanksTruth(idPost);
            }

            return new JsonResult { Data = resThanksTruth, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}