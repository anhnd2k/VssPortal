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
            string body = "Thân gửi Quý Anh/Chị: " + dataItem.NameSender + "<br/>"
                         + "Cảm ơn Anh/Chị đã gửi lời nói thật tới hòm thư \"Nói thật đê\" của Trung tâm VSS." + "<br/>"
                         + "Đóng góp của bạn đang được xử lý bởi phía phòng ban phụ trách. Nội dung đóng góp của Anh/Chị đã được đăng lên kênh thông tin \"Nói thật đê\" tại link: "
                         + string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("/RealTalk/NewFeedTruth"))
                         + " Anh/Chị có thể theo dõi nhé ạ" + "<br/>"
                         + "BTC cảm ơn bạn vì sự tận tụy của mình, VSS tự hào vì có sự đóng góp của bạn. " + "<br/>"
                         + "Xin trân trọng cảm ơn những nỗ lực của bạn.";
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
            string body = "Thân gửi Quý Anh/Chị: " + dataItem.NameSender + "<br/>"
                         + "Cảm ơn Anh/Chị đã gửi lời nói thật tới hòm thư \"Nói thật đê\" của Trung tâm VSS." + "<br/>"
                         + "Đóng góp của bạn đã được xử lý thành công. Cảm ơn bạn vì sự tận tuỵ này và VSS tự hào vì có sự đồng hành của bạn." + "<br/>"
                         + "Nội dung đóng góp của Anh/Chị đã được đăng lên kênh thông tin \"Nói thật đê\" tại link: "
                         + string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("/RealTalk/NewFeedTruth"))
                         + " Anh/Chị có thể theo dõi nhé ạ" + "<br/>"
                         + "BTC cảm ơn bạn vì sự tận tụy của mình, VSS tự hào vì có sự đóng góp của bạn. " + "<br/>"
                         + "Xin trân trọng cảm ơn những nỗ lực của bạn.";
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
            string body = "Thân gửi Quý Anh/Chị: " + dataItem.NameSender + "<br/>"
                         + "Cảm ơn Anh/Chị đã gửi lời nói thật tới hòm thư \"Nói thật đê\" của Trung tâm VSS." + "<br/>"
                         + "Đóng góp của bạn đã được tiếp nhận bởi phía Phòng ban phụ trách và đang được xử lý. Nội dung đóng góp của Anh/Chị đã được đăng lên kênh thông tin \"Nói thật đê\" tại link: "
                         + string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("/RealTalk/TruthRegestered"))
                         + " Anh/Chị có thể theo dõi nhé ạ" + "<br/>"
                         + "BTC cảm ơn bạn vì sự tận tụy của mình, VSS tự hào vì có sự đóng góp của bạn. " + "<br/>"
                         + "Xin trân trọng cảm ơn những nỗ lực của bạn.";
            bool resSendMail = service.SendMailAction(dataItem.MailSender, titleEmailhandel, body);
            if (resSendMail)
            {
                resThanksTruth = action.ThanksTruth(idPost);
            }

            return new JsonResult { Data = resThanksTruth, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}