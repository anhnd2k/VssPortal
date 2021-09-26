using DBConect;
using DBConect.Farmework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vss_portal_web.Areas.Admin.Code;
using vss_portal_web.Areas.Admin.Models;
using vss_portal_web.Controllers;
using vss_portal_web.Models;

namespace vss_portal_web.Areas.Admin.Controllers
{
    public class ManageTalkRealController : BaseAdminController
    {
        private string titleEmail = "Thông báo về việc xét duyệt Truth - Nói thật đêêê";
        private string titleEmailToResponsible = "Thông báo về việc tiếp nhận, xét duyệt, xử lý Truth - Nói thật đêêê ";

        // GET: Admin/ManageTalkReal
        public ActionResult Index(string searchString, int idFinterTruth = 0, int page = 1, int pageSize = 10)
        {
            TempData["myDataRedirect"] = CustomerRedirectLogin.CustomRedirects("Index", "Admin/ManageTalkReal");

            var action = new ActionPost();
            var res = action.GetListTalkRealAdmin(searchString, idFinterTruth, page, pageSize);
            var listFiend = action.getListFieldRealTalk();
            var listTruthStatus = action.getListTruthStatus();

            ViewData["tabBarSelection"] = "truth";

            ViewData["FiendList"] = listFiend;
            ViewData["TruthStatus"] = listTruthStatus;
            ViewData["idSelectedTruth"] = idFinterTruth;
            ViewData["searchString"] = searchString;

            return View(res);
        }

        // Màn hình quản lý người xét duyệt
        public ActionResult ManagePersionEditTruth()
        {
            TempData["myDataRedirect"] = CustomerRedirectLogin.CustomRedirects("ManagePersionEditTruth", "Admin/ManageTalkReal");

            var action = new ActionPost();

            ViewData["tabBarSelection"] = "truth";

            ModelControllerAdmin model = new ModelControllerAdmin();
            model.ListFieldRealTalk = action.getListFieldRealTalk(); ;
            model.ListPersionManageRealTalk = action.GetListPersionManageRealTalk();
            model.ListDepartment = action.getListDepartment();
            return View(model);
        }

        [HttpPost]
        public JsonResult DeletePersion(int idpersiondelete)
        {
            bool res = false;

            res = new ActionPost().DeletePersionInField(idpersiondelete);

            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult addNewManagePersion(PersionManageRealTalk model, int departmentCustom)
        {
            var listDepartment = new ActionPost().getListDepartment();
            listDepartment.ForEach(item =>
            {
                if (item.Id == departmentCustom)
                {
                    model.Department = item.NameDepartment;
                }
            });
            int res = 0;
            res = new ActionPost().addNewPersionInFieldRealTalk(model);

            if(res == 0)
            {
                TempData["statusAddNewPersion"] = "duplicate";
            }
            if(res == 1)
            {
                TempData["statusAddNewPersion"] = "success";
            }
            if (res == 2)
            {
                TempData["statusAddNewPersion"] = "err";
            }

            return RedirectToAction("ManagePersionEditTruth", "ManageTalkReal");
        }

        public JsonResult ApproveTruth(int id)
        {
            var dataItem = new ActionPost().DetailRealTalk(id);

            bool res = false;

            //gửi mail thông báo ý tưởng được phê duyệt đến tác giả
            EmailService service = new EmailService();
            string body = "Ý tưởng: " 
                          + dataItem.TitleRealTalk + " của bạn đã được ban quản trị ghi nhận. Đang trong quá trình xử lý ý kiến </br>" 
                          + "Thời gian phê duyệt: " + DateTime.Now + " <br/>Người phê duyệt:" + SessionHelper.GetSessionRoleAdmin().fullName + " <br/>"
                          + "Cảm ơn bạn đã đóng góp ý kiến, phản hồi đến chúng tôi.";
            bool resSendMail = service.SendMailAction(dataItem.MailSender, titleEmail, body);

            bool resSendEmailIndividual = true;

            List<PersionManageRealTalk> resPersion = new ActionPost().findPersionResponsible(dataItem.Field);
            string bodySendPersionResponsibleTruth = "Thông báo: </br> Bạn nhận được một yêu cầu xử lý ý kiến trong mục Nói Thật Đi - Truth với nội dung như sau: </br> "
                                                        + "Lĩnh vưc:" + dataItem.NameFieldRealTalk + "<br/>"
                                                        + "Lời nói thật: " + dataItem.TitleRealTalk + "<br/>"
                                                        + "Tác giả: " + dataItem.NameSender + "<br/>"
                                                        + "Email Viettel: " + dataItem.MailSender + "<br/>"
                                                        + "Chi tiết và cập nhật tiến trình xử lý Truth - Nói thật đê xử lý tại: " 
                                                        + string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("/Admin/HandleTruth/Index")) + "<br/>";
            foreach (var item in resPersion)
            {
                resSendEmailIndividual = service.SendMailAction(item.EmailManage, titleEmailToResponsible, bodySendPersionResponsibleTruth);
            }

            //check gửi mai thành công đến tác giả
            if (resSendMail && resSendEmailIndividual)
            {
                bool resApprove = new ActionPost().ApproveTruth(id);
                if (resApprove)
                {
                    res = true;
                }
            }

            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult updateField(int idPost, int idNewField)
        {
            var action = new ActionPost();
            bool resUdate = action.AdminUpdateField(idPost, idNewField);

            return new JsonResult { Data = resUdate, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult RejectTruth(int id)
        {
            var dataItem = new ActionPost().DetailRealTalk(id);

            bool res = false;

            //gửi mail thông báo ý tưởng được phê duyệt
            EmailService service = new EmailService();
            string body = "Ý kiến trong chuyên mục Nói Thật Đê: " + dataItem.TitleRealTalk + "của bạn đã bị từ chối. </br>"
                          + "Thời gian từ chối:  " + dataItem.TimeApproval + " <br/>"
                          + "Người phê duyệt: <br/>" + SessionHelper.GetSessionRoleAdmin().fullName + " <br/>"
                          + "Cảm ơn bạn đã đóng góp ý kiến, phản hồi đến chúng tôi.";
            bool resSendMail = service.SendMailAction(dataItem.MailSender, titleEmail, body);

            if (resSendMail)
            {
                bool resReject = new ActionPost().RejectTruth(id);
                if (resReject)
                {
                    res = true;
                }
            }

            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}