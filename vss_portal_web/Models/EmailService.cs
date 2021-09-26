using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace vss_portal_web.Models
{
    public class EmailService
    {

        //private string EMAIL = "hotro@viettelsoftware.net";
        //private string PWD = "V2dhVFsjPbqF5VvOW0lL";
        private string EMAIL = "iutube99445@gmail.com";
        private string PWD = "ngonhat94";

        public bool SendMailAction(string toEmail, string subject, string body)
        {
            try
            {
                //Tệp đính kèm
                //var attachment = new Attachment("C:/Users/Admin/Desktop/uwuu/fox_illustration_background_tongue_66520_1280x720.jpg");
                //mail.Attachments.Add(attachment);
                MailMessage mail = new MailMessage();
                mail.To.Add(toEmail);
                mail.From = new MailAddress(EMAIL);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                //smtp.Host = "smtp.office365.com";
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(EMAIL, PWD);
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SendRealTalk(string toEmail, string subject, string body, List<PathImgConten> pathConten)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(toEmail);
                mail.From = new MailAddress(EMAIL);
                mail.Subject = subject;
                mail.Body = body;

                AlternateView imgview = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

                foreach (var item in pathConten)
                {
                    LinkedResource pathContenImg = new LinkedResource(item.pathImg, MediaTypeNames.Image.Jpeg);
                    pathContenImg.ContentId = item.nameId;
                    imgview.LinkedResources.Add(pathContenImg);
                }

                mail.AlternateViews.Add(imgview);

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.office365.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(EMAIL, PWD);
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Send(string toEmail, string subject, string body, string path, List<PathImgConten> pathConten)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(toEmail);
                mail.From = new MailAddress(EMAIL);
                mail.Subject = subject;
                mail.Body = body;

                AlternateView imgview = AlternateView.CreateAlternateViewFromString(body + "<br/><img src=cid:imgPath width=500>", null, MediaTypeNames.Text.Html);
                LinkedResource lr = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                lr.ContentId = "imgPath";
                imgview.LinkedResources.Add(lr);

                foreach(var item in pathConten)
                {
                    LinkedResource pathContenImg = new LinkedResource(item.pathImg, MediaTypeNames.Image.Jpeg);
                    pathContenImg.ContentId = item.nameId;
                    imgview.LinkedResources.Add(pathContenImg);
                }
          

                mail.AlternateViews.Add(imgview);

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                //smtp.Host = "smtp.office365.com";
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(EMAIL, PWD);
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