using DBConect.Farmework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vss_portal_web.Models
{
    public class ModelController
    {
        public List<ListPost> ListPost { get; set; }
        public List<ListPost> OnboadingPost { get; set; }

        public List<ListThankCardImg> ThankCardImg { get; set; }
        public List<Department> ListDepartments { get; set; }
        public List<ThankCard> ListhankCard { get; set; }
    }
}