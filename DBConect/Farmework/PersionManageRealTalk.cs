using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConect.Farmework
{
    [Table("PersionManageRealTalk")]
    public partial class PersionManageRealTalk
    {
        public int id { get; set; }
        public int IdField { get; set; }
        public string FullNameManage { get; set; }
        public string EmailManage { get; set; }
        public string Department { get; set; }
    }
}
