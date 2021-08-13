using DBConect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vss_portal_web.Controllers
{
    public class AllPostsController : Controller
    {
        // GET: AllPosts
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
             var list = new ActionPost().GetListPostsAllPostHome(searchString, page, pageSize);

            return View(list);
        }
    }
}