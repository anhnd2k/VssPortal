using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;

namespace vss_portal_web.Models
{
    public class EmailService
    {
        public bool Send(string toEmail, string subject, string body, string path)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(toEmail);
                mail.From = new MailAddress("itbu.system@viettelimex.vn");
                mail.Subject = subject;

                AlternateView imgview = AlternateView.CreateAlternateViewFromString(body + "<br/><img src=cid:imgPath width=500>",null, MediaTypeNames.Text.Html);
                LinkedResource lr = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                lr.ContentId = "imgPath";
                imgview.LinkedResources.Add(lr);
                mail.AlternateViews.Add(imgview);

                mail.Body = lr.ContentId;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                //smtp.gmail.com
                smtp.Host = "smtp.office365.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("itbu.system@viettelimex.vn", "Thongbao.VTLM@T10");
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}