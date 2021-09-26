using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vss_portal_web.Areas.Admin.Code
{
    [Serializable]
    public class UserSession
    {
        public string UserName { get; set; }

        public string fullName { get; set; }

        public string Email { get; set; }

        public string Department { get; set; }
    }
}