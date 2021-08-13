using DBConect.Farmework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConect
{
    public class AccountModel
    {
        private VssDbContext context = null;

        public AccountModel()
        {
            context = new VssDbContext();
        }
        public bool Login (string username, string password)
        {
            //object[] sqlParams =
            //{
            //    new SqlParameter("@UserName", username),
            //    new SqlParameter("@PassWord", password)
            //};
            //var res = context.Database.SqlQuery<bool>("Admin_Login @UserName, @PassWord", sqlParams).SingleOrDefault();
            //return res;
            var res = context.AccountAdmins.Count(a => a.UserName == username && a.PassWord == password);
            if(res <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
