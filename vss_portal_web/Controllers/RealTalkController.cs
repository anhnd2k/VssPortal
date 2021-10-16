using DBConect;
using DBConect.Farmework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using vss_portal_web.Areas.Admin.Code;
using vss_portal_web.Areas.Admin.Models;
using vss_portal_web.Models;

namespace vss_portal_web.Controllers
{
    public class RealTalkController : Controller
    {
        // GET: RealTalk
        public ActionResult Index()
        {
            TempData["myDataRedirect"] = CustomerRedirectLogin.CustomRedirects("Index", "RealTalk");
            ViewData["fullNameUser"] = CheckLoginRole.getUserName();
            ViewData["checkRoleLogin"] = CheckLoginRole.check();
            ViewData["headerActive"] = "active";
            var cookie = SessionHelper.GetSession();
            if (cookie != null)
            {
                ViewBag.checkPermision = true;
            }
            var action = new ActionPost();
            var field = action.getListFieldRealTalk();
            var ListDepartment = action.getListDepartment();

            ModelController controllerModel = new ModelController();
            controllerModel.ListFieldRealTalk = field;
            controllerModel.ListDepartments = ListDepartment;
            return View(controllerModel);
        }

        public ActionResult NewFeedTruth(string searchString, int idTruhStatusFinter = 0, int idInterate = 0, int page = 1, int pageSize = 5)
        {
            TempData["myDataRedirect"] = CustomerRedirectLogin.CustomRedirects("NewFeedTruth", "RealTalk");
            ViewData["fullNameUser"] = CheckLoginRole.getUserName();
            ViewData["checkRoleLogin"] = CheckLoginRole.check();
            ViewData["headerActive"] = "active";

            var cookie = SessionHelper.GetSession();
            if (cookie == null)
            {
                return RedirectToAction("Index", "LoginForGuest");
            }

            var actionPost = new ActionPost();
            var actionCmt = new ActionCommentLikePost();

            var userName = SessionHelper.GetSession()?.UserName;

            ViewData["userName"] = userName;

            var listPostIdea = actionPost.GetListTruthSuccessForCmt(searchString, idTruhStatusFinter, idInterate, page, pageSize);

            ViewData["listCmtPost"] = actionCmt.ListCmtPostIdea();
            ViewData["listTruth"] = actionPost.GetListTruthv2();
            ViewData["TruthStatus"] = actionPost.getListTruthStatus();
            ViewData["searchString"] = searchString;
            ViewData["idSelectedTruth"] = idTruhStatusFinter;
            ViewData["idInterate"] = idInterate;

            return View(listPostIdea);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (new LdapAuthentication().AuthenticateUserV2(model.UserName, model.PassWord) && ModelState.IsValid)
            {
                return RedirectToAction("NewFeedTruth", "RealTalk");
            }
            else
            {
                TempData["messLogin"] = "false";
                ViewBag.checkPermision = "";
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            }
            return RedirectToAction("Index", "RealTalk");
        }

        [HttpPost]
        public JsonResult ActionHearPost(int idPost)
        {
            var userName = SessionHelper.GetSession()?.UserName;
            var actionLike = new ActionCommentLikePost();
            bool res = actionLike.LikePost(idPost, userName);

            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        //load more cmt
        [HttpPost]
        public JsonResult LoadMoreCmt(int idPost, int couted)
        {
            var res = new ActionCommentLikePost().GetCmtLoadMore(idPost, couted);

            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult AddCommentPost(int idPost, string valuePost)
        {
            var actionCmt = new ActionCommentLikePost();
            CommentIdeaPost model = new CommentIdeaPost();
            var session = SessionHelper.GetSession();
            model.IdPostIdea = idPost;
            model.FullNameUser = session.fullName;
            model.UserName = session.UserName;
            model.CommentValue = valuePost;
            
            actionCmt.AddNewCmt(model);

            return new JsonResult { Data = model.FullNameUser, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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

        //View realtalk đã đăng ký
        public ActionResult TruthRegestered()
        {
            TempData["myDataRedirect"] = CustomerRedirectLogin.CustomRedirects("TruthRegestered", "RealTalk");
            string emailVt = SessionHelper.GetSession()?.Email;
            if (emailVt != null)
            {
                var ListRealTalkByEm = new ActionPost().GetListRealTalkByEmail(emailVt);
                return View(ListRealTalkByEm);
            }
            return RedirectToAction("Index", "LoginForGuest");
        }

        //chi tiết real talk - truth
        public ActionResult DetailTruthRegester(int regesterTruth)
        {
            var DetailRealTalk = new ActionPost().DetailRealTalk(regesterTruth);
            return View(DetailRealTalk);
        }

        //create new new truth
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index (TalkReal model, Incognito statusIncognito, int departmentId)
        {
            var action = new ActionPost();
            var field = action.getListFieldRealTalk();
            var listDepartment = action.getListDepartment();
            var ListDepartment = action.getListDepartment();
            ModelController controllerModel = new ModelController();
            controllerModel.ListFieldRealTalk = field;
            controllerModel.ListDepartments = ListDepartment;

            if (statusIncognito.incognitoStatus)
            {
                model.NameSender = null;
                model.MailSender = null;
                model.DepartmentSender = null;
            }
            foreach(var item in listDepartment)
            {
                if(item.Id == departmentId)
                {
                    model.DepartmentSender = item.NameDepartment;
                    break;
                }
            }
            //else
            //{
            //    model.DepartmentSender = SessionHelper.GetSession()?.Department;
            //}

            bool res = new ActionPost().SendRealTalk(model);
            if (res)
            {
                ViewData["oke"] = "ok";
                return View(controllerModel);
            }
            else
            {
                ViewData["oke"] = "false";
                return View(controllerModel);
            }
        }
        
        
    }
}