using DBConect.Farmework;
using DBConect.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vss_portal_web.Areas.Admin.Models
{
    public class ModelControllerAdmin
    {
        public ListPost ListPost { get; set; }

        public List<CategoryPost> Category { get; set; }

    }
}