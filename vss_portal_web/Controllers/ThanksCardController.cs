using DBConect;
using DBConect.Farmework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using vss_portal_web.Models;

namespace vss_portal_web.Controllers
{
    public class ThanksCardController : Controller
    {
        // GET: ThanksCard
        public ActionResult Index()
        {
            var action = new ActionPost();
            var listImgThankCard = action.GetListImgThankCard();
            var ListDepartment = action.getListDepartment();

            ModelController controllerModel = new ModelController();
            controllerModel.ThankCardImg = listImgThankCard;
            controllerModel.ListDepartments = ListDepartment;

            return View(controllerModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(ThankCardUser model)
        {
            var action = new ActionPost();
            var listImgThankCard = action.GetListImgThankCard();
            var ListDepartment = action.getListDepartment();

            ModelController controllerModel = new ModelController();
            controllerModel.ThankCardImg = listImgThankCard;
            controllerModel.ListDepartments = ListDepartment;

            string path = Server.MapPath(@"ImageUpload/ThankCard/" + model.CardImg);

            string titleEmail = "Thank Card: " + model.TitleCard;

            //gửi 1 người

            if(model.sendOnly == true)
            {
                for (var i = 0; i < ListDepartment.Count; i++)
                {
                    if (ListDepartment[i].NameDepartment == model.TextDepartment)
                    {
                        model.Department = ListDepartment[i].Id;
                        break;
                    }
                }

                if (ModelState.IsValid && model.Incognito == true)
                {
                    model.Sender = null;
                    model.MailSender = null;

                    EmailService service = new EmailService();
                    string body = "Bạn nhận được một Thank Card với thông tin như sau:" + "<br/> From: Người bí ẩn " + "<br/> To: " + model.Receiver + "<br/> Tiêu đề: "+ model.TitleCard + " <br/>Nội dung: <br/>" + model.ContenCard + " <br/>";
                    bool res = service.Send(model.MailReceiver, titleEmail, body, path);


                    ViewData["oke"] = res ? "ok" : null;
                    ViewBag.Message = "ok";
                    new ActionPost().SendThankCard(model.Incognito, model.Sender, model.MailSender, model.Receiver, model.MailReceiver, model.Department, titleEmail, model.CardImg, model.ContenCard);
                    return View(controllerModel);
                }
                else if (ModelState.IsValid && model.Incognito == false)
                {
                    EmailService service = new EmailService();
                    string body = "Bạn nhận được một Thank Card với thông tin như sau:" + "<br/> From: <b> " + model.Sender + "</b> <br/> Email: " + model.MailSender + "<br/> To: " + model.Receiver + "<br/> Tiêu đề: " + model.TitleCard + " <br/>Nội dung: <br/>" + model.ContenCard + " <br/>";
                    bool res = service.Send(model.MailReceiver, titleEmail, body, path);

                    ViewData["oke"] = res ? "ok" : null;
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
                                for (var i = 0; i < ListDepartment.Count; i++){
                                    if (ListDepartment[i].NameDepartment == employee.Department)
                                    {
                                        model.Department = ListDepartment[i].Id;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                model.Department = 11;
                                model.Receiver = null;
                            }
                            string body = "Bạn nhận được một Thank Card với thông tin như sau:" + "<br/> From: Người bí ẩn " + "<br/> To: " + model.Receiver + "<br/> Tiêu đề: " + model.TitleCard + " <br/>Nội dung: <br/>" + model.ContenCard + " <br/>";
                            bool res = service.Send(mailPersion, titleEmail, body, path);
                            ViewData["oke"] = res ? "ok" : null;
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
                                for (var i = 0; i < ListDepartment.Count; i++)
                                {
                                    if (ListDepartment[i].NameDepartment == employee.Department)
                                    {
                                        model.Department = ListDepartment[i].Id;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                model.Department = 11;
                                model.Receiver = null;
                            }
                        string body = "Bạn nhận được một Thank Card với thông tin như sau:" + "<br/> From: <b> " + model.Sender + "</b> <br/> Email: " + model.MailSender + "<br/> To: " + model.Receiver + "<br/> Tiêu đề: " + model.TitleCard + " <br/>Nội dung: <br/>" + model.ContenCard + " <br/>";
                        bool res = service.Send(mailPersion, titleEmail, body, path);
                        ViewData["oke"] = res ? "ok" : null;
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
                for (var i = 0; i < ListDepartment.Count; i++)
                {
                    if (ListDepartment[i].NameDepartment == model.TextDepartment)
                    {
                        model.Department = ListDepartment[i].Id;
                        break;
                    }
                }
                var listSend = action.findEmployeetoDepartment(model.TextDepartment);
                foreach(var userReceiver in listSend)
                {
                    if (ModelState.IsValid && model.Incognito == true)
                    {
                        model.Sender = null;
                        model.MailSender = null;

                        EmailService service = new EmailService();
                        string body = "Bạn nhận được một Thank Card với thông tin như sau:" + "<br/> From: Người bí ẩn " + "<br/> To: " + userReceiver.Name + "<br/> Tiêu đề: " + model.TitleCard + " <br/>Nội dung: <br/>" + model.ContenCard + " <br/>";
                        bool res = service.Send(userReceiver.Email, titleEmail, body, path);


                        ViewData["oke"] = res ? "ok" : null;
                        ViewBag.Message = "ok";
                        new ActionPost().SendThankCard(model.Incognito, model.Sender, model.MailSender, userReceiver.Name, userReceiver.Email, model.Department, model.TitleCard, model.CardImg, model.ContenCard);
                    }
                    else if (ModelState.IsValid && model.Incognito == false)
                    {
                        EmailService service = new EmailService();
                        string body = "Bạn nhận được một Thank Card với thông tin như sau:" + "<br/> From: <b> " + model.Sender + "</b> <br/> Email: " + model.MailSender + "<br/> To: " + userReceiver.Name + "<br/> Tiêu đề: " + model.TitleCard + " <br/>Nội dung: <br/>" + model.ContenCard + " <br/>";
                        bool res = service.Send(userReceiver.Email, titleEmail, body, path);

                        ViewData["oke"] = res ? "ok" : null;
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