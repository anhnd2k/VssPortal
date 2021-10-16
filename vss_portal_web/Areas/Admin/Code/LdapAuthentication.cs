using DBConect;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace vss_portal_web.Areas.Admin.Code
{
    public class LdapAuthentication
    {
        public bool AuthenticateUserV2(string userName, string password)
        {
            bool ret = false;
            try
            {
                DirectoryEntry de = new DirectoryEntry("LDAP://10.30.190.3:389/OU=AzureAD,DC=itbu,DC=local", userName, password);
                DirectorySearcher search = null;
                search = BuildUserSearcher(de);
                search.Filter = "(SAMAccountName=" + userName + ")";
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return false;
                }
                string fullName = Regex.Replace(result.GetPropertyValue("name"), @"\(.*\)", string.Empty); 
                string emailVt = result.GetPropertyValue("userPrincipalName").Replace("@viettelimex.vn", "@viettel.com.vn");
                string resDistinguished = result.GetPropertyValue("distinguishedName");
                Regex rg = new Regex(@"OU=VSS\s.*(?=\,OU=AzureAD)");
                string department = rg.Match(resDistinguished).ToString().Trim();
                string resDepartment = department.Replace("OU=VSS", "");
                var userSession = new UserSession();
                userSession.UserName = userName;
                userSession.fullName = fullName;
                userSession.Email = emailVt;
                //userSession.Email = "hoanganh1112k@gmail.com";
                userSession.Department = resDepartment;
                //FormsAuthentication.SetAuthCookie(userName, false, emailVt);
                SessionHelper.SetSecssion(userSession);

                int checkRole = new AccountModel().CheckRoleUser(emailVt);

                if(checkRole == 1)
                {
                    SessionHelper.SetSessionRoleAdmin(userSession);
                }

                if(checkRole == 2)
                {
                    SessionHelper.SetSessionRoleResolverTruth(userSession);
                }

                ret = true;
                return ret;
            }
            catch
            {
                ret = false;
                return ret;
            }
        }

        private DirectorySearcher BuildUserSearcher(DirectoryEntry de)
        {
            DirectorySearcher ds = null;

            ds = new DirectorySearcher(de);

            // Full Name
            ds.PropertiesToLoad.Add("name");

            // Email Address
            ds.PropertiesToLoad.Add("mail");

            // First Name
            ds.PropertiesToLoad.Add("givenname");

            // Last Name (Surname)
            ds.PropertiesToLoad.Add("sn");

            // Login Name
            ds.PropertiesToLoad.Add("userPrincipalName");

            // Distinguished Name
            ds.PropertiesToLoad.Add("distinguishedName");

            ds.PropertiesToLoad.Add("usergroup");

            ds.PropertiesToLoad.Add("displayname");

            ds.PropertiesToLoad.Add("department");

            return ds;
        }
    }
    public static class ADExtensionMethods
    {
        public static string GetPropertyValue(this SearchResult sr, string propertyName)
        {
            string ret = string.Empty;

            if (sr.Properties[propertyName].Count > 0)
                ret = sr.Properties[propertyName][0].ToString();

            return ret;
        }
    }
}