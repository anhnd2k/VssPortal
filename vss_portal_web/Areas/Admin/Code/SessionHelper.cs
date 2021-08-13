using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vss_portal_web.Areas.Admin.Code
{
    public class SessionHelper
    {
        public static void SetSecssion(UserSession session)
        {
            HttpContext.Current.Session["loginSession"] = session;
        }
        public static UserSession GetSession()
        {
            var session = HttpContext.Current.Session["loginSession"];
            if(session == null)
            {
                return null;
            }
            else
            {
                return session as UserSession;
            }
        }
    }
}