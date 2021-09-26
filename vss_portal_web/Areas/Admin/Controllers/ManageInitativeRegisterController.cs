using DBConect;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vss_portal_web.Areas.Admin.Code;
using vss_portal_web.Controllers;
using vss_portal_web.Models;

namespace vss_portal_web.Areas.Admin.Controllers
{
    public class ManageInitativeRegisterController : BaseAdminController
    {
        private string titleEmail = "Thông báo về việc xét duyệt sáng kiến";
        // GET: Admin/ManageInitativeRegister
        public ActionResult Index(string searchString, int idStatus = 0, int page = 1, int pageSize = 10)
        {
            TempData["myDataRedirect"] = CustomerRedirectLogin.CustomRedirects("Index", "Admin/ManageInitativeRegister");

            var action = new ActionPost();
            var listInitativeRegester = action.GetListInitativeRegester(searchString, idStatus, page, pageSize);
            var status = action.getStatusIdea();
            ViewData["tabBarSelection"] = "manageInitative";
            ViewData["StatusIdeaInitative"] = status;
            ViewData["IdStatusFinter"] = idStatus;
            ViewData["searchString"] = searchString;

            return View(listInitativeRegester);
        }

        [HttpPost]
        public JsonResult ApproveInitativeV2(int id)
        {
            var dataItem = new ActionPost().DetailInitativeRegester(id);

            bool res = false;

            //gửi mail thông báo sáng kiến được phê duyệt
            EmailService service = new EmailService();
            string body = "Sáng kiến: "
                           + dataItem.NameInitative
                           + "của bạn đã được phê duyệt. </br>"
                           + "Tên sáng kiến: " + dataItem.NameInitative
                           + "Thời gian gửi sáng kiến :" + dataItem.TimeSendInitative
                           + "Thời gian phê duyệt:  " + DateTime.Now + " <br/>"
                           +" Người phê duyệt: <br/>" + SessionHelper.GetSessionRoleAdmin().fullName + " <br/>";
            bool resSendMail = service.SendMailAction(dataItem.EmailUserSend, titleEmail, body);

            bool resSendEmailIndividual = true;

            // gửi mail đến từng cá nhân tham gia đóng góp ý kiến
            if (dataItem.MailOfEach != null)
            {
                Array mail = dataItem.MailOfEach.Trim().Split(',');
                foreach (string mailPersion in mail)
                {
                    resSendEmailIndividual = service.SendMailAction(mailPersion, titleEmail, body);
                }
            }

            if (resSendMail)
            {
                bool resApprove = new ActionPost().ApproveInitative(id);
                if (resApprove)
                {
                    res = true;
                }
            }

            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult RejectInitativeV2(int id)
        {
            var dataItem = new ActionPost().DetailInitativeRegester(id);

            bool res = false;

            //gửi mail thông báo sáng kiến được phê duyệt
            EmailService service = new EmailService();
            string body = "Sáng kiến: "
                          + dataItem.NameInitative
                          + "của bạn đã bị từ chối. </br>"
                          + "Tên sáng kiến :" + dataItem.NameInitative
                          + "Thời gian gửi sáng kiến: " + dataItem.TimeSendInitative
                          + " Thời gian từ chối:  " + DateTime.Now + " <br/>"
                          + " Người phê duyệt: <br/>" + SessionHelper.GetSessionRoleAdmin().fullName + " <br/>";
            bool resSendMail = service.SendMailAction(dataItem.EmailUserSend, titleEmail, body);

            if (resSendMail)
            {
                bool resReject = new ActionPost().RejectInitative(id);
                if (resReject)
                {
                    res = true;
                }
            }

            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult DetailInitative (int id)
        {
            var detailInitativeRegester = new ActionPost().DetailInitativeRegester(id);
            return View(detailInitativeRegester);
        }

        public FileResult Download(string imgName)
        {

            if (imgName != "")
            {
                var filePath = "~/UploadFiles/FileInitative/" + imgName;
                return File(filePath, "application/force- download", Path.GetFileName(filePath));
            }
            return null;
        }

        public void ExportExcelInitative()
        {
            var action = new ActionPost();
            var listInitativeRegester = action.GetAllListInitativeRegester();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "Tên sáng kiến";
            Sheet.Cells["B1"].Value = "Tác giả";
            Sheet.Cells["C1"].Value = "Email Viettel";
            Sheet.Cells["D1"].Value = "Đơn vị áp dụng";
            Sheet.Cells["E1"].Value = "Lĩnh vực";
            Sheet.Cells["F1"].Value = "Tình trạng hiện tại";
            Sheet.Cells["G1"].Value = "Điểm mới sáng kiến";
            Sheet.Cells["H1"].Value = "Hiệu quả khi áp dụng";
            Sheet.Cells["I1"].Value = "Nội dung sáng kiến";
            Sheet.Cells["J1"].Value = "Nội dung bổ sung";
            Sheet.Cells["K1"].Value = "Ngày nộp đơn";

            int row = 2;
            foreach (var item in listInitativeRegester)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.NameInitative;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.UserSend;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.EmailUserSend;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.NameUnitApply;
                Sheet.Cells[string.Format("E{0}", row)].Value = item.NameField;
                Sheet.Cells[string.Format("F{0}", row)].Value = item.Applicability;
                Sheet.Cells[string.Format("G{0}", row)].Value = item.NewPoint;
                Sheet.Cells[string.Format("H{0}", row)].Value = item.EffectiveApply;
                Sheet.Cells[string.Format("I{0}", row)].Value = item.ContentInitative;
                Sheet.Cells[string.Format("J{0}", row)].Value = item.DetailMore;
                Sheet.Cells[string.Format("K{0}", row)].Value = item.TimeSendInitative?.ToString("MM/dd/yyyy hh:mm:ss tt");
                row++;
            }


            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "InitativeRegester.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();

        }
    }
}