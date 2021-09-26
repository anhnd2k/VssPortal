using DBConect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vss_portal_web.Areas.Admin.Controllers
{
    public class DetailIdeaRegesterController : BaseAdminController
    {
        // GET: Admin/DetailIdeaRegester
        public ActionResult Index(int id)
        {
            var detailIdeaRegester = new ActionPost().DetailIdeaRegester(id);
            ViewData["tabBarSelection"] = "Post";
            return View(detailIdeaRegester);
        }

        public FileResult Download (string imgName)
        {
            
            if(imgName != "")
            {
                var filePath = "~/UploadFiles/fileIdea/" + imgName;
                return File(filePath, "application/force- download", Path.GetFileName(filePath));
            }
            return null;
        }
    }
}