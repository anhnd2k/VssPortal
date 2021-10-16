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
    public class HandleTruthController : BaseResolverTruth
    {
        private string titleEmailhandel = "Thông báo về tiến trình phê duyệt, thực hiện - Nói thật đêêê";
        // GET: Admin/HandleTruth
        public ActionResult Index( string searchString, int idFinterTruth = 0, int page = 1, int pageSize = 10)
        {
            TempData["myDataRedirect"] = CustomerRedirectLogin.CustomRedirects("Index", "Admin/HandleTruth");
            TempData["redirectAction"] = "Handletruth";

            var mailUser = string.Empty;
            if (SessionHelper.GetSessionRoleAdmin() != null)
            {
                mailUser = SessionHelper.GetSessionRoleAdmin()?.Email;
            }
            else
            {
                mailUser = SessionHelper.GetSessionRoleResolverTruth()?.Email;
            }
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

        public JsonResult DoingTruth(int idPost, string cmt)
        {
            bool resDoingTruth = false;
            bool resSendMail = true;
            var dataItem = new ActionPost().DetailRealTalk(idPost);
            var action = new ActionPost();
            var namePersionHandle = SessionHelper.GetSessionRoleResolverTruth().fullName;
            var MailPersionHandle = SessionHelper.GetSessionRoleResolverTruth().Email;
            if (dataItem.NameSender != null && dataItem.MailSender != null)
            {
                EmailService service = new EmailService();
                string body = "Thân gửi Quý Anh/Chị: " + dataItem.NameSender + "<br/>"
                             + "Cảm ơn Anh/Chị đã gửi lời nói thật tới hòm thư \"Nói thật đê\" của Trung tâm VSS." + "<br/>"
                             + "Đóng góp của bạn đang được xử lý bởi phía phòng ban phụ trách. Nội dung đóng góp của Anh/Chị đã được đăng lên kênh thông tin \"Nói thật đê\" tại link: "
                             + string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("/RealTalk/NewFeedTruth"))
                             + " Anh/Chị có thể theo dõi nhé ạ" + "<br/>"
                             + "BTC cảm ơn bạn vì sự tận tụy của mình, VSS tự hào vì có sự đóng góp của bạn. " + "<br/>"
                             + "Xin trân trọng cảm ơn những nỗ lực của bạn.";
                resSendMail = service.SendMailAction(dataItem.MailSender, titleEmailhandel, body);
            }

            if (resSendMail)
            {
                resDoingTruth = action.DoingTruth(idPost);
                action.CreateCommentTruth(idPost, cmt, 3, namePersionHandle, MailPersionHandle);
            }

            return new JsonResult { Data = resDoingTruth, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult CloseTruth(int idPost, string cmt)
        {
            bool resCloseTruth = false;
            bool resSendMail = true;
            var dataItem = new ActionPost().DetailRealTalk(idPost);
            var action = new ActionPost();
            var namePersionHandle = SessionHelper.GetSessionRoleResolverTruth().fullName;
            var MailPersionHandle = SessionHelper.GetSessionRoleResolverTruth().Email;
            if (dataItem.NameSender != null && dataItem.MailSender != null)
            {
                EmailService service = new EmailService();
                string body = "Thân gửi Quý Anh/Chị: " + dataItem.NameSender + "<br/>"
                             + "Cảm ơn Anh/Chị đã gửi lời nói thật tới hòm thư \"Nói thật đê\" của Trung tâm VSS." + "<br/>"
                             + "Đóng góp của bạn đã được xử lý thành công. Cảm ơn bạn vì sự tận tuỵ này và VSS tự hào vì có sự đồng hành của bạn." + "<br/>"
                             + "Nội dung đóng góp của Anh/Chị đã được đăng lên kênh thông tin \"Nói thật đê\" tại link: "
                             + string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("/RealTalk/NewFeedTruth"))
                             + " Anh/Chị có thể theo dõi nhé ạ" + "<br/>"
                             + "BTC cảm ơn bạn vì sự tận tụy của mình, VSS tự hào vì có sự đóng góp của bạn. " + "<br/>"
                             + "Xin trân trọng cảm ơn những nỗ lực của bạn.";
                resSendMail = service.SendMailAction(dataItem.MailSender, titleEmailhandel, body);
            }

            if (resSendMail)
            {
                resCloseTruth = action.CloseTruth(idPost);
                action.CreateCommentTruth(idPost, cmt, 4, namePersionHandle, MailPersionHandle);
            }

            return new JsonResult { Data = resCloseTruth, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ThanksTruth(int idPost, string cmt)
        {
            bool resThanksTruth = false;
            bool resSendMail = true;
            var dataItem = new ActionPost().DetailRealTalk(idPost);
            var action = new ActionPost();
            var namePersionHandle = SessionHelper.GetSessionRoleResolverTruth().fullName;
            var MailPersionHandle = SessionHelper.GetSessionRoleResolverTruth().Email;
            if (dataItem.NameSender != null && dataItem.MailSender != null)
            {
                EmailService service = new EmailService();
                string body = "Thân gửi Quý Anh/Chị: " + dataItem.NameSender + "<br/>"
                             + "Cảm ơn Anh/Chị đã gửi lời nói thật tới hòm thư \"Nói thật đê\" của Trung tâm VSS." + "<br/>"
                             + "Đóng góp của bạn đã được tiếp nhận bởi phía Phòng ban phụ trách và đang được xử lý. Nội dung đóng góp của Anh/Chị đã được đăng lên kênh thông tin \"Nói thật đê\" tại link: "
                             + string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("/RealTalk/NewFeedTruth"))
                             + " Anh/Chị có thể theo dõi nhé ạ" + "<br/>"
                             + "BTC cảm ơn bạn vì sự tận tụy của mình, VSS tự hào vì có sự đóng góp của bạn. " + "<br/>"
                             + "Xin trân trọng cảm ơn những nỗ lực của bạn.";
                resSendMail = service.SendMailAction(dataItem.MailSender, titleEmailhandel, body);
            }
            if (resSendMail)
            {
                resThanksTruth = action.ThanksTruth(idPost);
                action.CreateCommentTruth(idPost, cmt, 5, namePersionHandle, MailPersionHandle);
            }

            return new JsonResult { Data = resThanksTruth, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}