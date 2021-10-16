using DBConect.Farmework;
using System;
using System.Collections;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace DBConect
{
    public class AccountModel
    {
        private VssDbContext context = null;

        private string fullName = string.Empty;
        private string emailVt = string.Empty;

        public AccountModel()
        {
            context = new VssDbContext();
        }
        public bool Login(string username, string password)
        {
            //object[] sqlParams =
            //{
            //    new SqlParameter("@UserName", username),
            //    new SqlParameter("@PassWord", password)
            //};
            //var res = context.Database.SqlQuery<bool>("Admin_Login @UserName, @PassWord", sqlParams).SingleOrDefault();
            //return res;
            var res = context.AccountAdmins.Count(a => a.UserName == username && a.PassWord == password);
            if (res <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int CheckRoleUser(string emailVt)
        {
                AccountAdmin res = context.AccountAdmins.Where(x => x.EmailVt == emailVt).FirstOrDefault();
                if(res == null)
                {
                    return 0;
                }

                if(res?.Role == "ADMIN")
                {
                    return 1;
                }

                if(res?.Role == "RESOLVER_TRUTH")
                {
                    return 2;
                }
            return 1;
        }

        

         //cách 1
        //public bool AuthenticateUserV2(string userName, string password)
        //{
        //    bool ret = false;
        //        DirectoryEntry de = new DirectoryEntry("LDAP://10.30.190.3:389/OU=AzureAD,DC=itbu,DC=local", userName, password);
        //        DirectorySearcher search = null;
        //        search = BuildUserSearcher(de);
        //        search.Filter = "(SAMAccountName=" + userName + ")";
        //        SearchResult result = search.FindOne();
        //        if (null == result)
        //        {
        //            return false;
        //        }
        //        fullName = result.GetPropertyValue("name");
        //        emailVt = result.GetPropertyValue("distinguishedName");
        //        ret = true;
         

        //    return ret;
        //}
        //cách 2
        //public bool PrincipalContext(string userName, string password)
        //{
        //    bool ret = false;
        //    using (var context = new PrincipalContext(ContextType.Domain, "10.30.190.3"))
        //    {
        //        ret = context.ValidateCredentials("ssomanagement", "VLMT@321qaz!@#");
        //    };

        //    return ret;
        //}
        //cách 3
        //public bool LdapConnection(string userName, string password)
        //{
        //    bool ret = false;
        //    using (LdapConnection connection = new LdapConnection("10.30.190.3"))
        //    {
        //        NetworkCredential credential = new NetworkCredential("ssomanagement", "VLMT@321qaz!@#");
        //        connection.Credential = credential;
        //        connection.Bind();
        //        Debug.WriteLine("logged in");
        //    };

        //    return ret;
        //}

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

    //public static class ADExtensionMethods
    //{
    //    public static string GetPropertyValue(this SearchResult sr, string propertyName)
    //    {
    //        string ret = string.Empty;

    //        if (sr.Properties[propertyName].Count > 0)
    //            ret = sr.Properties[propertyName][0].ToString();

    //        return ret;
    //    }
    //}
}
