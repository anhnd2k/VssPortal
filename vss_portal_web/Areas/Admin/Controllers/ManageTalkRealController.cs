using DBConect;
using DBConect.Farmework;
using OfficeOpenXml;
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
        private string titleEmail = "\"Nói thật đê 01\" Thư cảm ơn";
        private string titleEmailToResponsible = "\"Nói thật đê 01\" Thông báo Xử lý công việc";

        // GET: Admin/ManageTalkReal
        public ActionResult Index(string searchString, int idFinterTruth = 0, int page = 1, int pageSize = 10)
        {
            TempData["myDataRedirect"] = CustomerRedirectLogin.CustomRedirects("Index", "Admin/ManageTalkReal");

            var action = new ActionPost();
            var res = action.GetListTalkRealAdmin(searchString, idFinterTruth, page, pageSize);
            var listFiend = action.getListFieldRealTalk();
            var listTruthStatus = action.getListTruthStatus();

            ViewData["tabBarSelection"] = "truth";
            TempData["redirectAction"] = "";
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
            bool resSendMail = true;

            EmailService service = new EmailService();

            if(dataItem.NameSender != null && dataItem.MailSender != null)
            {
                string body = "Thân gửi Quý Anh/Chị: " + dataItem.NameSender + "<br/>"
                         + "Cảm ơn Anh/Chị đã gửi lời nói thật tới hòm thư \"Nói thật đê\" của Trung tâm VSS." + "<br/>"
                         + "Đóng góp của bạn đã gửi thành công. Phía Phòng ban phụ trách đang tiếp nhận & xử lý. Nội dung đóng góp của Anh/Chị đã được đăng lên kênh thông tin \"Nói thật đê\" tại link:  "
                         + string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("/RealTalk/NewFeedTruth"))
                         + "  .Anh/Chị có thể theo dõi nhé ạ" + "<br/>"
                         + "BTC cảm ơn bạn vì sự tận tụy của mình, VSS tự hào vì có sự đóng góp của bạn. " + "<br/>"
                         + "Xin trân trọng cảm ơn những nỗ lực của bạn.";
                resSendMail = service.SendMailAction(dataItem.MailSender, titleEmail, body);
            }

            bool resSendEmailIndividual = true;

            List<PersionManageRealTalk> resPersion = new ActionPost().findPersionResponsible(dataItem.Field);
            //gửi mail thông báo ý tưởng được phê duyệt đến tác giả
            foreach (var item in resPersion)
            {
                string bodySendPersionResponsibleTruth = "Thân gửi Quý Anh/Chị: " + item.FullNameManage
                                                        + "Hệ thống \"Nói thật đê\" của Trung tâm VSS nhận được một yêu cầu về: " + "<br/>"
                                                        + "Tiêu đề ý kiến: " + dataItem.TitleRealTalk + "<br/>"
                                                        + "Thực trạng: " + dataItem.Reality + "<br/>"
                                                        + "Đề xuất: " + dataItem.Suggestion + "<br/>"
                                                        + "Kính mong đồng chí và phòng ban " + item.Department + " hỗ trợ giải pháp để đảm bảo được tiến độ công việc của cán bộ nhân viên. Sau khi xử lý, Đồng chí vui lòng cập nhật trạng thái tại:  "
                                                        + string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("/Admin/HandleTruth/Index")) + "<br/>"
                                                        + "Xin trân trọng cảm ơn những nỗ lực của bạn.";
                resSendEmailIndividual = service.SendMailActionNoImg(item.EmailManage, titleEmailToResponsible, bodySendPersionResponsibleTruth);
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
            bool resSendMail = true;

            //gửi mail thông báo ý tưởng được phê duyệt
            if(dataItem.NameSender != null && dataItem.MailSender != null)
            {
                EmailService service = new EmailService();
                string body = "Thân gửi Quý Anh/Chị: " + dataItem.NameSender + "<br/>"
                             + "Cảm ơn Anh/Chị đã gửi lời nói thật tới hòm thư \"Nói thật đê\" của Trung tâm VSS." + "<br/>"
                             + "Đóng góp của bạn đã được tiếp nhận bởi phía Phòng ban phụ trách và đang được xử lý. Nội dung đóng góp của Anh/Chị đã được đăng lên kênh thông tin \"Nói thật đê\" tại link:  "
                             + string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("/RealTalk/NewFeedTruth"))
                             + "  .Anh/Chị có thể theo dõi nhé ạ" + "<br/>"
                             + "BTC cảm ơn bạn vì sự tận tụy của mình, VSS tự hào vì có sự đóng góp của bạn. " + "<br/>"
                             + "Xin trân trọng cảm ơn những nỗ lực của bạn.";
                resSendMail = service.SendMailAction(dataItem.MailSender, titleEmail, body);
            }

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

        public JsonResult setStatusFlag(int id)
        {
            bool res = new ActionPost().setStatusFlag(id);
            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult changeFieldName(string name, int idField)
        {
            bool res = new ActionPost().changeNameField(idField, name);
            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //add new filed
        public JsonResult addNewField(string nameField)
        {
            bool res = new ActionPost().addNewField(nameField);
            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //xuất excel truth

        public void ExportExcelTruth()
        {
            var action = new ActionPost();
            var ListTruth = action.ListTruth();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "STT";
            Sheet.Cells["B1"].Value = "Ngày tạo";
            Sheet.Cells["C1"].Value = "Người tạo";
            Sheet.Cells["D1"].Value = "Email";
            Sheet.Cells["E1"].Value = "Đơn vị";
            Sheet.Cells["F1"].Value = "Tên Truth";
            Sheet.Cells["G1"].Value = "Chủ đề";
            Sheet.Cells["H1"].Value = "Hiện trạng";
            Sheet.Cells["I1"].Value = "Đề xuất";
            Sheet.Cells["J1"].Value = "Ngày duyệt";
            Sheet.Cells["K1"].Value = "Trạng thái";
            Sheet.Cells["L1"].Value = "Ngày cập nhật trạng thái cuối";
            Sheet.Cells["M1"].Value = "Người xử lý";
            Sheet.Cells["N1"].Value = "Email";
            Sheet.Cells["O1"].Value = "Đơn vị";
            Sheet.Cells["P1"].Value = "Phản hồi cuối cùng";
            Sheet.Cells["Q1"].Value = "Đã lên bảng tin";
            Sheet.Cells["R1"].Value = "Số lượng comment";
            Sheet.Cells["S1"].Value = "Số lượng like";

            int row = 2;
            foreach (var data in ListTruth.Select((value, i) => new { value, i }))
            {
                var item = data.value;
                CommentDetailTruth infoPersionHandletruth = new CommentDetailTruth();
                if (item.TruthStatus != 1 && item.TruthStatus != 2)
                {
                    infoPersionHandletruth = action.getInfoTruth(item.id);
                }
                var findPersionHandelToField = action.findPersionHandelToField(item.Field);
                var mailHandelToFiend = action.mailHandelToFiend(item.Field);
                var departmentPersionHandel = string.Empty;
                if (infoPersionHandletruth.EmailManagerCmt != null)
                {
                    departmentPersionHandel = action.findDepartment(infoPersionHandletruth.EmailManagerCmt);
                }

                Sheet.Cells[string.Format("A{0}", row)].Value = Math.Abs(data.i + 1);
                Sheet.Cells[string.Format("B{0}", row)].Value = item.TimeSend?.ToString("MM/dd/yyyy hh:mm:ss tt");
                Sheet.Cells[string.Format("C{0}", row)].Value = item.NameSender != null ? item.NameSender : "Ẩn danh";
                Sheet.Cells[string.Format("D{0}", row)].Value = item.MailSender != null ? item.MailSender : "Ẩn danh";
                Sheet.Cells[string.Format("E{0}", row)].Value = item.DepartmentSender != null ? item.MailSender : "Không có dữ liệu";
                Sheet.Cells[string.Format("F{0}", row)].Value = item.TitleRealTalk;
                Sheet.Cells[string.Format("G{0}", row)].Value = item.NameFieldRealTalk;
                Sheet.Cells[string.Format("H{0}", row)].Value = item.Reality;
                Sheet.Cells[string.Format("I{0}", row)].Value = item.Suggestion;
                Sheet.Cells[string.Format("J{0}", row)].Value = item.TimeApproval?.ToString("MM/dd/yyyy hh:mm:ss tt");
                Sheet.Cells[string.Format("K{0}", row)].Value = item.TruthStatusName;
                Sheet.Cells[string.Format("L{0}", row)].Value = item.TimeUpdateStatus.ToString("MM/dd/yyyy hh:mm:ss tt");
                Sheet.Cells[string.Format("M{0}", row)].Value = item.TruthStatus == 1 ? "Truth đang chờ phê duyệt" : (item.TruthStatus == 2 ? findPersionHandelToField : infoPersionHandletruth?.NameManagerCmt);
                Sheet.Cells[string.Format("N{0}", row)].Value = item.TruthStatus == 1 ? "Truth đang chờ phê duyệt" : (item.TruthStatus == 2 ? mailHandelToFiend : infoPersionHandletruth?.EmailManagerCmt);
                Sheet.Cells[string.Format("O{0}", row)].Value = infoPersionHandletruth != null ? departmentPersionHandel : "";
                Sheet.Cells[string.Format("P{0}", row)].Value = item.TruthStatus == 1 ? "Truth đang chờ phê duyệt" : (item.TruthStatus == 2 ? "Đang chờ người quản lý phê duyệt" : infoPersionHandletruth?.CommentTruth); ;
                Sheet.Cells[string.Format("Q{0}", row)].Value = item.ShowInNewFeed ? "Đã hiển thị" : "Đã ẩn";
                Sheet.Cells[string.Format("R{0}", row)].Value = item.UserNameComment != null ? item.UserNameComment.Trim().Split(',').Length : 0;
                Sheet.Cells[string.Format("S{0}", row)].Value = item.UserNameLike != null ? item.UserNameLike.Trim().Split(',').Length : 0;
                row++;
            }


            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "IdeaRegester.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();

        }

    }
}