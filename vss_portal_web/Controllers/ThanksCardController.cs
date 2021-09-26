using DBConect;
using DBConect.Farmework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using vss_portal_web.Areas.Admin.Code;
using vss_portal_web.Models;

namespace vss_portal_web.Controllers
{
    public class ThanksCardController : Controller
    {
        // GET: ThanksCard
        public ActionResult Index()
        {
            ViewData["fullNameUser"] = CheckLoginRole.getUserName();
            ViewData["checkRoleLogin"] = CheckLoginRole.check();
            ViewData["departmentUser"] = SessionHelper.GetSession()?.Department;
            var action = new ActionPost();
            var listImgThankCard = action.GetListImgThankCard();
            var ListDepartment = action.getListDepartment();

            ModelController controllerModel = new ModelController();
            controllerModel.ThankCardImg = listImgThankCard;
            controllerModel.ListDepartments = ListDepartment;

            ViewData["headerActive"] = "active";

            return View(controllerModel);
        }

        [HttpPost]
        public JsonResult autocompeleteMail(string value)
        {
            var action = new ActionPost();
            var resEmployee = action.autocompleteEmail(value);

            return new JsonResult { Data = resEmployee, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult findName(string e)
        {
            var action = new ActionPost();
            var resNameByMail = action.findNameByEmail(e);

            return new JsonResult { Data = resNameByMail, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(ThankCardUser model)
        {
            ViewData["fullNameUser"] = CheckLoginRole.getUserName();
            ViewData["checkRoleLogin"] = CheckLoginRole.check();
            var action = new ActionPost();
            var listImgThankCard = action.GetListImgThankCard();
            var ListDepartment = action.getListDepartment();

            ModelController controllerModel = new ModelController();
            controllerModel.ThankCardImg = listImgThankCard;
            controllerModel.ListDepartments = ListDepartment;

            string path = Server.MapPath(@"ImageUpload/ThankCard/" + model.CardImg);

            string titleEmail = "Thank Card: " + model.TitleCard;

            string contenTextArea = model.ContenCard;

            List<PathImgConten> pathImgConten = new List<PathImgConten>();
            string pattern = @"<(img)\b[^>]*>";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(contenTextArea);
            string bodySuccess = contenTextArea;
            for (int i = 0, l = matches.Count; i < l; i++)
            {
                var ahihi = Regex.Match(matches[i].Value, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                var resImg = ahihi.Replace("/ImageUpload/images/", "");
                var arrNameId = resImg.Split('.');
                var resfull = arrNameId[0];
                string pathImgItem = Server.MapPath(@ahihi);
                bodySuccess = bodySuccess.Replace(ahihi, "cid:"+resfull);
                PathImgConten pathSuccess = new PathImgConten();
                pathSuccess.pathImg = pathImgItem;
                pathSuccess.nameId = resfull;
                pathImgConten.Add(pathSuccess);
            }

            //gửi 1 người


            if (model.sendOnly == true)
            {
                if (ModelState.IsValid && model.Incognito == true)
                {
                    model.Sender = null;
                    model.MailSender = null;

                    EmailService service = new EmailService();
                    string body = "Bạn nhận được một Thank Card với thông tin như sau:" + "<br/> From: Người bí ẩn " + "<br/> To: " + model.Receiver + "<br/> Tiêu đề: "+ model.TitleCard + " <br/>Nội dung: <br/>" + bodySuccess + " <br/>";
                    bool res = service.Send(model.MailReceiver, titleEmail, body, path, pathImgConten);

                    ViewData["oke"] = res ? "ok" : "false";
                    if(res == false)
                    {
                        return View(controllerModel);
                    }
                    ViewBag.Message = "ok";
                    new ActionPost().SendThankCard(model.Incognito, model.Sender, model.MailSender, model.Receiver, model.MailReceiver, model.Department, titleEmail, model.CardImg, model.ContenCard);
                    return View(controllerModel);
                }
                else if (ModelState.IsValid && model.Incognito == false)
                {
                    EmailService service = new EmailService();
                    string body = "Bạn nhận được một Thank Card với thông tin như sau:" + "<br/> From: <b> " + model.Sender + "</b> <br/> Email: " + model.MailSender + "<br/> To: " + model.Receiver + "<br/> Tiêu đề: " + model.TitleCard + " <br/>Nội dung: <br/>" + bodySuccess + " <br/>";
                    bool res = service.Send(model.MailReceiver, titleEmail, body, path, pathImgConten);

                    ViewData["oke"] = res ? "ok" : "false";
                    if (res == false)
                    {
                        return View(controllerModel);
                    }
                    ViewBag.Message = "ok";
                    new ActionPost().SendThankCard(model.Incognito, model.Sender, model.MailSender, model.Receiver, model.MailReceiver, model.Department, titleEmail, model.CardImg, model.ContenCard);
                    return View(controllerModel);
                }
                else
                {
                    ViewData["errSendCard"] = "Oop, có lỗi xảy ra, vui lòng thử lại!";
                }
            }

            //gửi nhiều người
            if(model.sendMorePersion == true)
            {
                EmailService service = new EmailService();
                Array mail = model.MailSendMore.Trim().Split(',');

                    if (ModelState.IsValid && model.Incognito == true)
                    {
                        model.Sender = null;
                        model.MailSender = null;
                        foreach(string mailPersion in mail)
                        {
                            var employee = action.findEmployee(mailPersion.Trim());
                            if (employee != null)
                            {
                                model.Receiver = employee.Name;
                                foreach (var i in ListDepartment)
                                {
                                    if (employee.Department == i.NameDepartment)
                                    {
                                        model.Department = i.Id;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                model.Department = 11;
                                model.Receiver = null;
                            }
                            string body = "Bạn nhận được một Thank Card với thông tin như sau:" + "<br/> From: Người bí ẩn " + "<br/> To: " + model.Receiver + "<br/> Tiêu đề: " + model.TitleCard + " <br/>Nội dung: <br/>" + bodySuccess + " <br/>";
                            bool res = service.Send(mailPersion, titleEmail, body, path, pathImgConten);
                            ViewData["oke"] = res ? "ok" : "false";
                            if (res == false)
                            {
                                return View(controllerModel);
                            }
                        ViewBag.Message = "ok";
                            action.SendThankCard(model.Incognito, model.Sender, model.MailSender, model.Receiver, mailPersion, model.Department, model.TitleCard, model.CardImg, model.ContenCard);
                        }
                        
                        return View(controllerModel);
                    }
                    else if (ModelState.IsValid && model.Incognito == false)
                    {
                        foreach (string mailPersion in mail)
                        {
                            var employee = action.findEmployee(mailPersion.Trim());
                            if(employee != null)
                            {
                                model.Receiver = employee.Name;
                                foreach (var i in ListDepartment)
                                {
                                    if (employee.Department == i.NameDepartment)
                                    {
                                        model.Department = i.Id;
                                        break;
                                    }
                                }
                        }
                            else
                            {
                                model.Department = 11;
                                model.Receiver = null;
                            }
                        string body = "Bạn nhận được một Thank Card với thông tin như sau:" + "<br/> From: <b> " + model.Sender + "</b> <br/> Email: " + model.MailSender + "<br/> To: " + model.Receiver + "<br/> Tiêu đề: " + model.TitleCard + " <br/>Nội dung: <br/>" + bodySuccess + " <br/>";
                        bool res = service.Send(mailPersion, titleEmail, body, path, pathImgConten);
                        ViewData["oke"] = res ? "ok" : "false";
                        if (res == false)
                        {
                            return View(controllerModel);
                        }
                        ViewBag.Message = "ok";
                        action.SendThankCard(model.Incognito, model.Sender, model.MailSender, model.Receiver, mailPersion, model.Department, model.TitleCard, model.CardImg, model.ContenCard);
                        }
                        return View(controllerModel);
                    }

                return View(controllerModel);
            }

            //gửi theo danh sách phòng
            if(model.sendDepartment == true)
            {
                var TextDepartment = "";
                for (var i = 0; i < ListDepartment.Count; i++)
                {
                    if (ListDepartment[i].Id == model.Department)
                    {
                        TextDepartment = ListDepartment[i].NameDepartment;
                        break;
                    }
                }
                var listSend = action.findEmployeetoDepartment(TextDepartment);
                foreach(var userReceiver in listSend)
                {
                    if (ModelState.IsValid && model.Incognito == true)
                    {
                        model.Sender = null;
                        model.MailSender = null;

                        EmailService service = new EmailService();
                        string body = "Bạn nhận được một Thank Card với thông tin như sau:" + "<br/> From: Người bí ẩn " + "<br/> To: " + userReceiver.Name + "<br/> Tiêu đề: " + model.TitleCard + " <br/>Nội dung: <br/>" + bodySuccess + " <br/>";
                        bool res = service.Send(userReceiver.Email, titleEmail, body, path, pathImgConten);
                        ViewData["oke"] = res ? "ok" : "false";
                        if (res == false)
                        {
                            return View(controllerModel);
                        }
                        ViewBag.Message = "ok";
                        new ActionPost().SendThankCard(model.Incognito, model.Sender, model.MailSender, userReceiver.Name, userReceiver.Email, model.Department, model.TitleCard, model.CardImg, model.ContenCard);
                    }
                    else if (ModelState.IsValid && model.Incognito == false)
                    {
                        EmailService service = new EmailService();
                        string body = "Bạn nhận được một Thank Card với thông tin như sau:" + "<br/> From: <b> " + model.Sender + "</b> <br/> Email: " + model.MailSender + "<br/> To: " + userReceiver.Name + "<br/> Tiêu đề: " + model.TitleCard + " <br/>Nội dung: <br/>" + bodySuccess + " <br/>";
                        bool res = service.Send(userReceiver.Email, titleEmail, body, path, pathImgConten);
                        ViewData["oke"] = res ? "ok" : "false";
                        if (res == false)
                        {
                            return View(controllerModel);
                        }
                        ViewBag.Message = "ok";
                        new ActionPost().SendThankCard(model.Incognito, model.Sender, model.MailSender, userReceiver.Name, userReceiver.Email, model.Department, model.TitleCard, model.CardImg, model.ContenCard);
                    }
                    else
                    {
                        ViewData["errSendCard"] = "Oop, có lỗi xảy ra, vui lòng thử lại!";
                    }
                }
                return View(controllerModel);
            }


            return View(controllerModel);
        }
    }
}