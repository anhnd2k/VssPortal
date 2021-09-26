using DBConect;
using DBConect.model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using vss_portal_web.Areas.Admin.Code;
using vss_portal_web.Controllers;
using vss_portal_web.Models;

namespace vss_portal_web.Areas.Admin.Controllers
{
    public class ManageIdeaRegisterController : BaseAdminController
    {
        private string titleEmail = "Thông báo về việc xét duyệt ý tưởng";
        // GET: Admin/ManageIdeaRegister
        public ActionResult Index(string searchString, int idStatus = 0, int page = 1, int pageSize = 10)
        {
            TempData["myDataRedirect"] = CustomerRedirectLogin.CustomRedirects("Index", "Admin/ManageIdeaRegister");

            var action = new ActionPost();
            var listIdeaRegester = action.GetListIdeaRegester(searchString, idStatus, page, pageSize);
            var status = action.getStatusIdea();

            ViewData["tabBarSelection"] = "manageIdea";
            ViewData["StatusIdeaInitative"] = status;
            ViewData["IdStatusFinter"] = idStatus;
            ViewData["searchString"] = searchString;
            return View(listIdeaRegester);
        }

        [HttpPost]
        public JsonResult ApproveV2(int id)
        {
            var dataItem = new ActionPost().DetailIdeaRegester(id);

            bool res = false;

            //gửi mail thông báo ý tưởng được phê duyệt
            EmailService service = new EmailService();
            string body = "Ý tưởng: " 
                          + dataItem.NameIdea + "của bạn đã được phê duyệt. </br>"
                          + "Tên ý tưởng: " + dataItem.NameIdea
                          + "Thời gian gửi ý tưởng: " + dataItem.DateSend
                          + "Thời gian phê duyệt:  " + DateTime.Now + "<br/>"
                          + "Cảm ơn bạn về sáng kiến tuyệt vời."
                          + "Người phê duyệt: <br/>" + SessionHelper.GetSessionRoleAdmin().fullName + " <br/>";
            bool resSendMail = service.SendMailAction(dataItem.AuthorEmail, titleEmail, body);

            bool resSendEmailIndividual = true;

            // gửi mail đến từng cá nhân tham gia đóng góp ý kiến
            if(dataItem.EmailIndividual != null)
            {
                Array mail = dataItem.EmailIndividual.Trim().Split(',');
                foreach (string mailPersion in mail)
                {
                    resSendEmailIndividual = service.SendMailAction(mailPersion, titleEmail, body);
                }
            }

            //check gửi mai thành công đến tác giả
            if (resSendMail && resSendEmailIndividual)
            {
                bool resApprove = new ActionPost().ApproveIdea(id);
                if (resApprove)
                {
                    res = true;
                }
            }

            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult RejectIdeaV2(int id)
        {
            var dataItem = new ActionPost().DetailIdeaRegester(id);

            bool res = false;

            //gửi mail thông báo ý tưởng được phê duyệt
            EmailService service = new EmailService();
            string body = "Ý tưởng: " 
                          + dataItem.NameIdea 
                          + "của bạn đã bị từ chối. </br>"
                          + "Tên ý tưởng: " + dataItem.NameIdea
                          + "Thời gian gửi ý tưởng: " + dataItem.DateSend
                          + "Thời gian phê duyệt:  " + DateTime.Now + "<br/>"
                          + "Cảm ơn bạn đã đóng góp ý tưởng."
                          + "Người phê duyệt: <br/>" + SessionHelper.GetSessionRoleAdmin().fullName + " <br/>";
            bool resSendMail = service.SendMailAction(dataItem.AuthorEmail, titleEmail, body);
            bool resSendEmailIndividual = true;

            // gửi mail đến từng cá nhân tham gia đóng góp ý kiến
            if (dataItem.EmailIndividual != null)
            {
                Array mail = dataItem.EmailIndividual.Trim().Split(',');
                foreach (string mailPersion in mail)
                {
                    resSendEmailIndividual = service.SendMailAction(mailPersion, titleEmail, body);
                }
            }
            if (resSendMail && resSendEmailIndividual)
            {
                bool resReject = new ActionPost().RejectIdea(id);
                if (resReject)
                {
                    res = true;
                }
            }

            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public void ExportExcel()
        {
            var action = new ActionPost();
            var listIdeaRegester = action.GetAllListIdeaRegester();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "Tên ý tưởng";
            Sheet.Cells["B1"].Value = "Tác giả";
            Sheet.Cells["C1"].Value = "Email Viettel";
            Sheet.Cells["D1"].Value = "Tình trạng trước khi áp dụng ý tưởng";
            Sheet.Cells["E1"].Value = "Lĩnh vực";
            Sheet.Cells["F1"].Value = "Phạm vi";
            Sheet.Cells["G1"].Value = "Hiệu quả";
            Sheet.Cells["H1"].Value = "Nội dung";
            Sheet.Cells["I1"].Value = "Tài liệu kèm theo";
            Sheet.Cells["J1"].Value = "Ngày nộp đơn";

            int row = 2;
            foreach (var item in listIdeaRegester)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.NameIdea;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.AuthorFullName;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.AuthorEmail;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.StatusIdeaBefore;
                Sheet.Cells[string.Format("E{0}", row)].Value = item.NameField;
                Sheet.Cells[string.Format("F{0}", row)].Value = item.LimitApply;
                Sheet.Cells[string.Format("G{0}", row)].Value = item.Effective;
                Sheet.Cells[string.Format("H{0}", row)].Value = item.contenIdea;
                Sheet.Cells[string.Format("I{0}", row)].Value = item.DocumentIdea;
                Sheet.Cells[string.Format("J{0}", row)].Value = item.DateSend?.ToString("MM/dd/yyyy hh:mm:ss tt");
                row++;
            }


            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "IdeaRegester.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();

        }
        [ValidateInput(false)]
        public FileResult Export(string GridHtml)
        {
            return File(Encoding.ASCII.GetBytes(GridHtml), "application/vnd.ms-excel", "Grid.xls");
        }

    }
}