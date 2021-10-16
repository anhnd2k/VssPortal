using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace vss_portal_web.Areas.Admin.Code
{
    public class SessionHelper
    {
        // role user using page
        public static void SetSecssion(UserSession session)
        {
            HttpContext.Current.Session["loginSession"] = session;
        }

        // role user admin
        public static void SetSessionRoleAdmin(UserSession sess)
        {
            HttpContext.Current.Session["loginSessionRoleAdmin"] = sess;
        }

        //role user resolver truth nói thật đi
        public static void SetSessionRoleResolverTruth(UserSession sesResolver)
        {
            HttpContext.Current.Session["loginSessionRoleSesolver"] = sesResolver;
        }

        //get role user using page
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

        // get role admin
        public static UserSession GetSessionRoleAdmin()
        {
            var session = HttpContext.Current.Session["loginSessionRoleAdmin"];
            if (session == null)
            {
                return null;
            }
            else
            {
                return session as UserSession;
            }
        }

        // get role resolver truth
        public static UserSession GetSessionRoleResolverTruth()
        {
            var session = HttpContext.Current.Session["loginSessionRoleSesolver"];
            if (session == null)
            {
                return null;
            }
            else
            {
                return session as UserSession;
            }
        }
        public static void resetSession()
        {
            HttpContext.Current.Session.Abandon();
        }
    }
}